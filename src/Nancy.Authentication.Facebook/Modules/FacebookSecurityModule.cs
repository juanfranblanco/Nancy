using System;
using Nancy.Authentication.Facebook.Helpers;
using Nancy.Authentication.Forms;
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
                                            //Assign a temporary GUID to identify the user, the application login can check the database or whatever store if necessary
                                            //for the facebookId and align guids
                                            var temporaryUserId = Guid.NewGuid();
                                            FacebookAuthentication.AddAuthenticatedUserToCache(code, temporaryUserId);
                                            //Doing a redirect to the application login, this way we remove coupling with any application internal storage authentication (ie Forms cookie store).
                                            //Problem is that we have to pass the guid and redirect.. maybe a configurable interface will be better
                                            return Context.GetRedirect("/loginAppAfterFacebookOAth/" + GuidEncoder.Encode(temporaryUserId));
                                            //return this.LoginAndRedirect(userId);
                                        }
                                        //This should be configurable...  Logout of App (redirect to Application Logout)
                                        return Context.GetRedirect("/logoutApp");
                                        //return this.LogoutAndRedirect("~/");
                                    };


                Get[FacebookAuthentication.Configuration.LogoutPath] = x =>
                                                                           {
                                                                               //Logout of facebook and then redirection to logout of the app.
                                                                              //Note: this should be configurable.
                                                                              //this way remove coupling of any application authentication storage provider (ie the Forms cookies:)).
                                                                              //Even if we wanted to use a configurable interface to the pre-logout of the app when using forms cookies there was not easy way to clear the authentication so this seems a good option.
                                 
                                                                               return
                                                                                   FacebookAuthentication.
                                                                                       LogoutAndRedirect(Context, "/logoutApp");
                                                                           };

                


            }
        }
    }
}