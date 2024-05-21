using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.AllBet
{
    public class ABRegisterResponseModel : ABBaseResponseWithDataModel<ABUserBaseModel>
    {
    }

    public class ABLunchGameModel
    {
        public string GameLoginUrl { get; set; }
    }

    public class ABLunchGameResponseModel : ABBaseResponseWithDataModel<ABLunchGameModel>
    {
    }

    public class ABTransferResponseModel : ABBaseResponseModel
    { }

    public class CheckTransferModel
    {
        /// <summary> 转账前的玩家额度 </summary>
        public decimal? PlayerCreditBefore { get; set; }

        /// <summary> 转账后的玩家额度 </summary>
        public decimal? PlayerCreditAfter { get; set; }

        /// <summary> 转账前的代理额度 </summary>
        public decimal? AgentCreditBefore { get; set; }

        /// <summary> 转账后的代理额度 </summary>
        public decimal? AgentCreditAfter { get; set; }

        /// <summary> 转账的状态 </summary>
        public int TransferState { get; set; }

        /// <summary> 转账时间 </summary>
        public string TransferTime { get; set; }
    }

    public class ABCheckTransferResponseModel : ABBaseResponseWithDataModel<CheckTransferModel>
    {
    }

    public class ABBalanceModel : ABUserBaseModel
    {
        public string Amount { get; set; }

        public string Currency { get; set; }
    }

    public class ABBalanceDataModel
    {
        public int Count { get; set; }

        public List<ABBalanceModel> List { set; get; }
    }

    public class ABBalanceResponseModel : ABBaseResponseWithDataModel<ABBalanceDataModel>
    {
    }

    public class ABBetLogModel
    {
        public string StartDateTime { get; set; }

        public string EndDateTime { get; set; }

        public int Total { get; set; }

        public int PageNum { get; set; }

        public int PageSize { get; set; }

        public List<ABEBBetLog> List { get; set; }
    }

    public class ABBetLogResponseModel : ABBaseResponseWithDataModel<ABBetLogModel>
    {
    }
}