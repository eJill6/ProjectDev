using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MMService.Models.Auth;
using MS.Core.Extensions;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models.Auth.Enums;
using MS.Core.MM.Models.Auth.ServiceReq;
using MS.Core.MM.Models.Entities.BossShop;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Repos;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Auth;
using MS.Core.MMModel.Models.Auth.Enums;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models;
using MS.Core.Services;
using MS.Core.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace MMService.Services
{
    /// <summary>
    /// 權限相關
    /// </summary>
    public class AuthService : BaseService, IAuthService
    {
        /// <summary>
        /// 會員資源
        /// </summary>
        private readonly IUserInfoRepo _userInfoRepo;

        /// <summary>
        ///jwt設定
        /// </summary>
        private readonly IOptionsMonitor<JwtSettings> _jwtSetting;

        /// <summary>
        /// 零一服務
        /// </summary>
        private readonly IZeroOneApiService _zeroOneService;

        /// <summary>
        /// 使用者總覽服務
        /// </summary>
        private readonly IUserSummaryService _userSummaryService;

        /// <summary>
        /// 會員服務
        /// </summary>
        private readonly IVipService _vipService;

        /// <summary>
        /// 媒體資源
        /// </summary>
        private readonly IMediaRepo _mediaRepo;

        /// <summary>
        /// 記憶體控管
        /// </summary>
        private readonly IMemoryCache _memoryCache;
        /// <summary>
        /// 尚未审核过的店铺信息
        /// </summary>

        private readonly IBossShopRepo _bossShopRepo;

        /// <summary>
        /// 登入快取前贅詞
        /// </summary>
        private readonly string UserLoginCachePrefixKey = "UserLogIn:";

        /// <summary>
        /// 權限相關
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="jwtSetting">jwt設定</param>
        /// <param name="userInfoRepo">會員資源</param>
        /// <param name="zeroOneService">零一服務</param>
        /// <param name="userSummaryService">零一服務</param>
        /// <param name="vipService">vip會員服務</param>
        /// <param name="mediaRepo">媒體資源</param>
        public AuthService(ILogger<AuthService> logger,
            IOptionsMonitor<JwtSettings> jwtSetting,
            IUserInfoRepo userInfoRepo,
            IZeroOneApiService zeroOneService,
            IUserSummaryService userSummaryService,
            IVipService vipService,
            IMemoryCache memoryCache,
            IMediaRepo mediaRepo, IBossShopRepo bossShopRepo) : base(logger)
        {
            _userInfoRepo = userInfoRepo;
            _jwtSetting = jwtSetting;
            _zeroOneService = zeroOneService;
            _userSummaryService = userSummaryService;
            _vipService = vipService;
            _mediaRepo = mediaRepo;
            _memoryCache = memoryCache;
            _bossShopRepo = bossShopRepo;
        }

        /// <summary>
        /// 取得祕色用戶資訊
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<ZOUserInfoRes?> GetZeroUserInfo(int userId)
        {
            ZOUserInfoRes? msUserInfo = null;
            try
            {
                msUserInfo = (await _zeroOneService.GetUserInfo(new ZOUserInfoReq(userId)))?.DataModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}。取得祕色會員資訊發生錯誤。UserId = {userId}");
            }

            return msUserInfo;
        }

        /// <summary>
        /// 建立或登入帳號
        /// </summary>
        /// <param name="param">登入資訊</param>
        /// <returns></returns>
        private async Task<BaseReturnModel> CreateAndLogin(SignInData param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var key = $"{UserLoginCachePrefixKey}{param.UserId}";
                if (_memoryCache.Get<int>(key) == 1)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                ZOUserInfoRes? msUserInfo = await GetZeroUserInfo(param.UserId);

                DateTime currentTime = DateTime.Now;
                bool isAccess = await _userInfoRepo.UserUpsert(new MMUserInfo
                {
                    UserId = param.UserId,
                    UserIdentity = 0,
                    UserLevel = 0,
                    RewardsPoint = 0,
                    AvatarUrl = msUserInfo?.Avatar ?? string.Empty,
                    Nickname = msUserInfo?.NickName ?? param.Nickname ?? string.Empty,
                    RegisterTime = msUserInfo == null || msUserInfo?.CreateTime == default(DateTime) ?
                            currentTime :
                            (DateTime)msUserInfo?.CreateTime,
                    UpdateTime = currentTime,
                    CreateTime = currentTime
                }, param.Type);

                if (!isAccess)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
                }
                _memoryCache.Set<int>(key, 1, TimeSpan.FromSeconds(5));
                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <summary>
        /// 產生token
        /// </summary>
        /// <param name="param">登入資訊</param>
        /// <param name="loginType">登入類型 *前台、後台</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<SignInResponse>> GenerateToken(SignInData param, LoginType loginType)
        {
            if (param.UserId == default(int) || string.IsNullOrWhiteSpace(param.Nickname))
            {
                return new BaseReturnDataModel<SignInResponse>(ReturnCode.MissingNecessaryParameter);
            }

            // 前台才做建帳號
            if (loginType == LoginType.FrontendSide)
            {
                var createResult = await CreateAndLogin(param);
                if (createResult.IsSuccess == false)
                {
                    _logger.LogError($"GenerateToken.CreateAndLogin fail, result:{JsonUtil.ToJsonString(createResult)}");
                    return new BaseReturnDataModel<SignInResponse>(ReturnCode.OperationFailed);
                }
            }

            var tokenClaims = new List<Claim>();
            tokenClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            tokenClaims.Add(new Claim(ClaimNamesDefine.UserId.ToString(), param.UserId.ToString()));
            tokenClaims.Add(new Claim(ClaimNamesDefine.Nickname.ToString(), param.Nickname));

            var userClaimsIdentity = new ClaimsIdentity(tokenClaims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.CurrentValue.SignKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var expiresAt = DateTime.Now.AddSeconds(_jwtSetting.CurrentValue.ExpirationSec);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSetting.CurrentValue.Issuer,
                Subject = userClaimsIdentity,
                Expires = expiresAt,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<SignInResponse>();
                result.DataModel = new SignInResponse()
                {
                    AccessToken = serializeToken
                };
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, serializeToken);
        }

        /// <summary>
        /// 取得認證資訊
        /// </summary>
        /// <param name="user">用戶id</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<CertificationResponse>> CertificationInfo(ReqUserId user)
        {
            return await TryCatchProcedure(async (user) =>
            {
                var userData = await _vipService.GetUserSummaryInfoData(user.UserId);

                var applyInfo = (await _userInfoRepo.GetIdentityApplyData(user.UserId))
                    .OrderByDescending(p => p.CreateTime)
                    .FirstOrDefault();

                var applyStatus = IdentityApplyStatus.NotYet;
                var applyIdentity = IdentityType.General;
                if (applyInfo != null)
                {
                    switch (applyInfo?.Status)
                    {
                        case (byte)ReviewStatus.UnderReview:
                            applyStatus = IdentityApplyStatus.Applying;
                            break;

                        case (byte)ReviewStatus.Approval:
                            applyStatus = IdentityApplyStatus.Pass;
                            break;
                    }

                    applyIdentity = (IdentityType)applyInfo.ApplyIdentity;
                }

                // 最後以 userinfo 主表為主，若萬一產生髒資料的時候可以避免錯誤
                if (userData.UserInfo != null)
                {
                    // 註：如果發現最後身份與申請結果的不符，有可能是 db 資訊不同步
                    if (userData.UserInfo.UserIdentity == (byte)IdentityType.General &&
                        applyStatus != IdentityApplyStatus.Applying)
                    {
                        // 身份是一般人且不在申請中，則覆寫成尚未申請
                        applyIdentity = IdentityType.General;
                        applyStatus = IdentityApplyStatus.NotYet;
                    }
                    else if (userData.UserInfo.UserIdentity != (byte)IdentityType.General)
                    {
                        // 如果已有身份，則覆寫成該申請的身份，狀態為通過
                        applyIdentity = (IdentityType)userData.UserInfo.UserIdentity;
                        applyStatus = IdentityApplyStatus.Pass;
                    }
                }

                var result = new BaseReturnDataModel<CertificationResponse>();
                result.DataModel = new CertificationResponse()
                {
                    RemainPublish = userData.RemainingUnlock,
                    ApplyIdentity = applyIdentity,
                    ApplyStatus = applyStatus
                };
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, user);
        }

        /// <summary>
        /// 建立申請身份資料
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task<(MMIdentityApply?, MMUserInfo?)> CreateApplyIdentityData(int userId, IdentityType type)
        {
            return await CreateApplyIdentityData(userId, type, "", "");
        }

        private async Task<(MMIdentityApply?, MMUserInfo?)> CreateApplyIdentityData(int userId, IdentityType type, string contactApp, string contactInfo)
        {
            var userInfo = await _userInfoRepo.GetUserInfo(userId);

            MMIdentityApply mmIdentity = null;
            if (userInfo != null)
            {
                mmIdentity = new MMIdentityApply()
                {
                    UserId = userId,
                    OriginalIdentity = userInfo.UserIdentity,
                    ApplyIdentity = (byte)type,
                    Status = (byte)ReviewStatus.UnderReview,
                    CreateTime = DateTime.Now,
                    ContactApp = contactApp,
                    ContactInfo = contactInfo
                };
            }

            return (mmIdentity, userInfo);
        }

        /// <summary>
        /// 檢查申請資訊
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<BaseReturnModel> CheckApplyInfo(int userId)
        {
            ZOUserInfoRes? msUserInfo = await GetZeroUserInfo(userId);
            if (msUserInfo == null)
            {
                return new BaseReturnModel(ReturnCode.FailedToGetPhoneAuthentication);
            }
            else if (msUserInfo.HasPhone == false)
            {
                return new BaseReturnModel(ReturnCode.PhoneHasNotBeenBound);
            }

            if (await _userInfoRepo.IsAlreadyIdentityApply(userId))
            {
                return new BaseReturnModel(ReturnCode.IsAlreadyIdentityApply);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>
        /// 檢查用戶身份申請狀態
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private async Task<BaseReturnModel> CheckUserIdentityApplyStatus(MMUserInfo? userInfo)
        {
            //會員不存在
            if (userInfo == null)
            {
                return new BaseReturnModel(ReturnCode.UserDoesNotExist);
            }

            if (userInfo.UserIdentity != (byte)IdentityType.General)
            {
                return new BaseReturnModel(ReturnCode.AlreadyHaveIdentityCannotApplyAgain);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>
        /// 送出申請資訊。適用覓經紀、覓女郎
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="identityType"></param>
        /// <returns></returns>
        private async Task<BaseReturnModel> SendApply(int userId, IdentityType identityType)
        {
            return await SendApply(userId, identityType, "", "");
        }

        private async Task<BaseReturnModel> SendApply(int userId, IdentityType identityType, string contactApp, string contactInfo)
        {
            var checkApply = await CheckApplyInfo(userId);
            if (!checkApply.IsSuccess)
            {
                return checkApply;
            }

            var (entity, userInfo) = await CreateApplyIdentityData(userId, identityType, contactApp, contactInfo);

            var checkIdentityStatus = await CheckUserIdentityApplyStatus(userInfo);
            if (!checkIdentityStatus.IsSuccess)
            {
                return checkIdentityStatus;
            }

            bool isAccess = (entity != null && await _userInfoRepo.ApplyIdentity(entity));
            if (!isAccess)
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>
        /// 覓經紀申請
        /// </summary>
        /// <param name="user">用戶id</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> AgentIdentityApply(ReqAgentIdentityApplyData model)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
				return await SendApply(param.UserId, IdentityType.Agent, model.ContactType, model.Contact);
            }, model);
        }

        /// <summary>
        /// 覓女郎申請
        /// </summary>
        /// <param name="user">用戶id</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> GirlIdentityApply(ReqUserId user)
        {
            return await TryCatchProcedure(async (user) =>
            {
                return await SendApply(user.UserId, IdentityType.Girl);


            }, user);
        }
        /// <summary>
        /// 修改或申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseReturnModel> BossIdentityApplyOrUpdate(ReqBossApplyOrUpdateData model)
        {
            return await TryCatchProcedure(async (param) => {


            var checkApply = await CheckApplyInfo(param.UserId);
            if (!checkApply.IsSuccess) {
                return checkApply;
            }

            if (!param.IsAdminApply) {

                if (string.IsNullOrEmpty(param.ShopName) || string.IsNullOrEmpty(param.Introduction) || string.IsNullOrEmpty(param.ContactInfo) ||
                    string.IsNullOrEmpty(param.ContactApp) || string.IsNullOrEmpty(param.ShopAvatarSource) || !param.Girls.HasValue ||
                    !param.DealOrder.HasValue || !param.SelfPopularity.HasValue || !param.ShopYears.HasValue)
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
            }
            List<string> imageIds = param.BusinessPhotoSource.ToList();
            imageIds.Add(param.ShopAvatarSource);

            var imageWhere = imageIds
                 .Where(p => !string.IsNullOrWhiteSpace(p))?
                 .ToArray() ?? Array.Empty<string>();

            var medias = await _mediaRepo.Get(imageWhere,true);

            if (medias == null) {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            medias = medias
                    .Where(c => (c.SourceType == (int)SourceType.BusinessPhoto || c.SourceType == (int)SourceType.BossApply) && c.MediaType == (int)MediaType.Image)
                    .ToArray();


            
            if (string.IsNullOrEmpty(model.ApplyId)) {
                //新增
                var bossEntity = new MMBoss() {
                    ShopName = param.ShopName,
                    Introduction = param.Introduction,
                    Girls = param.Girls.ToString(),
                    SelfPopularity = param.SelfPopularity,
                    ShopYears = param.ShopYears,
                    DealOrder = param.DealOrder,
                    Contact = param.ContactInfo,
                    CreateTime = DateTime.Now,
                    ViewBaseCount=800
                };
              
                var userInfo = await _userInfoRepo.GetUserInfo(param.UserId);

                var applyEntity = new MMIdentityApply()
                {
                    UserId = param.UserId,
                    OriginalIdentity = userInfo.UserIdentity,
                    ApplyIdentity = (byte)IdentityType.Boss,
                    Status = (byte)ReviewStatus.UnderReview,
                    CreateTime = DateTime.Now,
                    ContactApp = param.ContactApp,
                    ContactInfo = param.ContactInfo
                };

                var checkIdentityStatus = await CheckUserIdentityApplyStatus(userInfo);
                if (!checkIdentityStatus.IsSuccess && !param.IsAdminApply) {
                    return checkIdentityStatus;
                }

                bool isAccess = (applyEntity != null && await _userInfoRepo.ApplyBossIdentity(applyEntity, bossEntity, medias, param.IsAdminApply));
                if (!isAccess) {
                    return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
                }

            }
            else {
                    var boos = await _userInfoRepo.GetByApplyId(param.ApplyId);
                    if (boos == null){
                        return new BaseReturnModel(ReturnCode.UpdateFailed);
                    }
                    var userInfo= await _userInfoRepo.GetUserInfo(param.UserId);
                    if(userInfo==null)
                        return new BaseReturnModel(ReturnCode.UpdateFailed);

                    var applyInfo = new MMIdentityApply()
                    {
                        UserId = userInfo.UserId,
                        OriginalIdentity = userInfo.UserIdentity,
                        ApplyIdentity = (byte)IdentityType.Boss,
                        Status = (byte)ReviewStatus.UnderReview,
                        CreateTime = DateTime.Now,
                        ContactApp = param.ContactApp,
                        ContactInfo = param.ContactInfo,
                        ApplyId = await _userInfoRepo.GetMMIdentityApplySequenceIdentity()
                    };

                    var model = new MMBossShop(){ 
                        ApplyId = applyInfo.ApplyId,
                        BossId = boos.BossId,
                        UserId = param.UserId,
                        ContactApp = param.ContactApp,
                        ContactInfo = param.ContactInfo,
                        ShopName = param.ShopName,
                        Girls = param.Girls.Value,
                        ShopYears = param.ShopYears.Value,
                        DealOrder = param.DealOrder.Value,
                        SelfPopularity = param.SelfPopularity.Value,
                        Introduction = param.Introduction,
                        ShopAvatarSource = param.ShopAvatarSource,
                        BusinessPhotoSource = param.BusinessPhotoSource !=null? string.Join(",", param.BusinessPhotoSource):"",
                    };

                    model.Id = await _bossShopRepo.GetSequenceIdentity();

                    var isSuccess=  await _bossShopRepo.Create(model);
                    if (!isSuccess)
                    {
                        return new BaseReturnModel(ReturnCode.UpdateFailed);
                    }

                }

                return new BaseReturnModel(ReturnCode.Success);
            }, model);
        }
        /// <summary>
        /// 覓老闆申請 BossIdentityApplyOrUpdate
        /// </summary>
        /// <param name="model">申請資料</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> BossIdentityApply(ReqBossIdentityApplyData model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                if (!model.IsAdminApply)
                {
                    var checkApply = await CheckApplyInfo(param.UserId);
                    if (!checkApply.IsSuccess)
                    {
                        return checkApply;
                    }
                }

                MMMedia mediaEntity = new MMMedia();
                if (!param.IsAdminApply)
                {
                    param.PhotoIds = param.PhotoIds
                  .Where(p => !string.IsNullOrWhiteSpace(p))?
                  .ToArray() ?? Array.Empty<string>();

                if (param.ShopName.Length > 12 || param.Girls.Length > 12 || param.ShopIntroduce.Length > 17 ||
                    param.Contact.Length > 15 || param.ShopAge > 99 || param.ShopAge < 1 ||
                    param.Contact.Length > 20 || param.PhotoIds.Length > 1 || param.ShopPhotoIds.Length > 3)
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
                    mediaEntity = await _mediaRepo.Get(param.PhotoIds.First());
                    if (mediaEntity == null)
                    {
                        return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                    }
                    else if (mediaEntity.MediaType != (int)MediaType.Image ||
                        mediaEntity.SourceType != (int)SourceType.BossApply)
                    {
                        return new BaseReturnModel(ReturnCode.InvalidPhoto);
                    }
                }

                var bossEntity = new MMBoss()
                {
                    ShopName = param.ShopName,
                    Introduction = param.ShopIntroduce,
                    Girls = param.Girls,
                    SelfPopularity = param.Rating,
                    ShopYears = param.ShopAge,
                    DealOrder = param.OrderQuantity,
                    Contact = param.Contact,
                    CreateTime = DateTime.Now
                };



                var (entity, userInfo) = await CreateApplyIdentityData(param.UserId, IdentityType.Boss, param.ContactApp, param.Contact);

                var checkIdentityStatus = await CheckUserIdentityApplyStatus(userInfo);
                if (!checkIdentityStatus.IsSuccess && !param.IsAdminApply){
                    return checkIdentityStatus;
                }

                bool isAccess = (entity != null && await _userInfoRepo.ApplyBossIdentity(entity, bossEntity, mediaEntity, param.IsAdminApply));
                if (!isAccess)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
                }

                return new BaseReturnModel(ReturnCode.Success);
            }, model);
        }
    }
}