using MS.Core.Attributes;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Models;
using MS.Core.MMModel.Extensions;

namespace MS.Core.MM.Models.Entities.PostTransaction
{
    public class MMVipType : BaseDBModel
    {
        /// <summary>
        /// VipType
        /// </summary>
        [PrimaryKey]
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
