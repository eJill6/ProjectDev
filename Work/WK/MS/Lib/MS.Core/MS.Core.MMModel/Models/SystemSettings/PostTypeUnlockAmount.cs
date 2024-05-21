using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models.SystemSettings
{
    public class PostTypeUnlockAmount
    {
        /// <summary>
        /// 发贴类型
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>
        /// 解锁价格
        /// </summary>
        public decimal UnlockAmount { get; set; }
    }
}