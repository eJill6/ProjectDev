using ControllerShareLib.Infrastructure.Attributes;
using JxBackendService.Interface.Model.User;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Param.Client;

namespace ControllerShareLib.Models.Account
{
    public class BaseLogonParam
    {
        /// <summary> 用戶ID </summary>
        [CustomizedRequired]
        public int? UserID { get; set; }

        /// <summary> 用戶名稱 </summary>
        [CustomizedRequired]
        public string UserName { get; set; }
    }

    public class MobileApiLogonParam : BaseLogonParam
    {
    }

    public class ValidateLogonParam : BaseLogonParam, IMiseLogonUserParam
    {
        public string? RoomNo { get; set; }

        /// <summary>彩種資訊</summary>
        public string? GameID { get; set; }

        public string? DepositUrl { get; set; }

        public int LogonMode { get; set; }

        public int UserKeyExpiredMinutes { get; set; }

        public bool IsSlidingExpiration { get; set; }
    }

    public class LogonParam : BaseLogonParam, IMiseLogonUserParam
    {
        public string? ClientWebPageValue { get; set; } = ClientWebPage.LotterySpa.Value;

        [LogonParamRequiredIf(clientWebPageNames: new string[] { nameof(ClientWebPage.LotterySpa) })]
        public string? RoomNo { get; set; }

        /// <summary>彩種資訊</summary>
        [LogonParamRequiredIf(clientWebPageNames: new string[] { nameof(ClientWebPage.LotterySpa), nameof(ClientWebPage.EnterThirdPartyGame) })]
        public string? GameID { get; set; }
        
        public string? DepositUrl { get; set; }

        public int LogonMode { get; set; }

        public string? OrderNo { get; set; }

        /// <summary>主播ID</summary>
        [LogonParamRequiredIf(clientWebPageNames: new string[] { nameof(ClientWebPage.PMEBAnchorRoom) })]
        public int? AnchorId { get; set; }

        /// <summary>根據ClientWebPage傳送對應形式的參數</summary>
        public string? PageParamInfo { get; set; }
    }

    public class LogonResult
    {
        public string Token { get; set; }

        public long ExpiredTimestamp { get; set; }
    }
}