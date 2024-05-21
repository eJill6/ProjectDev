using System.ComponentModel;

namespace MS.Core.MMModel.Models.Post.Enums
{
    public enum ReportType : byte
    {
        /// <summary>
        /// 骗子
        /// </summary>
        [Description("骗子")]
        Fraud,

        /// <summary>
        /// 广告骚扰
        /// </summary>
        [Description("广告骚扰")]
        Advertisement,

        /// <summary>
        /// 貨不對版
        /// </summary>
        [Description("货不对版")]
        Fake,

        /// <summary>
        /// 无效联络方式
        /// </summary>
        [Description("无效联络方式")]
        InvalidContact
    }
}