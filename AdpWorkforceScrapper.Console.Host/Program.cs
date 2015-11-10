using System.Net;
using System.Net.Cache;
using Newtonsoft.Json;

namespace AdpWorkforceScrapper.Console.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest.DefaultCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

            var token = (new WorkForceNowLoginService()).Login("theeducat");
            foreach (var opening in (new WorkForceNowOpeningsListService("theeducat", token).FetchOpenings()))
            {
                new WorkForceNowOpeningService("theeducat", token).FetchOpeningDetails(opening);
                System.Console.WriteLine(JsonConvert.SerializeObject(opening, Formatting.Indented));
            }
        }
    }
}
