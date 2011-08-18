using System;
using Nancy.Authentication.Forms;

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

                                         return FacebookAuthentication.RedirectToFaceobookLoginUrl(Context);
                                     };

                Get[FacebookAuthentication.Configuration.OAthPath] = x =>
                                    {
                                        string code = Context.Request.Query.code;
                                        if(FacebookAuthentication.IsOAthResultSuccess(Context))
                                        {
                                                //Assign a temporary GUID to identify the user via cookies, we follow Nancy Forms Authentication to prevent storing facebook ids or tokens in cookies.
                                                //What if I want to store users using the Guid? I would like to get the Guid that match the UserId no?
                                                var userId = Guid.NewGuid();
                                                FacebookAuthentication.AddAuthenticatedUserToCache(code, userId);
                                                return this.LoginAndRedirect(userId);
                                        }
                                        //Depends on forms Authentication it would be nice remove the dependency
                                        return this.LogoutAndRedirect("~/");
                                    };


                Get[FacebookAuthentication.Configuration.LogoutPath] = x =>
                                      {
                                          //TODO: Logout of facebook and the application
                                          //Depends on forms authentication 
                                          return this.LogoutAndRedirect("~/");

                                      };
            }
        }
    }
}