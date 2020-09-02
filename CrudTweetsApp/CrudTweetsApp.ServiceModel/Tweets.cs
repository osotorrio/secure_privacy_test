using CrudTweetsApp.ServiceModel.Types;
using ServiceStack;
using System;
using System.Collections.Generic;

namespace CrudTweetsApp.ServiceModel
{
    [Route("/tweets/{Id}", "GET")]
    public class GetTweetById : IReturn<Tweet>
    {
        public Guid Id { get; set; }
    }

    [Route("/users/{nickname}/tweets", "GET")]
    public class GetTweetsByUser : IReturn<List<Tweet>>
    {
        public string NickName { get; set; }
    }

    [Route("/tweets", "POST")]
    public class CreateTweet : IReturn<Tweet>
    {
        public string Text { get; set; }

        public string UserNickname { get; set; }
    }

    [Route("/tweets/{Id}", "PUT")]
    public class UpdateTweet : IReturnVoid
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }

    [Route("/tweets/{Id}", "DELETE")]
    public class DeleteTweet : IReturnVoid
    {
        public Guid Id { get; set; }
    }
}
