using JxBackendService.Repository.StoredProcedure;
using JxBackendService.Repository.Transfer;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Enums.MSL;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using JxBackendService.Service.ThirdPartyTransfer.SqliteToken;
using JxBackendService.Service.User;
using System;
using System.Collections.Generic;

namespace JxBackendService.Model.Enums
{
    public class PlatformProduct : BaseStringValueModel<PlatformProduct>
    {
        private PlatformProduct()
        { }

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
        public ProductTypeSetting ProductType { get; private set; }

        /// <summary>
        /// 是否有合約關係(合約終止後報表要能查詢到)
        /// </summary>
        public bool HasContract { get; private set; } = true;

        /// <summary> 是否支援直接開啟熱門遊戲（若是true則會在後台熱門遊戲管理出現選項） </summary>
        public bool IsSupportHotGame { get; private set; } = false;

        /// <summary>是否在後台維護熱門遊戲的時候出現第二層選單,判斷是否開啟,要去看db FrontsideMenu下同個ProductCode下有無空白GameCode</summary>
        public bool IsSubGameOptionOfHotGameVislble { get; private set; } = false;

        /// <summary>
        /// 聚星
        /// </summary>
        public static readonly PlatformProduct Lottery = new PlatformProduct()
        {
            Value = "Lottery",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Lottery),
            Sort = 10,
            UserInfoServiceType = null,
            StoredProcedureRepType = typeof(LotteryStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameLotteryApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(PlatformProductLotterySettingMSLService) },
            },
            ProductType = ProductTypeSetting.Lottery,
            IsSelfProduct = true,
            IsSupportHotGame = true,
            IsSubGameOptionOfHotGameVislble = true,
        };

        /// <summary>
        /// AG真人
        /// </summary>
        public static readonly PlatformProduct AG = new PlatformProduct()
        {
            Value = "AG",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AGWallet),
            Sort = 50,
            UserInfoServiceType = typeof(AGUserInfoService),
            StoredProcedureRepType = typeof(AGStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameAGApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(PlatformProductAGSettingMSLService) },
            },
            ProductType = ProductTypeSetting.LiveCasino,
            IsSupportHotGame = true,
            IsSubGameOptionOfHotGameVislble = true,
        };

        /// <summary>
        /// 沙巴体育
        /// </summary>
        public static readonly PlatformProduct Sport = new PlatformProduct()
        {
            Value = "Sport",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.Sport),
            Sort = 90,
            UserInfoServiceType = typeof(SportUserInfoService),
            StoredProcedureRepType = typeof(SportStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameSportApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Sports,
        };

        /// <summary>
        /// 龍城棋牌
        /// </summary>
        public static readonly PlatformProduct LC = new PlatformProduct()
        {
            Value = "LC",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.LC),
            Sort = 200,
            UserInfoServiceType = typeof(LCUserInfoService),
            StoredProcedureRepType = typeof(LCStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameLCApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.BoardGame,
            IsSupportHotGame = true,
        };

        /// <summary>
        /// IM电竞
        /// </summary>
        public static readonly PlatformProduct IM = new PlatformProduct()
        {
            Value = "IM",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IM),
            Sort = 220,
            UserInfoServiceType = typeof(IMUserInfoService),
            StoredProcedureRepType = typeof(IMStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameIMApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.ESports,
        };

        /// <summary>
        /// IMPT电游
        /// </summary>
        public static readonly PlatformProduct IMPT = new PlatformProduct()
        {
            Value = "IMPT",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPTWallet),
            Sort = 150,
            UserInfoServiceType = typeof(IMPTUserInfoService),
            StoredProcedureRepType = typeof(IMPTStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameIMPTApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Slots,
        };

        /// <summary>
        /// IMJDB/PP电子/SE
        /// </summary>
        public static readonly PlatformProduct IMPP = new PlatformProduct()
        {
            Value = "IMPP",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMPPWallet),
            Sort = 140,
            UserInfoServiceType = typeof(IMPPUserInfoService),
            StoredProcedureRepType = typeof(IMPPStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameIMPPApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Slots,
            IsSupportHotGame = true,
            IsSubGameOptionOfHotGameVislble = true,
        };

        /// <summary>
        /// IM体育
        /// </summary>
        public static readonly PlatformProduct IMSport = new PlatformProduct()
        {
            Value = "IMSport",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMSport),
            Sort = 100,
            UserInfoServiceType = typeof(IMSportUserInfoService),
            StoredProcedureRepType = typeof(IMSportStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameIMSportApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Sports,
        };

        /// <summary>
        /// IMeBET真人
        /// </summary>
        public static readonly PlatformProduct IMeBET = new PlatformProduct()
        {
            Value = "IMeBET",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMeBET),
            Sort = 60,
            UserInfoServiceType = typeof(IMeBETUserInfoService),
            StoredProcedureRepType = typeof(IMeBETStoredProcedureRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameIMeBETApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.LiveCasino,
        };

        /// <summary>
        /// IM棋牌
        /// </summary>
        public static readonly PlatformProduct IMBG = new PlatformProduct()
        {
            Value = "IMBG",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMBG),
            Sort = 210,
            UserInfoServiceType = typeof(IMBGUserInfoService),
            StoredProcedureRepType = typeof(IMBGStoredProcedureRep),
            TransferSqlLiteRep = typeof(IMBGTransferSqlLiteRep),
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameIMBGApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.BoardGame,
        };

        /// <summary>
        /// IM雙贏彩票
        /// </summary>
        public static readonly PlatformProduct IMSG = new PlatformProduct()
        {
            Value = "IMSG",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMSG),
            Sort = 20,
            UserInfoServiceType = typeof(IMSGUserInfoService),
            StoredProcedureRepType = typeof(IMSGStoredProcedureRep),
            TransferSqlLiteRep = typeof(IMSGTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameIMSGApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.OtherLottery,
        };

        /// <summary>
        /// IMVR彩票
        /// </summary>
        public static readonly PlatformProduct IMVR = new PlatformProduct()
        {
            Value = "IMVR",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMVR),
            Sort = 30,
            UserInfoServiceType = typeof(IMVRUserInfoService),
            StoredProcedureRepType = typeof(IMVRStoredProcedureRep),
            TransferSqlLiteRep = typeof(IMVRTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameIMVRApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.OtherLottery,
        };

        /// <summary>
        /// 歐博真人
        /// </summary>
        public static readonly PlatformProduct ABEB = new PlatformProduct()
        {
            Value = "ABEB",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.ABEB),
            Sort = 70,
            UserInfoServiceType = typeof(ABEBUserInfoService),
            StoredProcedureRepType = typeof(ABEBStoredProcedureRep),
            TransferSqlLiteRep = typeof(ABEBTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameABEBApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.LiveCasino,
        };

        /// <summary>
        /// PG電子
        /// </summary>
        public static readonly PlatformProduct PGSL = new PlatformProduct()
        {
            Value = "PGSL",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PGSL),
            Sort = 160,
            UserInfoServiceType = typeof(PGSLUserInfoService),
            StoredProcedureRepType = typeof(PGSLStoredProcedureRep),
            TransferSqlLiteRep = typeof(PGSLTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGamePGSLApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Slots,
            IsSupportHotGame = true,
        };

        /// <summary>
        /// OB捕魚王
        /// </summary>
        public static readonly PlatformProduct OBFI = new PlatformProduct()
        {
            Value = "OBFI",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.OBFI),
            Sort = 170,
            UserInfoServiceType = typeof(OBFIUserInfoService),
            StoredProcedureRepType = typeof(OBFIStoredProcedureRep),
            TransferSqlLiteRep = typeof(OBFITransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameOBFIApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Slots,
        };

        /// <summary>
        /// EVO真人
        /// </summary>
        public static readonly PlatformProduct EVEB = new PlatformProduct()
        {
            Value = "EVEB",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.EVEB),
            Sort = 80,
            UserInfoServiceType = typeof(EVEBUserInfoService),
            StoredProcedureRepType = typeof(EVEBStoredProcedureRep),
            TransferSqlLiteRep = typeof(EVEBTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameEVEBApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.LiveCasino,
        };

        /// <summary>
        /// BTI體育
        /// </summary>
        public static readonly PlatformProduct BTIS = new PlatformProduct()
        {
            Value = "BTIS",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.BTIS),
            Sort = 110,
            UserInfoServiceType = typeof(BTISUserInfoService),
            StoredProcedureRepType = typeof(BTISStoredProcedureRep),
            TransferSqlLiteRep = typeof(BTISTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameBTISApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Sports,
        };

        /// <summary>
        /// OB體育
        /// </summary>
        public static readonly PlatformProduct OBSP = new PlatformProduct()
        {
            Value = "OBSP",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.OBSP),
            Sort = 120,
            UserInfoServiceType = typeof(OBSPUserInfoService),
            StoredProcedureRepType = typeof(OBSPStoredProcedureRep),
            TransferSqlLiteRep = typeof(OBSPTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameOBSPApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Sports,
        };

        /// <summary>
        /// 秘色真人
        /// </summary>
        public static readonly PlatformProduct OBEB = new PlatformProduct()
        {
            Value = "OBEB",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.OBEB),
            Sort = 40,
            UserInfoServiceType = typeof(OBEBUserInfoService),
            StoredProcedureRepType = typeof(OBEBStoredProcedureRep),
            TransferSqlLiteRep = typeof(OBEBTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameOBEBApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.LiveCasino,
        };

        /// <summary>
        /// 開元棋牌
        /// </summary>
        public static readonly PlatformProduct IMKY = new PlatformProduct()
        {
            Value = "IMKY",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.IMKY),
            Sort = 190,
            UserInfoServiceType = typeof(IMKYUserInfoService),
            StoredProcedureRepType = typeof(IMKYStoredProcedureRep),
            TransferSqlLiteRep = typeof(IMKYTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameIMKYApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.BoardGame,
            IsSupportHotGame = true,
        };

        /// <summary>
        /// 开心電競
        /// </summary>
        public static readonly PlatformProduct FYES = new PlatformProduct()
        {
            Value = "FYES",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.FYES),
            Sort = 20,
            UserInfoServiceType = typeof(FYESUserInfoService),
            StoredProcedureRepType = typeof(FYESStoredProcedureRep),
            TransferSqlLiteRep = typeof(FYESTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameFYESApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.ESports,
            HasContract = false
        };

        /// <summary>
        /// JDB捕魚
        /// </summary>
        public static readonly PlatformProduct JDBFI = new PlatformProduct()
        {
            Value = "JDBFI",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.JDBFI),
            Sort = 240,
            UserInfoServiceType = typeof(JDBFIUserInfoService),
            StoredProcedureRepType = typeof(JDBFIStoredProcedureRep),
            TransferSqlLiteRep = typeof(JDBFITransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameJDBFIApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(PlatformProductJDBFISettingMSLService) },
            },
            ProductType = ProductTypeSetting.Slots,
            IsSupportHotGame = true,
        };

        /// <summary>
        /// WLBG瓦力棋牌
        /// </summary>
        public static readonly PlatformProduct WLBG = new PlatformProduct()
        {
            Value = "WLBG",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.WLBG),
            Sort = 55,
            UserInfoServiceType = typeof(WLBGUserInfoService),
            StoredProcedureRepType = typeof(WLBGStoredProcedureRep),
            TransferSqlLiteRep = typeof(WLBGTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameWLBGApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.BoardGame,
            IsSupportHotGame = true,
        };

        /// <summary>
        /// AWCSP体育
        /// </summary>
        public static readonly PlatformProduct AWCSP = new PlatformProduct()
        {
            Value = "AWCSP",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.AWCSP),
            Sort = 20,
            UserInfoServiceType = typeof(AWCSPUserInfoService),
            StoredProcedureRepType = typeof(AWCSPStoredProcedureRep),
            TransferSqlLiteRep = typeof(AWCSPTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BaseFileSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameAWCSPApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Sports,
        };

        /// <summary>
        /// PM棋牌
        /// </summary>
        public static readonly PlatformProduct PMBG = new PlatformProduct()
        {
            Value = "PMBG",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PMBG),
            Sort = 56,
            UserInfoServiceType = typeof(PMBGUserInfoService),
            StoredProcedureRepType = typeof(PMBGStoredProcedureRep),
            TransferSqlLiteRep = typeof(PMBGTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(PMSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGamePMBGApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.BoardGame,
        };

        /// <summary>
        /// PM電子
        /// </summary>
        public static readonly PlatformProduct PMSL = new PlatformProduct()
        {
            Value = "PMSL",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.PMSL),
            Sort = 57,
            UserInfoServiceType = typeof(PMSLUserInfoService),
            StoredProcedureRepType = typeof(PMSLStoredProcedureRep),
            TransferSqlLiteRep = typeof(PMSLTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(PMSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGamePMSLApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Slots,
        };

        /// <summary>
        /// CQ9電子
        /// </summary>
        public static readonly PlatformProduct CQ9SL = new PlatformProduct()
        {
            Value = "CQ9SL",
            ResourceType = typeof(PlatformProductElement),
            ResourcePropertyName = nameof(PlatformProductElement.CQ9SL),
            Sort = 67,
            UserInfoServiceType = typeof(CQ9SLUserInfoService),
            StoredProcedureRepType = typeof(CQ9SLStoredProcedureRep),
            TransferSqlLiteRep = typeof(CQ9SLTransferSqlLiteRep),
            SqliteTokenServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(CQ9SLSqliteTokenService) },
            },
            TPGameApiServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(TPGameCQ9SLApiMSLService) },
            },
            PlatformProductSettingServiceTypeMap = new Dictionary<PlatformMerchant, Type>()
            {
                { PlatformMerchant.MiseLiveStream, typeof(BasePlatformProductSettingMSLService) },
            },
            ProductType = ProductTypeSetting.Slots,
            IsSupportHotGame = true,
            IsSubGameOptionOfHotGameVislble = true,
        };
    }
}