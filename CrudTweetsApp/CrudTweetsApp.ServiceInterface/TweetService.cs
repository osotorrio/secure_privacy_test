using CrudTweetsApp.Repositories;
using CrudTweetsApp.ServiceModel;
using CrudTweetsApp.ServiceModel.Types;
using ServiceStack;
using System;

namespace CrudTweetsApp.ServiceInterface
{
    public class TweetService : Service
    {
        private ITweetRepository _tweetRepo;

        public TweetService(ITweetRepository tweetRepository)
        {
            _tweetRepo = tweetRepository;
        }

        public object Get(GetTweetsByUser request) => _tweetRepo.Query(t => t.User.Nickname, request.NickName);

        public object Get(GetTweetById request)
        {
            var tweet = _tweetRepo.GetById(request.Id);

            if (tweet == null)
                ThrowNotFound(request.Id);

            return tweet;
        }

        public object Post(CreateTweet request)
        {
            var tweet = new Tweet 
            {
                Id = Guid.NewGuid(),
                Text = request.Text,
                User = new User { Nickname = request.UserNickname }
            };

            _tweetRepo.Create(tweet);

            return tweet;
        }

        public void Put(UpdateTweet request)
        {
            var tweet = _tweetRepo.GetById(request.Id);

            if (tweet == null)
                ThrowNotFound(request.Id);

            tweet.Text = request.Text;
            _tweetRepo.Update(tweet);
        }

        public void Delete(DeleteTweet request) => _tweetRepo.Delete(request.Id);

        private static void ThrowNotFound(Guid id)
        {
            throw HttpError.NotFound($"Tweet '{id}' does not exist");
        }
    }
}
