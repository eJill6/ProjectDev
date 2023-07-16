using JxBackendService.Model.Entity.Base;
using System;

namespace JxBackendService.Model.Entity.PublishRecord
{
    /// <summary>
    /// 帖子详情
    /// </summary>
    public class PublishRecordModel : BaseEntityModel
    {
        /// <summary>
        /// 帖子ID
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 帖子类型 帖子類型。1：廣場、2：担保(原為中介)、3：官方、4：體驗
        /// </summary>
        public int PostType { get; set; }

        /// <summary>
        /// 封面照
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 发帖人的昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 预约次数
        /// </summary>
        public int AppointmentCount { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// 解锁价格
        /// </summary>
        public decimal UnlockAmount { get; set; }

        /// <summary>
        /// 申请解锁价格
        /// </summary>
        public decimal ApplyAmount { get; set; }

        /// <summary>
        /// 申请调价
        /// </summary>
        public decimal ApplyAdjustPrice { get; set; }

        /// <summary>
        /// 帖子标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 地区代码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// 罩杯
        /// </summary>
        public string Cup { get; set; }

        /// <summary>
        /// 营业时间
        /// </summary>
        public string BusinessHours { get; set; }

        /// <summary>
        /// 最低价格
        /// </summary>
        public string LowPrice { get; set; }

        /// <summary>
        /// 最高价格
        /// </summary>
        public string HighPrice { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 服务描述
        /// </summary>
        public string ServiceDescribe { get; set; }

        /// <summary>
        /// 收藏数
        /// </summary>
        public int Favorites { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int Comments { get; set; }

        /// <summary>
        /// 观看数
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 解锁数
        /// </summary>
        public int UnlockCount { get; set; }

        /// <summary>
        /// 热度
        /// </summary>
        public int Heat { get; set; }

        /// <summary>
        /// 是否精选
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string ExamineMan { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime ExamineTime { get; set; }

        /// <summary>
        /// 审核未通过的原因
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 审核中的描述
        /// </summary>
        public string OldViewData { get; set; }
    }
}