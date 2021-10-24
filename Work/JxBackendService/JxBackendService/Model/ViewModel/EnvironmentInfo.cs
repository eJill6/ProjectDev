using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace JxBackendService.Model.ViewModel
{
    public class EnvironmentInfo
    {
        public string EnvironmentCode { get; set; }
        public BasicUserInfo LoginUser { get; set; }
        public string AppName { get; set; }
        public string MachineName { get; set; }
        public string Url { get; set; }
    }

    public class JxSystemEnvironment
    {
        public JxApplication Application { get; set; }
        public EnvironmentCode EnvironmentCode => SharedAppSettings.GetEnvironmentCode(Application);
        public PlatformMerchant PlatformMerchant => SharedAppSettings.PlatformMerchant;
    }

    public class EnvironmentUser : JxSystemEnvironment
    {        
        public BasicUserInfo LoginUser { get; set; }
    }

    public static class EnvironmentUserExtensions
    {
        public static EnvironmentInfo ToEnvironmentInfo(this EnvironmentUser environmentUser)
        {
            var environmentInfo = new EnvironmentInfo()
            {
                AppName = environmentUser.Application.Value,
                EnvironmentCode = environmentUser.EnvironmentCode.Value,
                LoginUser = environmentUser.LoginUser,
                MachineName = Environment.MachineName
            };

            if (HttpContext.Current != null)
            {
                environmentInfo.Url = HttpContext.Current.Request.Url.AbsoluteUri;
            }

            return environmentInfo;
        }
    }
}
