using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Repository;
using JxBackendService.Repository.User;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using JxBackendService.Service.Game;

namespace JxBackendService
{
    public abstract class BaseTPGameAccountService : BaseService, ITPGameAccountService, ITPGameAccountReadService
    {
        private readonly IThirdPartyUserAccountRep _thirdPartyUserAccountRep;
        private readonly Dictionary<string, ITPGameUserInfoService> _tpGameUserInfoServiceMap;
        private readonly IPlatformProductService _platformProductService;
        private readonly IJxCacheService _jxCacheService;
        protected readonly IUserInfoRep UserInfoRep;

        /// <summary>預設 前綴詞</summary>
        protected abstract string DefaultTPGameAccountPrefixCode { get; }

        /// <summary>
        /// 利用規則推算的方式取得第三方帳號
        /// </summary>
        public abstract string GetTPGameAccountByRule(PlatformProduct platformProduct, int userId, string userName);

        /// <summary>
        /// 從第三方帳號反推平台帳號
        /// </summary>
        protected abstract BasicUserInfo GetLocalAccountByRule(PlatformProduct platformProduct, string tpGameAccount, bool isFillUserName = true);

        public BaseTPGameAccountService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            UserInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _thirdPartyUserAccountRep = ResolveJxBackendService<IThirdPartyUserAccountRep>();
            _tpGameUserInfoServiceMap = new Dictionary<string, ITPGameUserInfoService>();
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(envLoginUser.Application);

            //因為PlatformProduct有加入彩票，所以這裡只取第三方的產品列表
            foreach (PlatformProduct product in GetNonSelfProductWithoutPT())
            {
                if (product.UserInfoServiceType == null)
                {
                    continue;
                }

                _tpGameUserInfoServiceMap.Add(product.Value, ResolveJxBackendService<ITPGameUserInfoService>(product));
            }
        }

        public List<UserProductScore> GetAllTPGameUserScores(int userId, bool isForcedRefresh)
        {
            var searchCacheParam = new SearchCacheParam()
            {
                Key = CacheKey.AllTPGameUserScores(userId),
                CacheSeconds = 180,
                IsForceRefresh = isForcedRefresh,
                IsSlidingExpiration = false,
                IsCloneInstance = false
            };

            List<UserProductScore> returnResult = _jxCacheService.GetCache(searchCacheParam, () =>
            {
                List<UserProductScore> userProductScores = new List<UserProductScore>();
                UserInfo userInfo = UserInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo() { UserID = userId });

                if (userInfo == null)
                {
                    return userProductScores;
                }

                foreach (KeyValuePair<string, ITPGameUserInfoService> keyValuePair in _tpGameUserInfoServiceMap)
                {
                    string productCode = keyValuePair.Key;
                    BaseTPGameUserInfo baseTPGameUserInfo = keyValuePair.Value.GetTPGameUserInfo(userId);

                    if (baseTPGameUserInfo == null)
                    {
                        baseTPGameUserInfo = new BaseTPGameUserInfo();
                    }

                    decimal allGain = userInfo.GetType().GetProperty($"{productCode}AllGain").GetValue(userInfo).ToNonNullString().ToDecimal(hasDefaultValue: true);

                    userProductScores.Add(new UserProductScore()
                    {
                        ProductCode = productCode,
                        ProductName = PlatformProduct.GetName(productCode),
                        AvailableScores = baseTPGameUserInfo.GetAvailableScores(),
                        FreezeScores = baseTPGameUserInfo.FreezeScores.GetValueOrDefault(),
                        AllGain = allGain
                    });
                }

                return userProductScores;
            });

            return returnResult;
        }

        public BaseReturnDataModel<long> Create(int userId, string userName, PlatformProduct platformProduct, string tpGameAccount)
        {
            BaseReturnDataModel<long> returnModel = _thirdPartyUserAccountRep.CreateByProcedure(new ThirdPartyUserAccount()
            {
                UserID = userId,
                UserName = userName,
                ThirdPartyType = platformProduct.Value,
                Account = tpGameAccount,
                Creator = EnvLoginUser.LoginUser.UserName,
                CreateTime = DateTime.Now,
                Updater = EnvLoginUser.LoginUser.UserName,
                UpdateTime = DateTime.Now
            });

            return returnModel;
        }

        /// <summary>
        /// 檢查第三方帳號表是否存在
        /// </summary>
        public bool CheckTPGAccountExist(int userId, PlatformProduct searchProduct)
        {
            ThirdPartyUserAccount thirdPartyUserAccount = _thirdPartyUserAccountRep.GetSingleByUserId(userId, searchProduct);
            return thirdPartyUserAccount != null;
        }

        /// <summary>
        /// 從平台帳號查第三方帳號與餘額
        /// </summary>
        public BaseReturnDataModel<UserAccountSearchReault> GetByLocalAccount(string userName)
        {
            int? userId = UserInfoRep.GetFrontSideUserId(userName);

            if (!userId.HasValue)
            {
                return new BaseReturnDataModel<UserAccountSearchReault>(ReturnCode.SearchResultIsEmpty);
            }

            var userAccountSearchReault = new UserAccountSearchReault()
            {
                // 取得主帳戶餘額
                PlatformAccountSearchResult = GetPlatformAccountSearchResult(userId.Value)
            };

            //先找ThirdPartyUserAccount中有的資料
            List<ThirdPartyUserAccount> thirdPartyUserAccounts = _thirdPartyUserAccountRep.GetListByUserId(userId.Value)
                .Where(w => GetNonSelfProductWithoutPT().Any(p => p.Value == w.ThirdPartyType)).ToList();

            thirdPartyUserAccounts.ForEach(f =>
            {
                var platformProduct = PlatformProduct.GetSingle(f.ThirdPartyType);

                if (platformProduct != null)
                {
                    string tPGameAvailableScoreText = GetTPGameAvailableScore(platformProduct.Value, userId.Value);

                    userAccountSearchReault.TPGameAccountSearchResults.Add(new TPGameAccountSearchResult()
                    {
                        LocalAccount = userName,
                        TPGameProductCode = platformProduct.Value,
                        TPGameProductName = platformProduct.Name,
                        TPGameAccount = f.Account,
                        TPGameAvailableScoreText = tPGameAvailableScoreText,
                        Sort = platformProduct.Sort
                    });
                }
            });

            //把缺資料的產品再找各個第三方的資料, 用規則推算
            List<string> existProductCodes = thirdPartyUserAccounts.Select(s => s.ThirdPartyType).ToList();
            AddDiffTPGameAccountSearchResult(userId.Value, userName, existProductCodes, userAccountSearchReault.TPGameAccountSearchResults);
            //排序, 不要讓資料查詢不同用戶時顯示的產品順序不同
            userAccountSearchReault.TPGameAccountSearchResults = userAccountSearchReault.TPGameAccountSearchResults.OrderBy(o => o.Sort).ToList();

            return new BaseReturnDataModel<UserAccountSearchReault>(ReturnCode.Success, userAccountSearchReault);
        }

        /// <summary>
        /// 從指定平台帳號查第三方帳號
        /// </summary>
        public BaseReturnDataModel<string> GetTPGameAccountByLocalAccount(int userId, PlatformProduct searchProduct)
        {
            string tpgameAccount;
            string userName = UserInfoRep.GetFrontSideUserName(userId);

            if (string.IsNullOrWhiteSpace(userName))
            {
                return new BaseReturnDataModel<string>(ReturnCode.SystemError, string.Empty);
            }

            ThirdPartyUserAccount thirdPartyUserAccount = _thirdPartyUserAccountRep.GetSingleByUserId(userId, searchProduct);

            if (thirdPartyUserAccount != null)
            {
                tpgameAccount = thirdPartyUserAccount.Account;
            }
            else
            {
                tpgameAccount = GetTPGameAccountByRule(searchProduct, userId, userName);
            }

            return new BaseReturnDataModel<string>(ReturnCode.Success, tpgameAccount);
        }

        /// <summary>
        /// 從第三方帳號查平台帳號與餘額
        /// </summary>
        public BaseReturnDataModel<UserAccountSearchReault> GetByTPGameAccount(PlatformProduct searchProduct, string tpGameAccount)
        {
            if (tpGameAccount.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<UserAccountSearchReault>(ReturnCode.DataIsNotCompleted);
            }

            //先篩選要查詢哪些第三方產品
            List<PlatformProduct> searchProducts = new List<PlatformProduct>();
            //避免異動到快取中的物件
            searchProducts.AddRange(GetNonSelfProductWithoutPT());

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
                        string formatAccount = GetTPGameAccountByRule(product, userId.Value, null);

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
            List<PlatformProduct> diffPlatformProducts = GetNonSelfProductWithoutPT()
                .Where(w => (searchProduct == null || (searchProduct != null && w.Value == searchProduct.Value)) &&
                !existProductCodes.Contains(w.Value))
                .ToList();

            foreach (PlatformProduct diffPlatformProduct in diffPlatformProducts)
            {
                //先反轉第三方帳號回平台帳號
                BasicUserInfo basicUserInfo = GetLocalAccountByRule(diffPlatformProduct, tpGameAccount);

                if (basicUserInfo == null)
                {
                    continue;
                }

                //考量後台用量不會很大, 這邊就不使用union all的方式一次查詢
                //找第三方userInfo有無資料
                ITPGameUserInfoService tpGameUserInfoService = _tpGameUserInfoServiceMap[diffPlatformProduct.Value];
                bool isUserExists = tpGameUserInfoService.IsUserExists(basicUserInfo.UserId);

                if (!isUserExists)
                {
                    continue;
                }

                thirdPartyUserAccounts.Add(new ThirdPartyUserAccount()
                {
                    UserID = basicUserInfo.UserId,
                    UserName = basicUserInfo.UserName,
                    ThirdPartyType = diffPlatformProduct.Value,
                    Account = GetTPGameAccountByRule(diffPlatformProduct, basicUserInfo.UserId, basicUserInfo.UserName)
                });
            }

            if (searchProduct != null)
            {
                thirdPartyUserAccounts = thirdPartyUserAccounts.Where(w => w.ThirdPartyType == searchProduct.Value).ToList();
            }

            //thirdPartyUserAccounts.RemoveAll(r =>
            //{
            //    PlatformProduct product = PlatformProduct.GetSingle(r.ThirdPartyType);

            //    //跟原始輸入的查詢資料做比對
            //    if (product.IsParseUserIdFromSuffix.IsNullOrEmpty())
            //    {
            //        if (!r.Account.Equals(tpGameAccount, StringComparison.OrdinalIgnoreCase))
            //        {
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        if (!(product.IsParseUserIdFromSuffix + r.Account).Equals(tpGameAccount, StringComparison.OrdinalIgnoreCase))
            //        {
            //            return true;
            //        }
            //    }

            //    return false;
            //});

            if (!thirdPartyUserAccounts.AnyAndNotNull())
            {
                return new BaseReturnDataModel<UserAccountSearchReault>(ReturnCode.SearchResultIsEmpty);
            }

            var userAccountSearchReault = new UserAccountSearchReault
            {
                // 第三方帳戶與餘額
                TPGameAccountSearchResults = thirdPartyUserAccounts
                .Select(s =>
                {
                    PlatformProduct product = PlatformProduct.GetSingle(s.ThirdPartyType);
                    string tPGameAvailableScoreText = GetTPGameAvailableScore(product.Value, s.UserID);

                    return new TPGameAccountSearchResult()
                    {
                        LocalUserID = s.UserID,
                        LocalAccount = s.UserName,
                        TPGameAccount = s.Account,
                        TPGameProductCode = s.ThirdPartyType,
                        TPGameAvailableScoreText = tPGameAvailableScoreText,
                        TPGameProductName = product.Name,
                        Sort = product.Sort
                    };
                }).OrderBy(o => o.Sort).ToList()
            };

            // 主帳戶與餘額
            int localUserID = userAccountSearchReault.TPGameAccountSearchResults.First().LocalUserID;
            userAccountSearchReault.PlatformAccountSearchResult = GetPlatformAccountSearchResult(localUserID);

            return new BaseReturnDataModel<UserAccountSearchReault>(ReturnCode.Success, userAccountSearchReault);
        }

        /// <summary>
        /// 取得單一產品現有帳號,包含資料表與推算的
        /// </summary>
        public Dictionary<string, int> GetUserIdsByTPGameAccounts(PlatformProduct product, HashSet<string> tpGameAccounts)
        {
            List<ThirdPartyUserAccount> thirdPartyUserAccounts = _thirdPartyUserAccountRep.GetListByTPGameAccount(product, tpGameAccounts);
            Dictionary<string, int> resultMap = thirdPartyUserAccounts.ToDictionary(d => d.Account, d => d.UserID);

            //沒查到的用規則推算
            List<string> tpGameAccountsByRule = tpGameAccounts
                .Where(w => !thirdPartyUserAccounts.Any(a => a.Account.Equals(w, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            tpGameAccountsByRule.ForEach(account =>
            {
                if (!resultMap.ContainsKey(account))
                {
                    BasicUserInfo user = GetLocalAccountByRule(product, account, false);

                    if (user != null)
                    {
                        resultMap.Add(account, user.UserId);
                    }
                }
            });

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

        private string GetTPGameAvailableScore(string productCode, int userId)
        {
            ITPGameUserInfoService tpGameUserInfoService = _tpGameUserInfoServiceMap[productCode];
            BaseTPGameUserInfo baseTPGameUserInfo = tpGameUserInfoService.GetTPGameUserInfo(userId);
            decimal tPGameAvailableScore = 0M;

            if (baseTPGameUserInfo != null)
            {
                tPGameAvailableScore = baseTPGameUserInfo.GetAvailableScores();
            }

            return tPGameAvailableScore.ToCurrency(true);
        }

        /// <summary>
        /// 查出用戶主帳戶餘額
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private PlatformAccountSearchResult GetPlatformAccountSearchResult(int userId)
        {
            UserInfo userInfo = UserInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo { UserID = userId });

            return new PlatformAccountSearchResult
            {
                UserID = userInfo.UserID,
                UserName = userInfo.UserName,
                AvailableScoreText = userInfo.AvailableScores.GetValueOrDefault().ToCurrency(true)
            };
        }

        protected int? GetUserIdFromSuffix(string tpGameAccount)
        {
            string defaultAccountPrefix = GetDefaultTPGameAccountPrefix(EnvCode.AccountPrefixCode);
            int subStringStartIndex = tpGameAccount.LastIndexOf(defaultAccountPrefix, StringComparison.OrdinalIgnoreCase);

            if (subStringStartIndex == -1)
            {
                return null;
            }

            string parseData = tpGameAccount.Substring(subStringStartIndex + defaultAccountPrefix.Length);

            if (int.TryParse(parseData, out int userId))
            {
                return userId;
            }

            return null;
        }

        private void AddDiffTPGameAccountSearchResult(int userId, string userName, List<string> existProductCodes,
            List<TPGameAccountSearchResult> tpGameAccountSearchResults)
        {
            //把缺資料的產品再找各個第三方的資料, 用規則推算
            List<PlatformProduct> diffPlatformProducts = GetNonSelfProductWithoutPT().Where(w => !existProductCodes.Contains(w.Value)).ToList();

            foreach (PlatformProduct platformProduct in diffPlatformProducts)
            {
                //考量後台用量不會很大, 這邊就不使用union all的方式一次查詢
                //找第三方userInfo有無資料
                ITPGameUserInfoService tpGameUserInfoService = _tpGameUserInfoServiceMap[platformProduct.Value];

                bool isUserExists = tpGameUserInfoService.IsUserExists(userId);

                if (!isUserExists)
                {
                    continue;
                }

                string tpGameAccount = GetTPGameAccountByRule(platformProduct, userId, userName);

                if (!tpGameAccount.IsNullOrEmpty())
                {
                    BaseTPGameUserInfo baseTPGameUserInfo = tpGameUserInfoService.GetTPGameUserInfo(userId);
                    decimal tPGameAvailableScore = baseTPGameUserInfo.GetAvailableScores();

                    tpGameAccountSearchResults.Add(new TPGameAccountSearchResult()
                    {
                        LocalAccount = userName,
                        TPGameProductCode = platformProduct.Value,
                        TPGameProductName = platformProduct.Name,
                        TPGameAccount = GetTPGameAccountByRule(platformProduct, userId, userName),
                        TPGameAvailableScoreText = tPGameAvailableScore.ToCurrency(true),
                        Sort = platformProduct.Sort
                    });
                }
            }
        }

        private List<PlatformProduct> GetNonSelfProductWithoutPT()
        {
            return _platformProductService.GetNonSelfProductWithoutPT();
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

    public class TPGameAccountService : BaseTPGameAccountService
    {
        protected override string DefaultTPGameAccountPrefixCode => "jx";

        /// <summary>AG前綴詞</summary>
        private static readonly string _agAccountPrefix = "AgPlayer_";

        /// <summary>RG前綴詞</summary>
        private static readonly string _rgAccountPrefix = "jx_";

        private static readonly string _charAndNumAccountRegEx = "^[a-zA-Z0-9]*$";

        private static readonly string _rgAccountRegEx = "^[a-zA-Z0-9_]{4,16}$";

        public TPGameAccountService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public override string GetTPGameAccountByRule(PlatformProduct platformProduct, int userId, string userName)
        {
            //Default encoded string
            string tpGameAccount = null;

            if (platformProduct == PlatformProduct.AG)
            {
                if (EnvCode == Model.Enums.EnvironmentCode.Production)
                {
                    if (Regex.IsMatch(userName, _charAndNumAccountRegEx))
                    {
                        tpGameAccount = userName;
                    }
                    else
                    {
                        tpGameAccount = $"{_agAccountPrefix}{userId}";
                    }
                }
                else
                {
                    //TODO 需要配合SP做修改才能加上前墜
                    /*
                    declare @UserName NVARCHAR(50)='U_AgPlayer_123456'
                    DECLARE @PrefixString VARCHAR(10) = 'AgPlayer_'
                    DECLARE @SubstringStartPos INT = CHARINDEX(@PrefixString, @UserName) + LEN(@PrefixString)
                    SELECT CAST(SUBSTRING(@UserName, @SubstringStartPos, LEN(@UserName) - @SubstringStartPos + 1) AS INT)
                    */
                    tpGameAccount = string.Empty;
                }
            }
            else if (platformProduct == PlatformProduct.Sport)
            {
                //相容舊資料, LIVE沒有前綴詞
                if (EnvCode == Model.Enums.EnvironmentCode.Production)
                {
                    tpGameAccount = userId.ToString();
                }
                else
                {
                    tpGameAccount = GetPrefixAndEnvCode(EnvCode.AccountPrefixCode, userId);
                }
            }
            else if (platformProduct == PlatformProduct.PT)
            {
                tpGameAccount = userName;
            }
            else if (platformProduct == PlatformProduct.LC)
            {
                //相容舊資料, LIVE沒有前綴詞
                if (EnvCode == Model.Enums.EnvironmentCode.Production)
                {
                    tpGameAccount = userName;
                }
                else
                {
                    tpGameAccount = GetPrefixAndEnvCode(EnvCode.AccountPrefixCode, userName);
                }
            }
            else if (platformProduct == PlatformProduct.RG)
            {
                if (Regex.IsMatch(userName, _rgAccountRegEx))
                {
                    tpGameAccount = userName;
                }
                else
                {
                    tpGameAccount = $"{_rgAccountPrefix}{userId.ToString("00")}";
                }
            }
            else
            {
                //包含 PlatformProduct.IM, PlatformProduct.IMPT, 
                //PlatformProduct.IMPP, PlatformProduct.IMSport, PlatformProduct.IMeBET, PlatformProduct.IMBG
                tpGameAccount = GetPrefixAndEnvCode(EnvCode.AccountPrefixCode, userId);
            }

            return tpGameAccount;
        }

        /// <summary>
        /// 從第三方帳號反推平台帳號
        /// </summary>
        protected override BasicUserInfo GetLocalAccountByRule(PlatformProduct platformProduct, string tpGameAccount, bool isFillUserName = true)
        {
            var basicUserInfo = new BasicUserInfo();
            var platformProductSettingService = ResolveKeyed<IPlatformProductSettingService>(platformProduct);

            if (platformProductSettingService.IsParseUserIdFromSuffix)
            {
                int? userId = GetUserIdFromSuffix(tpGameAccount);

                if (userId.HasValue)
                {
                    basicUserInfo.UserId = userId.Value;
                }
            }
            else if (platformProduct == PlatformProduct.AG)
            {
                if (EnvCode == Model.Enums.EnvironmentCode.Production)
                {
                    if (tpGameAccount.StartsWith(_agAccountPrefix))
                    {
                        basicUserInfo.UserId = tpGameAccount.TrimStart(_agAccountPrefix).ToInt32();
                    }
                    else
                    {
                        basicUserInfo.UserName = tpGameAccount;
                    }
                }
                else
                {
                    //目前還沒測試環境實作
                    //do nothing;
                }
            }
            else if (platformProduct == PlatformProduct.Sport)
            {
                if (EnvCode == Model.Enums.EnvironmentCode.Production)
                {
                    if (int.TryParse(tpGameAccount, out int output))
                    {
                        basicUserInfo.UserId = tpGameAccount.ToInt32();
                    }
                    else
                    {
                        basicUserInfo = null;
                    }
                }
                else
                {
                    int? parseUserId = ParseUserIdFromTpGameAccount(EnvCode, tpGameAccount);

                    if (parseUserId.HasValue)
                    {
                        basicUserInfo.UserId = parseUserId.Value;
                    }
                    else
                    {
                        basicUserInfo = null;
                    }
                }
            }
            else if (platformProduct == PlatformProduct.PT)
            {
                basicUserInfo.UserName = tpGameAccount;
            }
            else if (platformProduct == PlatformProduct.LC)
            {
                if (EnvCode == Model.Enums.EnvironmentCode.Production)
                {
                    basicUserInfo.UserName = tpGameAccount;
                }
                else
                {
                    basicUserInfo.UserName = ParseUserNameFromTpGameAccount(EnvCode, tpGameAccount);
                }
            }
            else if (platformProduct == PlatformProduct.RG)
            {
                if (tpGameAccount.StartsWith(_rgAccountPrefix))
                {
                    basicUserInfo.UserId = tpGameAccount.TrimStart(_rgAccountPrefix).ToInt32();
                }
                else
                {
                    basicUserInfo.UserName = tpGameAccount;
                }
            }
            else
            {
                //一般規則反轉
                int? parseUserId = ParseUserIdFromTpGameAccount(EnvCode, tpGameAccount);

                if (parseUserId.HasValue)
                {
                    basicUserInfo.UserId = parseUserId.Value;
                }
                else
                {
                    basicUserInfo = null;
                }
            }

            if (basicUserInfo != null)
            {
                if (basicUserInfo.UserId == 0 && basicUserInfo.UserName.IsNullOrEmpty())
                {
                    basicUserInfo = null;
                }
                else if (basicUserInfo.UserId == 0)
                {
                    int? foundUserId = UserInfoRep.GetFrontSideUserId(basicUserInfo.UserName);

                    if (foundUserId.HasValue)
                    {
                        basicUserInfo.UserId = foundUserId.Value;
                    }
                    else
                    {
                        basicUserInfo = null;
                    }
                }
                else if (isFillUserName && basicUserInfo.UserName.IsNullOrEmpty())
                {
                    string foundUserName = UserInfoRep.GetFrontSideUserName(basicUserInfo.UserId);

                    if (!foundUserName.IsNullOrEmpty())
                    {
                        basicUserInfo.UserName = foundUserName;
                    }
                    else
                    {
                        basicUserInfo = null;
                    }
                }
            }

            return basicUserInfo;
        }

        private int? ParseUserIdFromTpGameAccount(EnvironmentCode environmentCode, string tpGameAccount)
        {
            string defaultTPGameAccountPrefix = GetDefaultTPGameAccountPrefix(environmentCode.AccountPrefixCode);

            if (tpGameAccount.IndexOf(defaultTPGameAccountPrefix, StringComparison.OrdinalIgnoreCase) == -1)
            {
                return null;
            }

            string suffix = tpGameAccount.TrimStart(defaultTPGameAccountPrefix);

            if (int.TryParse(suffix, out int output))
            {
                return output;
            }

            return null;
        }

        private string ParseUserNameFromTpGameAccount(EnvironmentCode environmentCode, string tpGameAccount)
        {
            return tpGameAccount.TrimStart(GetDefaultTPGameAccountPrefix(environmentCode.AccountPrefixCode));
        }
    }

    public class TPGameAccountCTSService : BaseTPGameAccountService
    {
        public TPGameAccountCTSService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        protected override string DefaultTPGameAccountPrefixCode => "cts";

        public override string GetTPGameAccountByRule(PlatformProduct platformProduct, int userId, string userName)
        {
            //Default encoded string
            string tpGameAccount = GetPrefixAndEnvCode(EnvCode.AccountPrefixCode, userId);
            return tpGameAccount;
        }

        protected override BasicUserInfo GetLocalAccountByRule(PlatformProduct platformProduct, string tpGameAccount, bool isFillUserName = true)
        {
            var basicUserInfo = new BasicUserInfo();
            var platformProductSettingService = ResolveKeyed<IPlatformProductSettingService>(platformProduct);

            if (platformProductSettingService.IsParseUserIdFromSuffix)
            {
                int? userId = GetUserIdFromSuffix(tpGameAccount);

                if (userId.HasValue)
                {
                    basicUserInfo.UserId = userId.Value;
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            if (basicUserInfo != null)
            {
                if (basicUserInfo.UserId == 0 && basicUserInfo.UserName.IsNullOrEmpty())
                {
                    basicUserInfo = null;
                }
                else if (basicUserInfo.UserId == 0)
                {
                    int? foundUserId = UserInfoRep.GetFrontSideUserId(basicUserInfo.UserName);

                    if (foundUserId.HasValue)
                    {
                        basicUserInfo.UserId = foundUserId.Value;
                    }
                    else
                    {
                        basicUserInfo = null;
                    }
                }
                else if (isFillUserName && basicUserInfo.UserName.IsNullOrEmpty())
                {
                    string foundUserName = UserInfoRep.GetFrontSideUserName(basicUserInfo.UserId);

                    if (!foundUserName.IsNullOrEmpty())
                    {
                        basicUserInfo.UserName = foundUserName;
                    }
                    else
                    {
                        basicUserInfo = null;
                    }
                }
            }

            return basicUserInfo;
        }
    }
}
