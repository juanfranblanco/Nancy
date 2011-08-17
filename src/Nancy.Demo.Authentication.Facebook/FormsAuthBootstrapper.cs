using Nancy.Authentication.Forms;

namespace Nancy.Demo.Authentication.Facebook
{
    public class FormsAuthBootstrapper : DefaultNancyBootstrapper
    {
        protected override void InitialiseInternal(TinyIoC.TinyIoCContainer container)
        {
            base.InitialiseInternal(container);

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                    {
                        RedirectUrl = "~" + SecurityModule.LoginPath,
                        UsernameMapper = container.Resolve<IUsernameMapper>(),
                    };

            FormsAuthentication.Enable(this, formsAuthConfiguration);

        }
    }
}