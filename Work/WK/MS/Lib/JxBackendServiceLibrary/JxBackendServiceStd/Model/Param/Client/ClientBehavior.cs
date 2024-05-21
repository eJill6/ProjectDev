using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;

namespace JxBackendService.Model.Param.Client
{
    public class ClientBehavior
    {
        /// <summary>行為模式</summary>
        public string ClientActionType { get; set; } = string.Empty;

        /// <summary>導轉頁面代碼</summary>
        public string RedirectPage { get; set; } = string.Empty;

        /// <summary>對話詳細設定 </summary>
        public DialogSetting DialogSetting { get; set; }
    }

    public class ClientActionType : BaseStringValueModel<ClientActionType>
    {
        private ClientActionType()
        { }

        /// <summary>導轉頁面</summary>
        public static ClientActionType Redirect = new ClientActionType() { Value = "Redirect" };

        /// <summary>只有確定按鈕對話</summary>
        public static ClientActionType AlertDialog = new ClientActionType() { Value = "AlertDialog" };

        /// <summary>有兩個按鈕的對話,其中一個為取消鈕</summary>
        public static ClientActionType ConfirmDialog = new ClientActionType() { Value = "ConfirmDialog" };

        /// <summary>Toast</summary>
        public static ClientActionType Toast = new ClientActionType() { Value = "Toast" };
    }

    /// <summary>
    /// client的對話窗設定, WCF不支援多型, 故把所有欄位裝在一起
    /// </summary>
    public class DialogSetting
    {
        /// <summary>對話文字</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>標題</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>確定按鈕的文字</summary>
        public string ConfirmText { get; set; } = string.Empty;

        /// <summary>按下確定後跳轉的頁面</summary>
        public string RedirectPageAfterConfirm { get; set; } = string.Empty;

        /// <summary>取消文字, 按下去直接關閉對話</summary>
        public string CancelText { get; set; } = string.Empty;

        /// <summary>按下取消後跳轉的頁面</summary>
        public string RedirectPageAfterCancel { get; set; } = string.Empty;

        /// <summary>icon網址</summary>
        public string IconUrl { get; set; } = string.Empty;

        public object Data { get; set; }
    }

    /// <summary>
    /// Web功能代碼
    /// </summary>
    public class ClientWebPage : BaseStringValueModel<ClientWebPage>
    {
        public string Action { get; private set; }

        public string Controller { get; private set; }

        public object RouteValues { get; private set; }

        /// <summary>Logon的時候呈現過場loading畫面的秒數</summary>
        public decimal? LogonViewWaitSeconds { get; private set; }

        private ClientWebPage()
        {
        }

        /// <summary>彩票投注</summary>
        public static ClientWebPage LotterySpa = new ClientWebPage()
        {
            Value = "LotterySpa",
            Controller = "LotterySpa",
            Action = "Index",
            LogonViewWaitSeconds = 0,
        };

        /// <summary>遊戲大廳(SubMerchant)</summary>
        public static ClientWebPage GameCenter = new ClientWebPage()
        {
            Value = "GameCenter",
            Controller = "Home",
            Action = "GameCenter"
        };

        /// <summary>PMEB直播間轉導頁</summary>
        public static ClientWebPage PMEBAnchorRoom = new ClientWebPage()
        {
            Value = "PMEBAnchorRoom",
            Controller = "GameCenter",
            Action = "Index",
            RouteValues = new { productCode = PlatformProduct.OBEB.Value },
            LogonViewWaitSeconds = 0.3m
        };

        /// <summary>AWC賽馬</summary>
        public static ClientWebPage AWCHB = new ClientWebPage()
        {
            Value = "AWCHB",
            Controller = "GameCenter",
            Action = "Index",
            RouteValues = new BaseGameCenterLogin()
            {
                ProductCode = ThirdPartySubGameCodes.AWCHB.PlatformProduct.Value,
                RemoteCode = ThirdPartySubGameCodes.AWCHB.Value,
            }
        };

        /// <summary>AWC鬥雞</summary>
        public static ClientWebPage AWCSV = new ClientWebPage()
        {
            Value = "AWCSV",
            Controller = "GameCenter",
            Action = "Index",
            RouteValues = new BaseGameCenterLogin()
            {
                ProductCode = ThirdPartySubGameCodes.AWCSV.PlatformProduct.Value,
                RemoteCode = ThirdPartySubGameCodes.AWCSV.Value,
            }
        };

        /// <summary>直接進入第三方遊戲</summary>
        public static ClientWebPage EnterThirdPartyGame = new ClientWebPage()
        {
            Value = "EnterThirdPartyGame",
            Controller = "GameCenter",
            Action = "EnterThirdPartyGame"
        };

        /// <summary>彩票投注</summary>
        public static ClientWebPage MM = new ClientWebPage()
        {
            Value = "MM",
            Controller = "MM",
            Action = "Index",
            LogonViewWaitSeconds = 1m,
        };
    }
}