using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Param.Game
{
    public class SlotGameManageQueryParam : BaseGameQueryParam
    {
        public string ThirdPartyCode { get; set; }
    }

    public class BaseSlotGameManageParam : BaseGameManageParam
    {
        [Display(Name = nameof(DisplayElement.SlotGameName), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired, CustomizedMaxLength(50)]
        public string GameName { get; set; }

        [Display(Name = nameof(DisplayElement.ThirdPartyOwnership), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired]
        public string ThirdPartyCode { get; set; }

        [Display(Name = nameof(DisplayElement.GameCode), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired, CustomizedMaxLength(50)]
        public string GameCode { get; set; }

        public bool IsHot { get; set; }
    }

    public class SlotGameManageCreateParam : BaseSlotGameManageParam
    {
        [Display(Name = nameof(DisplayElement.GameImage), ResourceType = typeof(DisplayElement))]
        [ImageFileLimitation]
        [CustomizedRequired]
        public override IFormFile ImageFile { get; set; }
    }

    public class SlotGameManageUpdateParam : BaseSlotGameManageParam
    {
    }
}