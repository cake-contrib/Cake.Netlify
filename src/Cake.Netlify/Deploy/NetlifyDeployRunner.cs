using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Netlify.Deploy {
    /// <summary>
    /// The Netlify Deploy runner used to deploy sites.
    /// </summary>
    public sealed class NetlifyDeployRunner : NetlifyTool<NetlfiyDeploySettings> {
        private readonly ICakeEnvironment _environment;

        public NetlifyDeployRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools) {
            _environment = environment;
        }

        /// <summary>
        /// Deploys a directory to Netlify using the specified settings.
        /// </summary>
        /// <param name="directoryToDeploy">The directory to deploy.</param>
        /// <param name="settings">The settings.</param>
        public void Deploy(DirectoryPath directoryToDeploy, NetlfiyDeploySettings settings) {
            if (directoryToDeploy == null) {
                throw new ArgumentNullException(nameof(directoryToDeploy));
            }
            if (settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }
            Run(settings, GetArguments(directoryToDeploy, settings));
        }

        private ProcessArgumentBuilder GetArguments(DirectoryPath directoryToDeploy, NetlfiyDeploySettings settings) {
            var builder = new ProcessArgumentBuilder();
            builder.Append("deploy");

            builder.Append("-p");
            builder.AppendQuoted(directoryToDeploy.MakeAbsolute(_environment).FullPath);

            if (!string.IsNullOrWhiteSpace(settings.SiteId)) {
                builder.Append("-s");
                builder.Append(settings.SiteId);
            }

            if (!string.IsNullOrWhiteSpace(settings.Token)) {
                builder.Append("-t");
                builder.Append(settings.Token);
            }

            if (settings.Draft) {
                builder.Append("-d");
            }

            if (!string.IsNullOrWhiteSpace(settings.Environment)) {
                builder.Append("-e");
                builder.Append(settings.Environment);
            }

            return builder;
        }
    }
}