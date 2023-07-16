using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.EVO
{
    public class EVCheckExistResponseModel : EVBaseResponseWtihDataModel<CheckExistModel>
    { }

    public class CheckExistModel
    {
        public int Status { get; set; }

        public List<CheckExistAccountModel> Accounts { get; set; }
    }

    public class CheckExistAccountModel
    {
        public int ProductCode { get; set; }

        public string Account { get; set; }

        public float Balance { get; set; }
    }

    public class EVQueryOrderResponseModel : EVBaseResponseWtihDataModel<int>
    {
    }

    public class EVTransferResponseModel : EVQueryOrderResponseModel
    {
        public decimal? Balance { get; set; }
    }

    public class EVBalanceResponseModel : EVBaseResponseWtihDataModel<decimal>
    {
    }

    public class EVLunchGameResponseModel : EVBaseResponseWtihDataModel<string>
    {
    }

    public class EVBetLogModel
    {
        public string BetId { get; set; }

        public string MemberAccount { get; set; }

        public DateTime WagersTime { get; set; }

        public decimal BetAmount { get; set; }

        public decimal Payoff { get; set; }

        public decimal Commissionable { get; set; }

        public DateTime UpdateTime { get; set; }

        public EVEBRowDataBetLog RawData { get; set; }
    }

    public class EVBetLogResponseModel : EVBaseResponseWtihDataModel<List<EVBetLogModel>>
    {
        public int Page { get; set; }

        public int TotalPage { get; set; }

        public int PageSize { get; set; }

        public int Count { get; set; }
    }
}