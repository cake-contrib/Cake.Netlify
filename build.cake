//////////////////////////////////////////////////////////////////////
// ADDINS
/////////////////////////////////////////////////////////////////////

#tool nuget:?package=Fixie

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/Cake.Netlify/bin") + Directory(configuration);
var solution = "./src/Cake.Netlify.sln";
var artifactsDir = Directory("./src/artifacts");
var assemblyInfo = ParseAssemblyInfo("./src/Cake.Netlify/Properties/AssemblyInfo.cs");
var buildNumber = AppVeyor.Environment.Build.Number;
Information(assemblyInfo.AssemblyVersion);
var version = assemblyInfo.AssemblyVersion; 

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => {
        CleanDirectory(buildDir);
        CleanDirectory(artifactsDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() => {
        NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() => {
      MSBuild(solution, settings =>
        settings.SetConfiguration(configuration));    
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() => {
        Fixie("./src/**/bin/" + configuration + "/*.Tests.dll");
});

Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>{
        EnsureDirectoryExists(artifactsDir);
        var nuGetPackSettings = new NuGetPackSettings {
            Id = "Cake.Netlfiy",
            Version = version,
            Title = "Cake.Netlfiy",
            Authors = new []{"Jamie Phillips"},
            Owners = new []{"Jamie Phillips"},
            Description = "Cake Addin for working with Netlify-CLI from a Cake script.",
            ProjectUrl = new Uri("https://github.com/phillipsj/Cake.Netlify"),
            IconUrl = new Uri("https://raw.githubusercontent.com/cake-build/graphics/master/png/cake-small.png"),
            LicenseUrl = new Uri("https://github.com/phillipsj/Cake.Netlify/blob/master/LICENSE.md"),
            Copyright = string.Format("Jamie Phillips {0}", DateTime.Now.Year),
            Tags = new []{"Cake", "Netlify"},
            RequireLicenseAcceptance = false,
            Symbols =  false,
            NoPackageAnalysis = true,
            Files = new []{
                new NuSpecContent {Source = "Cake.Netlify.dll", Target="lib/net45/Cake.Netlify.dll"},
                new NuSpecContent {Source = "Cake.Netlify.XML", Target="lib/net45/Cake.Netlify.xml"}
            },
            BasePath = "./src/Cake.Netlify/bin/" + configuration,
            OutputDirectory = artifactsDir
        };
        NuGetPack(nuGetPackSettings);
});

Task("Publish")
   .IsDependentOn("Package")
   .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
   .Does(() => {
    var apiKey = EnvironmentVariable("NUGET_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve Nuget API key.");
    }
    // Push the package.
    var package = "./src/artifacts/Cake.Netlify." + version + ".nupkg";
    NuGetPush(package, new NuGetPushSettings { 
        Source = "https://api.nuget.org/v3/index.json",
        ApiKey = apiKey
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);