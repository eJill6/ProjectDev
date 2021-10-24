using Newtonsoft.Json;

namespace JxBackendService.Model.ViewModel
{
    public class IpSystemApiResult
    {
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "province")]
        public string Province { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "district")]
        public string District { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }
    }
}
