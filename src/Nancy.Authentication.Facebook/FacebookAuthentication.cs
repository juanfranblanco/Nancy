using System;
using Facebook;
using Nancy.Authentication.Facebook.Repository;
using Nancy.Extensions;
using Nancy.Security;

namespace Nancy.Authentication.Facebook
{
    public class FacebookAuthentication
    {
        /// <summary>
        /// Returns if the facebook authentication is enabled
        /// </summary>
        public static bool Enabled { get; private set; }

        /// <summary>
        /// Returns the facebook authentication configuration
        /// </summary>
        public static FacebookAuthenticationConfiguration Configuration { get; private set; }

        /// <summary>
        /// Returns the configured facebook user cache
        /// </summary>
        public static IFacebookCurrentAuthenticatedUserCache FacebookCurrentAuthenticatedUserCache
        {
            get { return Configuration != null ? Configuration.FacebookCurrentAuthenticatedUserCache : null; }
        }


        /// <summary>
        /// Returns the configured application authenticator
        /// </summary>
        private static IApplicationAuthenticator ApplicationAuthenticator
        {
            get { return Configuration != null ? Configuration.ApplicationAuthenticator : null; }
        }

        /// <summary>
        /// Facebook client service
        /// </summary>
        public static FacebookClientService FacebookClientService { get; private set; }

        
        public static FacebookOAuthService FacebookOAuthService { get; private set; }

        /// <summary>
        /// Enable facebook authentication for the application
        /// </summary>
        /// <param name="configuration">
        /// The facebook authentication configuration
        /// </param>
        public static void Enable(FacebookAuthenticationConfiguration configuration)
        {
            Enable(configuration, new FacebookSDKObjectBuilder(), new FacebookUrlHelper(configuration));
        }

        /// <summary>
        /// Enable facebook authentication for the application
        /// </summary>
        /// <param name="configuration">
        /// The facebook authentication configuration
        /// </param>
        /// <param name="facebookSdkObjectBuilder">
        /// The facebook object builder
        /// </param>
        /// <param name="facebookUrlHelper">
        /// The facebook url helper
        /// </param>
        public static void Enable(FacebookAuthenticationConfiguration configuration, IFacebookSDKObjectBuilder facebookSdkObjectBuilder, IFacebookUrlHelper facebookUrlHelper)
        {

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (!configuration.IsValid)
            {
                throw new ArgumentException("Configuration is invalid", "configuration");
            }


            if (facebookSdkObjectBuilder == null)
            {
                throw new ArgumentNullException("facebookSdkObjectBuilder");
            }

            if (facebookUrlHelper == null)
            {
                throw new ArgumentNullException("facebookUrlHelper");
            }

            Enabled = true;
            Configuration = configuration;
            FacebookClientService = new FacebookClientService(ApplicationAuthenticator, FacebookCurrentAuthenticatedUserCache, facebookSdkObjectBuilder);
            FacebookOAuthService = new FacebookOAuthService(facebookSdkObjectBuilder, facebookUrlHelper);
            
        }

        public static Response RedirectToFacebookLoginUrl(NancyContext context)
        {
            //TODO: The login parameters should be configurable
            //TODO: Extract the redirect url (from the application authenticator) and pass it as a parameter
            return context.GetRedirect(FacebookOAuthService.GetAbsoluteLoginUrl(Configuration.FacebookExtendedPermissions));
        }

        public static Response LoginIntoApplicationWithFacebookOAthResponse(NancyContext context, string pathToRedirectOnAutheticationFailure)
        {
            string code = context.Request.Query.code;
            if (FacebookOAuthService.IsOAthResultSuccess(context))
            {
                var accessToken = FacebookOAuthService.GetAccessToken(code);
                var me = FacebookClientService.GetFacebookMe(accessToken);
                AddAuthenticatedUserToCache(me, accessToken);
                var facebookId = Convert.ToInt64(me.id);
                return LoginIntoTheApplicationAndRedirect(context, facebookId);
            }

            return context.GetRedirect(pathToRedirectOnAutheticationFailure);
        }

        /// <summary>
        /// Logs and redirects the facebook user into the application using the configured application authenticator (this is done normally after the success oAth from facebook)
        /// </summary>
        public static Response LoginIntoTheApplicationAndRedirect(NancyContext context, long facebookId)
        {
            return ApplicationAuthenticator.UserLoggedInRedirectResponse(context, facebookId);
        }

        /// <summary>
        /// Logs out the user from facebook and redirects to a given path (this will be normally the application logout path to complete the full logout)
        /// </summary>
        public static Response LogoutFromFacebookAndRedirect(NancyContext context, string path)
        {
            var facebookId = ApplicationAuthenticator.GetFacebookId(context);

            if (facebookId.HasValue)
            {
                var accessToken = FacebookCurrentAuthenticatedUserCache.GetAccessToken(facebookId.Value);

                return context.GetRedirect(FacebookOAuthService.GetFacebookLogoutUrl(path, accessToken));
            }

            return null;
        }


        public static Response RedirectToFacebookLoginAndResetAuthenticationWhenNotAuthenticatedByFacebook(NancyContext context)
        {
            if (Enabled && !IsAuthenticatedByFacebook(context))
            {

                RemoveUserFromCache(context);
                ApplicationAuthenticator.SetAsNotAuthenticated(context);
                return context.GetRedirect(Configuration.FacebookLoginPath);

            }
            return context.Response;

        }
       

        public static bool IsAuthenticatedByFacebook(NancyContext context)
        {
            try
            {
                var me = FacebookClientService.GetFacebookMe(context);
                return true;
            }
            catch (FacebookOAuthException)
            {
                return false;
            }
        }

        public static void AddAuthenticatedUserToCache(dynamic facebookMe, string accessToken)
        {
            long facebookId = Convert.ToInt64(facebookMe.id);
            FacebookCurrentAuthenticatedUserCache.AddUserToCache(facebookId, accessToken, (string)facebookMe.name);
        }


        private static void RemoveUserFromCache(NancyContext context)
        {
            var facebookId = ApplicationAuthenticator.GetFacebookId(context);
            if (facebookId.HasValue) FacebookCurrentAuthenticatedUserCache.RemoveUserFromCache(facebookId.Value);
        }

    }
}