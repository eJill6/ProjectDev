using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Model.Attributes.Security;
using Newtonsoft.Json;
using System;

namespace JxBackendService.Model.MiseLive.Request
{
    public class BaseMiseLiveRequest
    {
        public BaseMiseLiveRequest()
        {
        }

        [JsonIgnore]
        [MiseLiveSign]
        public long Ts { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        [JsonIgnore]
        public string Sign { get; set; }

        protected string GetDefaultApp() => "amd";
    }

    public class BaseMiseLiveSaltRequest : BaseMiseLiveRequest
    {
        public BaseMiseLiveSaltRequest()
        {
        }

        [JsonIgnore]
        [MiseLiveSign(SortNo = 99)]
        public string Salt { get; set; }
    }

    public class BaseMiseUserSaltRequest : BaseMiseLiveSaltRequest
    {
        public BaseMiseUserSaltRequest()
        {
        }

        [MiseLiveSign]
        public int UserId { get; set; }
    }
}