using JxBackendService.Model.Attributes.Security;

namespace MobileApiCreateSign.Model
{
    //源自ControllerShareLib.Models.Account.MobileApiLogonParam
    //原專案廢除了簽章和ECDH加密的機制，所以把相關參數複製到這邊，此專案也當備用/供參考
    internal class MobileApiLogonSignParam
    {
        /// <summary> 用戶ID </summary>
        [MobileApiSign]
        public int? UserID { get; set; }

        /// <summary> 用戶名稱 </summary>
        [MobileApiSign]
        public string UserName { get; set; }

        /// <summary> 充值網址 </summary>
        [MobileApiSign]
        public string DepositUrl { get; set; }

        /// <summary> 登入UTC時間戳 </summary>
        [MobileApiSign]
        public long Timestamp { get; set; }

        /// <summary> 用戶公鑰 </summary>
        public string Coordinate { get; set; }

        /// <summary>參數簽名</summary>
        public string Sign { get; set; }

        /// <summary> 加密key </summary>
        [MobileApiSign(sortNo: int.MaxValue)]
        public string? Key { get; set; }
    }
}