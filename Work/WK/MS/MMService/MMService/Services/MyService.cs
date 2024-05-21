using AutoMapper;
using AutoMapper.Execution;
using MMService.Models;

using MMService.Models.My;
using MS.Core.Extensions;
using MS.Core.Infrastructure.OSS;
using MS.Core.Infrastructures.Providers;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Models.Booking.Enums;
using MS.Core.MM.Models.Entities.HomeAnnouncement;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Models.User;
using MS.Core.MM.Models.Vip;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Services;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using MS.Core.MMModel.Models.AdminReport;
using MS.Core.MM.Models.Entities.MessageUserRead;
using RabbitMQ.Client;
using System.Threading;
using MS.Core.MM.Models.Booking.Req;
using MS.Core.MM.Models.Booking.Res;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Models.Post;
using MS.Core.MMModel.Models.Post;
using MS.Core.MMModel.Models.HomeAnnouncement;
using MS.Core.MMModel.Models.Favorite;
using MS.Core.MMModel.Models.Favorite.Enums;
using MS.Core.MMModel.Models.Media.Enums;
using Microsoft.Extensions.Options;
using static Quartz.Logging.OperationName;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.ReportMessage;

namespace MMService.Services
{
    /// <summary>
    /// 個人相關
    /// </summary>
    public class MyService : BaseService, IMyService
    {
        private IIncomeExpenseRepo _incomeExpense { get; }
        private IPostRepo _postRepo { get; }
        private IPostService _postService { get; }
        private IPostTransactionRepo _trans { get; }
        private IMapper _mapper { get; }

        private IZeroOneApiService ZeroOneApi { get; }
        private IVipService VipService { get; }

        private IUserSummaryService UserSummaryService { get; }

        private IObjectStorageService _ossService { get; }

        private IDateTimeProvider DateTimeProvider { get; }

        private IHomeAnnouncementRepo _announcement { get; }
        private IBookingRepo _bookingRepo { get; }
        private IUserToMessageOperationRepo _userMessageRepo { get; }

        /// <summary>
        /// 解鎖贴子回應服務項目最大數量
        /// </summary>
        private readonly int UnlockListMaxTakeServiceItemCount = 4;

        /// <summary>
        /// 用戶相關資源
        /// </summary>
        private readonly IUserInfoRepo _userInfoRepo;

        /// <summary>
        /// 媒體服務
        /// </summary>
        private readonly IEnumerable<IMediaService> _mediaServices;

        /// <summary>
        /// 選項設定
        /// </summary>
        private readonly IOptionItemRepo _optionRepo;

        private IMediaService getMedia => _mediaServices.FirstOrDefault(m => m.Type == MediaType.Image);

        public MyService(IIncomeExpenseRepo incomeExpense,
            IPostRepo post,
            IPostTransactionRepo trans,
            IPostService postService,
            IMapper mapper,
            IVipService vip,
            IZeroOneApiService zeroOneApi,
            IUserSummaryService userQuantityService,
            IObjectStorageService ossService,
            IDateTimeProvider dateTimeProvider,
            IUserInfoRepo userInfoRepo,
            IBookingRepo bookingRepo,
            IHomeAnnouncementRepo homeAnnouncementRepo,
            IUserToMessageOperationRepo userToMessageOperationRepo,
            ILogger<MyService> logger, IEnumerable<IMediaService> mediaServices, IOptionItemRepo optionRepo) : base(logger)
        {
            _incomeExpense = incomeExpense;
            _postRepo = post;
            _trans = trans;
            _postService = postService;
            _mapper = mapper;
            _ossService = ossService;
            ZeroOneApi = zeroOneApi;
            VipService = vip;
            UserSummaryService = userQuantityService;
            DateTimeProvider = dateTimeProvider;
            _userInfoRepo = userInfoRepo;
            _bookingRepo = bookingRepo;
            _announcement = homeAnnouncementRepo;
            _userMessageRepo = userToMessageOperationRepo;
            _mediaServices = mediaServices;
            _optionRepo = optionRepo;
        }

        /// <summary>
        /// 取出選項設定中的值
        /// </summary>
        private static readonly Func<int, Dictionary<int, string>, string> TryGetOptionValue =
            (key, options) =>
            {
                options.TryGetValue(key, out var value);
                return value ?? string.Empty;
            };
        /// <summary>
        /// 抽出服務項目的中文字
        /// </summary>
        private Func<string, MMPostServiceMapping[], Dictionary<int, string>, string[]> MappingServiceItem =
            (postId, mappingItem, options) =>
            {
                return mappingItem
                    .Where(p => p.PostId == postId)
                    .Select(p => TryGetOptionValue(p.ServiceId, options))
                    .Where(p => !string.IsNullOrWhiteSpace(p))?
                    .ToArray() ?? Array.Empty<string>();
            };

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<CenterInfo>> Center(int userId)
        {
            return await base.TryCatchProcedure(async () =>
            {
                UserSummaryInfoData userInfoData = await VipService.GetUserSummaryInfoData(userId);

                ZOUserInfoRes zOUserInfo = await ZeroOneApi.GetUserInfo(new ZOUserInfoReq(userId)).GetReturnDataAsync();

                #region 根据不同身份获取用户所有发帖数
                MMUserInfo mUserInfo =  await _userInfoRepo.GetUserInfo(userId);

                int postCount = 0;

                switch((IdentityType)mUserInfo.UserIdentity)
                {
                    case IdentityType.Boss:
                        postCount = userInfoData.PostTypeSendTimes(PostType.Agency) + userInfoData.PostTypeSendTimes(PostType.Official);
                        break;
                    case IdentityType.Agent:
                        postCount = userInfoData.PostTypeSendTimes(PostType.Agency) + userInfoData.PostTypeSendTimes(PostType.Square);
                        break;
                    default:
                        postCount = userInfoData.PostTypeSendTimes(PostType.Square);
                        break;
                }
                #endregion


                int putOnShelvesCount = await _postRepo.GetPostCount(userId, ReviewStatus.Approval);
                int officialPutOnShelvesCount = await _postRepo.GetOfficialPostCount(userId, ReviewStatus.Approval);

                int unlockCount = await _trans.GetUnlockCount(userId);

                var unlockPostId = await _trans.GetUnlockPostId(userId);
                if (unlockPostId.IsNotEmpty()) {
                    int unlock = await _postRepo.GetPostCountByIdAndStatus(unlockPostId.ToArray(), ReviewStatus.Approval);
                    unlockCount = unlock;
                }
                ///获取用户未读消息数量
                var unread = await _announcement.GetUnreadCount(userId);

                int collectTotal = 0;
                int collectShopCount = 0;
                int collectXfgCount = 0;
                int collectSquareCount = 0;

                var collectData = await _postRepo.GetAllMyFavorite(new FavoriteListParam() { UserId = userId });

                if (collectData != null)
                {
                    collectTotal = collectData.Count();
                    collectShopCount = collectData.Count(c => c.Type == (int)FavoriteType.Shop);
                    var postIds = collectData.Where(c => c.Type == (int)FavoriteType.Post).Select(c => c.PostId).ToArray();
                    if (postIds.Any())
                    {
                        var xfgPostData = await _userInfoRepo.GetFavoritePost(postIds, (int)PostType.Agency);
                        if (xfgPostData.Any())
                        {
                            collectXfgCount = xfgPostData.Count();
                        }
                        var squarePostData = await _userInfoRepo.GetFavoritePost(postIds, (int)PostType.Square);
                        if (squarePostData.Any())
                        {
                            collectSquareCount = squarePostData.Count();
                        }
                    }
                }

                decimal income = await _incomeExpense.GetMonthIncome(userId, DateTimeProvider.Now);

                int postDailyCount = await _postRepo.GetPostDailyCount(userId);

                //剩余发帖数
                int remainingSend = userInfoData.RemainingUnlock;

                int showRemainingSend = userInfoData.RemainingUnlock;

                if (userInfoData.IsVip && userInfoData.UserInfo.UserIdentity != (int)IdentityType.Boss && userInfoData.UserInfo.UserIdentity != (int)IdentityType.Agent)
                {
                    showRemainingSend = showRemainingSend - 1;

                    remainingSend = postDailyCount > 0 ? remainingSend : remainingSend + 1;

                    //发帖次数如果<=0 就只显示用户的免费发帖次数，否则就显示剩余发帖次数
                    if(showRemainingSend <= 0)
                    {
                        showRemainingSend = postDailyCount > 0 ? 0 : 1;
                    }
                }

                int bookingCount = await _bookingRepo.GetBookingPostCount(new BookingFilter()
                {
                    UserId = userId,
                    Statuses = new List<BookingStatus>() {
                        BookingStatus.InService,
                        BookingStatus.RefundInProgress,
                    }
                });

                //获取用户收藏数
                //UserFavoriteStatistics[] statisticInfos = GetUserFavoriteStatistics(userInfoData);

                return new BaseReturnDataModel<CenterInfo>(ReturnCode.Success)
                {
                    DataModel = new CenterInfo()
                    {
                        UserId = userId,
                        BookingCount = bookingCount,
                        PutOnShelvesCount = Math.Abs(postCount),
                        Income = income.ToString(GlobalSettings.AmountFormat),
                        Point = zOUserInfo?.Point.ToString(GlobalSettings.AmountFormat) ?? "0",
                        RewardsPoint = 0,
                        Vips = userInfoData.Vips?.OrderByDescending(e => e.Priority).Select(e =>
                        {
                            DateTime time = DateTimeProvider.Now;
                            return new ResUserVip
                            {
                                Type = e.VipType,
                                Name = e.TypeName
                            };
                        })?.ToArray() ?? Array.Empty<ResUserVip>(),
                        Quantity = new UserSummaryInfo
                        {
                            ShowRemainingSend= showRemainingSend,
                            RemainingFreeUnlock = userInfoData.RemainingFreeUnlock,
                            RemainingSend = remainingSend < 0 ? 0 : remainingSend,
                            TotalSend = userInfoData.TotalPostSendTimes,
                            TotalIncome = userInfoData.TotalIncome
                        },
                        Amount = zOUserInfo?.Amount.ToString(GlobalSettings.AmountFormat) ?? "0",
                        RegisterTime = userInfoData.UserInfo.RegisterTime,
                        UnlockCount = unlockCount,
                        Identity = (IdentityType)userInfoData.UserInfo.UserIdentity,
                        HasPhone = zOUserInfo?.HasPhone ?? false,
                        ContactType = ContactType.Phone,
                        Contact = "",
                        Avatar = zOUserInfo?.Avatar ?? "",
                        UnreadMessage= unread,
                        CollectShopCount = collectShopCount,
                        CollectSquareCount = collectSquareCount,
                        CollectXfgCount = collectXfgCount,
                        //UserFavorites= statisticInfos,
                    },
                };
            });
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<Overview>> Overview(int userId, string nickname)
        {
            return await TryCatchProcedure(async (param) =>
            {
                UserSummaryInfoData userInfo = await VipService.GetUserSummaryInfoData(param.UserId);

                ZOUserInfoRes zOUserInfo = await ZeroOneApi.GetUserInfo(new ZOUserInfoReq(param.UserId)).GetReturnDataAsync();

                decimal freezeIncome = await _incomeExpense.GetFreezeIncome(param.UserId);

                decimal income = await _incomeExpense.GetMonthIncome(param.UserId, DateTimeExtension.GetCurrentTime());

                var count= await _postRepo.GetOfficialPostCount(userId);


                List<int> cardType = new List<int>();
                VipType? postUserVipType = (await VipService.GetUserInfoData(param.UserId)).CurrentVip?.VipType;
                if (postUserVipType.HasValue)
                {
                    cardType.Add((int)postUserVipType.Value);
                }

                OverviewPostTypeStatisticInfo[] statisticInfos = GetPostTypeStatisticInfos(userInfo);

                return new BaseReturnDataModel<Overview>(ReturnCode.Success)
                {
                    DataModel = new Overview()
                    {
                        AvatarUrl = zOUserInfo?.Avatar,
                        IsOpen = userInfo.UserInfo.IsOpen,
                        EarnestMoney = userInfo.UserInfo.EarnestMoney.ToString(GlobalSettings.AmountFormat),
                        FreezeIncome = freezeIncome.ToString(GlobalSettings.AmountFormat),
                        Income = income.ToString(GlobalSettings.AmountFormat),
                        Integral = 0,
                        Level = userInfo.UserInfo.UserLevel,
                        UserIdentity = (IdentityType)(userInfo.UserInfo?.UserIdentity ?? (int)IdentityType.General),
                        CardType = cardType.ToArray(),
                        Nickname = zOUserInfo.NickName,
                        PublishLimit = userInfo.PublishLimit,
                        RemainPublish = userInfo.RemainingUnlock,
                        Statistic = statisticInfos,
                        RemainingFreeUnlock= userInfo.RemainingFreeUnlock,
                    },
                };
            }, new { UserId = userId, Nickname = nickname });
        }
        /// <summary>
        /// 用户广场区,寻芳阁,店铺的收藏数
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static UserFavoriteStatistics[] GetUserFavoriteStatistics(UserSummaryInfoData userInfo)
        {
            UserFavoriteStatistics[] userFavoriteStatistics = new[]
            {
               GetUserFavoriteStatistic(userInfo,UserFavoriteCategoryEnum.Agency),
               GetUserFavoriteStatistic(userInfo,UserFavoriteCategoryEnum.Square),
               GetUserFavoriteStatistic(userInfo,UserFavoriteCategoryEnum.Shop),
            };
            return userFavoriteStatistics;
        }
        /// <summary>
        /// 發贴數據統計
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private static OverviewPostTypeStatisticInfo[] GetPostTypeStatisticInfos(UserSummaryInfoData userInfo)
        {
            OverviewPostTypeStatisticInfo[] statisticInfos = new[]
               {
                    GetPostTypeStatisticInfo(userInfo, PostType.Square),
                    GetPostTypeStatisticInfo(userInfo, PostType.Agency),
                    GetPostTypeStatisticInfo(userInfo, PostType.Official),
                };
            return statisticInfos;
        }

        /// <summary>
        /// 發贴數據統計
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="postType"></param>
        /// <returns></returns>
        private static OverviewPostTypeStatisticInfo GetPostTypeStatisticInfo(
            UserSummaryInfoData userInfo, PostType postType)
        {
            return new OverviewPostTypeStatisticInfo
            {
                PublishedCount = userInfo.PostTypeSendTimes(postType),
                Type = postType,
                UnlockCount = postType == PostType.Official ? userInfo.PostTypeAppointmentedCount(postType) : userInfo.PostTypeUnlocked(postType),
                TotalIncome = userInfo.PostTypeIncome(postType).ToString(GlobalSettings.AmountFormat),
            };
        }

        /// <summary>
        /// 收藏数统计
        /// </summary>
        /// <param name="userSummaryInfo"></param>
        /// <param name="userFavoriteCategory"></param>
        /// <returns></returns>
        private static UserFavoriteStatistics GetUserFavoriteStatistic(UserSummaryInfoData userSummaryInfo, UserFavoriteCategoryEnum userFavoriteCategory)
        {
            return new UserFavoriteStatistics
            {
                FavoriteEnum = userFavoriteCategory,
                Favorites = userSummaryInfo.FavoritesCount(userFavoriteCategory),
            };
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<PageResultModel<MyUnlockPostList>>> UnlockPost(MyUnlockQueryParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<MyUnlockPostList>>();
                param.Status = ReviewStatus.Approval;

                result.DataModel = _mapper.Map<PageResultModel<MyUnlockPostList>>(await _postRepo.PostSearch(param));
                var postIds = result.DataModel?.Data?.Select(p => p.PostId)?.ToArray() ?? Array.Empty<string>();

                var options = await _optionRepo.GetPostTypeOptions(param.PostType);
                var favorites = await _postRepo.GetMMPostFavorite(param.UserId, postIds);

                foreach (var item in result.DataModel.Data)
                {
                    try
                    {
                        item.IsFavorite = favorites.Count(c => c.PostId == item.PostId) > 0 ? true : false;
                        item.Job = TryGetOptionValue(item.MessageId, options);
                        item.ServiceItem = (await _postService.GetServiceItemsText(item.PostId))
                            .Take(UnlockListMaxTakeServiceItemCount)
                            .ToArray();

                        item.Unlocks = item.Unlocks + item.UnlockBaseCount;
                        item.Views = (int.Parse(item.Views) + item.ViewBaseCount).ToString();
                        item.CoverUrl = await _ossService.GetCdnPath(item.CoverUrl);
                        item.LowPrice = Convert.ToInt64(
                        item.LowPrice.Substring(0, item.LowPrice.IndexOf('.') > 0 ?
                                item.LowPrice.IndexOf('.') :
                                item.LowPrice.Length)).ToString();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "UnlockPost fails");
                    }
                }
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
        /// <summary>
        /// 用户操作所有消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<BaseReturnModel> UserToMessageAll(MessageOperationParamForClient param)
        {
            return TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnModel();

                ///获取用户已经有的消息操作记录
                var userMessageList = await _userMessageRepo.GetMessageListByUserId(param.UserId);

                var updateModels = new List<MMUserToMessageOperation>();
                var insertModels = new List<MMUserToMessageOperation>();

                var MessageIds= new List<string>();

                //如果没传MessageIds 的话， 就是用户已读或者删除全部消息
                if (param.MessageIds.Any()){
                    MessageIds.AddRange(param.MessageIds);
                }
                else{

                    if (param.MessageType == MessageType.Announcement){

                        var announcementData = await _announcement.GetAllAnnouncement();
                        if (announcementData.Any())
                            MessageIds.AddRange(announcementData.Select(c => c.Id.ToString()));
                    }
                    else{

                        var reportData = await _postRepo.GetReportAllByUserId(param.UserId);
                        if (reportData.Any())
                            MessageIds.AddRange(reportData);
                    }
                }



                var updateMessageIds = MessageIds.Intersect(userMessageList.Where(c => (param.MessageOperationType.Equals(MessageOperationType.UserRead) ? c.IsRead = true : c.IsDelete = true)).Select(c => c.MessageId));

                if (updateMessageIds.Any())
                {
                    foreach (var item in userMessageList)
                    {
                        if (updateMessageIds.Contains(item.MessageId))
                        {
                            if (param.MessageOperationType.Equals(MessageOperationType.UserDelete))
                                item.IsDelete = true;
                            else
                                item.IsRead = true;

                            updateModels.Add(item);
                        }
                    }
                }

                var insertMessageIds = MessageIds.Except(userMessageList.Select(c => c.MessageId));
                if (insertMessageIds.Any())
                {
                    insertModels.AddRange(insertMessageIds.Select(c =>
                    new MMUserToMessageOperation()
                    {
                        CreateTime = DateTime.Now,
                        MessageId = c,
                        UserId = param.UserId,
                        MessageType = param.MessageType,

                        IsDelete = param.MessageOperationType.Equals(MessageOperationType.UserDelete),
                        IsRead = param.MessageOperationType.Equals(MessageOperationType.UserDelete)?true:param.MessageOperationType.Equals(MessageOperationType.UserRead)
                    }));
                }

                await _userMessageRepo.InsertAndUpdateMessage(insertModels, updateModels);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
         }
       
        /// <summary>
        /// 用户已读全部消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<BaseReturnModel> UserToMessageOperation(MessageOperationParamForClient param)
        {
            return TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnModel();

                if (!param.MessageIds.Any())
                {
                    result.SetCode(ReturnCode.OperationFailed);
                    return result;
                }

                var userMessageList = await _userMessageRepo.GetMessageListByUserId(param.UserId);

                var updateModels = new List<MMUserToMessageOperation>();
                var insertModels = new List<MMUserToMessageOperation>();

                var updateMessageIds = param.MessageIds.Intersect(userMessageList.Where(c => (param.MessageOperationType.Equals(MessageOperationType.UserRead) ? c.IsRead = true : c.IsDelete = true)).Select(c => c.MessageId));

                if (updateMessageIds.Any())
                {
                    foreach (var item in userMessageList)
                    {
                        if (updateMessageIds.Contains(item.MessageId))
                        {
                            if (param.MessageOperationType.Equals(MessageOperationType.UserDelete))
                                item.IsDelete = true;
                            else
                                item.IsRead = true;

                            updateModels.Add(item);
                        }
                    }
                }

                var insertMessageIds = param.MessageIds.Except(userMessageList.Select(c => c.MessageId));
                if (insertMessageIds.Any())
                {
                    insertModels.AddRange(insertMessageIds.Select(c =>
                    new MMUserToMessageOperation()
                    {
                        CreateTime = DateTime.Now,
                        MessageId = c,
                        UserId = param.UserId,
                        MessageType = param.MessageType,

                        IsDelete = param.MessageOperationType.Equals(MessageOperationType.UserDelete),
                        IsRead = param.MessageOperationType.Equals(MessageOperationType.UserRead)
                    }));
                }

                await _userMessageRepo.InsertAndUpdateMessage(insertModels, updateModels);

                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
        /// <summary>
        /// 根据ID获取公告详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<BaseReturnDataModel<MyAnnouncementViewModel>> GetAnnouncementById(int Id)
        {
            return TryCatchProcedure(async (Id) =>
            {
                var model = await _announcement.GetAnnouncementById(Id);

                return new BaseReturnDataModel<MyAnnouncementViewModel>(ReturnCode.Success)
                {
                    DataModel = new MyAnnouncementViewModel()
                    {
                        Id = model.Id,
                        HomeContent = model.HomeContent,
                        CreateDate = model.CreateDate,
                        ModifyDate = model.ModifyDate,
                        RedirectUrl = model.RedirectUrl,
                        Title = model.Title,
                    }
                };
            }, Id);
        }
        public Task<BaseReturnDataModel<ReportDetailViewModel>> ReportDetail(string reportId)
        {
            return TryCatchProcedure(async (reportId) =>
            {
                var model = (await _postService.ReportDetail(reportId)).DataModel;
                if (model == null)
                {
                    return new BaseReturnDataModel<ReportDetailViewModel>(ReturnCode.DataIsNotExist);
                }

                var resultData = new ReportDetailViewModel()
                {
                    CreateTime = model.ExamineTime != null ? model.ExamineTime.Value : model.CreateTime,
                    Describe = model.Describe,
                    Memo = model.Memo,
                    PhotoIds = model.PhotoIds,
                    PostId = model.PostId,
                    ReportId = model.ReportId,
                    ReportTypeText = model.ReportTypeText,
                    PostType = model.PostType,
                };

                if (model.PostType == PostType.Official)
                {
                    var postInfo = await _postRepo.GetOfficialPostStatusByPostId(model.PostId);
                    if (postInfo == null)
                    {
                        resultData.PostStatus = ReviewStatus.NotApproved;
                        resultData.PostIsDelete = true;
                    }
                    else
                    {
                        resultData.PostStatus = postInfo.Status;
                        resultData.PostIsDelete = postInfo.IsDelete;
                    }
                }
                else
                {
                    var postInfo = await _postRepo.GetPostStatusByPostId(model.PostId);

                    if (postInfo == null)
                    {
                        resultData.PostStatus = ReviewStatus.NotApproved;
                    }
                    else
                    {
                        resultData.PostStatus = postInfo.Status;
                    }
                }

                return new BaseReturnDataModel<ReportDetailViewModel>(ReturnCode.Success)
                {
                    DataModel = resultData
                };
            }, reportId);
        }

        /// <summary>
        /// 取消收藏
        ///
        /// </summary>
        /// <param name="favoriteId"></param>
        /// <returns></returns>
        public Task<BaseReturnModel> CanCelFavorite(string favoriteId)
        {
            return TryCatchProcedure(async (favoriteId) =>
            {
                var result = await _postRepo.CanCelFavorite(favoriteId);

                return new BaseReturnModel(ReturnCode.Success);
            }, favoriteId);
        }
        /// <summary>
        /// 获取我的收藏
        /// </summary>
        /// <returns></returns>
        public Task<BaseReturnDataModel<PageResultModel<MyFavorite>>> GetMyFavorite(MyFavoriteQueryParamForClient param)
        {
            return TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<MyFavorite>>();
                result.SetCode(ReturnCode.Success);
                result.DataModel = new PageResultModel<MyFavorite>() { };

                var paramterFavoriteType = param.FavoriteType == FavoriteTypeParamter.Shop ? FavoriteType.Shop : FavoriteType.Post;
                var postType = param.FavoriteType == FavoriteTypeParamter.SquarePost ? PostType.Square : PostType.Agency;

                var favoriteData = await _postRepo.GetMyFavoriteByUserId(param.UserId, (int)paramterFavoriteType);
                if (!favoriteData.Any())
                {
                    return result;
                }



                if (paramterFavoriteType == FavoriteType.Shop)
                {

                    
                    var shopDatas = await GetMyFavoriteOfficialShopData(favoriteData.Select(c=>c.PostId).ToArray());
                    if (!shopDatas.Any())
                    {
                        return result;
                    }
                  
                 

                    //删除审核不通过的店铺收藏记录
                    DeleteFavoriteShop(favoriteData, shopDatas);

                    var myFavoriteResult = _mapper.Map<PageResultModel<MyFavorite>>(await _postRepo.GetMyFavoritePageAsync(new FavoriteListParam()
                    {
                        FavoriteType = (int)paramterFavoriteType,
                        Page = param.Page,
                        PageNo = param.PageNo,
                        PageSize = param.PageSize,
                        UserId = param.UserId,
                    }, null));
                    if (!myFavoriteResult.Data.Any())
                    {
                        return result;
                    }

                    //根据查询到的分页数据生成ID的数组，用于去查询商铺和帖子列表数据
                    //var applyIdOrPostIds = myFavoriteResult.Data.Select(c => c.PostId).ToArray();

                    //获取到商铺列表数据
                    //var shopDatas = await GetMyFavoriteOfficialShopData(applyIdOrPostIds);
                    if (!shopDatas.Any())
                    {
                        return result;
                    }
                    //将商铺数据加入到收藏model中
                    foreach (var item in myFavoriteResult.Data)
                    {
                        var shopInfo = shopDatas.Where(c => c.ApplyId == item.PostId).First();

                        var mediaObject = (await getMedia.Get(SourceType.BossApply, new string[] { shopInfo.BossId }));
                        if (mediaObject == null || !mediaObject.DataModel.Any())
                        {
                            continue;
                        }

                        string strFullMediaUrl = mediaObject.DataModel[0].FullMediaUrl;

                        item.PostId = shopInfo.ApplyId;
                        item.AvatarSource = strFullMediaUrl;
                        item.Title = shopInfo.ShopName;
                        item.Girls = shopInfo.Girls;
                        item.ViewBaseCount = shopInfo.ViewBaseCount + shopInfo.Views;
                        item.ShopYears = shopInfo.ShopYears;
                        item.OrderQuantity = shopInfo.OrderQuantity;
                        item.SelfPopularity = shopInfo.SelfPopularity;
                    }

                    result.SetCode(ReturnCode.Success);
                    result.DataModel = myFavoriteResult;
                }
                else
                {
                    //删除审核不通过的帖子收藏记录
                    DeleteFavoritePost(favoriteData, (int)postType);

                    //查询到该用户所有的帖子收藏记录
                    var favorites = await _postRepo.GetAllMyFavorite(new FavoriteListParam()
                    {
                        FavoriteType = (int)paramterFavoriteType,
                        Page = param.Page,
                        PageNo = param.PageNo,
                        PageSize = param.PageSize,
                        UserId = param.UserId,
                    });
                    if (!favoriteData.Any())
                    {
                        return result;
                    }
                    //从收藏记录内提取到记录的postID
                    var postIdArray = favoriteData.Select(c => c.PostId).ToArray();
                    //根据ID和类型 查询帖子列表数据
                    var postData = await _userInfoRepo.GetFavoritePost(postIdArray, (int)postType);
                    if (!postData.Any())
                    {
                        return result;
                    }

                    var allServiceSettings = (await _postRepo.GetPostMappingService(postData.Select(c => c.PostId).ToArray())) ?? Array.Empty<MMPostServiceMapping>();

                    //根据帖子的ID集合在去查询收藏记录的分页数据
                    var myFavoriteResult = _mapper.Map<PageResultModel<MyFavorite>>(await _postRepo.GetMyFavoritePageAsync(new FavoriteListParam()
                    {
                        FavoriteType = (int)paramterFavoriteType,
                        Page = param.Page,
                        PageNo = param.PageNo,
                        PageSize = param.PageSize,
                        UserId = param.UserId,
                    }, postData.Select(c => c.PostId).ToArray()));

                    if (!myFavoriteResult.Data.Any())
                    {
                        return result;
                    }
                    var options = await _optionRepo.GetPostTypeOptions(postType);

                    //将帖子数据填充到收藏记录表内返回给前台
                    foreach (var item in myFavoriteResult.Data)
                    {
                        var postInfo = postData.Where(c => c.PostId == item.PostId).SingleOrDefault();
                        if (postInfo == null)
                        {
                            continue;
                        }
                        var mediaObject = (await getMedia.Get(SourceType.Post, new string[] { postInfo.PostId }));
                        if (mediaObject == null || !mediaObject.DataModel.Any())
                        {
                            continue;
                        }

                        string strFullMediaUrl = mediaObject.DataModel[0].FullMediaUrl;

                        string jop = TryGetOptionValue(postInfo.MessageId, options);

                        item.Title = postInfo.Title;
                        item.AvatarSource = strFullMediaUrl;
                        item.AreaCode = postInfo.AreaCode;
                        item.Jop = jop;

                        item.Age = Enum.IsDefined(typeof(AgeDefined), (int)postInfo.Age) ?
                                ((AgeDefined)(int)postInfo.Age).GetDescription() :
                                AgeDefined.Y_Plus.GetDescription();

                        item.Height = Enum.IsDefined(typeof(BodyHeightDefined), (int)postInfo.Height) ?
                                ((BodyHeightDefined)(int)postInfo.Height).GetDescription() :
                                BodyHeightDefined.H_Plus.GetDescription();

                        item.Cup = Enum.IsDefined(typeof(CupDefined), (int)postInfo.Cup) ?
                                ((CupDefined)(int)postInfo.Cup).GetDescription() :
                                CupDefined.Plus.GetDescription();


                        item.LowPrice = postInfo.LowPrice;
                        item.Views = postInfo.ViewBaseCount.HasValue ? postInfo.ViewBaseCount.Value + int.Parse(postInfo.Views.ToString()) : int.Parse(postInfo.Views.ToString());
                        item.UnlockCount = postInfo.UnlockBaseCount.HasValue ? postInfo.UnlockBaseCount.Value + postInfo.UnlockCount : postInfo.UnlockCount;
                        item.ServiceItem = MappingServiceItem(postInfo.PostId, allServiceSettings, options)
                                        .Take(3)
                                        .ToArray();
                    }
                    result.SetCode(ReturnCode.Success);
                    result.DataModel = myFavoriteResult;
                }

                return result;
            }, param);
        }

        #region 收藏之帖子操作

        /// <summary>
        /// 删除已经下架了的帖子的收藏记录
        /// </summary>
        /// <param name="favoriteData"></param>
        private async void DeleteFavoritePost(IEnumerable<MMPostFavorite> favoriteData, int postType)
        {
            if (!favoriteData.Any())
                return;

            var applyIdsArray = favoriteData.Select(c => c.PostId).ToArray();
            if (!applyIdsArray.Any())
                return;

            var postData = await _userInfoRepo.GetFavoritePost(applyIdsArray, postType);
            if (!postData.Any())
                return;

            var postDataPostId = postData.Where(c => (int)c.Status != 1).Select(c => c.PostId).ToArray();
            if (!postDataPostId.Any())
                return;

            foreach (var item in favoriteData)
            {
                if (postDataPostId.Contains(item.PostId))
                    await _userInfoRepo.DeleteFavorite(item);
            }
        }

        #endregion 收藏之帖子操作

        #region 收藏之店铺操作

        private async Task<List<MyFavoriteOfficialShop>> GetMyFavoriteOfficialShopData(string[] applyIds)
        {
            var data = await _userInfoRepo.GetFavoriteBoss(applyIds);
            if (!data.Any())
                return null;

            return _mapper.Map<List<MyFavoriteOfficialShop>>(data);
        }

        /// <summary>
        /// 删除已经下架了的店铺的收藏记录
        /// </summary>
        /// <param name="favoriteData"></param>
        private async void DeleteFavoriteShop(IEnumerable<MMPostFavorite> favoriteData, IEnumerable<MyFavoriteOfficialShop> myBossData)
        {
            var applyIds = myBossData.Select(c => c.ApplyId).ToArray();

            var applyDta = await _userInfoRepo.GetFavoriteApply(applyIds);
            if (!applyDta.Any())
                return;

            var userIds = favoriteData.Select(c => c.UserId);
            var userData = await _userInfoRepo.GetUserInfos(userIds);


            //筛选出身份不是觅老板并且状态不是审核通过
            var applyStatusShowArray = applyDta.Where(c => c.Status != 1 && c.ApplyIdentity != 3).Select(c => c.ApplyId).ToArray();
  
            //筛选出身份不是觅老板的用户ID集合
            var notBossUserIds = userData.Where(c => c.UserIdentity != 3).Select(c => c.UserId);

            //筛选出店铺名称为空的ApplayId数组
            var bossApplayIds = myBossData.Where(c => c.ShopName == "").Select(c => c.ApplyId);
            if(bossApplayIds.Any())
            {
                applyStatusShowArray.ToList().AddRange(bossApplayIds);
            }

            foreach (var item in favoriteData)
            {
                if (applyStatusShowArray.Contains(item.PostId) || notBossUserIds.Contains(item.UserId))
                    await _userInfoRepo.DeleteFavorite(item);
            }
        }

        #endregion 收藏之店铺操作

        /// <summary>
        /// 获取我的消息
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public Task<BaseReturnDataModel<PageResultModel<MyMessageList>>> MyMessage(MyMessageQueryParamForClient param)
        {
            return TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<MyMessageList>>();

                var configMap = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<MMHomeAnnouncement, MyMessageList>()
                       .ForMember(target => target.MessageContent, source => source.MapFrom(c => c.HomeContent))
                       .ForMember(target => target.MessageTitle, source => source.MapFrom(c => c.Title))
                       .ForMember(targer => targer.PublishTime, source => source.MapFrom(c => c.CreateDate))
                       .ForMember(target => target.Id, source => source.MapFrom(c => c.Id.ToString()))
                       .ForMember(target => target.MessageType, source => source.MapFrom(c => MessageType.Announcement));

                    cfg.CreateMap<PageResultModel<MMHomeAnnouncement>, PageResultModel<MyMessageList>>()
                       .ForPath(target => target.Data, source => source.MapFrom(c => c.Data));

                    cfg.CreateMap<MMPostReport, MyMessageList>()
                       .ForMember(target => target.MessageContent, source => source.MapFrom(c => c.Memo))
                       .ForMember(target => target.MessageTitle, source => source.MapFrom(c => c.ReportType))
                       .ForMember(targer => targer.PublishTime, source => source.MapFrom(c => c.ExamineTime))
                       .ForMember(target => target.Id, source => source.MapFrom(c => c.ReportId))
                       .ForMember(target => target.MessageType, source => source.MapFrom(c => MessageType.ComplaintPost));

                    cfg.CreateMap<PageResultModel<MMPostReport>, PageResultModel<MyMessageList>>()
                       .ForPath(target => target.Data, source => source.MapFrom(c => c.Data));
                });

                var mapper = new Mapper(configMap);

                var userMessageList = await _userMessageRepo.GetMessageListByUserId(param.UserId);
              
                if (param.MessageInfoType.Equals(MessageType.Announcement))
                {
                    var resultModel = (await _announcement.GetAnnouncementPageAsync(param));
                    result.DataModel = mapper.Map<PageResultModel<MyMessageList>>(resultModel);
                }
                else{

                    var deleteReortMessageIds = userMessageList.Where(c => c.IsDelete && c.MessageType == MessageType.ComplaintPost).Select(c => c.MessageId).ToArray();

                    var resultModel = await _postRepo.ReportMessage(new ReportMessageParam()
                    {
                        DeleteMessageIds = deleteReortMessageIds,
                        UserId = param.UserId,
                        Page = param.Page,
                        PageNo = param.PageNo,
                        PageSize = param.PageSize
                    }) ;

                    result.DataModel = mapper.Map<PageResultModel<MyMessageList>>(resultModel);

                }

                if (result.DataModel.Data.Any() && userMessageList.Any())
                {
                

                    foreach (var item in result.DataModel.Data)
                    {
                        item.IsRead = userMessageList.Where(c => c.IsRead && c.MessageId.Equals(item.Id)).Any();
                    }
                }

                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<PageResultModel<MyPostList>>> ManagePost(MyPostQueryParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<MyPostList>>();

                if (param.PostType == PostType.Official)
                {
                    result.DataModel = _mapper.Map<PageResultModel<MyPostList>>(await _postRepo.OfficialPostSearch(param));
                }
                else
                {
                    result.DataModel = _mapper.Map<PageResultModel<MyPostList>>(await _postRepo.PostSearch(param));
                }

                foreach (var item in result.DataModel.Data)
                {
                    item.CoverUrl = await _ossService.GetCdnPath(item.CoverUrl);
                }
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }

        /// <summary>
        /// 搜寻官方帖子
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PageResultModel<MyOfficialPostList>>> OfficialBossPostPageAsync(MyOfficialQueryParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<MyOfficialPostList>>();
                result.DataModel = _mapper.Map<PageResultModel<MyOfficialPostList>>(await _postRepo.OfficialBossPostPageAsync(param));

                foreach (var item in result.DataModel.Data)
                {
                    item.CoverUrl = await _ossService.GetCdnPath(item.CoverUrl);
                }
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }

        public async Task<BaseReturnDataModel<UserInfoRes>> GetUserInfo(UserInfoReq req)
        {
            return await TryCatchProcedure(async (param) =>
            {
                BaseReturnDataModel<UserSummaryModel[]> totalIncome = await UserSummaryService.GetUserSummaris(req.UserId);

                if (totalIncome.IsSuccess == false)
                {
                    return new BaseReturnDataModel<UserInfoRes>(totalIncome);
                }

                return new BaseReturnDataModel<UserInfoRes>(ReturnCode.Success)
                {
                    DataModel = new UserInfoRes
                    {
                        TotalIncome = totalIncome.DataModel.Where(e => e.Type == UserSummaryTypeEnum.Income).Sum(e => e.Amount).ToString(GlobalSettings.AmountFormat),
                        UserId = req.UserId,
                    }
                };
            }, req);
        }

        /// <summary>
        /// 設定營業中開關
        /// </summary>
        /// <param name="req">參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<ShopOpenClosed>> ShopOpen(UserInfoReq req)
        {
            return await TryCatchProcedure(async (param) =>
            {
                await _userInfoRepo.ShopOpened(param.UserId);

                var user = await _userInfoRepo.GetUserInfo(param.UserId);

                return new BaseReturnDataModel<ShopOpenClosed>(ReturnCode.Success)
                {
                    DataModel = new ShopOpenClosed()
                    {
                        IsOpen = user?.IsOpen ?? false
                    }
                };
            }, req);
        }

        /// <summary>
        /// 設定營業中開關
        /// </summary>
        /// <param name="req">參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> DeleteOfficialPost(ReqOfficialPostDelete req)
        {
            return await TryCatchProcedure(async (param) =>
            {
                // 實作刪除的部份
                var officialPost = await _postRepo.GetOfficialPostByIds(param.PostIds);

                // 檢查贴子中是不是有非自已的
                if (officialPost.Any(p => p.UserId != param.UserId))
                {
                    return new BaseReturnModel(ReturnCode.IllegalUser);
                }

                if (officialPost.Any(p => p.Status == (byte)ReviewStatus.UnderReview))
                {
                    return new BaseReturnModel(ReturnCode.UnderReviewPostCannotBeDeleted);
                }

                var bookings = await _bookingRepo.GetBookingPost(new BookingFilter()
                {
                    PostUserId = req.UserId,
                    Statuses = new[] {
                        BookingStatus.InService,
                        BookingStatus.RefundInProgress
                    },
                    PostIds = param.PostIds,
                });

                if (bookings.Any())
                {
                    return new BaseReturnModel(ReturnCode.OrdersInProgressCannotBeDeleted);
                }

                var result = await _postRepo.SetOfficialPostIsDeleted(param.PostIds);

                return new BaseReturnModel(result ? ReturnCode.Success : ReturnCode.RunSQLFail);
            }, req);
        }
        /// <summary>
        /// 上架官方帖子
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnModel> SetShelfOfficialPost(ReqSetShelfOfficialPost req)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var officialPost = await _postRepo.GetOfficialPostByIds(param.PostIds);

                // 檢查贴子中是不是有非自已的
                if (officialPost.Any(p => p.UserId != param.UserId))
                {
                    return new BaseReturnModel(ReturnCode.IllegalUser);
                }

                var result = await _postRepo.SetShelfOfficialPost(param.PostIds, param.IsDelete);
                return new BaseReturnModel(result ? ReturnCode.Success : ReturnCode.RunSQLFail);
            }, req);
        }
        /// <summary>
        /// 编辑店铺营业时间
        /// </summary>
        /// <param name="paramter"></param>
        /// <returns></returns>
        public async Task<BaseReturnModel> EditShopDoBusinessTime(EditDoBusinessTimeParamter paramter)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var officialMyBossInfo = await _postRepo.GetMyBossInfo(param.BossId);
                if (officialMyBossInfo == null)
                {
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }

                if (paramter.EditType == 1)
                    officialMyBossInfo.BusinessDate = paramter.Content;
                else
                    officialMyBossInfo.BusinessHour = paramter.Content;

                var result = await _postRepo.UpdateBossInfo(officialMyBossInfo);
                return new BaseReturnModel(result ? ReturnCode.Success : ReturnCode.OperationFailed);
            }, paramter);
        }

        /// <summary>
        /// 設定官方帖子删除状态
        /// </summary>
        /// <param name="req">參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> ModifyDeleteStatusOfficialPost(ReqOfficialPostDelete req)
        {
            return await TryCatchProcedure(async (param) =>
            {
                // 實作刪除的部份
                var officialPost = await _postRepo.GetOfficialPostByIds(param.PostIds);

                // 檢查贴子中是不是有非自已的
                if (officialPost.Any(p => p.UserId != param.UserId))
                {
                    return new BaseReturnModel(ReturnCode.IllegalUser);
                }

                if (officialPost.Any(p => p.Status == (byte)ReviewStatus.UnderReview))
                {
                    return new BaseReturnModel(ReturnCode.UnderReviewPostCannotBeDeleted);
                }

                var bookings = await _bookingRepo.GetBookingPost(new BookingFilter()
                {
                    PostUserId = req.UserId,
                    Statuses = new[] {
                        BookingStatus.InService,
                        BookingStatus.RefundInProgress,
                    },
                    PostIds = param.PostIds,
                });

                if (bookings.Any())
                {
                    return new BaseReturnModel(ReturnCode.OrdersInProgressCannotBeDeleted);
                }

                var result = await _postRepo.SetOfficialPostIsDeletedStatus(param.PostIds, param.IsDelete);

                return new BaseReturnModel(result ? ReturnCode.Success : ReturnCode.RunSQLFail);
            }, req);
        }
    }
}