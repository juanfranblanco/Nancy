using Nancy.Authentication.Facebook.Modules;

namespace Nancy.Authentication.Facebook.FacebookExtensions
{
    public static class ModuleExtensions
    {
        public static void RequiresFacebookLoggedIn(this NancyModule module)
        {
            module.Before.AddItemToEndOfPipeline(FacebookAuthentication.RedirectToFacebookLoginAndResetAuthenticationWhenNotAuthenticatedByFacebook) ;
        }

        public static Response RedirectToFacebookLoginUrl(this NancyModule module)
        {
            return FacebookAuthentication.RedirectToFacebookLoginUrl(module.Context);
        }

        public static Response LoginIntoApplicationWithFacebookOAthResponse(this NancyModule module, string pathToRedirectOnAutheticationFailure)
        {
            return FacebookAuthentication.LoginIntoApplicationWithFacebookOAthResponse(module.Context, pathToRedirectOnAutheticationFailure);
        }

        public static Response LogoutFromFacebookAndRedirect(this NancyModule module, string path)
        {
            return FacebookAuthentication.LogoutFromFacebookAndRedirect(module.Context, path);
        }
    }

}
