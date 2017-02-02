using Cake.Core.IO;
using Cake.Netlify.Deploy;
using Cake.Testing.Fixtures;

namespace Cake.Netlify.Tests.Fixture {
    internal sealed class NetlifyDeployFixture : ToolFixture<NetlfiyDeploySettings> {
        public DirectoryPath DirectoryToDeploy { get; set; }

        public NetlifyDeployFixture() : base("netlify.cmd") {
            DirectoryToDeploy = "dist";
        }

        protected override void RunTool() {
            var tool = new NetlifyDeployRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Deploy(DirectoryToDeploy, Settings);
        }
    }
}