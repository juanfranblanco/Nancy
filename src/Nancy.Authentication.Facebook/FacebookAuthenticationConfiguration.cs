using System;
using Nancy.Authentication.Facebook.Modules;
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

       

        /// <summary>
        /// The facebook user cache, to store and retrieve current authenticated users
        /// </summary>
        public IFacebookCurrentAuthenticatedUserCache FacebookCurrentAuthenticatedUserCache { get; set; }

        /// <summary>
        /// The Application Authenticator, to authenticate users in the current application after a successful login into facebook
        /// </summary>
        public IApplicationAuthenticator ApplicationAuthenticator { get; set; }
        
        /// <summary>
        /// Comma delimited set of facebook extended permissions, to allow the usage of specific features.
        /// For a full list look at: http://developers.facebook.com/docs/reference/api/permissions/
        /// </summary>
        /// <example>
        ///  user_about_me,publish_stream,offline_access
        /// </example>
        /// <remarks>
        /// This is not a required field.
        /// </remarks>
        public string FacebookExtendedPermissions { get; set; }

        private string applicationBasePath;
        /// <summary>
        /// The application base path
        /// </summary>
        /// <example>
        /// http://localhost/
        /// </example>
        public string ApplicationBasePath
        {
            get { return this.applicationBasePath; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    this.applicationBasePath = String.Empty;
                    return;
                }

                this.applicationBasePath = value.TrimEnd('/');
            }
        }
        
        /// <summary>
        /// The path to the module that will start the facebook authentication
        /// <see cref="Nancy.Authentication.Facebook.Modules.FacebookSecurityModule" />
        /// </summary>
        public string FacebookLoginPath { get; set; }

        /// <summary>
        /// The path to the module that will get the oath response of the facebook authentication
        /// <see cref="Nancy.Authentication.Facebook.Modules.FacebookSecurityModule" />
        /// </summary>
        public string FacebookOAthResponsePath { get; set; }

        /// <summary>
        /// The path to the module that will start the facebook logout
        /// <see cref="Nancy.Authentication.Facebook.Modules.FacebookSecurityModule" />
        /// </summary>
        public string FacebookLogoutPath { get; set; }

        /// <summary>
        /// The path to the module that will start the logout of the application after the successful logout of facebook
        /// <see cref="Nancy.Authentication.Facebook.Modules.FacebookSecurityModule" />
        /// </summary>
        public string ApplicationLogoutPath { get; set; }

        public FacebookAuthenticationConfiguration()
        {
            FacebookLoginPath = LOGIN_PATH;
            FacebookOAthResponsePath = O_ATH_PATH;
            FacebookLogoutPath = FACEBOOK_LOGOUT_PATH;
        }

        /// <summary>
        /// Gets a value indicating whether the configuration is valid or not.
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.ApplicationBasePath))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(this.FacebookOAthResponsePath))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(this.FacebookLoginPath))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(this.FacebookLogoutPath))
                {
                    return false;
                }

                if(string.IsNullOrEmpty(this.ApplicationLogoutPath))
                {
                    return false;
                }

                if (FacebookCurrentAuthenticatedUserCache == null)
                {
                    return false;
                }

                if (ApplicationAuthenticator == null)
                {
                    return false;
                }

                return true;
            }
        }
    }
}