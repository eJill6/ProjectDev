using Flurl;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ViewModel.Game
{
    public class BaseGameManageModel : GameCenterManageModel
    {
        public string ImageUrl { get; set; }

        public string AesFullImageUrl => !ImageUrl.IsNullOrEmpty() ? Url.Combine(SharedAppSettings.BucketAESCdnDomain, ImageUrl.ConvertToAESExtension()) : string.Empty;

        public string IsActiveText => IsActive.GetActionName();
    }

    public class HotGameManageModel : BaseGameManageModel
    {
        public string ProductCode { get; set; }

        public string PlatformProductName => PlatformProduct.GetName(ProductCode);

        public string RemoteCode { get; set; }
    }

    public class SlotGameManageModel : BaseGameManageModel
    {
        public string ThirdPartyCode { get; set; }

        public string ProductName { get; set; }

        public string GameCode { get; set; }

        public bool IsHot { get; set; }
    }
}