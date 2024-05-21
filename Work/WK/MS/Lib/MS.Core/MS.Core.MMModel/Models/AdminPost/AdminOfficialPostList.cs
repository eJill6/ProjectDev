using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using System;

namespace MS.Core.MMModel.Models.AdminPost
{
    /// <summary>
    /// 官方贴子清單
    /// </summary>
    public class AdminOfficialPostList
    {
        /// <summary>
        ///  官方贴 Id
        /// </summary>
        public string PostId { get; set; } = string.Empty;

        /// <summary>
        /// 發贴類型
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>
        /// 封面照片
        /// </summary>
        public string CoverUrl { get; set; } = string.Empty;

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 身高
        /// </summary>
        public string Height { get; set; } = string.Empty;

        /// <summary>
        /// 年齡
        /// </summary>
        public string Age { get; set; } = string.Empty;

        /// <summary>
        /// 罩杯
        /// </summary>
        public string Cup { get; set; } = string.Empty;

        /// <summary>
        /// 地區編碼
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 最低價格
        /// </summary>
        public string LowPrice { get; set; } = string.Empty;

        /// <summary>
        /// 顏值
        /// </summary>
        public string FacialScore { get; set; } = string.Empty;

        /// <summary>
        /// 用戶身份
        /// </summary>
        public IdentityType UserIdentity { get; set; }

        /// <summary>
        /// 会员身份
        /// </summary>
        public string UserIdentityText
        {
            get
            {
                return EnumExtension.GetDescription(UserIdentity);
            }
        }

        /// <summary>
        /// 是否營業中。0：休息、1：營業中
        /// </summary>
        public bool? IsOpen { get; set; }

        /// <summary>
        /// 是否營業中
        /// </summary>
        public string IsOpenText => IsOpen.HasValue ? IsOpen.Value ? "营业中" : "休息中" : "-";

        /// <summary>
        /// 前台删除。
        /// </summary>
        public bool? IsDelete { get; set; }

        /// <summary>
        /// 前台删除
        /// </summary>
        public string IsDeleteText => IsDelete.HasValue ? IsDelete.Value ? "刪除" : "-" : "-";

        /// <summary>
        /// 會員卡內容
        /// </summary>
        public string VipCard { get; set; } = string.Empty;

        /// <summary>
        /// 使用者類型
        /// </summary>
        public string UserType { get; set; } = string.Empty;

        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 首次送審時間
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 再次送審時間
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 再次審核時間
        /// </summary>
        public string ExamineTime { get; set; }

        /// <summary>
        /// 首次送審時間
        /// </summary>
        public string CreateTimeText => string.IsNullOrWhiteSpace(CreateTime) ? "-" : Convert.ToDateTime(CreateTime).ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 再次送審時間
        /// </summary>
        public string UpdateTimeText => string.IsNullOrWhiteSpace(UpdateTime) ? "-" : Convert.ToDateTime(UpdateTime).ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 再次審核時間
        /// </summary>
        public string ExamineTimeText => string.IsNullOrWhiteSpace(ExamineTime) ? "-" : Convert.ToDateTime(ExamineTime).ToString(GlobalSettings.DateTimeFormat);

        /// <summary>
        /// 贴子狀態
        /// </summary>
        public ReviewStatus Status { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string StatusText => Status != ReviewStatus.Approval ? Status.GetDescription() : "展示中";

        /// <summary>
        /// 编辑锁定 0 禁用 1 启用
        /// </summary>
        public bool LockStatus { get; set; }
    }
}