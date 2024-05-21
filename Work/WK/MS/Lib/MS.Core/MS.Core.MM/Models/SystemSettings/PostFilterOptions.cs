using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MM.Models.SystemSettings
{
    /// <summary>
    /// 贴子篩選選項
    /// </summary>
    public class PostFilterOptions
    {
        /// <summary>
        /// 年齡
        /// </summary>
        public Dictionary<string, int[]> Age { get; set; } = new Dictionary<string, int[]>();

        /// <summary>
        /// 身高
        /// </summary>
        public Dictionary<string, int[]> Height { get; set; } = new Dictionary<string, int[]>();

        /// <summary>
        /// 價格
        /// </summary>
        public Dictionary<string, PriceLowAndHigh> Price { get; set; } = new Dictionary<string, PriceLowAndHigh>();

        /// <summary>
        /// 罩杯
        /// </summary>
        public OptionItem[] Cup { get; set; } = Array.Empty<OptionItem>();

        /// <summary>
        /// 服務項目
        /// </summary>
        public OptionItem[] Service { get; set; } = Array.Empty<OptionItem>();
    }
}