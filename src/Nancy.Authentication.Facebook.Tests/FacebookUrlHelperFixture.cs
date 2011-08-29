using Nancy.Tests;
using Nancy.Tests.Fakes;
using Xunit;

namespace Nancy.Authentication.Facebook.Tests
{
    public class FacebookUrlHelperFixture
    {
        private FacebookUrlHelper urlHelper;
        private FacebookAuthenticationConfiguration config;

        public FacebookUrlHelperFixture()
        {
            config = new FacebookAuthenticationConfiguration()
                         {
                             ApplicationLogoutPath = "logoutApp",
                             ApplicationBasePath = "http://127.0.0.1",
                             FacebookLoginPath = "/loginFacebook",
                             FacebookOAthResponsePath = "/oAthFacebook",
                             FacebookLogoutPath = "/logoutFacebook"

                         };

            this.urlHelper = new FacebookUrlHelper(
                config
                );
        }


        [Fact]
        public void Should_return_the_oath_absolute_using_the_configured_base_path_and_facebook_oath_response_path()
        {
            //Given
            config.ApplicationBasePath = "http://127.0.0.1/";
            config.FacebookOAthResponsePath = "/OAuth";
            //When
            var result = urlHelper.GetOathAbsoluteUrl();
            //Then
            result.ShouldEqual("http://127.0.0.1/OAuth");
        }

        [Fact]
        public void Should_return_the_expanded_path_using_the_configured_application_base_path()
        {
            //Given
            config.ApplicationBasePath = "http://127.0.0.1";
            //When
            var result = urlHelper.ExpandPath("test/test1");
            //Then
            result.ShouldEqual("http://127.0.0.1/test/test1");
        }

        [Fact]
        public void Should_ensure_url_has_got_the_right_slashes_when_expanding_paths()
        {
            //Given (a base path with no trailing forward slash)
            config.ApplicationBasePath = "http://127.0.0.1";

            //a path with no starting forward slash
            var path = "test/test1";

            //When
            var result = urlHelper.ExpandPath(path);

            //Then
            result.ShouldEqual("http://127.0.0.1/test/test1");
        }


        [Fact]
        public void Should_return_the_absolute_url_of_a_request_with_the_configured_base_application_path()
        {
            //Given
            config.ApplicationBasePath = "http://127.0.0.1";
            var requestPath = "test/test1";
            var context = new NancyContext();
            context.Request = new FakeRequest("GET", requestPath);

            //When
            var result = urlHelper.GetRequestAbsoluteUrl(context);
            //Then
            result.ShouldEqual("http://127.0.0.1/test/test1");
        }

    }
}