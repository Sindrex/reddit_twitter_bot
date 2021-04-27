using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedditTwitterBot.Features.RedditScrape
{
    public static class RedditScrape
    {
        //url makeup: https://www.reddit.com/r/{subreddit}/comments/{user}/{abr.title}/

        private static string TitleSearch = "class=\"_eYtD2XCVieq6emjKBH3m\">";
        private static string UrlSearch = "https://www.reddit.com/r/"; //"href=\"https://www.reddit.com/r/\"";
        private static string TimeSincePostSearch = "hours ago";
        private static string VotesSearch = "class=\"_1rZYMD_4xY3gRcSS3p8ODO _25IkBM0rRUqWX5ZojEMAFQ\"";
        private static string CommentsSearch = "class=\"FHCV02u6Cp2zYL0fhQPsO\">";

        static readonly HttpClient client = new HttpClient();

        public static async Task<List<RedditPost>> Scrape(string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            List<RedditPost> redditPosts = new List<RedditPost>();

            string[] spliturl = responseBody.Split(UrlSearch);
            foreach(string post in spliturl)
            {
                if (!post.Contains("/comments/")) continue;

                RedditPost redditPost = new RedditPost();

                //a post +
                string[] values = post.Split("/", 5); //nextfuckinglevel/comments/myvhfh/man_makes_amazing_auto_domino_machine/
                if (values.Length < 5) continue;
                redditPost.Subreddit = $"/r/{ values[0]}";
                redditPost.User = $"/u/{values[2]}";
                redditPost.Url = $"https://www.reddit.com/r/{values[0]}/{values[1]}/{values[2]}/{values[3]}/";

                string rest = values[4];
                string[] restSplit = rest.Split(TimeSincePostSearch, 2);
                if (restSplit.Length < 2) continue;
                redditPost.TimeSincePost = restSplit[0].Split(">")[1].Trim();

                string[] titleSplitClass = restSplit[1].Split(TitleSearch, 2);
                if (titleSplitClass.Length < 2) continue;
                string[] titleSplit = titleSplitClass[1].Split("</h3>", 2);
                redditPost.Title = titleSplit[0];

                string[] voteSplitClass = titleSplit[1].Split(VotesSearch, 2);
                if (voteSplitClass.Length < 2) continue;
                string[] voteSplit = voteSplitClass[1].Split("</", 2);
                if (voteSplit.Length < 2) continue;
                string[] voteSplit2 = voteSplit[0].Split(">", 2);
                if (voteSplit2.Length < 2) continue;
                redditPost.Votes = voteSplit2[1];

                string[] commentSplitClass = voteSplitClass[1].Split(CommentsSearch, 2);
                if (commentSplitClass.Length < 2) continue;
                string[] commentSplit = commentSplitClass[1].Split(" comments", 2);
                redditPost.Comments = commentSplit[0];

                redditPosts.Add(redditPost);
                //Console.WriteLine("Found post: " + redditPost);
            }

            return redditPosts;
        }
    }
}
