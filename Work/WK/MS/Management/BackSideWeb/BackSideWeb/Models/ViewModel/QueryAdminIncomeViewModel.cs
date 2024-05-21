using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using MS.Core.Models.Models;

namespace BackSideWeb.Models.ViewModel
{
    public class QueryAdminIncomeViewModel : IDataKey
    {
        /// <summary>
        /// 收益单ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomePayId), ResourceType = typeof(DisplayElement), Sort = 1)]
        public string Id { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomePostId), ResourceType = typeof(DisplayElement), Sort = 2)]
        public string PostId { get; set; }

        /// <summary>
        /// 帖子区域
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomeCategoryText), ResourceType = typeof(DisplayElement), Sort = 3)]
        public string CategoryText { get; set; }

        /// <summary>
        /// 帖子区域
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 收益会员ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomeUserId), ResourceType = typeof(DisplayElement), Sort = 4)]
        public int UserId { get; set; }

        /// <summary>
        /// 解锁单/预约单ID
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomeTargetId), ResourceType = typeof(DisplayElement), Sort = 5)]
        public string TargetId { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 解锁时间
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomeCreateTimeText), ResourceType = typeof(DisplayElement), Sort = 6)]
        public string CreateTimeText { get; set; }

        /// <summary>
        /// 收益单狀態
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 收益单狀態
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomeStatusText), ResourceType = typeof(DisplayElement), Sort = 7)]
        public string StatusText { get; set; }

        /// <summary>
        /// 注解
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomeUnusualMemo), ResourceType = typeof(DisplayElement), Sort = 8)]
        public string UnusualMemo { get; set; }

        /// <summary>
        /// 暂锁收益
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomeAmountText), ResourceType = typeof(DisplayElement), Sort = 9)]
        public string AmountText { get; set; }

        /// <summary>
        /// 入账收益
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomeIncomeAmountText), ResourceType = typeof(DisplayElement), Sort = 10)]
        public string IncomeAmountText { get; set; }

        /// <summary>
        /// 入账时间
        /// </summary>

        public DateTime? DistributeTime { get; set; }

        /// <summary>
        /// 入账时间
        /// </summary>
        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomeDistributeTimeText), ResourceType = typeof(DisplayElement), Sort = 11)]
        public string DistributeTimeText => DistributeTime.HasValue ? DistributeTime.Value.ToString(GlobalSettings.DateTimeFormat) : "-";

        /// <summary>
        /// 入账审核
        /// </summary>

        [Export(ResourcePropertyName = nameof(DisplayElement.AdminIncomeDistributeStatusText), ResourceType = typeof(DisplayElement), Sort = 12)]
        public string DistributeStatusText => Status == 10 ? "已派发" : Status == 1 || Status == 11 ? "已审核" : "-";

        /// <summary>
        /// 应入账时间
        /// </summary>
        public string ApplyTimeText { get; set; }

        /// <summary>
        /// 是否到期
        /// </summary>
        public bool IsOntime { get; set; }

        public string IsOntimeText => IsOntime ? "是" : "否";

        /// <summary>
        /// 锁定状态
        /// </summary>
        public string LockTypeText { get; set; }

        /// <summary>
        /// 實際解鎖鑽石
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 收益
        /// </summary>
        public decimal Rebate { get; set; }

        /// <summary>
        /// 投诉单ID
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// 实际解锁钻石
        /// </summary>
        public string PointText { get; set; }

        /// <summary>
        /// 來源編號(卡片解鎖編號)
        /// </summary>

        public string SourceId { get; set; }

        /// <summary>
        /// 投诉单状态
        /// </summary>
        public int ReportStatus { get; set; }

        public string KeyContent => Id.ToString();
    }
}