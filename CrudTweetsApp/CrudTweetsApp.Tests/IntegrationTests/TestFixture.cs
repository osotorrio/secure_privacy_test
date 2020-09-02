using CrudTweetsApp.ServiceModel.Types;
using MongoDB.Driver;
using System;

namespace CrudTweetsApp.Tests.IntegrationTests
{
    public class TestFixture
    {
        private const string CollectionName = "tweets";
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Tweet> _collection;

        public TestFixture()
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("crudTweetsDB");
            _collection = _database.GetCollection<Tweet>(CollectionName);
            DropCollections();
        }

        public void DropCollections()
        {
            _database.DropCollection(CollectionName);
        }

        public void AddTweet(Tweet tweet)
        {
            _collection.InsertOne(tweet);
        }

        public Tweet GetTweetById(Guid id)
        {
            return _collection.Find(t => t.Id == id).SingleOrDefault();
        }
    }
}
