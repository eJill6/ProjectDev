using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using System;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Param.Security
{
    public class LoginParam
    {
        [CustomizedRequired]
        [Display(ResourceType = typeof(CommonElement), Name = nameof(CommonElement.ValidateCode))]
        public string EncryptLoginString { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class LoginDetailParam
    {
        public string UserName { get; set; }

        public string UserPWD { get; set; }

        public string AuthenticatorCode { get; set; }

        public string MachineName { get; set; }

        public string WinLoginName { get; set; }

        public string LoginToolVersion { get; set; }

        public DateTime UTCTime { get; set; }
    }
}