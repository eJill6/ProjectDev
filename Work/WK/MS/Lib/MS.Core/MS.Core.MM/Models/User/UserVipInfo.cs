using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Models.User
{
    public class UserVipInfo : IVipTypeInfo
    {
        public int UserId { get; set; }
        /// <summary>
        /// VipType(致富銀卡、致富金卡)
        /// </summary>
        public VipType VipType { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string TypeName { get; set; } = string.Empty;
        /// <summary>
        /// 優先度
        /// </summary>
        public int Priority { get; set; }
    }
}
