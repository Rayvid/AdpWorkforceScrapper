using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using Newtonsoft.Json;

namespace AdpWorkforceScrapper
{
    public class WorkForceNowOpeningsListService : IOpeningsListService
    {
        [JsonObject(MemberSerialization.OptIn)]
        private class OpeningsListContainer
        {
            [JsonProperty(PropertyName = "data")]
            public OpeningsList ListContainer;
        }
        [JsonObject(MemberSerialization.OptIn)]
        private class OpeningsList
        {
            [JsonProperty(PropertyName = "results")]
            public Opening[] List;
        }

        private readonly string _userName;
        private readonly string _token;

        public WorkForceNowOpeningsListService(string userName, string token)
        {
            _userName = userName;
            _token = token;
        }

        public IEnumerable<Opening> FetchOpenings()
        {
            var request = WebRequest.Create("https://workforcenow.adp.com/jobs/apply/metaservices/JobSearchService/legacySearch/en_US/E?client=" + _userName);
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";

            var httpRequest = request as HttpWebRequest;
            if (httpRequest.CookieContainer == null)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }
            httpRequest.CookieContainer.Add(new Cookie("JWFNID", _token, "/", ".adp.com"));
            httpRequest.CookieContainer.Add(new Cookie("ADPPORTAL", "WFNPortal^App^PORTALWFN58", "/", ".adp.com"));
            httpRequest.CookieContainer.Add(new Cookie("ADPLangLocaleCookie", "en_US", "/", ".adp.com"));
            httpRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            httpRequest.KeepAlive = true;

            var dataStream = request.GetRequestStream();
            var uft8Data =
                System.Text.Encoding.UTF8.GetBytes(
                    "{\"inputSearch\":\"\",\"startIndex\":1,\"lastIndex\":5000,\"sortBy\":\"POSTED_ON\",\"sortOrder\":\"DESC\",\"locationOidList\":[],\"employeeTypeList\":[],\"jobClassList\":[],\"postedType\":\"\",\"minSalary\":0,\"maxSalary\":1000000,\"acceptApplications\":1,\"acceptReferrals\":1}");
            dataStream.Write(uft8Data, 0, uft8Data.Length);
            dataStream.Close();

            using (var response = request.GetResponse())
            {
                var openings =
                    JsonConvert.DeserializeObject<OpeningsListContainer>(
                        new StreamReader(response.GetResponseStream()).ReadToEnd());

                return openings.ListContainer.List;
            }
        }
    }
}
