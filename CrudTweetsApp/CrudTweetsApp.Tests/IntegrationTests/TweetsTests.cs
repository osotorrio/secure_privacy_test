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
    public class TweetsTests
    {
        const string BaseUri = "http://localhost:2000/";
        private readonly ServiceStackHost appHost;
        private TestFixture _fixture;

        class AppHost : AppSelfHostBase
        {
            public AppHost() : base(nameof(TweetsTests), typeof(TweetService).Assembly) { }

            public override void Configure(Container container)
            {
                container.AddScoped<ITweetRepository, TweetRepository>();
            }
        }

        public TweetsTests()
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
        public void Get_tweets_by_user_test()
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

            // Act
            var client = CreateClient();
            var tweets = client.Get(new GetTweetsByUser { NickName = "test_user" });


            // Assert
            Assert.That(tweets.Count, Is.EqualTo(2));
            Assert.That(tweets.First().Text, Is.EqualTo("one tweet"));
            Assert.That(tweets.Last().Text, Is.EqualTo("another tweet"));
        }

        [Test]
        public void Get_tweet_by_id_test()
        {
            // Arrange
            _fixture = new TestFixture();

            var tweetId = Guid.NewGuid();

            _fixture.AddTweet(new Tweet
            {
                Id = tweetId,
                Text = "one tweet",
                User = new User { Nickname = "test_user" }
            });

            // Act
            var client = CreateClient();
            var tweet = client.Get(new GetTweetById { Id = tweetId });

            // Assert
            Assert.That(tweet.Text, Is.EqualTo("one tweet"));
            Assert.That(tweet.User.Nickname, Is.EqualTo("test_user"));
        }

        [Test]
        public void Create_tweet_test()
        {
            // Arrange
            _fixture = new TestFixture();

            var client = CreateClient();
            var tweetReponse = client.Post(new CreateTweet 
            {
                Text = "some tweet",
                UserNickname = "test_user"
            });

            // Act
            var tweetDatabase = _fixture.GetTweetById(tweetReponse.Id);


            // Assert
            Assert.That(tweetReponse.Id, Is.EqualTo(tweetDatabase.Id));
            Assert.That(tweetReponse.Text, Is.EqualTo(tweetDatabase.Text));
            Assert.That(tweetReponse.User.Nickname, Is.EqualTo(tweetDatabase.User.Nickname));
        }

        [Test]
        public void Update_tweet_test()
        {
            // Arrange
            _fixture = new TestFixture();

            var tweetId = Guid.NewGuid();

            _fixture.AddTweet(new Tweet
            {
                Id = tweetId,
                Text = "tweet before update",
                User = new User { Nickname = "test_user" }
            });

            // Act
            var client = CreateClient();
            client.Put(new UpdateTweet
            {
                Id = tweetId,
                Text = "tweet after update"
            });

            // Assert
            var tweetDatabase = _fixture.GetTweetById(tweetId);

            Assert.That(tweetDatabase.Id, Is.EqualTo(tweetId));
            Assert.That(tweetDatabase.Text, Is.EqualTo("tweet after update"));
            Assert.That(tweetDatabase.User.Nickname, Is.EqualTo("test_user"));
        }

        [Test]
        public void Delete_tweet_test()
        {
            // Arrange
            _fixture = new TestFixture();

            var tweetId = Guid.NewGuid();

            _fixture.AddTweet(new Tweet
            {
                Id = tweetId,
                Text = "tweet to be deleted",
                User = new User { Nickname = "test_user" }
            });

            // Act
            var client = CreateClient();
            client.Delete(new DeleteTweet
            {
                Id = tweetId
            });

            // Assert
            var tweetDatabase = _fixture.GetTweetById(tweetId);
            Assert.That(tweetDatabase, Is.Null);
        }
    }
}