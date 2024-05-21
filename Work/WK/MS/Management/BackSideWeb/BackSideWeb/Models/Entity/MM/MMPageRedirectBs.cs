using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums.MM;
using MS.Core.MMModel.Extensions;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMPageRedirectBs : BaseEntityModel
    {
        /// <summary>
        /// 记录Id
        /// </summary>
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
        public RedirectType? Type { get; set; }

        /// <summary>
        /// 转导页面
        /// </summary>
        public string TypeText => Type?.GetDescription() ?? "-";

        /// <summary>
        /// 编号(会员/帖子ID)
        /// </summary>
        public string? RefId { get; set; } = string.Empty;

        /// <summary>
        /// 转导地址
        /// </summary>
        public string RedirectUrl { get; set; } = string.Empty;
    }
}