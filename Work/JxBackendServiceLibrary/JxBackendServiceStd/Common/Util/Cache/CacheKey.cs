using JxBackendService.Model.Enums;

namespace JxBackendService.Common.Util.Cache
{
    public enum CacheTypes
    {
        /// <summary>
        /// 本地端快取
        /// </summary>
        LocalMemory,

        /// <summary>
        /// 遠端共用快取
        /// </summary>
        CacheServer,

        /// <summary>
        /// 先取local，沒有才從遠端取，再沒有則從Func拿
        /// </summary>
        LocalAndCacheServer
    }

    public class CacheKey : BaseStringValueModel<CacheKey>
    {
        public CacheTypes CacheType { get; private set; }

        public DbIndexes DbIndex { get; private set; } = DbIndexes.Default;

        /// <summary>
        /// 是否要正規化key
        /// </summary>
        public bool IsFormatKey { get; private set; } = true;

        public bool IsDoSerialize { get; private set; } = true;

        private CacheKey()
        { }

        private CacheKey(CacheTypes cacheType, string value) : this(cacheType, value, DbIndexes.Default)
        {
        }

        private CacheKey(CacheTypes cacheType, string value, DbIndexes dbIndex)
        {
            CacheType = cacheType;
            Value = value;
            DbIndex = dbIndex;
        }

        public static CacheKey GetFrontSideUserInfoKey(string userKey) => new CacheKey(CacheTypes.LocalAndCacheServer, $"WebUserToken:{userKey}")
        {
            IsFormatKey = false,
        };

        public static CacheKey AllTPGameUserScores(int userId) => new CacheKey(CacheTypes.LocalMemory, $"{nameof(AllTPGameUserScores)}:{userId}");

        public static CacheKey UserInfoAdditionalLock(int userId) => new CacheKey(CacheTypes.CacheServer, nameof(UserInfoAdditionalLock), DbIndexes.Helper);

        public static CacheKey ActiveFrontsideMenu => new CacheKey(CacheTypes.LocalMemory, $"{nameof(ActiveFrontsideMenu)}");

        public static CacheKey FrontsideMenuViewModel => new CacheKey(CacheTypes.LocalMemory, $"{nameof(FrontsideMenuViewModel)}");

        public static readonly CacheKey EVEBTokenKey = new CacheKey(CacheTypes.LocalMemory, $"{nameof(EVEBTokenKey)}");

        public static readonly CacheKey LotteryInfo = new CacheKey(CacheTypes.LocalMemory, $"{nameof(LotteryInfo)}");

        public static CacheKey ThirdPartyUserAccount(int userId) => new CacheKey(CacheTypes.LocalAndCacheServer, $"{nameof(ThirdPartyUserAccount)}:{userId}");

        public static CacheKey TransferOutAllAmount(int userId) => new CacheKey(CacheTypes.CacheServer, $"{nameof(TransferOutAllAmount)}:{userId}");

        public static CacheKey TransferOutBySingleProduct(int userId, PlatformProduct product) => new CacheKey(CacheTypes.CacheServer, $"{nameof(TransferOutBySingleProduct)}:{userId}:{product.Value}");

        public static readonly CacheKey LotteryTypeMap = new CacheKey(CacheTypes.LocalAndCacheServer, nameof(LotteryTypeMap));

        public static readonly CacheKey PlayTypeNameMap = new CacheKey(CacheTypes.LocalAndCacheServer, nameof(PlayTypeNameMap));

        public static readonly CacheKey PlayTypeRadioNameMap = new CacheKey(CacheTypes.LocalAndCacheServer, nameof(PlayTypeRadioNameMap));

        public static CacheKey TPAnchors(PlatformProduct platformProduct) => new CacheKey(CacheTypes.LocalAndCacheServer, $"{nameof(TPAnchors)}:{platformProduct}");

        public static CacheKey CheckUserAllowedLogin(string userKey) => new CacheKey(CacheTypes.LocalMemory, $"{nameof(CheckUserAllowedLogin)}:{userKey}");

        public static CacheKey ActiveGameLobbyList(GameLobbyType gameLobbyType)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(ActiveGameLobbyList)}:{gameLobbyType.Value}");

        public static CacheKey ActiveGameLobbyMap(GameLobbyType gameLobbyType)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(ActiveGameLobbyMap)}:{gameLobbyType.Value}");

        public static CacheKey ApiGameList(PlatformProduct product)
               => new CacheKey(CacheTypes.LocalMemory, $"{nameof(ApiGameList)}:{product.Value}");

        public static CacheKey BackSideUser(string userKey)
            => new CacheKey(CacheTypes.LocalAndCacheServer, $"{nameof(BackSideUser)}:{userKey}", DbIndexes.BackSideWeb);

        public static CacheKey BackSideUserMenu(string userKey)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(BackSideUserMenu)}:{userKey}", DbIndexes.BackSideWeb);

        public static CacheKey BackSideUserRole()
            => new CacheKey(CacheTypes.LocalAndCacheServer, nameof(BackSideUserRole), DbIndexes.BackSideWeb);

        public static CacheKey AllowExecutingActionWithLock(string key)
            => new CacheKey(CacheTypes.CacheServer, $"{nameof(AllowExecutingActionWithLock)}:{key}");

        public static CacheKey AllowExecutingAction(string key)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(AllowExecutingAction)}:{key}");
    }
}