using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.PostTransaction
{
    public class MMVipTransaction : BaseDBModel
    {
        /// <summary>
        ///
        /// </summary>
        [PrimaryKey]
        [EntityType(DbType.String)]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int VipId { get; set; }
        /// <summary>
        /// 付費用戶Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 消費時間
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 期限
        /// </summary>
        public DateTime? EffectiveTime { get; set; }

        /// <summary>
        /// 消費金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public IncomeExpensePayType PayType { get; set; }
        /// <summary>
        /// 會員卡名稱
        /// </summary>
        public string VipName { get; set; }

        /// <summary>
        /// Vip Type
        /// </summary>
        public VipType Type { get; set; }
    }
}
