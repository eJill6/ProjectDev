using MS.Core.MMModel.Models;

namespace MS.Core.MM.Models.SystemSettings
{
    public class PostTypeOptions
    {
        /// <summary>
        /// 價格
        /// </summary>
        public OptionItem[] Price { get; set; } = new OptionItem[0];

        /// <summary>
        /// 訊息種類
        /// </summary>
        public OptionItem[] MessageType { get; set; } = new OptionItem[0];

        /// <summary>
        /// 服務項目
        /// </summary>
        public OptionItem[] Service { get; set; } = new OptionItem[0];

        /// <summary>
        /// 年齡
        /// </summary>
        public OptionItem[] Age { get; set; } = new OptionItem[0];

        /// <summary>
        /// 身高
        /// </summary>
        public OptionItem[] BodyHeight { get; set; } = new OptionItem[0];

        /// <summary>
        /// Cup
        /// </summary>
        public OptionItem[] Cup { get; set; } = new OptionItem[0];
    }
}