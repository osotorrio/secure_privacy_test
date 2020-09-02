using CrudTweetsApp.ServiceModel;
using CrudTweetsApp.ServiceModel.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CrudTweetsApp.Repositories
{
    public interface ITweetRepository
    {
        void Create(Tweet tweet);

        void Delete(Guid id);

        List<Tweet> Query(Expression<Func<Tweet, string>> field, string value);

        Tweet GetById(Guid id);

        void Update(Tweet tweet);

        IEnumerable<CountTweetsByUserResponse> CountTweetsByUser();
    }

    public class TweetRepository : ITweetRepository
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Tweet> _collection;

        public TweetRepository()
        {
            _client = new MongoClient();
            _database = _client.GetDatabase("crudTweetsDB");
            _collection = _database.GetCollection<Tweet>("tweets");
        }

        public void Create(Tweet tweet)
        {
            _collection.InsertOne(tweet);
        }

        public void Delete(Guid id)
        {
            var filter = Builders<Tweet>.Filter.Eq(t => t.Id, id);
            _collection.DeleteOne(filter);
        }

        public List<Tweet> Query(Expression<Func<Tweet, string>> field, string value)
        {
            var filter = Builders<Tweet>.Filter.Eq(field, value);
            return _collection.Find(filter).ToList();
        }

        public Tweet GetById(Guid id)
        {
            return _collection.Find(t => t.Id == id).SingleOrDefault();
        }

        public void Update(Tweet tweet)
        {
            var filter = Builders<Tweet>.Filter.Eq(t => t.Id, tweet.Id);
            var update = Builders<Tweet>.Update.Set(t => t.Text, tweet.Text);
            _collection.UpdateOne(filter, update);
        }

        public IEnumerable<CountTweetsByUserResponse> CountTweetsByUser()
        {
            var group = new BsonDocument 
            {
                { "_id", "$User.Nickname" },
                { "count", new BsonDocument("$sum", 1) }
            };

            var sort = new BsonDocument 
            { 
                { "count", -1 } 
            };

            var aggregate = _collection.Aggregate().Group(group).Sort(sort);


            BsonClassMap.RegisterClassMap<CountTweetsByUserResponse>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapProperty(x => x.UserNickname).SetElementName("_id");
                map.MapProperty(x => x.NumberOfTweets).SetElementName("count");
            });

            return aggregate.ToList().Select(doc => BsonSerializer.Deserialize<CountTweetsByUserResponse>(doc));
        }
    }
}
