using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MMService.Services;
using MS.Core.Extensions;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Extensions;
using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Models.Auth.ServiceReq;
using MS.Core.MM.Models.Auth;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.Media;
using MS.Core.MM.Models.User;
using MS.Core.MM.Models.Vip;
using MS.Core.MM.Repos;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Utils;
using System.Linq;
using System.Reflection;

namespace MMService.Controllers
{
    public class AdminUserManagerController : ApiControllerBase
    {
        /// <summary>
        /// 會員卡相關
        /// </summary>
        private readonly IVipTransactionRepo _vipTransactionRepo;

        /// <summary>
        /// 會員相關
        /// </summary>
        private readonly IUserInfoRepo _userInfoRepo;

        /// <summary>
        /// 會員數量相關
        /// </summary>
        private readonly IUserSummaryRepo _userQuantityRepo;

        /// <summary>
        /// 會員卡資訊相關
        /// </summary>
        private readonly IVipService _vipService;

        /// <summary>
        /// 型別轉換
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 01 API
        /// </summary>
        private readonly IZeroOneApiService _zeroOneApiService;

        /// <summary>
        /// 使用者快取資料
        /// </summary>
        private readonly IUserSummaryService _userSummaryService;

        private readonly IBookingService _bookingService;

        /// <summary>
        /// 贴子相關
        /// </summary>
        private readonly IPostRepo _postRepo;

        /// <summary>
        /// 收支相關
        /// </summary>
        private readonly IIncomeExpenseRepo _incomeExpense;

        /// <summary>
        /// 身份认证相关
        /// </summary>
        private readonly IIdentityApplyRepo _identityApply;

        private readonly IUserVipRepo _userVipRepo;

        private readonly IMediaRepo _mediaRepo;

        /// <summary>
        /// 權限相關服務
        /// </summary>
        private readonly IAuthService _auth;

        /// <summary>
        /// 媒體服務
        /// </summary>
        private readonly IEnumerable<IMediaService> _mediaServices;

        private IMediaService GetMedia => _mediaServices.FirstOrDefault(m => (m.SourceType == SourceType.BossApply || m.SourceType == SourceType.BusinessPhoto) && m.Type == MediaType.Image);

        public AdminUserManagerController(IUserInfoRepo userInfoRepo,
            IVipTransactionRepo vipTransactionRepo,
            IUserSummaryRepo userQuantityRepo,
            IVipService vipService,
            IMapper mapper,
            IZeroOneApiService zeroOneApiService,
            IUserSummaryService userSummaryService,
            IBookingService bookingService,
            IPostRepo postRepo,
            IIncomeExpenseRepo incomeExpense,
            IIdentityApplyRepo identityApply,
            IUserVipRepo userVipRepo,
            IMediaRepo mediaRepo,
            IAuthService auth,
            IEnumerable<IMediaService> mediaServices,
            ILogger logger) : base(logger)
        {
            _vipTransactionRepo = vipTransactionRepo;
            _userInfoRepo = userInfoRepo;
            _userQuantityRepo = userQuantityRepo;
            _vipService = vipService;
            _mapper = mapper;
            _zeroOneApiService = zeroOneApiService;
            _userSummaryService = userSummaryService;
            _bookingService = bookingService;
            _postRepo = postRepo;
            _incomeExpense = incomeExpense;
            _identityApply = identityApply;
            _userVipRepo = userVipRepo;
            _mediaRepo = mediaRepo;
            _mediaServices = mediaServices;
            _auth = auth;
        }

        /// <summary>
        /// 会员查询
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminUserManagerUsersList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Users(AdminUserManagerUsersListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<AdminUserManagerUsersList>>();
                if (param.VipId.HasValue)
                {
                    var vipCards = await _userVipRepo.GetUserEfficientVipsByType((VipType)param.VipId.Value);
                    model.UserIds = vipCards.Select(x => x.UserId).ToArray();
                }
                var userInfos = await _userInfoRepo.GetUserInfos(model);

                result.DataModel = JsonUtil.CastByJson<PageResultModel<AdminUserManagerUsersList>>(userInfos);

                var userIds = result.DataModel.Data.Select(x => x.UserId).ToArray();

                var userQuantitiesQuery = await _userQuantityRepo.GetUserSummaries(userIds);

                var userQuantitiesDic = userQuantitiesQuery.GroupBy(x => x.UserId).ToDictionary(x => x.Key, x => x.ToArray());

                var vipCardsByUserQuery = await _userVipRepo.GetUserVips(userIds);

                var vipCardsByUserDic =
                    vipCardsByUserQuery
                    .GroupBy(x => x.UserId)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                var vipTypes = await _vipService.GetVipTypes();

                foreach (var user in result.DataModel.Data)
                {
                    if (userQuantitiesDic.ContainsKey(user.UserId))
                    {
                        var userQuantities = userQuantitiesDic[user.UserId];
                        user.PostQuantity = userQuantities.Where(x => x.Type == UserSummaryTypeEnum.Send).Sum(x => (int)x.Amount).ToString();
                        //user.UnlockedQuantity = userQuantities.Where(x => x.Type == UserSummaryTypeEnum.Unlocked).Sum(x => (int)x.Amount).ToString();
                        user.Income = userQuantities.Where(x => x.Type == UserSummaryTypeEnum.Income).Sum(x => x.Amount).ToString("f2");
                    }
                    user.DealOrder = 0;
                    if (user.UserIdentity == IdentityType.Boss)
                    {
                        var userIdentityApply = _mapper.Map<AdminUserManagerIdentityApplyList>(await _identityApply.DetailByUserId(user.UserId, 1));
                        var boss = await _userInfoRepo.GetByApplyId(userIdentityApply?.ApplyId);

                        user.DealOrder = boss?.DealOrder ?? 0;
                    }
                }

                result.SetCode(ReturnCode.Success);
                return result;
            }, model));
        }

        /// <summary>
        /// 會員詳細
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ApiResponse<AdminUserManagerUsersDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UserDetail(int userId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<AdminUserManagerUsersDetail>();
                ZOUserInfoRes zoUser = await _zeroOneApiService.GetUserInfo(new ZOUserInfoReq(userId)).GetReturnDataAsync();

                if (zoUser == null)
                {
                    throw new MMException(ReturnCode.InvalidUser);
                }

                UserSummaryInfoData user = await _vipService.GetUserSummaryInfoData(userId);

                //本月收益
                decimal monthIncome = await _incomeExpense.GetMonthIncome(userId, DateTimeExtension.GetCurrentTime());

                //暫鎖收益
                decimal freezeIncome = await _incomeExpense.GetFreezeIncome(userId);

                //能發贴次數
                int postLimit = user.PublishLimit;
                //已發贴次數
                int sendPostCount = user.TotalPostSendTimes;

                int postCount = await _postRepo.GetPostCountByUserId(userId);

                int officialPostCount = await _postRepo.GetOfficialPostCountByUserId(userId);

                //剩余发帖数
                int remainingSend = user.RemainingUnlock;
                //如果会员当天已经发过一次，则减一次发帖次数
                if (user.IsVip && user.UserInfo.UserIdentity != (int)IdentityType.Boss && user.UserInfo.UserIdentity != (int)IdentityType.Agent)
                {
                    int postDailyCount = await _postRepo.GetPostDailyCount(userId);
                    remainingSend = postDailyCount > 0 ? remainingSend - 1 : remainingSend;
                }

                //評論待審核數量
                var postTypeCommentReview =
                    (await _postRepo.QueryCommentCountSummary(userId))
                    .Where(e => e.Status == ReviewStatus.UnderReview)
                    .ToDictionary(e => e.PostType, e => e.Count);

                //所有帖子
                var postTypeCountList = await _postRepo.QueryPostCountSummary(userId);

                var postReviewDic = postTypeCountList.Where(x => x.Status == ReviewStatus.UnderReview).ToDictionary(e => e.PostType, e => e.Count);

                var postDic = postTypeCountList.Where(x => x.Status == ReviewStatus.Approval).ToDictionary(e => e.PostType, e => e.Count);

                var bookingCount = await _bookingService.GetInProgressBookingCount(userId);

                AdminSquareQuantity[] quantity = EnumExtension.GetAll<PostType>().Select(postType =>
                {
                    int postReviewCount = postReviewDic.GetValueOrDefault(postType);

                    int postCount = postDic.GetValueOrDefault(postType);

                    int commentReviewCount = postTypeCommentReview.GetValueOrDefault(postType);

                    return new AdminSquareQuantity
                    {
                        PostType = postType,
                        Comment = user.PostTypeComment(postType),
                        Income = user.PostTypeIncome(postType),
                        Post = postCount,
                        Unlock = user.PostTypeUnLock(postType),
                        Unlocked = user.PostTypeUnlocked(postType),
                        CommentReview = commentReviewCount,
                        PostReview = postReviewCount,
                        BookCount = user.PostTypeAppointmentCount(postType),
                        BookedCount = user.PostTypeAppointmentedCount(postType),
                        BookInProgressCount = postType == PostType.Official ? bookingCount : 0
                    };
                }).ToArray();

                AdminCardDetail[] cardDetails = user.VipTypes.Select(vipType =>
                {
                    var isCurrentVip = user.CurrentVip?.VipType == vipType.Id;
                    return new AdminCardDetail
                    {
                        CardName = vipType.Name,
                        FreeUnlockCount = isCurrentVip ? user.RemainingFreeUnlock : 0,
                        UnlockDiscount = isCurrentVip ? user.Discount(PostType.Square) : 1,
                        BookingDiscount = isCurrentVip ? user.Discount(PostType.Official) : 1
                    };
                }).ToArray();

                return new BaseReturnDataModel<AdminUserManagerUsersDetail>(ReturnCode.Success)
                {
                    DataModel = new AdminUserManagerUsersDetail
                    {
                        Amount = zoUser.Amount,
                        AvatarUrl = zoUser.Avatar,
                        RegisterTime = zoUser.CreateTime,
                        Point = zoUser.Point,
                        NickName = zoUser.NickName,
                        UserId = zoUser.UserId,
                        RewardsPoint = 0,
                        MonthIncome = monthIncome,
                        EarnestMoney = user.UserInfo.EarnestMoney,
                        ContactApp = user.UserInfo.ContactApp,
                        Contact = user.UserInfo.Contact,
                        Memo = user.UserInfo.Memo,
                        ComingIncome = freezeIncome,
                        UserIdentity = (IdentityType)user.UserInfo.UserIdentity,
                        VipCards = user.CurrentVip?.TypeName ?? string.Empty,
                        PostLimit = postLimit,
                        VipCardEffectiveTime = null,
                        PostRemain = remainingSend < 0 ? 0 : remainingSend,
                        Quantity = quantity,
                        CardDetails = cardDetails
                    }
                };
            }, userId));
        }

        /// <summary>
        /// 会员卡消费记录
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminUserManagerUserCardsList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UserCards(AdminUserManagerUserCardsListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<AdminUserManagerUserCardsList>>();
                result.DataModel = JsonUtil.CastByJson<PageResultModel<AdminUserManagerUserCardsList>>(await _vipTransactionRepo.GetVips(param));
                result.SetCode(ReturnCode.Success);
                return result;
            }, model));
        }

        /// <summary>
        /// 觅会员收付记录
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminUserManagerIncomeExpensesList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IncomeExpenses(AdminUserManagerIncomeExpensesListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                IncomeExpenseTransactionTypeEnum? transactionType = GetTransactionType(model.Category);

                List<IncomeExpenseCategoryEnum> categories = GetCategoriesByParam(model);

                PageResultModel<MMIncomeExpenseModel> pageResult =
                    await _incomeExpense.GetPageTransactionByFilter(new PageIncomeExpenseFilter
                    {
                        Ids = model.Id?.ToEnumerable() ?? Enumerable.Empty<string>(),
                        UserId = model.UserId,
                        Categories = categories,
                        PayType = model.PayType,
                        StartTime = model.BeginDate,
                        EndTime = model.EndDate,
                        TransactionType = transactionType,
                        Pagination = model
                    });

                return new BaseReturnDataModel<PageResultModel<AdminUserManagerIncomeExpensesList>>(ReturnCode.Success)
                {
                    DataModel = new PageResultModel<AdminUserManagerIncomeExpensesList>
                    {
                        PageNo = pageResult.PageNo,
                        PageSize = pageResult.PageSize,
                        TotalPage = pageResult.TotalPage,
                        TotalCount = pageResult.TotalCount,
                        Data = pageResult.Data.Select(e => new AdminUserManagerIncomeExpensesList
                        {
                            TransactionType = e.TransactionType,
                            Amount = e.Amount,
                            Category = e.Category,
                            CreateTime = e.CreateTime,
                            Id = e.Id,
                            Memo = e.Title,
                            PayType = e.PayType,
                            UserId = e.UserId,
                            Rebate = e.Rebate
                        }).ToArray()
                    }
                };
            }, model));
        }

        private static List<IncomeExpenseCategoryEnum> GetCategoriesByParam(AdminUserManagerIncomeExpensesListParam model)
        {
            List<IncomeExpenseCategoryEnum> categories = new List<IncomeExpenseCategoryEnum>();

            if (model.PostType.HasValue)
            {
                categories.Add(model.PostType.Value.ConvertToIncomeExpenseCategory());
            }
            else if (model.Category.HasValue)
            {
                switch (model.Category.Value)
                {
                    case AdminIncomeExpensesCategory.PostUnLock:
                    case AdminIncomeExpensesCategory.PostIncome:
                    case AdminIncomeExpensesCategory.UnLockRefund:
                        categories.Add(IncomeExpenseCategoryEnum.Square);
                        categories.Add(IncomeExpenseCategoryEnum.Agency);
                        categories.Add(IncomeExpenseCategoryEnum.Experience);
                        break;

                    case AdminIncomeExpensesCategory.UnBooking:
                    case AdminIncomeExpensesCategory.Booking:
                        categories.Add(IncomeExpenseCategoryEnum.Official);
                        break;

                    case AdminIncomeExpensesCategory.MembershipCard:
                        categories.Add(IncomeExpenseCategoryEnum.Vip);
                        break;
                }
            }

            return categories;
        }

        /// <summary>
        /// 身份认证申请记录
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminUserManagerIdentityApplyList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IdentityApplyRecord(AdminUserManagerIdentityApplyListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                PageResultModel<MMIdentityApply> pageResult = await _identityApply.List(model);
                return new BaseReturnDataModel<PageResultModel<AdminUserManagerIdentityApplyList>>(ReturnCode.Success)
                {
                    DataModel = _mapper.Map<PageResultModel<AdminUserManagerIdentityApplyList>>(pageResult)
                };
            }, model));
        }

        /// <summary>
        /// 身份认证申请
        /// </summary>
        /// <param name="param">申请信息</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> IdentityApply(AdminUserManagerIdentityApplyParam param)
        {
            return ApiSuccessResult(await TryCatchProcedure(async (param) =>
            {
                if (param.ApplyIdentity == 3)
                {
                    var identityApplyList = _mapper.Map<AdminUserManagerIdentityApplyList>(
                        await _identityApply.DetailByUserId(param.UserId, 1)
                    );

                    if (identityApplyList == null)
                    {
                        ReqBossIdentityApplyData model = new ReqBossIdentityApplyData
                        {
                            UserId = param.UserId,
                            IsAdminApply = true
                        };

                        var applyResult = await _auth.BossIdentityApply(model);
                        if (applyResult.IsSuccess)
                        {
                            var resultApply = await _userInfoRepo.UserIdentityApply(param);
                            return new BaseReturnModel(resultApply.GetReturnCode());
                        }
                        return new BaseReturnModel(applyResult.ReturnCode);
                    }
                }
                var result = await _userInfoRepo.UserIdentityApply(param);
                return new BaseReturnModel(result.GetReturnCode());
            }, param));
        }

        /// <summary>
        /// 根据审核id或用户id获取身份认证信息
        /// </summary>
        /// <param name="id">审核id或用户id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<AdminUserManagerIdentityApplyList>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IdentityApplyDetail(string id)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                AdminUserManagerIdentityApplyList result = null;

                if (int.TryParse(id, out int userId))
                {
                    var user = await _userInfoRepo.GetUserInfo(userId);
                    if (user.UserIdentity == (int)IdentityType.Boss)
                    {
                        // 根据用户id获取申请信息
                        result = _mapper.Map<AdminUserManagerIdentityApplyList>(await _identityApply.DetailByUserId(userId, 1));

                        // 用户已经是觅老板，但是没有申请记录的情况重新申请，避免数据关联不上
                        if (result == null)
                        {
                            ReqBossIdentityApplyData model = new ReqBossIdentityApplyData();
                            model.UserId = user.UserId;
                            model.IsAdminApply = true;

                            var applyResult = await _auth.BossIdentityApply(model);
                            //觅老板申请成功重新查询申请信息，避免后台编辑无数据报错
                            if (applyResult.IsSuccess)
                            {
                                result = _mapper.Map<AdminUserManagerIdentityApplyList>(await _identityApply.DetailByUserId(userId, 1));
                            }
                        }
                    }
                }
                else
                {
                    // 根据审核id获取信息
                    var detailById = await _identityApply.Detail(id);
                    if (detailById != null)
                    {
                        result = _mapper.Map<AdminUserManagerIdentityApplyList>(detailById);
                    }
                }

                if (result != null)
                {
                    var boss = await _userInfoRepo.GetByApplyId(result.ApplyId);

                    if (boss != null)
                    {
                        result.BossId = boss.BossId;
                        result.ShopName = boss.ShopName;
                        result.Girls = string.IsNullOrWhiteSpace(boss.Girls) ? null : int.Parse(boss.Girls);
                        result.ContactApp = result.ContactApp;
                        result.ContactInfo = result.ContactInfo;
                        result.DealOrder = boss.DealOrder;
                        result.ShopYears = boss.ShopYears;
                        result.SelfPopularity = boss.SelfPopularity;
                        result.ViewBaseCount = boss.ViewBaseCount;
                        result.Introduction = boss.Introduction;
                        result.BusinessDate = boss.BusinessDate;
                        result.BusinessHour = boss.BusinessHour;

                        var shopAvatar = (await GetMedia.Get(SourceType.BossApply, new string[] { boss.BossId }))?.DataModel?.OrderBy(p => p.CreateDate);
                        var businessPhotos = (await GetMedia.Get(SourceType.BusinessPhoto, new string[] { boss.BossId }))?.DataModel?.OrderBy(p => p.CreateDate);

                        result.ShopAvatarSource = shopAvatar?.Where(p => p.MediaType == (int)MediaType.Image)?.ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>();
                        result.BusinessPhotoSource = businessPhotos?.Where(p => p.MediaType == (int)MediaType.Image)?.ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>();
                    }
                }

                return new BaseReturnDataModel<AdminUserManagerIdentityApplyList>(ReturnCode.Success)
                {
                    DataModel = result
                };
            }, id));
        }

        /// <summary>
        /// 身份认证审核
        /// </summary>
        /// <param name="param">申请信息</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> IdentityAudit(AdminUserManagerIdentityAuditParam param)
        {
            return ApiSuccessResult(await TryCatchProcedure(async (param) =>
            {
                var result = await _identityApply.UserIdentityAudit(param);
                return new BaseReturnModel(result.GetReturnCode());
            }, param));
        }

        private static IncomeExpenseTransactionTypeEnum? GetTransactionType(AdminIncomeExpensesCategory? category)
        {
            if (category == null)
                return null;

            switch (category)
            {
                case AdminIncomeExpensesCategory.Booking:
                case AdminIncomeExpensesCategory.MembershipCard:
                case AdminIncomeExpensesCategory.PostUnLock:
                    {
                        return IncomeExpenseTransactionTypeEnum.Expense;
                    }
                case AdminIncomeExpensesCategory.PostIncome:
                    {
                        return IncomeExpenseTransactionTypeEnum.Income;
                    }
                case AdminIncomeExpensesCategory.UnLockRefund:
                case AdminIncomeExpensesCategory.UnBooking:
                    {
                        return IncomeExpenseTransactionTypeEnum.Refund;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 会员保证金调整记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<List<UserEarnestMoneyData>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UserEarnestMoneyList(AdminUserManagerEarnestMoneyHisParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<List<UserEarnestMoneyData>>
                {
                    DataModel = JsonUtil.CastByJson<List<UserEarnestMoneyData>>(await _userInfoRepo.GetEarnestMoneyData(param.UserId)).Take(param.MaxCount).ToList()
                };
                result.SetCode(ReturnCode.Success);
                return result;
            }, param));
        }

        /// <summary>
        /// 保证金修改审核
        /// </summary>
        /// <param name="param">申请信息</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> EarnestMoneyAudit(AdminUserManagerEarnestMoneyChangeParam param)
        {
            return ApiSuccessResult(await TryCatchProcedure(async (param) =>
            {
                var isSuccess = await _userInfoRepo.EarnestMoneyAudit(param);
                return new BaseReturnModel(isSuccess ? ReturnCode.Success : ReturnCode.OperationFailed);
            }, param));
        }

        /// <summary>
        /// 后台店铺编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> StoreEdit(AdminUserBossParam param)
        {
            return ApiSuccessResult(await TryCatchProcedure(async (param) =>
            {
                var isSuccess = await _userInfoRepo.StoreEdit(param);

                var businessPhotos = (await GetMedia.Get(SourceType.BusinessPhoto, param.BossId))?.DataModel;
                var shopAvatar = (await GetMedia.Get(SourceType.BossApply, param.BossId))?.DataModel;

                // 更新成功則刪除雲倉
                if (isSuccess && shopAvatar?.Any() == true)
                {
                    await DeleteBossOldMediaFile(param.ShopAvatar.Split(',').ToArray(),
                        shopAvatar);
                }
                if (isSuccess && businessPhotos?.Any() == true)
                {
                    await DeleteBossOldMediaFile(param.BusinessPhotoUrls.Split(',').ToArray(),
                        businessPhotos);
                }
                return new BaseReturnModel(isSuccess ? ReturnCode.Success : ReturnCode.OperationFailed);
            }, param));
        }

        private async Task DeleteBossOldMediaFile(string[] Ids, MediaInfo[]? medias)
        {
            var deleteImageId = medias?
                .Where(p => p.MediaType == (int)MediaType.Image)
                .Select(p => p.Id)?
                .ToArray()
                .Except(Ids) ?? Array.Empty<string>();

            foreach (var delMediaId in deleteImageId)
            {
                //刪除非絕對重要，因此若失敗可以忽略，不影響原流程
                try
                {
                    await GetMedia.DeleteToOss(delMediaId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}。刪除圖檔發生異常。Media table Id = {delMediaId}");
                }
            }
        }
    }
}