using Funq;
using ServiceStack;
using NUnit.Framework;
using CrudTweetsApp.ServiceInterface;
using CrudTweetsApp.ServiceModel;
using CrudTweetsApp.Repositories;
using CrudTweetsApp.ServiceModel.Types;
using System.Linq;
using System;

namespace CrudTweetsApp.Tests.IntegrationTests
{
    public class DashboardTests
    {
        const string BaseUri = "http://localhost:2000/";
        private readonly ServiceStackHost appHost;
        private TestFixture _fixture;

        class AppHost : AppSelfHostBase
        {
            public AppHost() : base(nameof(DashboardTests), typeof(DashboardService).Assembly) { }

            public override void Configure(Container container)
            {
                container.AddScoped<ITweetRepository, TweetRepository>();
            }
        }

        public DashboardTests()
        {
            appHost = new AppHost()
                .Init()
                .Start(BaseUri);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            appHost.Dispose();
        }

        [TearDown]
        public void CleanDatabaseAfterEachTest()
        {
            _fixture.DropCollections();
        }

        public IServiceClient CreateClient() => new JsonServiceClient(BaseUri);

        [Test]
        public void Count_number_of_tweets_by_user_test()
        {
            // Arrange
            _fixture = new TestFixture();

            _fixture.AddTweet(new Tweet
            {
                Id = Guid.NewGuid(),
                Text = "one tweet",
                User = new User { Nickname = "test_user" }
            });

            _fixture.AddTweet(new Tweet
            {
                Id = Guid.NewGuid(),
                Text = "another tweet",
                User = new User { Nickname = "test_user" }
            });

            _fixture.AddTweet(new Tweet
            {
                Id = Guid.NewGuid(),
                Text = "another tweet from another user",
                User = new User { Nickname = "another_test_user" }
            });

            // Act
            var client = CreateClient();
            var result = client.Get(new CountTweetsByUser());

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Find(x => x.UserNickname.Equals("test_user")).NumberOfTweets, Is.EqualTo(2));
            Assert.That(result.Find(x => x.UserNickname.Equals("another_test_user")).NumberOfTweets, Is.EqualTo(1));
        }
    }
}