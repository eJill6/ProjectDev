using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Entity
{
    public class LiveGameManage
    {
        [IdentityKey]
        public int No { get; set; }

        public int LotteryId { get; set; }

        /// <summary>名稱</summary>
        public string LotteryType { get; set; }

        public string ImageUrl { get; set; }

        public string Url { get; set; }

        public string ApiUrl { get; set; }

        public bool IsActive { get; set; }

        public decimal FrameRatio { get; set; }

        public int Style { get; set; }

        public bool IsCountdown { get; set; }

        public int Duration { get; set; }

        public bool IsH5 { get; set; }

        public bool IsFollow { get; set; }

        [Display(ResourceType = typeof(DisplayElement), Name = nameof(DisplayElement.Sort))]
        [CustomizedRequired]
        public int Sort { get; set; }

        /// <summary cref="FrontsideMenuTypeSetting">頁籤分類</summary>
        public int TabType { get; set; }

        public string ProductCode { get; set; }

        public string GameCode { get; set; }

        public string RemoteCode { get; set; }
    }
}