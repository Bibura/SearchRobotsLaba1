using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    internal class Program
    {

        public class SearchNAU
        {
            private string Url { get; set; }
            private int i = 479;

            public SearchNAU(string url)
            {
                Url = url;
            }

            public async Task<string> SearchAsync()
            {

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        while (i != 485)
                        {
                            HttpResponseMessage responseMessage = client.GetAsync(Url + i).Result;
                            if (responseMessage.IsSuccessStatusCode)
                            {
                                string html = await responseMessage.Content.ReadAsStringAsync();
                                var doc = new HtmlDocument();
                                doc.LoadHtml(html);
                                var textDivs = doc.DocumentNode.SelectNodes("//div[contains(@class, 'text')]");

                                if (textDivs != null)
                                {
                                    foreach (var div in textDivs)
                                    {
                                        if (div.InnerText.Contains("Володимир Артамонов"))
                                        {
                                            Console.WriteLine($"FOUND MATCH : {div.InnerText}");
                                        }
                                    }
                                }

                            }
                            i++;
                        }
                        return "Ended successfully";
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"HTTP error: {ex.Message}");
                        return "Error";
                    }
                }
            }


        }
            static async Task Main(string[] args)
        {
            string url = "https://nau.edu.ua/ua/news/?page=";

            SearchNAU searchNAU = new SearchNAU(url);
            Console.WriteLine(searchNAU.SearchAsync().Result);

        }
    }
}