using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Web;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using System;

namespace JxBackendService.Model.ViewModel
{
    public class EnvironmentInfo
    {
        public string Merchant { get; set; }

        public string EnvironmentCode { get; set; }

        public BasicUserInfo LoginUser { get; set; }

        public string AppName { get; set; }

        public string MachineName { get; set; }

        public string Url { get; set; }
    }

    public class JxSystemEnvironment
    {
        public JxApplication Application { get; set; }

        public EnvironmentCode EnvironmentCode => SharedAppSettings.GetEnvironmentCode();

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
            var platformMerchant = SharedAppSettings.PlatformMerchant;
            string platformMerchantCode = null;

            if (platformMerchant != null)
            {
                platformMerchantCode = platformMerchant.Value;
            }

            var environmentInfo = new EnvironmentInfo()
            {
                Merchant = platformMerchantCode,
                AppName = environmentUser.Application.Value,
                EnvironmentCode = environmentUser.EnvironmentCode.Value,
                LoginUser = environmentUser.LoginUser,
                MachineName = Environment.MachineName
            };

            IHttpContextService httpContextService = DependencyUtil.ResolveService<IHttpContextService>().Value;
            environmentInfo.Url = httpContextService.GetAbsoluteUri();

            return environmentInfo;
        }
    }
}