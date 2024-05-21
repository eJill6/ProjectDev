using MS.Core.Attributes;
using MS.Core.Models;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMPageRedirect : BaseDBModel
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        [AutoKey]
        public int Id { get; set; }

        /// <summary>
        /// 转导名称
        /// </summary>
        public string RedirectName { get; set; } = string.Empty;

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; } = string.Empty;

        /// <summary>
        /// 转导页面,1官方,2寻芳阁,3广场,4店铺,5官方帖子,6广场寻芳阁帖子
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 编号(会员/帖子ID)
        /// </summary>
        public string RefId { get; set; } = string.Empty;

        /// <summary>
        /// 转导地址
        /// </summary>
        public string RedirectUrl { get; set; } = string.Empty;

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string CreateUser { get; set; } = string.Empty;
    }
}