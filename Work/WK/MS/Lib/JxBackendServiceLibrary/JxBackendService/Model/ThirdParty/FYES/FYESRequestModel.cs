using System;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ThirdParty.FYES
{
    public class FYESBaseRequestModel
    {
        public string UserName { get; set; }
    }

    public class FYESRegisterRequestModel : FYESBaseRequestModel
    {
        public string Password { get; set; }

        /// <summary> 指定会员的币种，如果留空则为商户的开户默认币种。 请注意，会员币种为注册时设定，之后不可再修改 </summary>
        public string Currency { get; set; } = "CNY";
    }

    public class FYESLoginRequestModel : FYESBaseRequestModel
    {
        /// <summary> 可选，要进入的游戏。如果留空或者游戏ID错误，则默认进入游戏大厅首页 </summary>
        public string CateID { get; set; } = string.Empty;

        /// <summary> 可选，要进入的赛事。如果留空或者比赛ID错误，则默认进入游戏大厅首页 </summary>
        public string MatchID { get; set; } = string.Empty;
    }

    public class FYESGetBalanceRequestModel : FYESBaseRequestModel
    {
    }

    public class FYESTransferRequestModel : FYESBaseRequestModel
    {
        /// <summary> 转入：IN / 转出：OUT </summary>
        public string Type { get; set; }

        /// <summary> 转账金额，最多支持2位小数，不可为负数 </summary>
        public decimal Money { get; set; }

        /// <summary> 转账订单号，只能是数字字母下划线，32位以内 </summary>
        public string ID { get; set; }

        /// <summary> 转账币种，默认为商户开户币种。如会员币种与商户币种不一致，则此项必填 </summary>
        public string Currency { get; set; } = FYESSharedAppSetting.Currency;
    }

    public class FYESTransferType : BaseStringValueModel<FYESTransferType>
    {
        public static FYESTransferType In = new FYESTransferType() { Value = "IN" };

        public static FYESTransferType Out = new FYESTransferType() { Value = "OUT" };
    }

    public class FYESGetTransferInfoRequestModel
    {
        public string ID { get; set; }
    }

    public class FYESGetBetLogRequestModel
    {
        /// <summary> 要查询的订单类型，默认值为All </summary>
        public string OrderType { get; set; } = "All";

        /// <summary> 查询時間类型，默认值为 UpdateAt (最後更新時間)</summary>
        public string Type { get; set; } = "UpdateAt";

        public string StartAt { get; set; }

        public string EndAt { get; set; }

        /// <summary> 查询页码（默认1） </summary>
        public int PageIndex { get; set; }

        /// <summary> 每页记录数量（默认：20，最大值：1024） </summary>
        public int PageSize { get; set; } = 20;
    }
}