using JxBackendService.Model.Common;
using JxBackendService.Repository.StoredProcedure;
using JxBackendService.Repository.Transfer;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Enums.Product;
using JxBackendService.Service.Merchant.CTS;
using JxBackendService.Service.ThirdPartyTransfer;
using JxBackendService.Service.ThirdPartyTransfer.FetchFileBetLog;
using JxBackendService.Service.ThirdPartyTransfer.Mock;
using JxBackendService.Service.ThirdPartyTransfer.SqliteToken;
using JxBackendService.Service.User;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.Enums
{
    public enum ProductTypes
    {
        ///<summary>自營平台彩票</summary>
        Lottery = 1,
        ///<summary>其他彩票</summary>
        OtherLottery = 2,
        ///<summary>真人</summary>
        LiveCasino = 3,
        ///<summary>體育</summary>
        Sports = 4,
        ///<summary>电子</summary>
        Slots = 5,
        ///<summary>棋牌</summary>
        BoardGame = 6,
        ///<summary>电竞</summary>
        ESports = 7,
    }

    public class PlatformProduct : BaseStringValueModel<PlatformProduct>
    {
        private PlatformProduct() { }

        /// <summary>紀錄第三方User資訊的UserInfoServiceType</summary>
        public Type UserInfoServiceType { get; private set; }
        public Type StoredProcedureRepType { get; private set; }
        public Type TransferSqlLiteRep { get; private set; }
        public Dictionary<PlatformMerchant, Type> SqliteTokenServiceTypeMap { get; private set; }
        public Dictionary<PlatformMerchant, Type> TPGameApiServiceTypeMap { get; private set; }
        public Type OldTPGameApiServiceType { get; private set; }
        public Dictionary<PlatformMerchant, Type> PlatformProductSettingServiceTypeMap { get; private set; }

        /// <summary>
        /// 是否為自營產品
        /// </summary>
        public bool IsSelfProduct { get; private set; } = false;

        /// <summary>
        /// 遊戲類型
        /// </summary>
        public ProductTypes ProductType { get; private set; }

        /// <summary>
        /// 聚星
        /// </summary>
        public static readonly PlatformProduct Lottery = new PlatformProduct()
        {
            Value = "Lottery",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery),
            Sort = 0,
            UserInfoServiceType = null,
            StoredProcedureRepType = typeof(LotteryStoredProcedureRep),
            IsSelfProduct = true,
            ProductType = ProductTypes.Lottery,
        };

        /// <summary>
        /// AG真人
        /// </summary>
        public static readonly PlatformProduct AG = new PlatformProduct()
        {
            Value = "AG",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AG),
            Sort = 3,
            UserInfoServiceType = typeof(AGUserInfoService),
            StoredProcedureRepType = typeof(AGStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameAGApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameAGFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(PlatformProductAGSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.LiveCasino,
        };

        /// <summary>
        /// 沙巴体育
        /// </summary>
        public static readonly PlatformProduct Sport = new PlatformProduct()
        {
            Value = "Sport",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Sport),
            Sort = 7,
            UserInfoServiceType = typeof(SportUserInfoService),
            StoredProcedureRepType = typeof(SportStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameSportApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameSportFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(PlatformProductSportSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.Sports,
        };

        /// <summary>
        /// 老虎机
        /// </summary>
        public static readonly PlatformProduct PT = new PlatformProduct()
        {
            Value = "PT",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PT),
            Sort = 11,
            UserInfoServiceType = typeof(PtUserInfoService),
            StoredProcedureRepType = typeof(PTStoredProcedureRep),
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(PlatformProductPTSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.Slots,
        };

        /// <summary>
        /// LC棋牌
        /// </summary>
        public static readonly PlatformProduct LC = new PlatformProduct()
        {
            Value = "LC",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.LC),
            Sort = 16,
            UserInfoServiceType = typeof(LCUserInfoService),
            StoredProcedureRepType = typeof(LCStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameLCApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameLCFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(PlatformProductLCSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.BoardGame,
        };

        /// <summary>
        /// IM电竞
        /// </summary>
        public static readonly PlatformProduct IM = new PlatformProduct()
        {
            Value = "IM",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IM),
            Sort = 18,
            UserInfoServiceType = typeof(IMUserInfoService),
            StoredProcedureRepType = typeof(IMStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameIMApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameIMFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.ESports,
        };

        /// <summary>
        /// RG电竞
        /// </summary>
        public static readonly PlatformProduct RG = new PlatformProduct()
        {
            Value = "RG",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.RG),
            Sort = 19,
            UserInfoServiceType = typeof(RGUserInfoService),
            StoredProcedureRepType = typeof(RGStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameRGApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameRGFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(PlatformProductRGSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.ESports,
        };

        /// <summary>
        /// IMPT电游
        /// </summary>
        public static readonly PlatformProduct IMPT = new PlatformProduct()
        {
            Value = "IMPT",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPT),
            Sort = 12,
            UserInfoServiceType = typeof(IMPTUserInfoService),
            StoredProcedureRepType = typeof(IMPTStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameIMPTApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameIMPTFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.Slots,
        };

        /// <summary>
        /// IMPP电子
        /// </summary>
        public static readonly PlatformProduct IMPP = new PlatformProduct()
        {
            Value = "IMPP",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPP),
            Sort = 13,
            UserInfoServiceType = typeof(IMPPUserInfoService),
            StoredProcedureRepType = typeof(IMPPStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameIMPPApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameIMPPFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.Slots,
        };

        /// <summary>
        /// IM体育
        /// </summary>
        public static readonly PlatformProduct IMSport = new PlatformProduct()
        {
            Value = "IMSport",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMSport),
            Sort = 8,
            UserInfoServiceType = typeof(IMSportUserInfoService),
            StoredProcedureRepType = typeof(IMSportStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameIMSportApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameIMSportFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.Sports,
        };

        /// <summary>
        /// IMeBET真人
        /// </summary>
        public static readonly PlatformProduct IMeBET = new PlatformProduct()
        {
            Value = "IMeBET",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMeBET),
            Sort = 4,
            UserInfoServiceType = typeof(IMeBETUserInfoService),
            StoredProcedureRepType = typeof(IMeBETStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameIMeBETApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameIMeBETFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.LiveCasino,
        };

        /// <summary>
        /// IM棋牌
        /// </summary>
        public static readonly PlatformProduct IMBG = new PlatformProduct()
        {
            Value = "IMBG",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMBG),
            Sort = 17,
            UserInfoServiceType = typeof(IMBGUserInfoService),
            StoredProcedureRepType = typeof(IMBGStoredProcedureRep),
            TransferSqlLiteRep = typeof(IMBGTransferSqlLiteRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameIMBGApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameIMBGFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.BoardGame,
        };

        /// <summary>
        /// IM雙贏
        /// </summary>
        public static readonly PlatformProduct IMSG = new PlatformProduct()
        {
            Value = "IMSG",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMSG),
            Sort = 1,
            UserInfoServiceType = typeof(IMSGUserInfoService),
            StoredProcedureRepType = typeof(IMSGStoredProcedureRep),
            TransferSqlLiteRep = typeof(IMSGTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(IMLotterySqliteTokenService) },
                { PlatformMerchant.CheerTechSport, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameIMSGApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameIMSGFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            //TPGameApiMockServiceType = typeof(TPGameIMSGApiMockService),
            //FnIsUseMockService = () => true,
            ProductType = ProductTypes.OtherLottery,
        };

        /// <summary>
        /// IMVR彩票
        /// </summary>
        public static readonly PlatformProduct IMVR = new PlatformProduct()
        {
            Value = "IMVR",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMVR),
            Sort = 2,
            UserInfoServiceType = typeof(IMVRUserInfoService),
            StoredProcedureRepType = typeof(IMVRStoredProcedureRep),
            TransferSqlLiteRep = typeof(IMVRTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(IMLotterySqliteTokenService) },
                { PlatformMerchant.CheerTechSport, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameIMVRApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameIMVRFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.OtherLottery,
        };

        /// <summary>
        /// 歐博真人
        /// </summary>
        public static readonly PlatformProduct ABEB = new PlatformProduct()
        {
            Value = "ABEB",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.ABEB),
            Sort = 5,
            UserInfoServiceType = typeof(ABEBUserInfoService),
            StoredProcedureRepType = typeof(ABEBStoredProcedureRep),
            TransferSqlLiteRep = typeof(ABEBTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(ABEBSqliteTokenService) },
                { PlatformMerchant.CheerTechSport, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameABEBApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameABEBFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.LiveCasino,
        };

        /// <summary>
        /// PG電子
        /// </summary>
        public static readonly PlatformProduct PGSL = new PlatformProduct()
        {
            Value = "PGSL",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PGSL),
            Sort = 14,
            UserInfoServiceType = typeof(PGSLUserInfoService),
            StoredProcedureRepType = typeof(PGSLStoredProcedureRep),
            TransferSqlLiteRep = typeof(PGSLTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(PGSLSqliteTokenService) },
                { PlatformMerchant.CheerTechSport, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGamePGSLApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGamePGSLFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.Slots,
        };

        /// <summary>
        /// OB捕魚王
        /// </summary>
        public static readonly PlatformProduct OBFI = new PlatformProduct()
        {
            Value = "OBFI",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.OBFI),
            Sort = 15,
            UserInfoServiceType = typeof(OBFIUserInfoService),
            StoredProcedureRepType = typeof(OBFIStoredProcedureRep),
            TransferSqlLiteRep = typeof(OBFITransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(OBFISqliteTokenService) },
                { PlatformMerchant.CheerTechSport, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameOBFIApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameOBFIFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.Slots,
        };

        /// <summary>
        /// EVO真人
        /// </summary>
        public static readonly PlatformProduct EVEB = new PlatformProduct()
        {
            Value = "EVEB",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.EVEB),
            Sort = 6,
            UserInfoServiceType = typeof(EVEBUserInfoService),
            StoredProcedureRepType = typeof(EVEBStoredProcedureRep),
            TransferSqlLiteRep = typeof(EVEBTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(EVEBSqliteTokenService) },
                { PlatformMerchant.CheerTechSport, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameEVEBApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameEVEBFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.LiveCasino,
        };

        /// <summary>
        /// BTI體育
        /// </summary>
        public static readonly PlatformProduct BTIS = new PlatformProduct()
        {
            Value = "BTIS",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.BTIS),
            Sort = 9,
            UserInfoServiceType = typeof(BTISUserInfoService),
            StoredProcedureRepType = typeof(BTISStoredProcedureRep),
            TransferSqlLiteRep = typeof(BTISTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BTISSqliteTokenService) },
                { PlatformMerchant.CheerTechSport, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameBTISApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameBTISFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.Sports,
        };

        /// <summary>
        /// OB體育
        /// </summary>
        public static readonly PlatformProduct OBSP = new PlatformProduct()
        {
            Value = "OBSP",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.OBSP),
            Sort = 10,
            UserInfoServiceType = typeof(OBSPUserInfoService),
            StoredProcedureRepType = typeof(OBSPStoredProcedureRep),
            TransferSqlLiteRep = typeof(OBSPTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(OBSPSqliteTokenService) },
                { PlatformMerchant.CheerTechSport, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(TPGameOBSPApiService) },
                { PlatformMerchant.CheerTechSport, typeof(TPGameOBSPFileApiService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.CheerTechLottery, typeof(BasePlatformProductSettingCTLService) },
                { PlatformMerchant.CheerTechSport, typeof(BasePlatformProductSettingCTSService) },
            },
            ProductType = ProductTypes.Sports,
        };
    }

    public class PlatformProductSetting
    {
        public bool IsParseUserIdFromSuffix { get; set; }
    }
}
