using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Common
{
    public class SharedAppSettings
    {
        protected SharedAppSettings() { }

        protected static string Get(string name)
        {
            return ConfigurationManager.AppSettings[name].ToTrimString();
        }

        public static string CacheServiceUserName => "cacheServer";
        public static string CacheServiceKey => "F@lix123";

        /// <summary>
        /// 環境代碼物件, 根據不同應用程式有不同規則
        /// </summary>        
        public static EnvironmentCode GetEnvironmentCode(JxApplication jxApplication)
        {
            string environmentCodeInConfig = null;

            if (jxApplication == JxApplication.FrontSideWeb || jxApplication == JxApplication.MobileApi)
            {
                //WEB, API是單字碼，對回去環境
                environmentCodeInConfig = Get("EnvironmentCode");
            }
            else
            {
                environmentCodeInConfig = Get("Environment");
            }

            if (environmentCodeInConfig.IsNullOrEmpty())
            {
                return EnvironmentCode.Production;
            }

            EnvironmentCode environmentCode = null;

            if (jxApplication == JxApplication.FrontSideWeb || jxApplication == JxApplication.MobileApi)
            {
                //WEB SV 是設定第一碼
                environmentCode = EnvironmentCode.GetAll().SingleOrDefault(x => x.OrderPrefixCode == environmentCodeInConfig);
            }
            else
            {
                environmentCode = EnvironmentCode.GetSingle(environmentCodeInConfig);
            }

            if (environmentCode == null)
            {
                throw new ArgumentOutOfRangeException("EnvironmentCode not found");
            }

            return environmentCode;
        }

        //[Obsolete]
        //public static string CacheAddress => Get("cacheAddress");

        public static string RedisConnectionString => Get("RedisConnectionString");

        public static string TelegramApiUrl => Get("TelegramApiUrl");

        public static string IpSystemMid => Get("IpSystemMid");

        public static string IpSystemKey => Get("IpSystemKey");
        
        public static string IpSystemUrl => Get("IpSystemUrl");

        public static string UsdtTutorialId => Get("UsdtTutorialId");

        public static string CommonStaticFileCDNString => Get("CommonStaticFileCDNString");

        public static string InternalMailApi => Get("MailInterface");

        public static readonly SendGridSetting SendGridSetting = new SendGridSetting()
        {
            ApiUrl = Get("SendGrid.ApiUrl"),
            Token = Get("SendGrid.Token"),
            FromMail = Get("SendGrid.FromMail")
        };

        public static readonly PlatformMerchant PlatformMerchant = PlatformMerchant.GetSingle(Get("PlatformMerchantCode"));

        public static readonly AWSSendMailSetting AWSSendMailSetting = new AWSSendMailSetting()
        {
            Username = Get("AWSSendMail.Username"),
            Password = Get("AWSSendMail.Password"),
            Host = Get("AWSSendMail.Host"),
            Port = Get("AWSSendMail.Port"),
            SenderAddress = Get("AWSSendMail.SenderAddress"),
            SenderName = Get("AWSSendMail.SenderName")
        };

        public static string FrontSideWebUrl => Get("FrontSideWebUrl");
    }
}
