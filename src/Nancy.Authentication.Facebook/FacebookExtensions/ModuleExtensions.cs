using Nancy.Authentication.Facebook.Modules;

namespace Nancy.Authentication.Facebook.FacebookExtensions
{
    public static class ModuleExtensions
    {
        public static void RequiresFacebookLoggedIn(this NancyModule module)
        {
            module.Before.AddItemToEndOfPipeline(FacebookAuthentication.CheckUserIsNothAuthorisedByFacebookAnymore);
        }

    }
}
