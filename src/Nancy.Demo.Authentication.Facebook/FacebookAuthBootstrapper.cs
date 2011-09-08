using Nancy.Authentication.Facebook;
using Nancy.Authentication.Facebook.Modules;
using Nancy.Authentication.Facebook.Repository;
using Nancy.Authentication.Forms;

namespace Nancy.Demo.Authentication.Facebook
{
    public class FacebookAuthBootstrapper : DefaultNancyBootstrapper
    {
        protected override void InitialiseInternal(TinyIoC.TinyIoCContainer container)
        {
            base.InitialiseInternal(container);

            var facebookAuthConfiguration =
                new FacebookAuthenticationConfiguration()
                    {
                        ApplicationBasePath = "http://localhost:81",
                        FacebookCurrentAuthenticatedUserCache = container.Resolve<IFacebookCurrentAuthenticatedUserCache>(),
                        ApplicationAuthenticator = container.Resolve<IApplicationAuthenticator>(),
                        FacebookLoginPath = "/LoginFacebook",
                        FacebookOAthResponsePath = "/OathFacebook",
                        ApplicationLogoutPath = "/logoutApp",
                        FacebookExtendedPermissions =  "user_about_me,publish_stream,offline_access"
                    };

            FacebookAuthentication.Enable(facebookAuthConfiguration);

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                    {
                        RedirectUrl = facebookAuthConfiguration.FacebookLoginPath,
                        UserMapper = container.Resolve<IUserMapper>(),
                    };

            FormsAuthentication.Enable(this, formsAuthConfiguration);

        }
    }
}