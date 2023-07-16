using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Model.Common
{
    public class SharedAppSettings
    {
        private static int s_frontSideWebUrlIndex;

        private static Lazy<List<string>> s_frontSideWebUrls = new Lazy<List<string>>(() => GetList("FrontSideWebUrl"));

        private static object s_frontSideWebUrlsLocker = new object();

        private static List<string> GetList(string name, string defaultValue = null)
            => Get(name, defaultValue).ToNonNullString().TrimEnd(';').Split(';').ToList();

        protected SharedAppSettings()
        { }

        protected static string Get(string name, string defaultValue = null) => ConfigUtil.Get(name, defaultValue);

        /// <summary>
        /// 環境代碼物件, 根據不同應用程式有不同規則
        /// </summary>
        public static EnvironmentCode GetEnvironmentCode(JxApplication jxApplication)
        {
            var appSettingService = DependencyUtil.ResolveKeyed<IAppSettingService>(jxApplication, PlatformMerchant);

            return appSettingService.GetEnvironmentCode();
        }

        public static string RedisConnectionString => Get("RedisConnectionString");

        public static string TelegramApiUrl => Get("TelegramApiUrl", "https://api.telegram.org");

        public static string CommonStaticFileCDNString = Get("CommonStaticFileCDNString");

        public static readonly PlatformMerchant PlatformMerchant = PlatformMerchant.GetSingle(Get("PlatformMerchantCode"));

        public static bool IsRegisterMockService = Get("IsRegisterMockService") == "1";

        public static string StaticContentVersion => Get("StaticContentVersion");

        public static string BucketCdnDomain => Get("OSS.Bucket.CdnDomain");

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