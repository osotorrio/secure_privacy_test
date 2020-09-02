using System;

namespace CrudTweetsApp.ServiceModel.Types
{
    public class Tweet
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public User User { get; set; }
    }
}
