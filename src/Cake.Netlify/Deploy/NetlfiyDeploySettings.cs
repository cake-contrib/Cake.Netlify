namespace Cake.Netlify.Deploy {
    /// <summary>
    /// Contains settings used by <see cref="NetlifyDeployRunner"/>.
    /// </summary>
    public class NetlfiyDeploySettings : NetlifySettings {
        /// <summary>
        /// Gets or sets the site id to deploy.
        /// </summary>
        public string SiteId { get; set; }

        /// <summary>
        /// Gets or sets the environment to deploy.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the Netlify API Token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets if to deploy as a draft without publishing.
        /// </summary>
        public bool Draft { get; set; }
    }
}