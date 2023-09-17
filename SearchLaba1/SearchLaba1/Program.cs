using HtmlAgilityPack;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace SearchLaba1
{
    internal class Program
    {

        public class SearchSite
        {
            public string Url { get; set; }
            public string OutputFilePath { get; set; }

            public SearchSite(string url, string outputFilePath)
            {
                this.Url = url;
                this.OutputFilePath = outputFilePath;
            }

            public async Task<string> searchForNewsAsync()
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage responseMessage = client.GetAsync(Url).Result;

                        if (responseMessage.IsSuccessStatusCode)
                        {
                            string html = await responseMessage.Content.ReadAsStringAsync();
                            HashSet<string> uniqueURLs = new HashSet<string>();

                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(html);

                            HtmlNodeCollection links = doc.DocumentNode.SelectNodes("//a[@href]");

                            if (links != null)
                            {
                                foreach (var link in links)
                                {
                                    string href = link.GetAttributeValue("href", "");

                                    if (Regex.IsMatch(href, "novini", RegexOptions.IgnoreCase))
                                    {
                                        if (!href.Contains("category/novini"))
                                        {
                                            uniqueURLs.Add(href);
                                        }
                                    }
                                }
                            }
                            foreach (var item in uniqueURLs)
                            {
                                Console.WriteLine($"URL : {item}");
                            }
                            File.WriteAllLines(OutputFilePath, uniqueURLs);
                            Console.WriteLine($"URLs was written up in file");
                            return "true";
                        }
                        else
                        {
                            Console.WriteLine($"Error: {responseMessage.StatusCode}");
                            return "false";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return "false";
                    }
                }

            }

        }

        static async Task Main(string[] args)
        {
            string url = "http://ccs.nau.edu.ua/";
            string outputFilePath = "output.txt";

            SearchSite searchSite = new SearchSite(url, outputFilePath);

            Console.WriteLine(searchSite.searchForNewsAsync().Result);
        }
    }
}