using MS.Core.Attributes;
using MS.Core.Models;

namespace MS.Core.MM.Models.Entities.Post
{
    public class MMAdvertisingContent : BaseDBModel
    {
        /// <summary>
        /// 廣告Id
        /// </summary>
        [AutoKey]
        public int Id { get; set; }

        /// <summary>
        /// 廣告類型。1：什么是XX、2 : 如何发XX贴。3 : PT帳戶、4 : 提示設定、 5 : 私信页內文 、 6 : 私信页URL。
        /// </summary>
        public int AdvertisingType { get; set; }

        /// <summary>
        /// 廣告内文
        /// </summary>
        public string AdvertisingContent { get; set; } = string.Empty;

        /// <summary>
        /// (棄用)贴子類型。1：廣場、2：担保(原為中介)、3：官方、4：體驗
        /// </summary>
        public int PostType { get; set; }

        /// <summary>
        /// 文字類型。0:全域、1：廣場、2：中介、3：官方、4：體驗、5:觅经纪、6:觅老板、7:觅女郎、8:星觅官
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUser { get; set; } = string.Empty;

        /// <summary>
        /// 是否開啟
        /// </summary>
        public bool IsActive { get; set; }
    }
}