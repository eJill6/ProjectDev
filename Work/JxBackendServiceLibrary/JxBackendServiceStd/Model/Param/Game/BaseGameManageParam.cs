using Flurl;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Common;
using JxBackendService.Model.Paging;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Param.Game
{
    public class BaseGameQueryParam : BasePagingRequestParam
    {
        public string GameName { get; set; }
    }

    public class BaseGameManageParam : IImageFileUpload
    {
        public int No { get; set; }

        public string ImageUrl { get; set; }

        public string FullImageUrl => !ImageUrl.IsNullOrEmpty() ? Url.Combine(SharedAppSettings.BucketCdnDomain, ImageUrl) : string.Empty;

        [Display(Name = nameof(DisplayElement.GameImage), ResourceType = typeof(DisplayElement))]
        [ImageFileLimitation]
        public virtual IFormFile ImageFile { get; set; }

        [Display(Name = nameof(DisplayElement.Sort), ResourceType = typeof(DisplayElement))]
        [CustomizedRegularExpression(RegularExpressionEnumTypes.Sort)]
        [CustomizedRequired]
        public int? Sort { get; set; } = 999;

        [Display(Name = nameof(DisplayElement.IsActiveStatus), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired]
        public bool IsActive { get; set; }
    }
}