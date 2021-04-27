using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditTwitterBot.Features.RedditScrape
{
    public class RedditPost
    {
        public string Title;
        public string Subreddit;
        public string User;
        public string Url;
        public string Comments;
        public string Votes;
        public string TimeSincePost; //hours

        public override string ToString()
        {
            return $"\"{Title}\" from {Subreddit}, posted by {User} {TimeSincePost} hours ago. {Votes} upvotes, {Comments} comments. Url: {Url}";
        }
    }
}
