using System;
using Facebook;
using Nancy.Authentication.Facebook.Model;
using Nancy.Authentication.Facebook.Repository;
using Nancy.Authentication.Forms;
using Nancy.Extensions;
using Nancy.Authentication.Forms;


namespace Nancy.Authentication.Facebook.Modules
{
    public abstract class FacebookSecurityModule:NancyModule
    {
        public abstract string GetBasePath();

        private static string loginPath = "/login";
        public static string LoginPath
        {
            get { return loginPath; }
            set { loginPath = value; }
        }

        private static string oAthPath = "/oath";
        public static string OAthPath
        {
            get { return oAthPath; }
            set { oAthPath = value; }
        }

        private static string logoutPath = "/logout";
        public static string LogoutPath
        {
            get { return logoutPath; }
            set { logoutPath = value; }
        }


        public FacebookSecurityModule()
        {
            Get[LoginPath] = x =>
                                {
                                    
                                    return Context.GetRedirect(GetFacebookOAuthClient().GetLoginUrl().AbsoluteUri);
                                };

            Get[OAthPath] = x =>
                               {
                                   string code = Context.Request.Query.code;
                                   FacebookOAuthResult oauthResult;
                                   var stringUri = GetRequestUriAbsolutePath();
                                   
                                   if (FacebookOAuthResult.TryParse(stringUri, out oauthResult))
                                   {
                                       if (oauthResult.IsSuccess)
                                       {
                                           //Assign a temporary GUID to identify the user via cookies, we follow Nancy Forms Authentication to prevent storing facebook ids or tokens in cookies.
                                           var userId = Guid.NewGuid();
                                           AddAuthenticatedUserToCache(code, userId);
                                           return this.LoginAndRedirect(userId);

                                       }
                                   }

                                   return this.LogoutAndRedirect("~/");
                               };


            Get[LogoutPath] = x =>
                                 {
                                     return this.LogoutAndRedirect("~/");

                                 };
        }

        public String GetOathRedirectUrl()
        {
            return GetBasePath() + OAthPath;
        }

        private FacebookOAuthClient GetFacebookOAuthClient()
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(GetOathRedirectUrl());
            return oAuthClient;
        }


        //The base path is not part the context request.. so this needs to be configured :(.
        private string GetRequestUriAbsolutePath()
        {
            var url = Context.Request.Url;
            return GetBasePath() + "/" + url.Path + url.Query;
        }

        private void AddAuthenticatedUserToCache(string code, Guid userId)
        {
            var oAuthClient = GetFacebookOAuthClient();
            dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
            string accessToken = tokenResult.access_token;
            var facebookClient = new FacebookClient(accessToken);
            dynamic me = facebookClient.Get("me?fields=id,name");
            long facebookId = Convert.ToInt64(me.id);

            InMemoryUserCache.Add(new FacebookUser
                                      {
                                          UserId = userId,
                                          AccessToken = accessToken,
                                          FacebookId = facebookId,
                                          Name = (string)me.name,
                                      });
        }

    }
}