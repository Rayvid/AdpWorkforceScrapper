﻿using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace AdpWorkforceScrapper
{
    public class WorkForceNowOpeningService : IOpeningService
    {
        [JsonObject(MemberSerialization.OptIn)]
        private class OpeningContainer
        {
            [JsonProperty(PropertyName = "data")]
            public Opening Opening;
        }

        private readonly string _userName;
        private readonly string _token;

        public WorkForceNowOpeningService(string userName, string token)
        {
            _userName = userName;
            _token = token;
        }

        public void FetchOpeningDetails(Opening opening)
        {
            var request = WebRequest.Create(
                "https://workforcenow.adp.com/jobs/apply/metaservices/careerCenter/jobDetails/E/en_US?requisitionOid=" +
                opening.Id + "&client=" + _userName);
            request.Method = "GET";

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

            using (var response = request.GetResponse())
            {
                var openingDetails = JsonConvert.DeserializeObject<OpeningContainer>(new StreamReader(response.GetResponseStream()).ReadToEnd()).Opening;

                opening.Url =
                    string.Format(
                        "https://workforcenow.adp.com/jobs/apply/posting.html?client=theeducat&jobId={0}&lang=en_US&source=CC3",
                        openingDetails.ExternalId);
                opening.PostDate = openingDetails.PostDate;
                opening.JobTitle = openingDetails.JobTitle;
                opening.Description = openingDetails.Description;
            }
        }
    }
}
