using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ThirdParty.RG
{
    public class BaseRGApiRequest<T>
    {
        public string app_key => RGSharedAppSetting.AppKey;

        public T data { get; set; }
    }

    public class RGApiRequest<T> : BaseRGApiRequest<T>
    {
        public string method { get; set; }
    }

    public class LoginData
    {
        public string user_name { get; set; }
        public long iat { get; set; }
    }

    public class RegisterData
    {
        public string account_name { get; set; }
        public string nickname { get; set; }
        public string currency { get; set; }
    }

    public class BalanceData
    {
        public string account_name { get; set; }
    }

    public class BaseTransferMoneyData : BalanceData
    {
        public string out_trade_no { get; set; }
    }

    public class TransferMoneyData : BaseTransferMoneyData
    {
        public decimal amount { get; set; }
    }

    public class QueryOrderData : BaseTransferMoneyData
    {
        public int type { get; set; }
    }

    public class RGOrderType : BaseIntValueModel<RGOrderType>
    {
        public string RGApiMehod { get; private set; }

        private RGOrderType() { }

        public static RGOrderType Deposit = new RGOrderType()
        {
            Value = 0,
            RGApiMehod = RGSharedAppSetting.DepositMethod
        };

        public static RGOrderType Withdraw = new RGOrderType()
        {
            Value = 4,
            RGApiMehod = RGSharedAppSetting.WithdrawMethod
        };
    }
}
