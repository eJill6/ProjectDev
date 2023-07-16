using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService
{
    public abstract class BaseTPGameAccountService : BaseService, ITPGameAccountService, ITPGameAccountReadService
    {
        private readonly IThirdPartyUserAccountRep _thirdPartyUserAccountRep;

        private static readonly Dictionary<string, ITPGameUserInfoService> s_tpGameUserInfoServiceMap = new Dictionary<string, ITPGameUserInfoService>();

        private readonly IPlatformProductService _platformProductService;

        private readonly IJxCacheService _jxCacheService;

        private static readonly object s_locker = new object();

        private readonly IUserInfoRep _userInfoRep;

        protected IUserInfoRep UserInfoRep => _userInfoRep;

        /// <summary>預設 前綴詞</summary>
        protected abstract string DefaultTPGameAccountPrefixCode { get; }

        /// <summary>
        /// 利用規則推算的方式取得第三方帳號
        /// </summary>
        public abstract string GetTPGameAccountByRule(PlatformProduct platformProduct, int userId);

        /// <summary>
        /// 從第三方帳號反推平台帳號
        /// </summary>
        protected abstract BaseBasicUserInfo GetLocalAccountByRule(PlatformProduct platformProduct, string tpGameAccount);

        public BaseTPGameAccountService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _thirdPartyUserAccountRep = ResolveJxBackendService<IThirdPartyUserAccountRep>();
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);
            InitTPGameUserInfoServiceMap();
        }

        public List<UserProductScore> GetAllTPGameUserScores(SearchAllGameUserScoresParam param)
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.AllTPGameUserScores(param.UserID),
                CacheSeconds = 180,
                IsForceRefresh = param.IsForcedRefresh,
                IsSlidingExpiration = false,
                IsCloneInstance = false
            };

            List<UserProductScore> returnResult = _jxCacheService.GetCache(searchCacheParam, () =>
            {
                List<UserProductScore> userProductScores = new List<UserProductScore>();
                UserInfo userInfo = UserInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo() { UserID = param.UserID });

                if (userInfo == null)
                {
                    return userProductScores;
                }

                Dictionary<PlatformProduct, BaseTPGameUserInfo> productUserMap = _thirdPartyUserAccountRep.GetAllTPGameUserInfoMap(param.UserID);

                foreach (KeyValuePair<string, ITPGameUserInfoService> keyValuePair in s_tpGameUserInfoServiceMap)
                {
                    string productCode = keyValuePair.Key;
                    PlatformProduct product = _platformProductService.GetSingle(productCode);
                    productUserMap.TryGetValue(product, out BaseTPGameUserInfo baseTPGameUserInfo);

                    if (baseTPGameUserInfo == null)
                    {
                        baseTPGameUserInfo = new BaseTPGameUserInfo();
                    }

                    userProductScores.Add(new UserProductScore()
                    {
                        ProductCode = productCode,
                        ProductName = _platformProductService.GetName(productCode),
                        AvailableScores = baseTPGameUserInfo.AvailableScores,
                        FreezeScores = baseTPGameUserInfo.FreezeScores,
                        TransferIn = baseTPGameUserInfo.TransferIn,
                    });
                }

                if (param.IsIncludeMainScores)
                {
                    userProductScores.Add(new UserProductScore()
                    {
                        ProductCode = PlatformProduct.Lottery.Value,
                        ProductName = _platformProductService.GetName(PlatformProduct.Lottery.Value),
                        AvailableScores = userInfo.AvailableScores.GetValueOrDefault(),
                        FreezeScores = userInfo.FreezeScores.GetValueOrDefault(),
                        TransferIn = 0,
                    });
                }

                return userProductScores;
            });

            return returnResult;
        }

        public BaseReturnDataModel<long> Create(int userId, PlatformProduct platformProduct, string tpGameAccount, string tpGamePassword)
        {
            //避免外部用cache判斷造成寫入的時候資料重複,這邊在實際重取一次
            if (_thirdPartyUserAccountRep.GetSingleByKey(
                InlodbType.Inlodb,
                new ThirdPartyUserAccount() { UserID = userId, ThirdPartyType = platformProduct.Value }) != null)
            {
                return new BaseReturnDataModel<long>(ReturnCode.Success);
            }

            BaseReturnDataModel<long> returnModel = _thirdPartyUserAccountRep.CreateByProcedure(new ThirdPartyUserAccount()
            {
                UserID = userId,
                ThirdPartyType = platformProduct.Value,
                Account = tpGameAccount,
                Creator = EnvLoginUser.LoginUser.UserId.ToString(),
                CreateTime = DateTime.Now,
                Updater = EnvLoginUser.LoginUser.UserId.ToString(),
                UpdateTime = DateTime.Now,
                Password = tpGamePassword
            });

            if (returnModel.IsSuccess)
            {
                CacheKey cacheKey = CacheKey.ThirdPartyUserAccount(userId);
                _jxCacheService.RemoveCache(cacheKey);
            }

            return returnModel;
        }

        public BaseReturnModel UpdatePassword(int userId, PlatformProduct platformProduct, string tpGamePassword)
        {
            //避免外部用cache判斷造成寫入的時候資料重複,這邊在實際重取一次
            ThirdPartyUserAccount thirdPartyUserAccount = _thirdPartyUserAccountRep.GetSingleByKey(
                InlodbType.Inlodb,
                new ThirdPartyUserAccount() { UserID = userId, ThirdPartyType = platformProduct.Value });

            if (thirdPartyUserAccount == null)
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            thirdPartyUserAccount.Password = tpGamePassword;

            bool isSuccess = _thirdPartyUserAccountRep.UpdateByProcedure(thirdPartyUserAccount);

            if (!isSuccess)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            CacheKey cacheKey = CacheKey.ThirdPartyUserAccount(userId);
            _jxCacheService.RemoveCache(cacheKey);

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>
        /// 檢查第三方帳號表是否存在
        /// </summary>
        public bool CheckTPGAccountExist(int userId, PlatformProduct searchProduct)
        {
            ThirdPartyUserAccount thirdPartyUserAccount = GetThirdPartyUserAccount(userId, searchProduct);
            return thirdPartyUserAccount != null;
        }

        public ThirdPartyUserAccount GetThirdPartyUserAccount(int userId, PlatformProduct searchProduct)
        {
            return GetSingleThirdPartyUserAccount(userId, searchProduct);
        }

        /// <summary>
        /// 從平台帳號查第三方帳號與餘額
        /// </summary>
        public BaseReturnDataModel<UserAccountSearchResult> GetByLocalAccount(int userId)
        {
            UserInfo userInfo = UserInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo { UserID = userId });

            if (userInfo == null)
            {
                return new BaseReturnDataModel<UserAccountSearchResult>(ReturnCode.SearchResultIsEmpty);
            }

            var userAccountSearchReault = new UserAccountSearchResult()
            {
                // 取得主帳戶餘額
                PlatformAccountSearchResult = new PlatformAccountSearchResult
                {
                    UserID = userInfo.UserID,
                    UserName = userInfo.UserName,
                    AvailableScore = userInfo.AvailableScores.GetValueOrDefault(),
                }
            };

            //先找ThirdPartyUserAccount中有的資料
            List<ThirdPartyUserAccount> thirdPartyUserAccounts = GetUserThirdPartyUserAccounts(userId)
                .Where(w => GetNonSelfProduct().Any(p => p.Value == w.ThirdPartyType))
                .ToList();

            Dictionary<PlatformProduct, BaseTPGameUserInfo> productUserMap = _thirdPartyUserAccountRep.GetAllTPGameUserInfoMap(userId);

            thirdPartyUserAccounts.ForEach(f =>
            {
                var platformProduct = PlatformProduct.GetSingle(f.ThirdPartyType);

                if (platformProduct != null)
                {
                    decimal tpGameAvailableScore = 0;
                    decimal tpGameFreezeScore = 0;

                    if (productUserMap.TryGetValue(platformProduct, out BaseTPGameUserInfo tpGameUserInfo))
                    {
                        tpGameAvailableScore = tpGameUserInfo.AvailableScores;
                        tpGameFreezeScore = tpGameUserInfo.FreezeScores;
                    }

                    userAccountSearchReault.TPGameAccountSearchResults.Add(new TPGameAccountSearchResult()
                    {
                        LocalUserID = userId,
                        TPGameProductCode = platformProduct.Value,
                        TPGameProductName = _platformProductService.GetName(platformProduct.Value),
                        TPGameAccount = f.Account,
                        TPGameAvailableScore = tpGameAvailableScore,
                        TPGameFreezeScore = tpGameFreezeScore,
                        Sort = platformProduct.Sort
                    });
                }
            });

            //把缺資料的產品再找各個第三方的資料, 用規則推算
            List<string> existProductCodes = thirdPartyUserAccounts.Select(s => s.ThirdPartyType).ToList();
            AddDiffTPGameAccountSearchResult(userId, existProductCodes, userAccountSearchReault.TPGameAccountSearchResults);
            //排序, 不要讓資料查詢不同用戶時顯示的產品順序不同
            userAccountSearchReault.TPGameAccountSearchResults = userAccountSearchReault.TPGameAccountSearchResults.OrderBy(o => o.Sort).ToList();

            return new BaseReturnDataModel<UserAccountSearchResult>(ReturnCode.Success, userAccountSearchReault);
        }

        /// <summary>
        /// 從指定平台帳號查第三方帳號
        /// </summary>
        public CreateRemoteAccountParam GetTPGameAccountByLocalAccount(int userId, PlatformProduct searchProduct, out bool isDbExists)
        {
            ThirdPartyUserAccount thirdPartyUserAccount = GetSingleThirdPartyUserAccount(userId, searchProduct);
            isDbExists = thirdPartyUserAccount != null;

            if (thirdPartyUserAccount == null)
            {
                thirdPartyUserAccount = new ThirdPartyUserAccount()
                {
                    Account = GetTPGameAccountByRule(searchProduct, userId)
                };
            }

            return new CreateRemoteAccountParam()
            {
                TPGameAccount = thirdPartyUserAccount.Account,
                TPGamePassword = thirdPartyUserAccount.Password
            };
        }

        /// <summary>
        /// 取得單一產品現有帳號,包含資料表與推算的
        /// </summary>
        public Dictionary<string, int> GetUserIdsByTPGameAccounts(PlatformProduct product, HashSet<string> tpGameAccounts)
        {
            return GetUsersByTPGameAccounts(product, tpGameAccounts).ToDictionary(d => d.Key, d => d.Value.UserId);
        }

        /// <summary>
        /// 取得單一產品現有帳號,包含資料表與推算的
        /// </summary>
        public Dictionary<string, BaseBasicUserInfo> GetUsersByTPGameAccounts(PlatformProduct product, HashSet<string> tpGameAccounts)
        {
            List<ThirdPartyUserAccount> thirdPartyUserAccounts = _thirdPartyUserAccountRep.GetListByTPGameAccount(product, tpGameAccounts);

            Dictionary<string, BaseBasicUserInfo> resultMap = thirdPartyUserAccounts.ToDictionary(
                d => d.Account,
                d => new BaseBasicUserInfo() { UserId = d.UserID });

            //沒查到的用規則推算
            List<string> tpGameAccountsByRule = tpGameAccounts
                .Where(w => !thirdPartyUserAccounts.Any(a => a.Account.Equals(w, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            var userIdsByRule = new List<int>();
            var userIdMap = new Dictionary<int, string>();

            tpGameAccountsByRule.ForEach(account =>
            {
                if (!resultMap.ContainsKey(account))
                {
                    BaseBasicUserInfo user = GetLocalAccountByRule(product, account); //這邊不要一筆一筆查,會過慢,改成批次查詢

                    if (user != null)
                    {
                        userIdsByRule.Add(user.UserId);
                        userIdMap.Add(user.UserId, account);
                    }
                }
            });

            List<BaseBasicUserInfo> existUsers = UserInfoRep.GetUserInfos(userIdsByRule)
                .Select(s => new BaseBasicUserInfo() { UserId = s.UserID })
                .ToList();

            foreach (BaseBasicUserInfo existUser in existUsers)
            {
                resultMap.Add(userIdMap[existUser.UserId], existUser);
            }

            return resultMap;
        }

        public Dictionary<PlatformProduct, BaseBasicUserInfo> GetUsersByTPGameAccount(string tpGameAccount)
        {
            List<ThirdPartyUserAccount> thirdPartyUserAccounts = _thirdPartyUserAccountRep.GetListByTPGameAccount(product: null, new HashSet<string>() { tpGameAccount });

            Dictionary<PlatformProduct, BaseBasicUserInfo> resultMap = thirdPartyUserAccounts.ToDictionary(
                d => _platformProductService.GetSingle(d.ThirdPartyType),
                d => new BaseBasicUserInfo() { UserId = d.UserID });

            List<int> userIdsByRule = resultMap.Select(s => s.Value.UserId).ToList();
            var userIdMap = new Dictionary<int, string>();

            foreach (PlatformProduct platformProduct in _platformProductService.GetNonSelfProduct())
            {
                if (resultMap.ContainsKey(platformProduct))
                {
                    continue;
                }

                BaseBasicUserInfo user = GetLocalAccountByRule(platformProduct, tpGameAccount);

                if (user != null)
                {
                    resultMap.Add(platformProduct, user);
                    userIdsByRule.Add(user.UserId);

                    if (!userIdMap.ContainsKey(user.UserId))
                    {
                        userIdMap.Add(user.UserId, tpGameAccount);
                    }
                }
            }

            HashSet<int> existUserIds = UserInfoRep.GetUserInfos(userIdsByRule.Distinct().ToList())
                .Select(s => s.UserID).ConvertToHashSet();

            resultMap = resultMap.Where(w => existUserIds.Contains(w.Value.UserId)).ToDictionary(d => d.Key, d => d.Value);

            return resultMap;
        }

        /// <summary>
        /// 利用規則產生出第三方使用的userName
        /// </summary>
        public string GetTPGameUserNameByRule(PlatformProduct platformProduct, int userId)
        {
            //Default encoded string
            string tpGameUserName = null;
            tpGameUserName = GetPrefixAndEnvCode(EnvCode.AccountPrefixCode, userId);

            return tpGameUserName;
        }

        private ThirdPartyUserAccount GetSingleThirdPartyUserAccount(int userId, PlatformProduct searchProduct)
        {
            List<ThirdPartyUserAccount> cachedThirdPartyUserAccounts = GetUserThirdPartyUserAccounts(userId);
            ThirdPartyUserAccount thirdPartyUserAccount = null;

            if (cachedThirdPartyUserAccounts.AnyAndNotNull())
            {
                thirdPartyUserAccount = cachedThirdPartyUserAccounts.SingleOrDefault(s => s.ThirdPartyType == searchProduct);
            }

            if (thirdPartyUserAccount != null)
            {
                return thirdPartyUserAccount;
            }

            return _thirdPartyUserAccountRep.GetSingleByUserId(userId, searchProduct);
        }

        private List<ThirdPartyUserAccount> GetUserThirdPartyUserAccounts(int userId)
        {
            List<ThirdPartyUserAccount> thirdPartyUserAccounts = _jxCacheService
                .GetCache(CacheKey.ThirdPartyUserAccount(userId), () => _thirdPartyUserAccountRep.GetListByUserId(userId));

            return thirdPartyUserAccounts;
        }

        /// <summary> 從第三方帳號查平台帳號與餘額 </summary>
        public BaseReturnDataModel<UserAccountSearchResult> GetByTPGameAccount(PlatformProduct searchProduct, string tpGameAccount)
        {
            if (tpGameAccount.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<UserAccountSearchResult>(ReturnCode.DataIsNotCompleted);
            }

            //先篩選要查詢哪些第三方產品
            List<PlatformProduct> searchProducts = new List<PlatformProduct>();
            //避免異動到快取中的物件
            searchProducts.AddRange(GetNonSelfProduct());

            if (searchProduct != null)
            {
                searchProducts = searchProducts.Where(w => w.Value == searchProduct.Value).ToList();
            }

            //先查詢所有可能的資料
            var searchTPGameAccounts = new HashSet<string>
            {
                tpGameAccount
            };

            searchProducts.ForEach(product =>
            {
                var platformProductSettingService = ResolveKeyed<IPlatformProductSettingService>(product);

                if (platformProductSettingService.IsParseUserIdFromSuffix)
                {
                    int? userId = GetUserIdFromSuffix(tpGameAccount);

                    if (userId.HasValue)
                    {
                        string formatAccount = GetTPGameAccountByRule(product, userId.Value);

                        if (!searchTPGameAccounts.Contains(formatAccount))
                        {
                            searchTPGameAccounts.Add(formatAccount);
                        }
                    }
                }
            });

            List<ThirdPartyUserAccount> thirdPartyUserAccounts = _thirdPartyUserAccountRep.GetListByTPGameAccount(searchTPGameAccounts);
            //資料表內無資料的,查詢各個第三方表
            //把缺資料的產品再找各個第三方的資料, 用規則推算
            List<string> existProductCodes = thirdPartyUserAccounts.Select(s => s.ThirdPartyType).ToList();

            List<PlatformProduct> diffPlatformProducts = GetNonSelfProduct()
                .Where(w => (searchProduct == null || (searchProduct != null && w.Value == searchProduct.Value)) &&
                !existProductCodes.Contains(w.Value))
                .ToList();

            var diffTPGameAccountSearchResults = new List<TPGameAccountSearchResult>();
            var userProductUserMap = new Dictionary<int, Dictionary<PlatformProduct, BaseTPGameUserInfo>>();

            foreach (PlatformProduct diffPlatformProduct in diffPlatformProducts)
            {
                //先反轉第三方帳號回平台帳號
                BaseBasicUserInfo basicUserInfo = GetLocalAccountByRule(diffPlatformProduct, tpGameAccount);

                if (basicUserInfo == null)
                {
                    continue;
                }

                //找第三方userInfo有無資料
                if (!userProductUserMap.TryGetValue(basicUserInfo.UserId, out Dictionary<PlatformProduct, BaseTPGameUserInfo> productUserMap))
                {
                    productUserMap = _thirdPartyUserAccountRep.GetAllTPGameUserInfoMap(basicUserInfo.UserId);
                    userProductUserMap.Add(basicUserInfo.UserId, productUserMap);
                }

                if (!productUserMap.TryGetValue(diffPlatformProduct, out BaseTPGameUserInfo tpGameUserInfo))
                {
                    continue;
                }

                diffTPGameAccountSearchResults.Add(new TPGameAccountSearchResult()
                {
                    LocalUserID = basicUserInfo.UserId,
                    TPGameAccount = GetTPGameAccountByRule(diffPlatformProduct, basicUserInfo.UserId),
                    TPGameProductCode = diffPlatformProduct.Value,
                    TPGameAvailableScore = tpGameUserInfo.AvailableScores,
                    TPGameFreezeScore = tpGameUserInfo.FreezeScores,
                    TPGameProductName = _platformProductService.GetName(diffPlatformProduct.Value),
                    Sort = diffPlatformProduct.Sort
                });
            }

            if (searchProduct != null)
            {
                thirdPartyUserAccounts = thirdPartyUserAccounts.Where(w => w.ThirdPartyType == searchProduct.Value).ToList();
            }

            if (!thirdPartyUserAccounts.AnyAndNotNull())
            {
                return new BaseReturnDataModel<UserAccountSearchResult>(ReturnCode.SearchResultIsEmpty);
            }

            thirdPartyUserAccounts.RemoveAll(r => !s_tpGameUserInfoServiceMap.ContainsKey(r.ThirdPartyType));

            var tpGameAccountSearchResults = thirdPartyUserAccounts.Select(s =>
                {
                    PlatformProduct product = PlatformProduct.GetSingle(s.ThirdPartyType);
                    UserScore userScore = GetTPGameUserScore(product.Value, s.UserID);

                    return new TPGameAccountSearchResult()
                    {
                        LocalUserID = s.UserID,
                        TPGameAccount = s.Account,
                        TPGameProductCode = s.ThirdPartyType,
                        TPGameAvailableScore = userScore.AvailableScores,
                        TPGameFreezeScore = userScore.FreezeScores,
                        TPGameProductName = _platformProductService.GetName(product.Value),
                        Sort = product.Sort
                    };
                }).ToList();

            tpGameAccountSearchResults.AddRange(diffTPGameAccountSearchResults);
            tpGameAccountSearchResults = tpGameAccountSearchResults.OrderBy(o => o.Sort).ToList();

            var userAccountSearchReault = new UserAccountSearchResult
            {
                // 第三方帳戶與餘額
                TPGameAccountSearchResults = tpGameAccountSearchResults
            };

            if (!userAccountSearchReault.TPGameAccountSearchResults.Any())
            {
                userAccountSearchReault.PlatformAccountSearchResult = new PlatformAccountSearchResult();

                return new BaseReturnDataModel<UserAccountSearchResult>(ReturnCode.SearchResultIsEmpty, userAccountSearchReault);
            }

            // 主帳戶與餘額
            int localUserID = userAccountSearchReault.TPGameAccountSearchResults.First().LocalUserID;
            userAccountSearchReault.PlatformAccountSearchResult = GetPlatformAccountSearchResult(localUserID);

            return new BaseReturnDataModel<UserAccountSearchResult>(ReturnCode.Success, userAccountSearchReault);
        }

        protected int? GetUserIdFromSuffix(string tpGameAccount)
        {
            string defaultAccountPrefix = GetDefaultTPGameAccountPrefix(EnvCode.AccountPrefixCode);

            return GetUserIdFromSuffix(tpGameAccount, defaultAccountPrefix);
        }

        protected int? GetUserIdFromSuffix(string tpGameAccount, string accountPrefix)
        {
            int subStringStartIndex = tpGameAccount.LastIndexOf(accountPrefix, StringComparison.OrdinalIgnoreCase);

            if (subStringStartIndex == -1)
            {
                return null;
            }

            string parseData = tpGameAccount.Substring(subStringStartIndex + accountPrefix.Length);

            if (int.TryParse(parseData, out int userId))
            {
                return userId;
            }

            return null;
        }

        private void AddDiffTPGameAccountSearchResult(int userId, List<string> existProductCodes,
            List<TPGameAccountSearchResult> tpGameAccountSearchResults)
        {
            //把缺資料的產品再找各個第三方的資料, 用規則推算
            List<PlatformProduct> diffPlatformProducts = GetNonSelfProduct().Where(w => !existProductCodes.Contains(w.Value)).ToList();

            foreach (PlatformProduct platformProduct in diffPlatformProducts)
            {
                //找第三方userInfo有無資料
                Dictionary<PlatformProduct, BaseTPGameUserInfo> productUserMap = _thirdPartyUserAccountRep.GetAllTPGameUserInfoMap(userId);
                bool isUserExists = productUserMap.ContainsKey(platformProduct);

                if (!isUserExists)
                {
                    continue;
                }

                string tpGameAccount = GetTPGameAccountByRule(platformProduct, userId);

                if (!tpGameAccount.IsNullOrEmpty())
                {
                    if (!productUserMap.TryGetValue(platformProduct, out BaseTPGameUserInfo baseTPGameUserInfo))
                    {
                        continue;
                    }

                    tpGameAccountSearchResults.Add(new TPGameAccountSearchResult()
                    {
                        LocalUserID = userId,
                        TPGameProductCode = platformProduct.Value,
                        TPGameProductName = _platformProductService.GetName(platformProduct.Value),
                        TPGameAccount = GetTPGameAccountByRule(platformProduct, userId),
                        TPGameAvailableScore = baseTPGameUserInfo.AvailableScores,
                        TPGameFreezeScore = baseTPGameUserInfo.FreezeScores,
                        Sort = platformProduct.Sort
                    });
                }
            }
        }

        private void InitTPGameUserInfoServiceMap()
        {
            lock (s_locker)
            {
                if (s_tpGameUserInfoServiceMap.Any())
                {
                    return;
                }

                //因為PlatformProduct有加入彩票，所以這裡只取第三方的產品列表
                foreach (PlatformProduct product in GetNonSelfProduct())
                {
                    if (product.UserInfoServiceType == null)
                    {
                        continue;
                    }

                    s_tpGameUserInfoServiceMap.Add(product.Value, ResolveJxBackendService<ITPGameUserInfoService>(product));
                }
            }
        }

        private UserScore GetTPGameUserScore(string productCode, int userId)
        {
            ITPGameUserInfoService tpGameUserInfoService = s_tpGameUserInfoServiceMap[productCode];
            BaseTPGameUserInfo baseTPGameUserInfo = tpGameUserInfoService.GetTPGameUserInfo(userId);
            var userScore = new UserScore();

            if (baseTPGameUserInfo != null)
            {
                userScore.AvailableScores = baseTPGameUserInfo.AvailableScores;
                userScore.FreezeScores = baseTPGameUserInfo.FreezeScores;
            }

            return userScore;
        }

        /// <summary> 查出用戶主帳戶餘額 </summary>
        private PlatformAccountSearchResult GetPlatformAccountSearchResult(int userId)
        {
            UserInfo userInfo = UserInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo { UserID = userId });

            return new PlatformAccountSearchResult
            {
                UserID = userInfo.UserID,
                UserName = userInfo.UserName,
                AvailableScore = userInfo.AvailableScores.GetValueOrDefault()
            };
        }

        private List<PlatformProduct> GetNonSelfProduct()
        {
            return _platformProductService.GetNonSelfProduct();
        }

        protected string GetPrefixAndEnvCode(string environmentPrefixCode, int account)
        {
            return GetPrefixAndEnvCode(environmentPrefixCode, account.ToString());
        }

        protected string GetPrefixAndEnvCode(string environmentPrefixCode, string account)
        {
            return GetDefaultTPGameAccountPrefix(environmentPrefixCode) + account;
        }

        protected string GetDefaultTPGameAccountPrefix(string environmentAccountPrefixCode)
        {
            return $"{DefaultTPGameAccountPrefixCode}{environmentAccountPrefixCode}_";
        }
    }

    public class TPGameAccountMSLService : BaseTPGameAccountService
    {
        public TPGameAccountMSLService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override string DefaultTPGameAccountPrefixCode => "msl";

        public override string GetTPGameAccountByRule(PlatformProduct platformProduct, int userId)
        {
            string tpGameAccount;

            if (platformProduct == PlatformProduct.OBEB)
            {
                string prefixAndEnvCodeUserId = GetPrefixAndEnvCode(EnvCode.AccountPrefixCode, userId);
                tpGameAccount = $"{OBEBSharedAppSetting.MerchantCode}{prefixAndEnvCodeUserId}";
            }
            else if (platformProduct == PlatformProduct.JDBFI || platformProduct == PlatformProduct.AWCSP)
            {
                tpGameAccount = $"{DefaultTPGameAccountPrefixCode}{EnvCode.AccountPrefixCode}{userId}";
            }
            else
            {
                //Default encoded string
                tpGameAccount = GetPrefixAndEnvCode(EnvCode.AccountPrefixCode, userId);
            }

            return tpGameAccount;
        }

        protected override BaseBasicUserInfo GetLocalAccountByRule(PlatformProduct platformProduct, string tpGameAccount)
        {
            var basicUserInfo = new BaseBasicUserInfo();
            var platformProductSettingService = ResolveKeyed<IPlatformProductSettingService>(platformProduct);

            if (platformProductSettingService.IsParseUserIdFromSuffix)
            {
                int? userId = GetUserIdFromSuffix(tpGameAccount);

                if (userId.HasValue)
                {
                    basicUserInfo.UserId = userId.Value;
                }
            }
            else if (platformProduct == PlatformProduct.JDBFI)
            {
                string accountPrefix = $"{DefaultTPGameAccountPrefixCode}{EnvCode.AccountPrefixCode}";

                int? userId = GetUserIdFromSuffix(tpGameAccount, accountPrefix);

                if (userId.HasValue)
                {
                    basicUserInfo.UserId = userId.Value;
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            if (basicUserInfo != null && basicUserInfo.UserId == 0)
            {
                basicUserInfo = null;
            }

            return basicUserInfo;
        }
    }
}