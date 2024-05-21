using JxBackendService.Service.Enums;
using JxBackendService.Service.Setting;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.Enums
{
    public class JxApplication : BaseStringValueModel<JxApplication>
    {
        public string ShortValue { get; private set; }

        public string AuthenticationScheme { get; private set; }

        public Type AppSettingServiceType { get; private set; }

        public Type GameAppSettingServiceType { get; private set; }

        public Dictionary<PlatformMerchant, Type> PlatformProductServiceTypeMap { get; private set; } = new Dictionary<PlatformMerchant, Type>()
        {
            {PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductService) },
        };

        public int UserKeyExpiredMinutes { get; private set; }

        public bool IsSlidingUserKeyCache { get; private set; }

        public int AppCodeForLocalizationParam { get; private set; }

        private JxApplication()
        {
        }

        public static JxApplication FrontSideWeb = new JxApplication()
        {
            Value = "FrontSideWeb",
            ShortValue = "Front",
            AppSettingServiceType = typeof(FrontSideWebAppSettingService),
            GameAppSettingServiceType = typeof(FrontSideWebGameAppSettingService),
            UserKeyExpiredMinutes = 60 * 24 * 3, // 時效為3天,
            IsSlidingUserKeyCache = true,
            AppCodeForLocalizationParam = 1,
            AuthenticationScheme = "MiseWebToken",
        };

        public static JxApplication MobileApi = new JxApplication()
        {
            Value = "MobileApi",
            ShortValue = "MApi",
            AppSettingServiceType = typeof(MobileApiAppSettingService),
            GameAppSettingServiceType = typeof(MobileApiGameAppSettingService),
            UserKeyExpiredMinutes = 60 * 24 * 3, // 時效為3天
            IsSlidingUserKeyCache = false,
            AppCodeForLocalizationParam = 1,
            AuthenticationScheme = "MobileApiAuth",
        };

        public static JxApplication BackSideWeb = new JxApplication()
        {
            Value = "BackSideWeb",
            ShortValue = "Back",
            AppSettingServiceType = typeof(BackSideWebAppSettingService),
            UserKeyExpiredMinutes = 90, // 時效為90分鐘,
            AuthenticationScheme = "BackSideWebUserCookie"
        };

        public static JxApplication AppDownloadWeb = new JxApplication()
        {
            Value = "AppDownloadWeb",
            ShortValue = "AppDownload",
            AppSettingServiceType = typeof(AppDownloadWebAppSettingService),
        };

        public static JxApplication BatchService = new JxApplication()
        {
            Value = "BatchBackgroundService",
            AppSettingServiceType = typeof(BatchServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication AGTransferService = new JxApplication()
        {
            Value = "AGBackgroundService",
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
        };

        public static JxApplication SportTransferService = new JxApplication()
        {
            Value = "SportBackgroundService",
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
        };

        public static JxApplication LCTransferService = new JxApplication()
        {
            Value = "LCBackgroundService",
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
        };

        public static JxApplication IMTransferService = new JxApplication()
        {
            Value = "IMBackgroundService",
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
        };

        public static JxApplication RGTransferService = new JxApplication()
        {
            Value = "RGBackgroundService",
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
        };

        public static JxApplication IMPTTransferService = new JxApplication()
        {
            Value = "IMPTBackgroundService",
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
        };

        public static JxApplication IMPPTransferService = new JxApplication()
        {
            Value = "IMPPBackgroundService",
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
        };

        public static JxApplication IMSportTransferService = new JxApplication()
        {
            Value = "IMSportBackgroundService",
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
        };

        public static JxApplication IMeBetTransferService = new JxApplication()
        {
            Value = "IMeBetBackgroundService",
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
        };

        public static JxApplication IMBGTransferService = new JxApplication()
        {
            Value = "IMBGBackgroundService",
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
        };

        public static JxApplication IMSGTransferService = new JxApplication()
        {
            Value = "IMSGBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication IMVRTransferService = new JxApplication()
        {
            Value = "IMVRBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication ABEBTransferService = new JxApplication()
        {
            Value = "ABEBBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication PGSLTransferService = new JxApplication()
        {
            Value = "PGSLBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication OBFITransferService = new JxApplication()
        {
            Value = "OBFITransferService),",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication EVEBTransferService = new JxApplication()
        {
            Value = "EVEBBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication BTISTransferService = new JxApplication()
        {
            Value = "BTISBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication OBSPTransferService = new JxApplication()
        {
            Value = "OBSPBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication OBEBTransferService = new JxApplication()
        {
            Value = "OBEBBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication IMKYTransferService = new JxApplication()
        {
            Value = "IMKYBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication FYESTransferService = new JxApplication()
        {
            Value = "FYESBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication JDBFITransferService = new JxApplication()
        {
            Value = "JDBFIBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication WLBGTransferService = new JxApplication()
        {
            Value = "WLBGBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication AWCSPTransferService = new JxApplication()
        {
            Value = "AWCSPBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication PMBGTransferService = new JxApplication()
        {
            Value = "PMBGBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication PMSLTransferService = new JxApplication()
        {
            Value = "PMSLBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };

        public static JxApplication CQ9SLTransferService = new JxApplication()
        {
            Value = "CQ9SLBackgroundService",
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
        };
    }
}