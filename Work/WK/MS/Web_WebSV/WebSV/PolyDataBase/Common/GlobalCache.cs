using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;

namespace SLPolyGame.Web.Common
{
    public static class GlobalCache
    {
        private static List<string> whiteFunctionList = new List<string>()
        {
            "ValidateLogin",
            "GetVersion",
            "Check",
            "CheckIp",
            "GetCustomerServiceUrl",
            "Get_AllWebLotteryType",
            "GetPlayTypeInfo",
            "GetPlayTypeRadio",
            "GetSysSettings",
            "GetLatestWinningListItems",
        };

        static GlobalCache()
        {
        }

        public static string cacheAddress = string.Empty;

        public static string memcachedAddres = string.Empty;

        public static string mailto = "alarm@mail.luck111.com";

        public static string mailinterface = "http://mail.luck111.com:8082/SendEmail.ashx";

        public static string IpDataPath = @"D:\Data\IpData.db";

        public static string AccountDataPath = @"D:\Data\AccountData.db";

        public static string EmailDataPath = @"D:\Data\EmailData.db";

        public static string SportSecretKey = "F@lix123";

        public static int ChatEnabled = 1;

        public static int ChatOneMinuteCount = 10;

        public static int MaxPasswordAttempt = 6;

        public static int MonthMaxVerificationCodeCount = 15;

        public static int DayMaxVerificationCodeCount = 3;

        public static string SMSServices = string.Empty;

        public static string ProductName => "聚星";

        public static string PhoneHash = string.Empty;

        public static string EmailHash = string.Empty;

        public static List<string> WhiteFunctionList
        {
            get
            {
                return whiteFunctionList;
            }
        }

        /// <summary>
        /// 密色帳戶相關溝通的位址
        /// </summary>
        public static string MSSealAddress => s_configUtilService.Value.Get("MSSealAddress");

        /// <summary>
        /// 密色帳戶使用秘鑰
        /// </summary>
        public static string MSSealSalt => s_configUtilService.Value.Get("MSSealSalt");

        public static string RedisConnectionString => s_configUtilService.Value.Get("RedisConnectionString");

        private static readonly Lazy<IConfigUtilService> s_configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
    }
}