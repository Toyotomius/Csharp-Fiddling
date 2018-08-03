using System;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using RedditSharp.Things;
using RedditSharp.Utils;
using Newtonsoft.Json.Linq;
using RedditSharp;
using System.Security.Authentication;
using System.Security;
using Newtonsoft.Json;


namespace RedditBot1
{
    class Program
    {
        static void Main(string[] args)
        {

            DateTime start_time = DateTime.UtcNow;

            while (true)
            {
                start_time = DateTime.UtcNow;
                start:
                
                try
                {
                    var reddit = new Reddit();
                    var user = reddit.LogIn("Username", "Password");
                    var subreddit = reddit.GetSubreddit("/r/HooksPlays");

                    foreach (var post in subreddit.Comments.Take(25))
                    {
                        if (post.Author == "devnull_the_cat")
                        {

                            post.Upvote();
                        }

                    }
                    
                    foreach (var c in subreddit.New.Take(25))
                    {
                        if (c.AuthorName == "Hooks017" && c.CreatedUTC > start_time)
                        {
                            var reply = c.Comment(
                                string.Format(
                                    "This cat guy knows what he's talking about! He deserves an upvote!"));
                            Console.WriteLine($"{DateTime.Now} - New Post!");
                        }
                    }

                    
                }
                catch (RateLimitException e)
                {
                    Console.WriteLine("Rate Limit Hit By RedditSharp Exception");
                    start_time = DateTime.UtcNow;
                    System.Threading.Thread.Sleep(600000);
                    goto start;
                }
                catch (NullReferenceException e1)
                {
                    
                    Console.WriteLine("Rate Limit Hit By My Own Nonsense");
                    start_time = DateTime.UtcNow;
                    System.Threading.Thread.Sleep(600000);
                    goto start;

                }
                finally
                {
                }
                
                
            }
        }
    }
}

