using System;
using Nancy.Authentication.Facebook.Repository;

namespace Nancy.Authentication.Facebook
{
    /// <summary>
    /// Configuration options for facebook authentication
    /// </summary>
    public class FacebookAuthenticationConfiguration
    {
        private const string LOGIN_PATH = "/login";
        private const string O_ATH_PATH = "/oath";
        private const string FACEBOOK_LOGOUT_PATH = "/logout";
        private const string FACEBOOK_LOG_OUT_URL_FORMAT = "https://www.facebook.com/logout.php?next={0}&access_token={1}";

        public IFacebookUserCache FacebookUserCache { get; set; }

        private string basePath;

        public string BasePath
        {
            get { return this.basePath; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                this.basePath = value.TrimEnd('/');
            }
        }

        public string LoginPath { get; set; }
        public string OAthPath { get; set; }
        public string LogoutPath { get; set; }
      

        public FacebookAuthenticationConfiguration()
        {
            LoginPath = LOGIN_PATH;
            OAthPath = O_ATH_PATH;
            LogoutPath = FACEBOOK_LOGOUT_PATH;
        }

        public string GetOathAbsoluteUrl()
        {
            return BasePath + OAthPath;
        }

        private string ExpandPath(string path)
        {
            return BasePath + path;
        }

        private string ExpandPath(string path, string query)
        {
            return ExpandPath(path) + query;
        }

        public string GetRequestAbsoluteUrl(NancyContext context)
        {
            var url = context.Request.Url;
            return ExpandPath(url.Path, url.Query);
        }

        public string GetFacebookLogoutUrl(string redirectPath, string accessToken)
        {
            return
                string.Format(FACEBOOK_LOG_OUT_URL_FORMAT,
                                            ExpandPath(redirectPath),
                                            accessToken);
        }

        /// <summary>
        /// Gets a value indicating whether the configuration is valid or not.
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.BasePath))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(this.OAthPath))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(this.LoginPath))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(this.LogoutPath))
                {
                    return false;
                }

                if (FacebookUserCache == null)
                {
                    return false;
                }

                return true;
            }
        }
    }
}