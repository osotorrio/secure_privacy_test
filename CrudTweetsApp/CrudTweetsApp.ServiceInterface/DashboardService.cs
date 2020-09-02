using CrudTweetsApp.Repositories;
using CrudTweetsApp.ServiceModel;
using ServiceStack;

namespace CrudTweetsApp.ServiceInterface
{
    public class DashboardService : Service
    {
        private ITweetRepository _tweetRepo;

        public DashboardService(ITweetRepository tweetRepository)
        {
            _tweetRepo = tweetRepository;
        }

        public object Get(CountTweetsByUser request) => _tweetRepo.CountTweetsByUser();
    }
}
