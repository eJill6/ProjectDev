using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Model.Common
{
    public class SharedAppSettings
    {
        private static int s_frontSideWebUrlIndex;

        private static readonly Lazy<List<string>> s_frontSideWebUrls = new Lazy<List<string>>(() => GetList("FrontSideWebUrl"));

        private static readonly object s_frontSideWebUrlsLocker = new object();

        private static readonly Lazy<IConfigUtilService> s_configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        private static List<string> GetList(string name, string defaultValue = null)
            => Get(name, defaultValue).ToNonNullString().TrimEnd(';').Split(';').ToList();

        protected SharedAppSettings()
        { }

        protected static string Get(string name, string defaultValue = null) => s_configUtilService.Value.Get(name, defaultValue);

        /// <summary>
        /// 環境代碼物件, 根據不同應用程式有不同規則
        /// </summary>
        public static EnvironmentCode GetEnvironmentCode()
        {
            var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(s_environmentService.Value.Application, PlatformMerchant).Value;

            return appSettingService.GetEnvironmentCode();
        }

        public static string RedisConnectionString => Get("RedisConnectionString");

        public static string TelegramApiUrl => Get("TelegramApiUrl", "https://api.telegram.org");

        public static string CommonStaticFileCDNString => Get("CommonStaticFileCDNString");

        public static PlatformMerchant PlatformMerchant => PlatformMerchant.GetSingle(Get("PlatformMerchantCode"));

        //public static bool IsRegisterMockService = Get("IsRegisterMockService") == "1";

        public static string StaticContentVersion => Get("StaticContentVersion");

        public static string BucketCdnDomain => Get("OSS.Bucket.CdnDomain");

        public static string BucketAESCdnDomain => Get("OSS.Bucket.AESCdnDomain");

        public static string WebCoreCDN => Get("Web.CoreCDN");

        public static string FrontSideWebUrl
        {
            get
            {
                List<string> urls = s_frontSideWebUrls.Value;
                string url = null;

                lock (s_frontSideWebUrlsLocker)
                {
                    s_frontSideWebUrlIndex %= urls.Count;
                    url = urls[s_frontSideWebUrlIndex];
                    s_frontSideWebUrlIndex++;
                }

                return url;
            }
        }
    }
}