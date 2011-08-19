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
                        BasePath = "http://localhost:81",
                        FacebookUserCache = container.Resolve<IFacebookUserCache>(),
                        ApplicationAuthenticator = container.Resolve<IApplicationAuthenticator>(),
                        LoginPath = "/LoginFacebook",
                        OAthPath = "/OathFacebook",
                        ApplicationLogoutPath = "/logoutApp"
                    };

            FacebookAuthentication.Enable(facebookAuthConfiguration);

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                    {
                        RedirectUrl = facebookAuthConfiguration.LoginPath,
                        UsernameMapper = container.Resolve<IUsernameMapper>(),
                    };

            FormsAuthentication.Enable(this, formsAuthConfiguration);

        }
    }
}