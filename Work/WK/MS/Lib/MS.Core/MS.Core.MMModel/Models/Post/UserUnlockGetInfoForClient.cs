namespace MS.Core.MMModel.Models.Post
{
    /// <summary>
    /// 用戶解鎖可以得到的資訊
    /// </summary>
    public class UserUnlockGetInfoForClient
    {
        /// <summary>
        /// 聯擊方式
        /// </summary>
        public ContactInfoForClient[] ContactInfos { get; set; }

        /// <summary>
        /// 聯繫方式 - 地址
        /// </summary>
        public string Address { get; set; } = string.Empty;
    }
}