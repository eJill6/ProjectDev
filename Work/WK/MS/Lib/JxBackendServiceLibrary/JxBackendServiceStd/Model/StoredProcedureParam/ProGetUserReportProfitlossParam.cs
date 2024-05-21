using JxBackendService.Model.Attributes;
using System;

namespace JxBackendService.Model.StoredProcedureParam
{
    public class RequestUserReportProfitlossParam
    {
        public int UserID { get; set; }
        public int UserType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ProGetUserReportProfitlossParam : RequestUserReportProfitlossParam
    {
        [NVarcharColumnInfo(500)]
        public string CommissionTypeJson { get; set; }
    }
}
