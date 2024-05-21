using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ViewModel
{
    public class BaseBasicUserInfo
    {
        public int UserId { get; set; }
    }

    public class BasicUserInfo : BaseBasicUserInfo
    {
        [IgnoreRead]
        public string UserKey { get; set; }
    }

    public class BaseUserInfoToken : BaseBasicUserInfo
    {
        public string Key { get; set; }
    }
}