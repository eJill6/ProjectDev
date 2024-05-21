namespace MS.Core.MMModel.Models.Post.Enums
{
    /// <summary>
    /// 宣傳文字類型
    /// </summary>
    /// XX為PostType
    public enum AdvertisingType : byte
    {
        /// <summary>
        /// 什么是XX
        /// </summary>
        What = 1,

        /// <summary>
        /// 如何发XX贴
        /// </summary>
        How = 2,

        /// <summary>
        /// PT帳戶
        /// </summary>
        PT_Account = 3,

        /// <summary>
        /// 提示設定
        /// </summary>
        Tip = 4,

        //私信頁
        /// <summary>
        /// 下载提示文字
        /// </summary>
        MailSetting = 5,

        //私信頁
        /// <summary>
        /// 下载URL
        /// </summary>
        MailSettingUrl = 6,

        /// <summary>
        /// XX跑马灯
        /// </summary>
        Marquee = 7,

        /// <summary>
        /// XX新客必看
        /// </summary>
        MustSee = 8,

        /// <summary>
        /// XX官方提示
        /// </summary>
        MainTip = 9,
    }
}