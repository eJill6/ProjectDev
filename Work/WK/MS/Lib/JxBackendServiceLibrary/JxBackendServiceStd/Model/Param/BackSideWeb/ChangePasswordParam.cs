using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Param.BackSideWeb
{
    public class ChangePasswordParam
    {
        [Display(Name = nameof(DisplayElement.OldPassword), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired]
        public string OldPassword { get; set; }

        [Display(Name = nameof(DisplayElement.NewPassword), ResourceType = typeof(DisplayElement))]
        [CustomizedRegularExpression(RegularExpressionEnumTypes.Password)]
        [CustomizedRequired]
        public string NewPassword { get; set; }
    }
}