using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.PG
{
    public class PGUserBaseRequestModel
    {
        public string player_name { get; set; }
    }

    public class PGRegisterRequestModel : PGUserBaseRequestModel
    {
        public string nickname { get; set; }
        public string currency { get; set; }
    }

    public class PGTransferRequestModel : PGUserBaseRequestModel
    {
        public string amount { get; set; }
        public string currency { get; set; }
        public string transfer_reference { get; set; }
    }

    public class PGCheckTransferRequestModel : PGUserBaseRequestModel
    {
        public string transfer_reference { get; set; }
    }

    public class PGBetLogRequestModel
    {
        public int count { get; set; }
        public int bet_type { get; set; }
        public long row_version { get; set; }
    }

    public class PGLaunchGameRequestModel
    {
        public string Acc { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EnvCode { get; set; }
        public string Key { get; set; }
    }
}
