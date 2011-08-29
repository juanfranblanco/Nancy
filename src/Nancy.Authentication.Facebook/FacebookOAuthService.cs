using System;
using System.Collections.Generic;
using Facebook;

namespace Nancy.Authentication.Facebook
{
    public class FacebookOAuthService
    {
        private readonly IFacebookAPIFactory facebookAPIFactory;
        private readonly IFacebookUrlHelper facebookUrlHelper;

        /// <summary>
        /// The facebook logout url  with the parameters "next" which is the redirect url, and access_token which is the token of the current authenticated user. 
        /// </summary>
        public const string FACEBOOK_LOG_OUT_URL_FORMAT = "https://www.facebook.com/logout.php?next={0}&access_token={1}";

        public FacebookOAuthService(IFacebookAPIFactory facebookAPIFactory, IFacebookUrlHelper facebookUrlHelper)
        {
            if (facebookAPIFactory == null) throw new ArgumentNullException("facebookAPIFactory");
            if (facebookUrlHelper == null) throw new ArgumentNullException("facebookUrlHelper");

            this.facebookAPIFactory = facebookAPIFactory;
            this.facebookUrlHelper = facebookUrlHelper;
        }

        public FacebookOAuthClient GetFacebookOAuthClient()
        {
            var oAuthClient = facebookAPIFactory.CreateNewFacebookOAuthClient();
            oAuthClient.RedirectUri = new Uri(facebookUrlHelper.GetOathAbsoluteUrl());
            return oAuthClient;
        }

        public string GetAbsoluteLoginUrl(string extendedPermissions)
        {
            //TODO: The login parameters should be configurable
            //TODO: Extract the redirect url (from the application authenticator) and pass it as a parameter
            var loginParameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                loginParameters["scope"] = extendedPermissions;
            }
            return GetFacebookOAuthClient().GetLoginUrl(loginParameters).AbsoluteUri;
        }


        /// <summary>
        /// Using the contexts request url checks if the oAth authentication has been a success
        /// </summary>
        public bool IsOAthResultSuccess(NancyContext context)
        {
            FacebookOAuthResult oauthResult;
            var stringUri = facebookUrlHelper.GetRequestAbsoluteUrl(context);
            if (FacebookOAuthResult.TryParse(stringUri, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    return true;
                }
            }
            return false;
        }


        public string GetAccessToken(string code)
        {
            var oAuthClient = GetFacebookOAuthClient();
            dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
            return tokenResult.access_token;
        }

       
        /// <summary>
        /// Returns the facebook logout url with a redirectPath and acessToken.
        /// </summary>
        //This method of login out comes from http://forum.developers.facebook.net/viewtopic.php?id=87109 it seems to be an issue with login out.
        public string GetFacebookLogoutUrl(string redirectPath, string accessToken)
        {
            return
                string.Format(FACEBOOK_LOG_OUT_URL_FORMAT,
                              facebookUrlHelper.ExpandPath(redirectPath),
                              accessToken);
        }
       
    }
}