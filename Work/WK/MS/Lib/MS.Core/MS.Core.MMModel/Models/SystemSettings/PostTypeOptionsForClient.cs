namespace MS.Core.MMModel.Models.SystemSettings
{
    public class PostTypeOptionsForClient
    {
        /// <summary>
        /// 價格
        /// </summary>
        public OptionItemForClient[] Price { get; set; }

        /// <summary>
        /// 訊息種類
        /// </summary>
        public OptionItemForClient[] MessageType { get; set; }

        /// <summary>
        /// 服務項目
        /// </summary>
        public OptionItemForClient[] Service { get; set; }

        /// <summary>
        /// 年齡
        /// </summary>
        public OptionItemForClient[] Age { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public OptionItemForClient[] BodyHeight { get; set; }

        /// <summary>
        /// Cup
        /// </summary>
        public OptionItemForClient[] Cup { get; set; }
    }
}