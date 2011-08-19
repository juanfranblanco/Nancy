using System;
using Nancy.Extensions;

namespace Nancy.Authentication.Facebook.Modules
{
    public class FacebookSecurityModule:NancyModule
    {
        
        public FacebookSecurityModule()
        {
            if (FacebookAuthentication.Enabled)
            {
                Get[FacebookAuthentication.Configuration.LoginPath] = x =>
                                     {

                                         return FacebookAuthentication.RedirectToFacebookLoginUrl(Context);
                                     };

                Get[FacebookAuthentication.Configuration.OAthPath] = x =>
                                    {
                                        string code = Context.Request.Query.code;
                                        if (FacebookAuthentication.IsOAthResultSuccess(Context))
                                        {
                                            var facebookId = FacebookAuthentication.RetrieveAndAddAuthenticatedUserToCache(code);
                                            return FacebookAuthentication.LoginAndRedirect(Context, facebookId);
                                        }
                                        
                                        return Context.GetRedirect(FacebookAuthentication.Configuration.ApplicationLogoutPath);
                                    };


                Get[FacebookAuthentication.Configuration.FacebookLogoutPath] = x =>
                                                                           {
                                                                               //Logout of facebook and then redirection to logout of the app.
                                                                              //this way remove coupling of any application authentication storage provider (ie the Forms cookies:)).
                                                                              //Even if we wanted to use a configurable interface to the pre-logout of the app when using forms cookies there was not easy way to clear the authentication so this seems a good option.
                                                                               return
                                                                                   FacebookAuthentication.
                                                                                       LogoutAndRedirect(Context, FacebookAuthentication.Configuration.ApplicationLogoutPath);
                                                                           };

            }
        }
    }
}