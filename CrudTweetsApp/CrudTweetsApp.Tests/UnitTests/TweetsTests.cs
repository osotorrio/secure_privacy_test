using CrudTweetsApp.Repositories;
using CrudTweetsApp.ServiceInterface;
using CrudTweetsApp.ServiceModel;
using NSubstitute;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using System;

namespace CrudTweetsApp.Tests.UnitTests
{
    public class TweetsTests
    {
        private readonly ServiceStackHost appHost;
        private readonly ITweetRepository _fakeTweetRepository;
        public TweetsTests()
        {
            appHost = new BasicAppHost().Init();

            _fakeTweetRepository = Substitute.For<ITweetRepository>();
            appHost.Container.AddTransient(typeof(ITweetRepository), () => _fakeTweetRepository);
            appHost.Container.AddTransient<TweetService>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        [Test]
        public void GetTweetById_throws_exception_when_the_tweet_is_not_found_test()
        {
            // Arrange
            _fakeTweetRepository.GetById(Arg.Any<Guid>()).Returns(x =>  null);
            var service = appHost.Container.Resolve<TweetService>();

            // Act & Assert
            Assert.Throws<HttpError>(() => service.Get(new GetTweetById { Id = Guid.NewGuid() }));
        }

        [Test]
        public void UpdateTweet_throws_exception_when_the_tweet_is_not_found_test()
        {
            // Arrange
            _fakeTweetRepository.GetById(Arg.Any<Guid>()).Returns(x => null);
            var service = appHost.Container.Resolve<TweetService>();

            // Act & Assert
            Assert.Throws<HttpError>(() => service.Put(new UpdateTweet { Id = Guid.NewGuid() }));
        }
    }
}
