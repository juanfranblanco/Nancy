using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;
using Nancy.Authentication.Facebook.AuthorisationPipelines;
using Nancy.Security;

namespace Nancy.Authentication.Facebook.SecurityExtensions
{
    public static class ModuleExtensions
    {
        public static void RequiresFacebookLoggedIn(this NancyModule module)
        {
            module.Before.AddItemToEndOfPipeline(FacebookAuthenticatedCheckPipeline.CheckUserIsNothAuthorisedByFacebookAnymore);
        }

    }
}
