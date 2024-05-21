using System;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class SearchPlatformProfitLossParam
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ProductCode { get; set; }
    }
}
