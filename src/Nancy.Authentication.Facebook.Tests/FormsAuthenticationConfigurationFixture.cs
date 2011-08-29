
namespace Nancy.Authentication.Facebook.Tests
{
    using FakeItEasy;
    using Repository;
    using Facebook;
    using Nancy.Tests;
    using Xunit;

    public class FacebookAuthenticationConfigurationFixture
    {
        private FacebookAuthenticationConfiguration config;

        public FacebookAuthenticationConfigurationFixture()
        {

            this.config = new FacebookAuthenticationConfiguration()
                              {
                                  ApplicationAuthenticator = A.Fake<IApplicationAuthenticator>(),
                                  ApplicationLogoutPath =  "logoutApp",
                                  ApplicationBasePath = "http://127.0.0.1",
                                  FacebookExtendedPermissions = "user_about_me",
                                  FacebookCurrentAuthenticatedUserCache = A.Fake<IFacebookCurrentAuthenticatedUserCache>(),
                                  FacebookLoginPath = "/loginFacebook",
                                  FacebookOAthResponsePath = "/oAthFacebook",
                                  FacebookLogoutPath = "/logoutFacebook"
                                  
                              };
        }

        [Fact]
        public void Should_be_valid_with_all_properties_set()
        {
            var result = config.IsValid;
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_not_be_valid_with_empty_application_logout_path()
        {
            config.ApplicationLogoutPath = "";

            var result = config.IsValid;

            result.ShouldBeFalse();
        }


        [Fact]
        public void Should_not_be_valid_with_empty_base_path()
        {
            config.ApplicationBasePath = "";

            var result = config.IsValid;

            result.ShouldBeFalse();
        }


        [Fact]
        public void Should_not_be_valid_with_empty_facebook_logout_path()
        {
            config.FacebookLogoutPath = "";

            var result = config.IsValid;

            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_not_be_valid_with_empty_oath_path()
        {
            config.FacebookOAthResponsePath = "";

            var result = config.IsValid;

            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_not_be_valid_with_empty_login_path()
        {
            config.FacebookLoginPath = "";

            var result = config.IsValid;

            result.ShouldBeFalse();
        }


        [Fact]
        public void Should_not_be_valid_with_null_application_authenticator()
        {
            config.ApplicationAuthenticator = null;

            var result = config.IsValid;

            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_not_be_valid_with_null_facebook_user_cache()
        {
            config.FacebookCurrentAuthenticatedUserCache = null;

            var result = config.IsValid;

            result.ShouldBeFalse();
        }

       
    }
}