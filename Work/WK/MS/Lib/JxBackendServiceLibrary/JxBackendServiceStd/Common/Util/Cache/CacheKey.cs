using JxBackendService.Model.Enums;
using System;

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

        public static CacheKey UserInfoLock(int userId) => new CacheKey(CacheTypes.CacheServer, nameof(UserInfoLock), DbIndexes.Helper);

        public static CacheKey TPGameUserInfoLock(int userId) => new CacheKey(CacheTypes.CacheServer, nameof(TPGameUserInfoLock), DbIndexes.Helper);

        public static CacheKey ThirdPartyUserAccountLock(int userId) => new CacheKey(CacheTypes.CacheServer, nameof(ThirdPartyUserAccountLock), DbIndexes.Helper);

        public static CacheKey ActiveFrontsideMenu => new CacheKey(CacheTypes.LocalAndCacheServer, $"{nameof(ActiveFrontsideMenu)}");

        public static CacheKey MobileApiGameLobbyMenu => new CacheKey(CacheTypes.LocalAndCacheServer, $"{nameof(MobileApiGameLobbyMenu)}");

        public static readonly CacheKey EVEBTokenKey = new CacheKey(CacheTypes.LocalMemory, $"{nameof(EVEBTokenKey)}");

        public static readonly CacheKey LotteryInfo = new CacheKey(CacheTypes.LocalMemory, $"{nameof(LotteryInfo)}");

        public static CacheKey ThirdPartyUserAccount(int userId) => new CacheKey(CacheTypes.LocalMemory, $"{nameof(ThirdPartyUserAccount)}:{userId}");

        public static CacheKey TransferOutAllAmount(int userId) => new CacheKey(CacheTypes.CacheServer, $"{nameof(TransferOutAllAmount)}:{userId}");

        public static CacheKey TransferOutBySingleProduct(int userId, PlatformProduct product) => new CacheKey(CacheTypes.CacheServer, $"{nameof(TransferOutBySingleProduct)}:{userId}:{product.Value}");

        public static readonly CacheKey LotteryTypeMap = new CacheKey(CacheTypes.LocalMemory, nameof(LotteryTypeMap));

        public static readonly CacheKey PlayTypeNameMap = new CacheKey(CacheTypes.LocalMemory, nameof(PlayTypeNameMap));

        public static readonly CacheKey PlayTypeRadioNameMap = new CacheKey(CacheTypes.LocalMemory, nameof(PlayTypeRadioNameMap));

        public static CacheKey TPAnchors(PlatformProduct platformProduct) => new CacheKey(CacheTypes.CacheServer, $"{nameof(TPAnchors)}:{platformProduct}");

        public static CacheKey DunplicateTPAnchors(PlatformProduct platformProduct) => new CacheKey(CacheTypes.LocalMemory, $"{nameof(DunplicateTPAnchors)}:{platformProduct}");

        public static CacheKey CheckUserAllowedLogin(string userKey) => new CacheKey(CacheTypes.LocalMemory, $"{nameof(CheckUserAllowedLogin)}:{userKey}");

        public static CacheKey ActiveGameLobbyList(GameLobbyType gameLobbyType)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(ActiveGameLobbyList)}:{gameLobbyType.Value}");

        public static CacheKey ActiveGameLobbyMap(GameLobbyType gameLobbyType)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(ActiveGameLobbyMap)}:{gameLobbyType.Value}");

        public static CacheKey ApiGameList(PlatformProduct product)
               => new CacheKey(CacheTypes.LocalMemory, $"{nameof(ApiGameList)}:{product.Value}");

        public static CacheKey BackSideUser(int userId)
            => new CacheKey(CacheTypes.LocalAndCacheServer, $"{nameof(BackSideUser)}:{userId}", DbIndexes.BackSideWeb);

        public static CacheKey BackSideUserMenu(int userId)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(BackSideUserMenu)}:{userId}", DbIndexes.BackSideWeb);

        public static CacheKey BackSideUserRole()
            => new CacheKey(CacheTypes.LocalMemory, nameof(BackSideUserRole), DbIndexes.BackSideWeb);

        public static CacheKey CheckUserPasswordExpiration(int userId)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(CheckUserPasswordExpiration)}:{userId}", DbIndexes.BackSideWeb);

        public static CacheKey AllowExecutingActionWithLock(string key)
            => new CacheKey(CacheTypes.CacheServer, $"{nameof(AllowExecutingActionWithLock)}:{key}");

        public static CacheKey AllowExecutingActionBySingleServer(string key)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(AllowExecutingActionBySingleServer)}:{key}");

        public static CacheKey AllowExecutingActionByMultipleServers(string key)
            => new CacheKey(CacheTypes.LocalAndCacheServer, $"{nameof(AllowExecutingActionByMultipleServers)}:{key}");

        public static CacheKey UserExists(int userId)
            => new CacheKey(CacheTypes.LocalAndCacheServer, $"{nameof(UserExists)}:{userId}");

        public static CacheKey IMOneJackpotAmount(string typeName)
            => new CacheKey(CacheTypes.LocalAndCacheServer, $"{nameof(IMOneJackpotAmount)}:{typeName}");

        public static CacheKey UpdateTPGameUserScore(PlatformProduct product, int userId)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(UpdateTPGameUserScore)}:{product}:{userId}");

        public static CacheKey TPGameLaunchURLHTML(string token)
            => new CacheKey(CacheTypes.CacheServer, $"{nameof(TPGameLaunchURLHTML)}:{token}");

        public static CacheKey LastMessageInfokey(int ownerUserID, int publishUserID)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(LastMessageInfokey)}:{ownerUserID}:{publishUserID}");

        public static CacheKey StaticFileVersion(JxApplication application)
            => new CacheKey(CacheTypes.CacheServer, $"{nameof(StaticFileVersion)}:{application}", DbIndexes.FrontSide);

        public static CacheKey StaticFileVersionLock(JxApplication application)
            => new CacheKey(CacheTypes.CacheServer, $"{nameof(StaticFileVersionLock)}:{application}", DbIndexes.Helper);

        public static CacheKey RequestRateLimiter(string identity, string path)
            => new CacheKey(CacheTypes.LocalMemory, $"{nameof(RequestRateLimiter)}:{identity}:{path}", DbIndexes.FrontSide);
    }
}