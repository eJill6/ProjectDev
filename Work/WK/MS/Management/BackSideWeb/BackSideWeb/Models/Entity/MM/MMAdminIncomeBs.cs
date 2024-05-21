using JxBackendService.Model.Entity.Base;
using MS.Core.Models.Models;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMAdminIncomeBs : BaseEntityModel
    {

        /// <summary>
        /// 收益单ID
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// 帖子ID
        /// </summary>
        public string PostId { get; set; }


        /// <summary>
        /// 帖子区域
        /// </summary>
        public string CategoryText { get; set; }

        /// <summary>
        /// 帖子区域
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 收益会员ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 解锁单ID
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        public string CreateTimeText { get; set; }

        /// <summary>
        /// 入账时间
        /// </summary>
        public DateTime DistributeTime { get; set; }

        /// <summary>
        /// 入账时间
        /// </summary>
        public string DistributeTimeText => DistributeTime == DateTime.MinValue ? "-" : DistributeTime.ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 应入账时间
        /// </summary>
        public string ApplyTimeText { get; set; }

        /// <summary>
        /// 是否到期
        /// </summary>
        public bool IsOntime { get; set; }

        /// <summary>
        /// 锁定状态
        /// </summary>
        public string LockTypeText { get; set; }

        /// <summary>
        /// 收益单狀態
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 收益单狀態
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// 實際解鎖鑽石
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 收益
        /// </summary>
        public decimal Rebate { get; set; }

        /// <summary>
        /// 注解
        /// </summary>
        public string UnusualMemo { get; set; }

        /// <summary>
        /// 投诉单ID
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// 实际解锁钻石
        /// </summary>
        public string PointText { get; set; }

        /// <summary>
        /// 暂锁收益
        /// </summary>
        public string AmountText { get; set; }


        /// <summary>
        /// 入账收益
        /// </summary>
        public string IncomeAmountText { get; set; }

        /// <summary>
        /// 來源編號(卡片解鎖編號)
        /// </summary>
        public string SourceId { get; set; }
    }
}
