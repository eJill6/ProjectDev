using System;

namespace BackSideWebLoginToolService.Model
{
    public static class LoginParamSettings
    {
        public static readonly string Key = "jt7ANIqh";
        public static readonly string Iv = "ELo737Uq";
        public static readonly int LoginStringExpiredSeconds = 30;
        public static readonly string[] ntpservers = new string[] { "TIME.google.com", "time.stdtime.gov.tw", "tick.stdtime.gov.tw" };
    }
    public class BWLoginToolParam
    {
        /// <summary>
        /// 登入帳號
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 登入密碼
        /// </summary>
        public string UserPWD { get; set; }

        /// <summary>
        /// 主機名稱
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 作業系統登入帳號
        /// </summary>
        public string WinLoginName { get; set; }

        /// <summary>
        /// 使用者IP列表
        /// </summary>
        public string[] UserIPs { get; set; }

        /// <summary>
        /// 使用者本機UTC時間
        /// </summary>
        public DateTime UTCTime { get; set; }

        /// <summary>
        /// 登入工具版本
        /// </summary>
        public string LoginToolVersion { get; set; }

        public string AuthenticatorCode { get; set; }
    }
}