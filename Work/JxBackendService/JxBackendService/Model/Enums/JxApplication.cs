using JxBackendService.Service.Enums;
using JxBackendService.Service.Setting;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.Enums
{
    public class JxApplication : BaseStringValueModel<JxApplication>
    {
        public string ShortValue { get; private set; }

        public Type AppSettingServiceType { get; private set; }

        public Dictionary<PlatformMerchant, Type> PlatformProductServiceTypeMap { get; private set; } = new Dictionary<PlatformMerchant, Type>() 
        {
            {PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductService) },
            {PlatformMerchant.CheerTechSport, typeof(PlatformProductCTSService) }
        };

        public Type ReportTypeServiceType { get; private set; }

        public Type DeviceServiceType { get; private set; }

        public int UserKeyExpiredMinutes { get; private set; }


        private JxApplication()
        {
        }

        public static JxApplication FrontSideWeb = new JxApplication()
        {
            Value = nameof(FrontSideWeb),
            ShortValue = "Front",
            AppSettingServiceType = typeof(FrontSideWebAppSettingService),
            ReportTypeServiceType = typeof(FrontSideWebReportTypeService),
            DeviceServiceType = typeof(FrontSideDeviceService),
            UserKeyExpiredMinutes = 40 // 時效為40分鐘
        };

        public static JxApplication BackSideWeb = new JxApplication()
        {
            Value = nameof(BackSideWeb),
            ShortValue = "Back",
            AppSettingServiceType = typeof(BackSideWebAppSettingService),
            ReportTypeServiceType = typeof(BackSideReportTypeService),
            DeviceServiceType = typeof(BackSideDeviceService)
        };

        public static JxApplication MobileApi = new JxApplication()
        {
            Value = nameof(MobileApi),
            ShortValue = "Mobile",
            AppSettingServiceType = typeof(MobileApiAppSettingService),
            ReportTypeServiceType = typeof(MobileApiReportTypeService),
            DeviceServiceType = typeof(MobileApiDeviceService),
            UserKeyExpiredMinutes = 60 * 24 * 60 // 時效為60天
        };

        public static JxApplication AGTransferService = new JxApplication()
        {
            Value = nameof(AGTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication SportTransferService = new JxApplication()
        {
            Value = nameof(SportTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };        

        public static JxApplication LCTransferService = new JxApplication()
        {
            Value = nameof(LCTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMTransferService = new JxApplication()
        {
            Value = nameof(IMTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication RGTransferService = new JxApplication()
        {
            Value = nameof(RGTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMPTTransferService = new JxApplication()
        {
            Value = nameof(IMPTTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMPPTransferService = new JxApplication()
        {
            Value = nameof(IMPPTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMSportTransferService = new JxApplication()
        {
            Value = nameof(IMSportTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMeBetTransferService = new JxApplication()
        {
            Value = nameof(IMeBetTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMBGTransferService = new JxApplication()
        {
            Value = nameof(IMBGTransferService),
            AppSettingServiceType = typeof(OldTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMSGTransferService = new JxApplication()
        {
            Value = nameof(IMSGTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication IMVRTransferService = new JxApplication()
        {
            Value = nameof(IMVRTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication ABEBTransferService = new JxApplication()
        {
            Value = nameof(ABEBTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication PGSLTransferService = new JxApplication()
        {
            Value = nameof(PGSLTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication BatchService = new JxApplication()
        {
            Value = nameof(BatchService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication OBFITransferService = new JxApplication()
        {
            Value = nameof(OBFITransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication EVEBTransferService = new JxApplication()
        {
            Value = nameof(EVEBTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication BTISTransferService = new JxApplication()
        {
            Value = nameof(BTISTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };

        public static JxApplication OBSPTransferService = new JxApplication()
        {
            Value = nameof(OBSPTransferService),
            AppSettingServiceType = typeof(NewTransferServiceAppSettingService),
            DeviceServiceType = typeof(TransferDeviceService)
        };
    }
}
