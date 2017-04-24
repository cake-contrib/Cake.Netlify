using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Netlify {
    /// <summary>
    /// Base class for all Netlify-CLI tools.
    /// </summary>
    /// <typeparam name="TSettings"></typeparam>
    public abstract class NetlifyTool<TSettings> : Tool<TSettings> where TSettings : NetlifySettings {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetlifyTool{TSettings}"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        protected NetlifyTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools) {
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected sealed override string GetToolName() {
            return "Netlify";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected sealed override IEnumerable<string> GetToolExecutableNames() {
            return new[] {"netlify.cmd", "netlify"};
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(TSettings settings)
        {
            return new []
            {
                new FilePath("./node_modules/.bin/netlify.cmd"),
                new FilePath("./node_modules/.bin/netlify")
            };
        }
    }
}
