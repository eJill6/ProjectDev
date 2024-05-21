using System;
using System.Collections.Generic;

namespace MS.Core.MMModel.Models.SystemSettings
{    /// <summary>
     /// 贴子篩選選項
     /// </summary>
    public class PostFilterOptionsForClient
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
        public Dictionary<string, PriceLowAndHighForClient> Price { get; set; } = new Dictionary<string, PriceLowAndHighForClient>();

        /// <summary>
        /// 罩杯
        /// </summary>
        public OptionItemForClient[] Cup { get; set; } = Array.Empty<OptionItemForClient>();

        /// <summary>
        /// 服務項目
        /// </summary>
        public OptionItemForClient[] Service { get; set; } = Array.Empty<OptionItemForClient>();
    }
}