using MS.Core.Attributes;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Models;
using System.ComponentModel;

namespace MS.Core.MM.Models.Entities.User
{
    /// <summary>
    /// VIP 福利
    /// </summary>
    public class MMVipWelfare : BaseDBModel
    {
        /// <summary>
        /// VIP Welfare Id
        /// </summary>
        [AutoKey]
        public int Id { get; set; }

        /// <summary>
        /// VIP Id(已經棄用)
        /// </summary>
        [Obsolete]
        public int VipId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VipType VipType { get; set; }

        public VIPWelfareCategoryEnum Category { get; set; }
        /// <summary>
        /// 種類
        /// </summary>
        public VIPWelfareTypeEnum Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string? Memo { get; set; }
    }
}