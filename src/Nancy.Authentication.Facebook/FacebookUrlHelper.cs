namespace Nancy.Authentication.Facebook
{
    public class FacebookUrlHelper : IFacebookUrlHelper
    {
        private readonly FacebookAuthenticationConfiguration facebookAuthenticationConfiguration;

        public FacebookUrlHelper(FacebookAuthenticationConfiguration facebookAuthenticationConfiguration)
        {
            this.facebookAuthenticationConfiguration = facebookAuthenticationConfiguration;
        }

        /// <summary>
        /// Returns the Oath absolute url
        /// </summary>
        public string GetOathAbsoluteUrl()
        {
            return ExpandPath(facebookAuthenticationConfiguration.FacebookOAthResponsePath);
        }

        /// <summary>
        /// Expands a path using the base path
        /// </summary>
        public string ExpandPath(string path)
        {
            return facebookAuthenticationConfiguration.ApplicationBasePath + EnsurePathStartsWithForwardSlash(path);
        }
        
        /// <summary>
        /// Ensures that a path starts with a forward slash
        /// </summary>
        /// <returns>The path starting with a forward slash</returns>
        private string EnsurePathStartsWithForwardSlash(string path)
        {
            return path.StartsWith("/") ? path : "/" + path;
        }

        /// <summary>
        /// Expands a path and a query
        /// </summary>
        private string ExpandPath(string path, string query)
        {
            return ExpandPath(path) + query;
        }

        /// <summary>
        /// Returns the request absolute url using the configured base path
        /// </summary>
        public string GetRequestAbsoluteUrl(NancyContext context)
        {
            var url = context.Request.Url;
            return ExpandPath(url.Path, url.Query);
        }

    }
}