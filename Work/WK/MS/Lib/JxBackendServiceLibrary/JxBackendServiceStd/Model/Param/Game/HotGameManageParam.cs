using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Param.Game
{
    public class HotGameManageQueryParam : BaseGameQueryParam
    {
    }

    public class BaseHotGameManageParam : BaseGameManageParam
    {
        [Display(Name = nameof(DisplayElement.HotGameName), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired, CustomizedMaxLength(50)]
        public string MenuName { get; set; }

        [Display(Name = nameof(DisplayElement.ThirdPartyOwnership), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired]
        public string ProductCode { get; set; }

        public string GameCode { get; set; }

        [Display(Name = nameof(DisplayElement.GameCode), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired, CustomizedMaxLength(50)]
        public string RemoteCode { get; set; }
    }

    public class HotGameManageCreateParam : BaseHotGameManageParam
    {
        [Display(Name = nameof(DisplayElement.GameImage), ResourceType = typeof(DisplayElement))]
        [ImageFileLimitation]
        [CustomizedRequired]
        public override IFormFile ImageFile { get; set; }
    }

    public class HotGameManageUpdateParam : BaseHotGameManageParam
    {
    }
}