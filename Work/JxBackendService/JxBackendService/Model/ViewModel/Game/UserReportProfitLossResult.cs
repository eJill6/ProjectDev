using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Game
{
    public class UserReportProfitLossResult
    {
        public UserReportProfitlossType1Table0Result Type1Table0Result { get; set; }
        public List<UserReportProfitlossType1Table1Result> Type1Table1Results { get; set; }

        public UserReportProfitlossType2Table0Result Type2Table0Result { get; set; }

        public List<UserReportProfitlossType2Result> Type2Table1Results { get; set; }
        public List<UserReportProfitlossType2Result> Type2Table2Results { get; set; }
        public List<UserReportProfitlossType2Result> Type2Table3Results { get; set; }
    }

    public class UserReportProfitlossType1Table0Result
    {
        public decimal TotalTZProfitLossMoney { get; set; }
        public decimal TotalZKYProfitLossMoney { get; set; }
    }

    public class UserReportProfitlossType1Table1Result
    {
        public DateTime RecordDate { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
        public int ParentID { get; set; }
        public string CommissionType { get; set; }
        public decimal TZProfitLossMoney { get; set; }
        public decimal KYProfitLossMoney { get; set; }
        public decimal FDProfitLossMoney { get; set; }
        public decimal ZKYProfitLossMoney { get; set; }
        public DateTime CreateDate { get; set; }
    }


    public class UserReportProfitlossType2Table0Result
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int RegCount { get; set; }
        public int DayLoginCount { get; set; }
        public int TZCount { get; set; }
        public decimal TotalTZAmount { get; set; }
        public decimal TotalZKYAmount { get; set; }
        public int DepositCount { get; set; }
        public decimal TotalDepositAmount { get; set; }
        public int WithdrawalCount { get; set; }
        public decimal TotalWithdrawalAmount { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class UserReportProfitlossType2Result
    {
        public int TID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int RankID { get; set; }
        public int ParentID { get; set; }
        public int FatherParentID { get; set; }
        public int RankType { get; set; }
        public decimal RankAmount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
