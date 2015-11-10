using Newtonsoft.Json;

namespace AdpWorkforceScrapper
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Opening
    {
        [JsonProperty(PropertyName = "url")]
        public string Url;

        [JsonProperty(PropertyName = "jobName")]
        public string JobTitle;

        [JsonProperty(PropertyName = "requisitionOid")]
        public string Id;

        [JsonProperty(PropertyName = "postedOn")]
        public string PostDate;

        [JsonProperty(PropertyName = "description")]
        public string Description;
    }
}
