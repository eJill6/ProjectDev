using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.Base;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMAdvertisingContentBs : BaseEntityModel
    {
        /// <summary>
        /// 廣告Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  宣傳文字類型。(XX為PostType)
        /// 1：什么是XX、2 : 如何发XX帖。3 : PT帳戶、4 : 提示設定、5 : 下载提示文字(私信頁)、
        /// 6 : 下载URL(私信頁)、7 : XX跑马灯、8 : XX新客必看、9 : XX官方提示
        /// </summary>
        public int AdvertisingType { get; set; }

        /// <summary>
        /// 廣告内文
        /// </summary>
        public string AdvertisingContent { get; set; } = string.Empty;

        /// <summary>
        /// 內文類型。1：广场、2：寻芳阁、3：官方、4：体验、5：觅经纪、6：觅老板、7：觅女郎、8：星觅官
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 是否開啟
        /// </summary>
        public bool IsActive { get; set; }

        public string ModifyDateText => ModifyDate.ToFormatDateTimeString();
    }
}