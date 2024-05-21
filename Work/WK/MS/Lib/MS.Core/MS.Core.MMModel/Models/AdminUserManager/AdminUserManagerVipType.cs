using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class AdminUserManagerVipType
    {
        public VipType Id { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 優先度
        /// </summary>
        public int Priority { get; set; }
    }
}