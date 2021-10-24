using JxBackendService.Model.Enums;
using JxBackendService.Common.Extensions;
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

        private CacheKey() { }

        private CacheKey(CacheTypes cacheType, string value)
        {
            CacheType = cacheType;
            Value = value;
        }

        /// <summary>
        /// 為了相容SV的舊版CacheBase,新開發的請勿使用此方法
        /// </summary>
        public static CacheKey GetKeyForOldService(CacheTypes cacheType, string key) => new CacheKey(cacheType, key)
        {
            IsFormatKey = false,
            IsDoSerialize = false
        };

        /// <summary>
        /// 為了相容SV的舊版CacheBase,新開發的請勿使用此方法
        /// </summary>
        public static CacheKey GetKeyForDistributeMemCache(string key) => new CacheKey(CacheTypes.CacheServer, key)
        {            
            IsFormatKey = false,
            IsDoSerialize = false,
            DbIndex = DbIndexes.ReplaceDistributeMemCache
        };

        public static CacheKey GetBackSideUserInfoKey(string loginHash) => new CacheKey(CacheTypes.CacheServer, loginHash);

        public static CacheKey GetFrontSideUserInfoKey(string loginHash) => new CacheKey(CacheTypes.LocalAndCacheServer, loginHash)
        {
            IsFormatKey = false,
            IsDoSerialize = false
        };

        public static CacheKey GetUserOnlineDetails(int userId) => new CacheKey(CacheTypes.CacheServer, $"{nameof(GetUserOnlineDetails)}_{userId}");

        public static CacheKey AllGameTypeInfo => new CacheKey(CacheTypes.CacheServer, nameof(AllGameTypeInfo));

        public static CacheKey GetIsAllowSearchUserName(int loginUserId, string queryUserName) =>
            new CacheKey(CacheTypes.LocalMemory, $"{nameof(GetIsAllowSearchUserName)}_{loginUserId}_{queryUserName.ToTrimString().ToLower()}");

        public static CacheKey GetIsAllowSearchUserId(int loginUserId, int queryUserId) =>
            new CacheKey(CacheTypes.LocalMemory, $"{nameof(GetIsAllowSearchUserId)}_{loginUserId}_{queryUserId}");

        public static readonly CacheKey IPSystemInfoSuspend = new CacheKey(CacheTypes.LocalMemory, nameof(IPSystemInfoSuspend));

        public static CacheKey AllTPGameUserScores(int userId) => new CacheKey(CacheTypes.LocalMemory, $"{nameof(AllTPGameUserScores)}_{userId}");

        public static CacheKey FrontsideMenuGrouped => new CacheKey(CacheTypes.LocalMemory, $"{nameof(FrontsideMenuGrouped)}");

        public static CacheKey FrontsideMenuList => new CacheKey(CacheTypes.LocalMemory, $"{nameof(FrontsideMenuList)}");

        public static CacheKey ActiveFrontsideMenuList => new CacheKey(CacheTypes.LocalMemory, $"{nameof(ActiveFrontsideMenuList)}");

        public static CacheKey GameMenusForApi => new CacheKey(CacheTypes.LocalMemory, $"{nameof(GameMenusForApi)}");

        public static CacheKey LotteryRecommendInfo => new CacheKey(CacheTypes.LocalMemory, $"{nameof(LotteryRecommendInfo)}");

        public static CacheKey LotteryCenterInfo() => new CacheKey(CacheTypes.LocalMemory, $"{nameof(LotteryCenterInfo)}");

        public static CacheKey AllFirstChildUserInfo(int userId) => new CacheKey(CacheTypes.CacheServer, $"{nameof(AllFirstChildUserInfo)}_{userId}");

        public static CacheKey GroupChatRoomUserIdList(int chatRoomId) => new CacheKey(CacheTypes.CacheServer, $"{nameof(GroupChatRoomUserIdList)}_{chatRoomId}");

        public static CacheKey ChatRoomConfigSetting => new CacheKey(CacheTypes.CacheServer, $"{nameof(ChatRoomConfigSetting)}");

        public static CacheKey ChatRoomUserSendMessageCountInOneMinute(int userId) => new CacheKey(CacheTypes.CacheServer, $"{nameof(ChatRoomUserSendMessageCountInOneMinute)}_{userId}");

        public static CacheKey EVEBTokenKey => new CacheKey(CacheTypes.LocalMemory, $"{nameof(EVEBTokenKey)}");

        public static CacheKey IpValidation(string ipAddress, int webActionTypeValue) => new CacheKey(CacheTypes.CacheServer, $"{ipAddress}_{webActionTypeValue}");

        public static CacheKey CheckUserAllowedLogin(string userKey) => new CacheKey(CacheTypes.LocalMemory, $"{nameof(CheckUserAllowedLogin)}_{userKey}");

        public static CacheKey LotteryInfo => new CacheKey(CacheTypes.LocalMemory, $"{nameof(LotteryInfo)}");

        public static CacheKey GraphicValidateCode(int actionType, string userIdentityKey) => new CacheKey(CacheTypes.CacheServer, $"{nameof(GraphicValidateCode)}_{HashExtension.MD5(actionType + "_" + userIdentityKey)}");

        public static CacheKey GraphicValidateCodeIntervalCheck(int actionType, string userIdentityKey) => new CacheKey(CacheTypes.LocalMemory, $"{nameof(GraphicValidateCodeIntervalCheck)}_{HashExtension.MD5(actionType + "_" + userIdentityKey)}");

        public static CacheKey GetApolloServiceTypeInfos(int userLevel) => new CacheKey(CacheTypes.LocalMemory, $"{nameof(GetApolloServiceTypeInfos)}_{userLevel}");
    }
}