using System;
using Facebook;
using Nancy.Authentication.Forms;
using Nancy.Extensions;
using Nancy.Authentication.Forms;


namespace Nancy.Authentication.Facebook.Modules
{
    public class FacebookSecurityModule:NancyModule
    {
        public static bool Enabled { get; private set;}

        public static void Enable(FacebookAuthenticationConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if(!configuration.IsValid)
            {

                throw new ArgumentException("Configuration is invalid", "configuration");
            }

            Enabled = true;
            Configuration = configuration;
        }

        public static FacebookAuthenticationConfiguration Configuration { get; private set; }


        public FacebookSecurityModule()
        {
            if (Enabled)
            {
                Get[Configuration.LoginPath] = x =>
                                     {

                                         return Context.GetRedirect(GetFacebookOAuthClient().GetLoginUrl().AbsoluteUri);
                                     };

                Get[Configuration.OAthPath] = x =>
                                    {
                                        string code = Context.Request.Query.code;
                                        FacebookOAuthResult oauthResult;
                                        var stringUri = GetRequestUriAbsolutePath();

                                        if (FacebookOAuthResult.TryParse(stringUri, out oauthResult))
                                        {
                                            if (oauthResult.IsSuccess)
                                            {
                                                //Assign a temporary GUID to identify the user via cookies, we follow Nancy Forms Authentication to prevent storing facebook ids or tokens in cookies.
                                                //What if I want to store users using the Guid? I would like to get the Guid that match the UserId no?
                                                var userId = Guid.NewGuid();
                                                AddAuthenticatedUserToCache(code, userId);
                                                return this.LoginAndRedirect(userId);

                                            }
                                        }

                                        return this.LogoutAndRedirect("~/");
                                    };


                Get[Configuration.LogoutPath] = x =>
                                      {
                                          //TODO: Logout of facebook and the application
                                          return this.LogoutAndRedirect("~/");

                                      };
            }
        }

       //All Facebook specific stuff could be put somewhere else

        private FacebookOAuthClient GetFacebookOAuthClient()
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(Configuration.GetOathRedirectUrl());
            return oAuthClient;
        }


        //The base path is not part the context request.. so this needs to be configured :(.
        //This should go in a helper
        //What about 2 / in the url?
        private string GetRequestUriAbsolutePath()
        {
            var url = Context.Request.Url;
            return Configuration.BasePath + "/" + url.Path + url.Query;
        }

        public string GetAccessToken(string code)
        {
            var oAuthClient = GetFacebookOAuthClient();
            dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
            return tokenResult.access_token;
        }

        private void AddAuthenticatedUserToCache(string code, Guid userId)
        {
            string accessToken = GetAccessToken(code);
            var facebookClient = new FacebookClient(accessToken);
            dynamic me = facebookClient.Get("me?fields=id,name");
            long facebookId = Convert.ToInt64(me.id);
            Configuration.FacebookUserCache.AddUserToCache(userId, facebookId, accessToken, (string) me.name);
        }

    }
}