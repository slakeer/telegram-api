using System.Net;
using System.Text.RegularExpressions;
using WebApplication1.Models;

namespace WebApplication1.Clients;

class CryptoSlateNews
{
    public static async Task<Article> DisplayRandomArticle()
    {
        string url = "https://cryptoslate.com";

        WebClient client = new WebClient();
        string response = await client.DownloadStringTaskAsync(url);
        string pattern = "<article id=\"post-(.*?)\">.*?<a href=\"(.*?)\" title=\"(.*?)\"";
        MatchCollection matches = Regex.Matches(response, pattern);

        List<Article> articles = new List<Article>();

        if (matches.Count > 0)
        {
            foreach (Match match in matches)
            {
                string articleId = match.Groups[1].Value;
                string title = match.Groups[3].Value;
                string link = match.Groups[2].Value;

                Article article = new Article(articleId, title, link);
                articles.Add(article);
            }

            if (articles.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, articles.Count);
                Article randomArticle = articles[randomIndex];
                return randomArticle;
            }

            {
                Console.WriteLine("No articles available.");
            }
        }
        else
        {
            Console.WriteLine("No matches found.");
        }

        return null;
    }
}