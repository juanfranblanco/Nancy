using System;
using Nancy.Authentication.Facebook.Repository;

namespace Nancy.Authentication.Facebook.Modules
{
    /// <summary>
    /// Configuration options for facebook authentication
    /// </summary>
    public class FacebookAuthenticationConfiguration
    {

        public IFacebookUserCache FacebookUserCache { get; set; }

        public string BasePath { get; set; }

        private string loginPath = "/login";
        public string LoginPath
        {
            get { return loginPath; }
            set { loginPath = value; }
        }

        private string oAthPath = "/oath";
        public string OAthPath
        {
            get { return oAthPath; }
            set { oAthPath = value; }
        }

        private string logoutPath = "/logout";
        public string LogoutPath
        {
            get { return logoutPath; }
            set { logoutPath = value; }
        }

        public String GetOathRedirectUrl()
        {
            return BasePath + OAthPath;
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

                if(string.IsNullOrEmpty(this.LoginPath))
                {
                    return false;
                }

                if(string.IsNullOrEmpty(this.LogoutPath))
                {
                    return false;
                }

                if(FacebookUserCache == null)
                {
                    return false;
                }

                return true;
            }
        }
    }
}