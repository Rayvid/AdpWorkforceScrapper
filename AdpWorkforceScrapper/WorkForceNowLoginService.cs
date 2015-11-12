using System.Net;

namespace AdpWorkforceScrapper
{
    public class WorkForceNowLoginService : ILoginService
    {
        public string Login(string userName)
        {
            var request = WebRequest.Create("https://workforcenow.adp.com/jobs/apply/posting.html?client=" + userName);
            request.Method = "GET";

            var httpRequest = request as HttpWebRequest;
            httpRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            httpRequest.KeepAlive = true;
            httpRequest.AllowAutoRedirect = false;

            string token = null;
            using (var response = request.GetResponse())
            {
                foreach (var c1 in response.Headers["Set-Cookie"].Split(';'))
                {
                    var parts = c1.Split('=');
                    if (parts[0] == "JWFNID")
                    {
                        token = parts[1];
                        break;
                    }
                }
            }

            request = WebRequest.Create("https://workforcenow.adp.com/jobs/apply/posting.html?client=" + userName);
            request.Method = "GET";

            httpRequest = request as HttpWebRequest;
            httpRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            httpRequest.KeepAlive = true;
            httpRequest.AllowAutoRedirect = false;
            if (httpRequest.CookieContainer == null)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }
            httpRequest.CookieContainer.Add(new Cookie("ADPPORTAL", "WFNPortal^App^PORTALWFN58", "/", ".adp.com"));
            httpRequest.CookieContainer.Add(new Cookie("ADPLangLocaleCookie", "en_US", "/", ".adp.com"));
            httpRequest.CookieContainer.Add(new Cookie("JWFNID", token, "/", ".adp.com"));

            using (var response = request.GetResponse())
            {
                var httpResponse = response as HttpWebResponse;
                token = httpResponse.Cookies["JWFNID"].Value;
            }

            request = WebRequest.Create("https://workforcenow.adp.com/jobs/apply/common/careercenter.faces?client=" + userName + "&op=0&locale=en_US&mode=LIVE&access=E&A=N");
            request.Method = "GET";

            httpRequest = request as HttpWebRequest;
            httpRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            httpRequest.KeepAlive = true;
            httpRequest.AllowAutoRedirect = false;
            if (httpRequest.CookieContainer == null)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }
            httpRequest.CookieContainer.Add(new Cookie("ADPPORTAL", "WFNPortal^App^PORTALWFN58", "/", ".adp.com"));
            httpRequest.CookieContainer.Add(new Cookie("ADPLangLocaleCookie", "en_US", "/", ".adp.com"));
            httpRequest.CookieContainer.Add(new Cookie("JWFNID", token, "/", ".adp.com"));

            using (var response = request.GetResponse())
            { }

            return token;
        }
    }
}
