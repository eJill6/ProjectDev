using System;

namespace JxBackendService.Model.Param.Security
{
    public class LoginParam
    {
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