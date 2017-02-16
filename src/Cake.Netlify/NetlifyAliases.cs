using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Netlify.Deploy;

namespace Cake.Netlify {
    /// <summary>
    /// <para>Contains functionality related to manage netlify sites using https://www.netlify.com/docs/cli/"</para>
    /// <para>
    /// In order to use the commands for this alias, the netlify-cli will need to be installed on the machine where the Cake script is being executed.
    /// This is typically achieved by installing the npm module.
    /// </para>
    /// </summary>
    [CakeAliasCategory("Netlify")]
    public static class NetlifyAliases {
        /// <summary>
        /// Deploys the directory to Netlify using the current working directory, site id, and token.
        /// </summary>
        /// <example>
        /// <code>
        ///     NetlifyDeploy("my-site-id", "my-api-token"); 
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="siteId">The site id.</param>
        /// <param name="token">The api token.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Deploy")]
        [CakeNamespaceImport("Cake.Netlify.Deploy")]
        public static void NetlifyDeploy(this ICakeContext context, string siteId, string token) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            if (string.IsNullOrWhiteSpace(siteId)) {
                throw new ArgumentNullException(nameof(siteId));
            }
            if (string.IsNullOrWhiteSpace(token)) {
                throw new ArgumentNullException(nameof(token));
            }

            var netlifyDeploy = new NetlifyDeployRunner(context.FileSystem, context.Environment, context.ProcessRunner,
                context.Tools);
            netlifyDeploy.Deploy(context.Environment.WorkingDirectory,
                new NetlfiyDeploySettings {SiteId = siteId, Token = token});
        }

        /// <summary>
        /// Deploys directory to Netlify using the specified directory, site id, and token.
        /// </summary>
        /// <example>
        /// <code>
        ///     NetlifyDeploy(Directory("dist"), "my-site-id", "my-api-token"); 
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="directoryToDeploy">The directory to deploy.</param>
        /// <param name="siteId">The site id.</param>
        /// <param name="token">The api token.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Deploy")]
        [CakeNamespaceImport("Cake.Netlify.Deploy")]
        public static void NetlifyDeploy(this ICakeContext context, DirectoryPath directoryToDeploy, string siteId,
            string token) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }
            if (string.IsNullOrWhiteSpace(siteId)) {
                throw new ArgumentNullException(nameof(siteId));
            }
            if (string.IsNullOrWhiteSpace(token)) {
                throw new ArgumentNullException(nameof(token));
            }

            var netlifyDeploy = new NetlifyDeployRunner(context.FileSystem, context.Environment, context.ProcessRunner,
                context.Tools);
            netlifyDeploy.Deploy(directoryToDeploy, new NetlfiyDeploySettings {SiteId = siteId, Token = token});
        }

        /// <summary>
        /// Deploys directory to Netlify using the specified directory and settings.
        /// </summary>
        /// <example>
        /// <code>
        ///     NetlifyDeploy(Directory("dist"), new NetlfiyDeploySettings { SiteId = "my-site-id", Token = "my-api-token" }); 
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="directoryToDeploy">The directory to deploy.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Deploy")]
        [CakeNamespaceImport("Cake.Netlify.Deploy")]
        public static void NetlifyDeploy(this ICakeContext context, DirectoryPath directoryToDeploy,
            NetlfiyDeploySettings settings) {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            var netlifyDeploy = new NetlifyDeployRunner(context.FileSystem, context.Environment, context.ProcessRunner,
                context.Tools);
            netlifyDeploy.Deploy(directoryToDeploy, settings);
        }
    }
}
