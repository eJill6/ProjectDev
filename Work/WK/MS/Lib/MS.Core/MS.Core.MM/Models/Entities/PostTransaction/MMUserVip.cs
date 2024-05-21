using MS.Core.Attributes;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Models;

namespace MS.Core.MM.Models.Entities.PostTransaction
{
    public class MMUserVip : BaseDBModel
    {
        /// <summary>
        /// Id
        /// </summary>
        [AutoKey]
        public int Id { get; set; }
        /// <summary>
        /// UserId
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// VipType(致富銀卡、致富金卡)
        /// </summary>
        public VipType VipType { get; set; }
        /// <summary>
        /// 有效期限
        /// </summary>
        public DateTime EffectiveTime { get; set; }
        /// <summary>
        /// 延長天數
        /// </summary>
        public int ExtendDay { get; set; }
        /// <summary>
		/// 凍結時間
		/// </summary>
		public DateTime? FreezeTime { get; set; }
    }
}
