using System.Dynamic;
using Facebook;
using FakeItEasy;
using Nancy.Authentication.Facebook;
using Nancy.Authentication.Facebook.Repository;
using Xunit;
using Nancy.Tests;

namespace Nancy.Authentication.Forms.Tests
{

    public class FacebookClientServiceTestFixture
    {
        private FacebookClientService facebookClientService;
        private IApplicationAuthenticator applicationAuthenticator;
        private IFacebookCurrentAuthenticatedUserCache facebookCurrentAuthenticatedUserCache;
        private IFacebookAPIFactory facebookApiFactory;

        public FacebookClientServiceTestFixture()
        {
            applicationAuthenticator = A.Fake<IApplicationAuthenticator>();
            facebookCurrentAuthenticatedUserCache = A.Fake<IFacebookCurrentAuthenticatedUserCache>();
            facebookApiFactory = A.Fake<IFacebookAPIFactory>();
            facebookClientService = new FacebookClientService(applicationAuthenticator, facebookCurrentAuthenticatedUserCache,
                                                              facebookApiFactory);
        }

        [Fact]
        public void Should_return_a_new_facebook_client_using_a_facebook_id()
        {
            var facebookId = 1;
            var accessCode = "123456";
            var facebookClient = A.Fake<FacebookClient>();

            A.CallTo(() => facebookCurrentAuthenticatedUserCache.GetAccessToken(facebookId)).Returns(accessCode);
            A.CallTo(() => facebookApiFactory.CreateNewFacebookClient(accessCode)).Returns(facebookClient);

            var result = facebookClientService.GetFacebookClient(facebookId);

            A.CallTo(() => facebookCurrentAuthenticatedUserCache.GetAccessToken(facebookId)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => facebookApiFactory.CreateNewFacebookClient(accessCode)).MustHaveHappened(Repeated.Exactly.Once);


            result.ShouldBeSameAs(facebookClient);
        }

        [Fact]
        public void Should_return_null_facebook_client_when_facebook_id_is_null()
        {
            long? facebookId = null;
            var result = facebookClientService.GetFacebookClient(facebookId);
            result.ShouldBeNull();
        }


        [Fact]
        public void Should_return_a_new_facebook_client_using_the_nancy_context()
        {
            var facebookId = 1;
            var accessCode = "123456";
            var facebookClient = A.Fake<FacebookClient>();
            var nancyContext = new NancyContext();

            A.CallTo(() => applicationAuthenticator.GetFacebookId(nancyContext)).Returns(facebookId);
            A.CallTo(() => facebookCurrentAuthenticatedUserCache.GetAccessToken(facebookId)).Returns(accessCode);
            A.CallTo(() => facebookApiFactory.CreateNewFacebookClient(accessCode)).Returns(facebookClient);
            
            var result = facebookClientService.GetFacebookClient(nancyContext);

            A.CallTo(() => applicationAuthenticator.GetFacebookId(nancyContext)).MustHaveHappened(Repeated.Exactly.Once);
          
            result.ShouldBeSameAs(facebookClient);
        }


        [Fact]
        public void Should_return_a_facebook_me_using_the_facebook_access_token()
        {
            var accessCode = "123456";
            var facebookClient = A.Fake<FacebookClient>();
            dynamic me = new ExpandoObject();
            me.name = "Nancy";
            
            A.CallTo(() => facebookApiFactory.CreateNewFacebookClient(accessCode)).Returns(facebookClient);
            A.CallTo(() => facebookClient.Get("me")).Returns((ExpandoObject)me);

            var result = facebookClientService.GetFacebookMe(accessCode);

            A.CallTo(() => facebookApiFactory.CreateNewFacebookClient(accessCode)).MustHaveHappened(Repeated.Exactly.Once);

            Assert.Same(me, result);
        }

        [Fact]
        public void Should_return_a_facebook_me_using_the_nancy_context()
        {
            var facebookId = 1;
            var accessCode = "123456";
            var facebookClient = A.Fake<FacebookClient>();
            var nancyContext = new NancyContext();
            dynamic me = new ExpandoObject();
            me.name = "Nancy";

            A.CallTo(() => applicationAuthenticator.GetFacebookId(nancyContext)).Returns(facebookId);
            A.CallTo(() => facebookCurrentAuthenticatedUserCache.GetAccessToken(facebookId)).Returns(accessCode);
            A.CallTo(() => facebookApiFactory.CreateNewFacebookClient(accessCode)).Returns(facebookClient);
            A.CallTo(() => facebookClient.Get("me")).Returns((ExpandoObject)me);

            var result = facebookClientService.GetFacebookMe(nancyContext);

            Assert.Same(me, result);
            
        }


        [Fact]
        public void Should_return_null_when_getting_facebook_me_and_could_not_create_a_facebook_client()
        {
            long? facebookId = null;
            var nancyContext = new NancyContext();
            A.CallTo(() => applicationAuthenticator.GetFacebookId(nancyContext)).Returns(facebookId);
           
            var result = facebookClientService.GetFacebookMe(nancyContext);

            ((object) result).ShouldBeNull();

        }
        
    }
}
