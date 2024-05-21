using JxBackendService.Model.Common;
using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.PG
{
    public class PGRegisterResponseModel : PGBaseResponseModel<PGRegisterResponseDataModel>
    {
    }

    public class PGRegisterResponseDataModel
    {
        public int action_result { get; set; }

        public bool actionResult { get; set; }
    }

    public class PGTransferResponseModel : PGBaseResponseModel<PGTransferResponseDataModel>
    {
    }

    public class PGTransferResponseDataModel
    {
        public string transactionId { get; set; }

        public decimal balanceAmountBefore { get; set; }

        public decimal balanceAmount { get; set; }

        public decimal amount { get; set; }
    }

    public class PGQueryOrderResponseModel : PGBaseResponseModel<PGQueryOrderResponseDataModel>
    {
    }

    public class PGQueryOrderResponseDataModel
    {
        public string transactionId { get; set; }

        public string playerName { get; set; }

        public string currencyCode { get; set; }

        public int transactionType { get; set; }

        public decimal transactionAmount { get; set; }

        public decimal transactionFrom { get; set; }

        public decimal transactionTo { get; set; }

        public long transactionDateTime { get; set; }
    }

    public class PGBalanceResponseModel : PGBaseResponseModel<PGBalanceResponseDataModel>
    {
    }

    public class PGBalanceResponseDataModel
    {
        public string currencyCode { get; set; }

        public decimal totalBalance { get; set; }

        public decimal cashBalance { get; set; }

        public decimal totalBonusBalance { get; set; }

        public decimal freeGameBalance { get; set; }

        public object[] bonuses { get; set; }

        public object[] freeGames { get; set; }

        public PGBalanceResponseCashwallet cashWallet { get; set; }
    }

    public class PGBalanceResponseCashwallet
    {
        public string key { get; set; }

        public int cashId { get; set; }

        public decimal cashBalance { get; set; }
    }

    public class PGBetLogResponseModel : PGBaseResponseModel<List<PGSLBetLog>>
    {
    }

    public class PGVerifySessionCallbackModel
    {
        public PGVerifySessionCallbackDataModel data { get; set; }

        public PGErrorModel error { get; set; }
    }

    public class PGVerifySessionCallbackDataModel
    {
        public PGVerifySessionCallbackDataModel()
        { }

        public PGVerifySessionCallbackDataModel(string player_name)
        {
            this.player_name = player_name;
            this.currency = PGSLSharedAppSetting.Instance.Currncy;
        }

        public PGVerifySessionCallbackDataModel(string player_name, string currency)
        {
            this.player_name = player_name;
            this.currency = currency;
        }

        public string player_name { get; set; }

        public string currency { get; set; }
    }
}