using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private ClientActionType() { }

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
    }

    /// <summary>
    /// 導轉功能代碼
    /// </summary>
    public class ClientAppPage : BaseStringValueModel<ClientAppPage>
    {
        private ClientAppPage() { }

        /// <summary>回首頁</summary>
        public static ClientAppPage BackToHome = new ClientAppPage() { Value = "BackToHome" };

        /// <summary>回上頁</summary>
        public static ClientAppPage BackToPrevious = new ClientAppPage() { Value = "BackToPrevious" };

        /// <summary>修改資金密碼</summary>
        public static ClientAppPage ModifyMoneyPassword = new ClientAppPage() { Value = "ModifyMoneyPassword" };

        /// <summary>綁定二次驗証</summary>
        public static ClientAppPage BindAuthenticator = new ClientAppPage() { Value = "BindAuthenticator" };

        /// <summary>初始化1.修改暱稱</summary>
        public static ClientAppPage InitialAccount = new ClientAppPage() { Value = "InitialAccount" };

        /// <summary>初始化2.設定密保問題</summary>
        public static ClientAppPage InitialSecurity = new ClientAppPage() { Value = "InitialSecurity" };

        /// <summary>綁定銀行卡</summary>
        public static ClientAppPage BindBankCard = new ClientAppPage() { Value = "BindBankCard" };

        /// <summary>綁定USDT</summary>
        public static ClientAppPage BindUSDT = new ClientAppPage() { Value = "BindUSDT" };

        /// <summary>下線轉帳成功頁</summary>
        public static ClientAppPage TransferToChildSuccess = new ClientAppPage() { Value = "TransferToChildSuccess" };

        /// <summary>一般提現成功頁</summary>
        public static ClientAppPage NormalWithdrawSuccess = new ClientAppPage() { Value = "NormalWithdrawSuccess" };

        /// <summary>USDT提現成功頁</summary>
        public static ClientAppPage USDTWithdrawSuccess = new ClientAppPage() { Value = "USDTWithdrawSuccess" };
    }


}
