using System;
using Nancy.Extensions;
using Nancy.Authentication.Facebook.FacebookExtensions;

namespace Nancy.Authentication.Facebook.Modules
{
    public class FacebookSecurityModule:NancyModule
    {
        public FacebookSecurityModule()
        {
            if (FacebookAuthentication.Enabled)
            {
                Get[FacebookAuthentication.Configuration.FacebookLoginPath] = x =>
                                    {
                                        return this.RedirectToFacebookLoginUrl();
                                    };

                Get[FacebookAuthentication.Configuration.FacebookOAthResponsePath] = x =>
                                    {
                                        return this.LoginIntoApplicationWithFacebookOAthResponse(FacebookAuthentication.Configuration.ApplicationLogoutPath);
                                    };


                Get[FacebookAuthentication.Configuration.FacebookLogoutPath] = x =>
                                   {
                                      //Logout of facebook and then redirection to logout of the app.
                                      //this way remove coupling of any application authentication storage provider (ie the Forms cookies:)).
                                      //Even if we wanted to use a configurable interface to the pre-logout of the app when using forms cookies there was not easy way to clear the authentication so this seems a good option.
                                      return this.LogoutFromFacebookAndRedirect(FacebookAuthentication.Configuration.ApplicationLogoutPath);
                                  };

            }
        }
    }
}