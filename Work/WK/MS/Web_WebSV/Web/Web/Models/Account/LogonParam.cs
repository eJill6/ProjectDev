using JxBackendService.Interface.Model.User;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Param.Client;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Web.Infrastructure.Attributes;

namespace Web.Models.Account
{
    public class LogonParam : IMiseLogonUserParam
    {
        [Required]
        public int? UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        [LogonParamRequiredIf(clientWebPageNames: new string[] { nameof(ClientWebPage.LotterySpa) })]
        public string RoomNo { get; set; }

        /// <summary>彩種資訊</summary>
        [LogonParamRequiredIf(clientWebPageNames: new string[] { nameof(ClientWebPage.LotterySpa), nameof(ClientWebPage.EnterThirdPartyGame) })]
        public string GameID { get; set; }

        [LogonParamRequiredIf(clientWebPageNames: new string[] { nameof(ClientWebPage.LotterySpa) })]
        public string DepositUrl { get; set; }

        public string ClientWebPageValue { get; set; } = ClientWebPage.LotterySpa.Value;

        public string OrderNo { get; set; }

        /// <summary>主播ID</summary>
        [LogonParamRequiredIf(clientWebPageNames: new string[] { nameof(ClientWebPage.PMEBAnchorRoom) })]
        public int? AnchorId { get; set; }

        public int LogonMode { get; set; }

        /// <summary>根據ClientWebPage傳送對應形式的參數</summary>
        public string PageParamInfo { get; set; }
    }
}