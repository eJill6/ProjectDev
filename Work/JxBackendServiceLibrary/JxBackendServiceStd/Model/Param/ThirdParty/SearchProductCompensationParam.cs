using System;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class SearchBaseCompensationParam
    {
        public string ProductCode { get; set; }
    }

    public class SearchProductCompensationParam : SearchBaseCompensationParam
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Type { get; set; }
    }

    public class SearchUserCompensationParam : SearchProductCompensationParam
    {
        public int UserID { get; set; }
    }
}