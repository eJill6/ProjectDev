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

        public Type DeviceServiceType { get; private set; }

        public int UserKeyExpiredMinutes { get; private set; }

        public int AppCodeForLocalizationParam { get; private set; }

        private JxApplication()
        {
        }

        public static JxApplication FrontSideWeb = new JxApplication()
        {
            Value = nameof(FrontSideWeb),
            ShortValue = "Front",
            AppSettingServiceType = typeof(FrontSideWebAppSettingService),
            GameAppSettingServiceType = typeof(FrontSideWebGameAppSettingService),
            DeviceServiceType = typeof(FrontSideDeviceService),
            UserKeyExpiredMinutes = 60 * 24, // 時效為1天
            AppCodeForLocalizationParam = 1,
            AuthenticationScheme = "FrontSideWebAuth",
        };

        public static JxApplication BackSideWeb = new JxApplication()
        {
            Value = nameof(BackSideWeb),
            ShortValue = "Back",
            AppSettingServiceType = typeof(BackSideWebAppSettingService),
            DeviceServiceType = typeof(BackSideDeviceService),
            UserKeyExpiredMinutes = 40, // 時效為40分鐘,
            AuthenticationScheme = "BackSideWebUserCookie"
        };

        public static JxApplication AppDownloadWeb = new JxApplication()
        {
            Value = nameof(AppDownloadWeb),
            ShortValue = "AppDownload",
            AppSettingServiceType = typeof(AppDownloadWebAppSettingService),
        };

        public static JxApplication BatchService = new JxApplication()
        {
            Value = nameof(BatchService),
            AppSettingServiceType = typeof(BatchServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication AGTransferService = new JxApplication()
        {
            Value = nameof(AGTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication SportTransferService = new JxApplication()
        {
            Value = nameof(SportTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication LCTransferService = new JxApplication()
        {
            Value = nameof(LCTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMTransferService = new JxApplication()
        {
            Value = nameof(IMTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication RGTransferService = new JxApplication()
        {
            Value = nameof(RGTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMPTTransferService = new JxApplication()
        {
            Value = nameof(IMPTTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMPPTransferService = new JxApplication()
        {
            Value = nameof(IMPPTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMSportTransferService = new JxApplication()
        {
            Value = nameof(IMSportTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMeBetTransferService = new JxApplication()
        {
            Value = nameof(IMeBetTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMBGTransferService = new JxApplication()
        {
            Value = nameof(IMBGTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(OldTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMSGTransferService = new JxApplication()
        {
            Value = nameof(IMSGTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMVRTransferService = new JxApplication()
        {
            Value = nameof(IMVRTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication ABEBTransferService = new JxApplication()
        {
            Value = nameof(ABEBTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication PGSLTransferService = new JxApplication()
        {
            Value = nameof(PGSLTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication OBFITransferService = new JxApplication()
        {
            Value = nameof(OBFITransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication EVEBTransferService = new JxApplication()
        {
            Value = nameof(EVEBTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication BTISTransferService = new JxApplication()
        {
            Value = nameof(BTISTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication OBSPTransferService = new JxApplication()
        {
            Value = nameof(OBSPTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication OBEBTransferService = new JxApplication()
        {
            Value = nameof(OBEBTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMKYTransferService = new JxApplication()
        {
            Value = nameof(IMKYTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication FYESTransferService = new JxApplication()
        {
            Value = nameof(FYESTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication JDBFITransferService = new JxApplication()
        {
            Value = nameof(JDBFITransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication WLBGTransferService = new JxApplication()
        {
            Value = nameof(WLBGTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication AWCSPTransferService = new JxApplication()
        {
            Value = nameof(AWCSPTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication PMBGTransferService = new JxApplication()
        {
            Value = nameof(PMBGTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication PMSLTransferService = new JxApplication()
        {
            Value = nameof(PMSLTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            GameAppSettingServiceType = typeof(NewTransferServiceGameAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };
    }
}