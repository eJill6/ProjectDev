using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.ThirdParty.FYES
{
    public class ErrorResponseModel
    {
        /// <summary cref="FYESErrorResponseCode"> 錯誤代碼 </summary>
        public string Error { get; set; }
    }

    public class FYESErrorResponseModel : FYESBaseInfoModel<ErrorResponseModel>
    {
    }

    public class FYESRegisterResponseModel : FYESBaseModel
    {
    }

    public class LoginResponseModel
    {
        /// <summary> 进入游戏的地址 </summary>
        public string Url { get; set; }
    }

    public class FYESLoginResponseModel : FYESBaseInfoModel<LoginResponseModel>
    {
    }

    public class GetBalanceResponseModel
    {
        /// <summary> 会员的币种 </summary>
        public string Currency { get; set; }

        /// <summary> 会员的余额 </summary>
        public string Money { get; set; }

        /// <summary> 会员的额度（等同余额/此字段将被废弃，请使用 Money 字段） </summary>
        public string Credit { get; set; }
    }

    public class FYESGetBalanceResponseModel : FYESBaseInfoModel<GetBalanceResponseModel>
    {
    }

    public class TransferResponseModel
    {
        /// <summary> AVIA系统内部转账流水号 </summary>
        public string ID { get; set; }

        /// <summary> 商户提交的转账流水号 </summary>
        public string OrderID { get; set; }

        /// <summary> 本次转账的币种 </summary>
        public string Currency { get; set; }

        /// <summary> 转账之后会员的余额 </summary>
        public string Balance { get; set; }

        /// <summary> 转账之后商户的额度 </summary>
        public string Credit { get; set; }
    }

    public class FYESTransferResponseModel : FYESBaseInfoModel<TransferResponseModel>
    {
    }

    public class GetTransferInfoResponseModel
    {
        /// <summary> 商户的转账订单号 </summary>
        public string ID { get; set; }

        /// <summary> 本次转账订单的币种 </summary>
        public string Currency { get; set; }

        /// <summary> 转账的发起时间 </summary>
        public string CreateAt { get; set; }

        /// <summary> 本次转账的金额 </summary>
        public string Money { get; set; }

        /// <summary> 转账的用户名 </summary>
        public string UserName { get; set; }

        /// <summary> 转账类型。 IN：转入游戏 / OUT：转出游戏 </summary>
        public string Type { get; set; }

        /// <summary cref="FYESTransferStatus"> 转账状态 示例：（None：未完成、Finish：已完成、Faild：失败） </summary>
        public string Status { get; set; }

        /// <summary> 转账之后的会员余额 </summary>
        public string Balance { get; set; }

        /// <summary> 转账之前的余额（废弃字段） </summary>
        public string BalanceBefore { get; set; }

        /// <summary> 当前余额（废弃字段） </summary>
        public string CurrentBalance { get; set; }

        /// <summary> 转账额度（废弃字段） </summary>
        public string Credit { get; set; }
    }

    /// <summary> 转账状态 示例：（None：未完成、Finish：已完成、Faild：失败） </summary>
    public class FYESTransferStatus : BaseStringValueModel<FYESTransferStatus>
    {
        public static FYESTransferStatus Processing = new FYESTransferStatus()
        {
            Value = "None",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.FYESTransferStatusProcessing),
        };

        public static FYESTransferStatus Finish = new FYESTransferStatus()
        {
            Value = "Finish",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.FYESTransferStatusFinish),
        };

        public static FYESTransferStatus Failed = new FYESTransferStatus()
        {
            Value = "Faild",
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.FYESTransferStatusFailed),
        };
    }

    public class FYESGetTransferInfoResponseModel : FYESBaseInfoModel<GetTransferInfoResponseModel>
    {
    }
}