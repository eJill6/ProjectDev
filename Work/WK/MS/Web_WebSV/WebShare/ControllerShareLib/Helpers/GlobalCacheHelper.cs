using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;

namespace ControllerShareLib.Helpers
{
    public static class GlobalCacheHelper
    {
        private static readonly Lazy<IConfigUtilService> s_configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();

        public static bool IsUseCDN => Convert.ToBoolean(s_configUtilService.Value.Get("UseCDN"));

        public static string CDNSite => s_configUtilService.Value.Get("CDNSite");

        public static string AESCDNSite => s_configUtilService.Value.Get("AESCDNSite");

        public static int DefaultCacheExpireDays { get; internal set; } = 30;

        public static string RazorShareContentPath => "~/_content/RazorShareLib";
    }
}