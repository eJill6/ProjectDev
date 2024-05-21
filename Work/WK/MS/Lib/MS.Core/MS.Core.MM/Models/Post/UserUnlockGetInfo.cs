using MS.Core.MMModel.Models;

namespace MS.Core.MM.Models.Post
{
    /// <summary>
    /// 用戶解鎖可以得到的資訊
    /// </summary>
    public class UserUnlockGetInfo
    {
        /// <summary>
        /// 聯擊方式
        /// </summary>
        public ContactInfo[] ContactInfos { get; set; } = new ContactInfo[0];

        /// <summary>
        /// 聯繫方式 - 地址
        /// </summary>
        public string Address { get; set; } = string.Empty;
    }
}