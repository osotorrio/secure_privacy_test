using ServiceStack;
using System.Collections.Generic;

namespace CrudTweetsApp.ServiceModel
{
    [Route("/count/tweets/users", "GET")]
    public class CountTweetsByUser : IReturn<List<CountTweetsByUserResponse>>
    {
    }

    public class CountTweetsByUserResponse
    {
        public string UserNickname { get; set; }

        public int NumberOfTweets { get; set; }
    }
}
