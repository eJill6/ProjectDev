using MMService.Models.Post;
using MS.Core.Extensions;
using MS.Core.Infrastructures.Providers;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models;
using MS.Core.MM.Models.Booking;
using MS.Core.MM.Models.Booking.Enums;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.Media;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Models.User;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.Bases;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.AdminReport;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Utils;
using System.Reflection;

using MS.Core.MM.Models;

using MS.Core.MM.Infrastructures.Exceptions;
using MMService.Models.My;
using MS.Core.Infrastructure.Redis;
using Amazon.Runtime.Internal.Util;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using NLog.Web.LayoutRenderers;
using Microsoft.Extensions.Options;

namespace MMService.Services
{
    /// <summary>
    /// 跟贴子相關的
    /// </summary>
    public class PostService : MMBaseService, IPostService
    {
        /// <summary>
        /// 會員資源
        /// </summary>
        private readonly IPostRepo _postRepo;

        /// <summary>
        /// 選項設定
        /// </summary>
        private readonly IOptionItemRepo _optionRepo;

        /// <summary>
        /// 解鎖贴資源
        /// </summary>
        private readonly IPostTransactionRepo _postTranRepo;

        /// <summary>
        /// 解鎖贴服務
        /// </summary>
        private readonly IPostTransactionService _postTransactionService;

        /// <summary>
        /// 媒體服務
        /// </summary>
        private readonly IEnumerable<IMediaService> _mediaServices;

        /// <summary>
        /// 零一服務
        /// </summary>
        private readonly IZeroOneApiService _zeroOneService;

        /// <summary>
        /// 貴賓服務
        /// </summary>
        private readonly IVipService _vipService;

        /// <summary>
        /// 宣傳內容資源
        /// </summary>
        private readonly IAdvertisingContentRepo _advertisingRepo;

        /// <summary>
        /// 會員資源
        /// </summary>
        private readonly IUserInfoRepo _userInfoRepo;

        /// <summary>
        /// DateTime
        /// </summary>
        private readonly IDateTimeProvider _dateTimeProvider;

        /// <summary>
        /// 預約
        /// </summary>
        private readonly IBookingRepo _bookingRepo;

        private readonly IIncomeExpenseRepo _incomeExpenseRepo;

        /// <summary>
        /// 会员统计信息
        /// </summary>
        private readonly IUserSummaryRepo _userSummaryRepo;

        /// <summary>
        /// 身份认证相关
        /// </summary>
        private readonly IIdentityApplyRepo _identityApplyRepo;

        private readonly ICityService _cityService;

        /// <summary>
        /// 可接受的狀態
        /// </summary>
        private static readonly ReviewStatus[] _acceptStatus = new ReviewStatus[3]
        {
            ReviewStatus.NotApproved,
            ReviewStatus.Approval,
            ReviewStatus.UnderReview
        };

        /// <summary>
        /// 設定取得贴子圖片
        /// </summary>
        private IMediaService _postMediaImageService => _mediaServices
            .FirstOrDefault(m => m.SourceType == SourceType.Post && m.Type == MediaType.Image);

        /// <summary>
        /// 設定取得贴子影片
        /// </summary>
        private IMediaService _postMediaVideoService => _mediaServices
            .FirstOrDefault(m => m.SourceType == SourceType.Post && m.Type == MediaType.Video);

        /// <summary>
        /// 設定取得評論圖片
        /// </summary>
        private IMediaService _commentMediaImageService => _mediaServices
            .FirstOrDefault(m => m.SourceType == SourceType.Comment && m.Type == MediaType.Image);

        /// <summary>
        /// 搜尋贴子回應服務項目最大數量
        /// </summary>
        private readonly int PostSearchMaxTakeServiceItemCount = 4;

        private readonly IRedisService _cache;

        /// <summary>
        /// 快取用的CacheKey
        /// </summary>
        private readonly string _cacheKey = "MMService:VipPostDayCacheKey";

        /// <summary>
        /// 快取用的Db Index
        /// </summary>
        private readonly int _cacheIndexes = 10;

        /// <summary>
        /// 贴子相關
        /// </summary>
        /// <param name="logger">log</param>
        /// <param name="postRepo">贴相關資源</param>
        /// <param name="optionRepo">選項設定</param>
        /// <param name="postTranRepo">解鎖贴資源</param>
        /// <param name="mediaServices">媒體服務</param>
        /// <param name="zeroOneService">零一服務</param>
        /// <param name="vipService">貴賓服務</param>
        /// <param name="postTransactionService">解鎖贴服務</param>
        /// <param name="advertisingRepo">宣傳內容資源</param>
        /// <param name="dateTimeProvider">DateTime</param>
        /// <param name="userInfoRepo">會員資源</param>
        public PostService(ILogger logger,
            IPostRepo postRepo,
            IOptionItemRepo optionRepo,
            IPostTransactionRepo postTranRepo,
            IEnumerable<IMediaService> mediaServices,
            IZeroOneApiService zeroOneService,
            IVipService vipService,
            IPostTransactionService postTransactionService,
            IAdvertisingContentRepo advertisingRepo,
            IDateTimeProvider dateTimeProvider,
            IUserInfoRepo userInfoRepo,
            IBookingRepo bookingRepo,
            IUserSummaryRepo userSummaryRepo,
            IIncomeExpenseRepo incomeExpenseRepo,
            IIdentityApplyRepo identityApplyRepo,
            IRedisService cache) : base(logger)
        {
            _postRepo = postRepo;
            _optionRepo = optionRepo;
            _postTranRepo = postTranRepo;
            _mediaServices = mediaServices;
            _zeroOneService = zeroOneService;
            _vipService = vipService;
            _postTransactionService = postTransactionService;
            _advertisingRepo = advertisingRepo;
            _userInfoRepo = userInfoRepo;
            _dateTimeProvider = dateTimeProvider;
            _bookingRepo = bookingRepo;
            _userSummaryRepo = userSummaryRepo;
            _incomeExpenseRepo = incomeExpenseRepo;
            _identityApplyRepo = identityApplyRepo;
            _cache = cache;
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

        /// <summary>
        /// 宣傳內容
        /// </summary>
        /// <param name="contentType">宣傳類型</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<WhatIsData>> WhatIs(AdvertisingContentType contentType)
        {
            return await TryCatchProcedure(async (contentType) =>
            {
                if (!Enum.IsDefined(typeof(AdvertisingContentType), contentType))
                {
                    return new BaseReturnDataModel<WhatIsData>(ReturnCode.ParameterIsInvalid);
                }

                var data = await _advertisingRepo.GetByPostType(contentType);

                var resultData = new WhatIsData()
                {
                    What = data?.FirstOrDefault(p => p.AdvertisingType == (int)AdvertisingType.What)?.AdvertisingContent ?? string.Empty,
                    How = data?.FirstOrDefault(p => p.AdvertisingType == (int)AdvertisingType.How)?.AdvertisingContent ?? string.Empty
                };

                var result = new BaseReturnDataModel<WhatIsData>();
                result.DataModel = resultData;
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, contentType);
        }

        /// <summary>
        /// 合成贴子新增或編輯的資訊
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">用戶暱稱</param>
        /// <param name="param">新增/修改參數</param>
        /// <returns></returns>
        private static PostUpsertData ComposePostUpsertData(string? postId, int userId, string nickname, PostData param)
        {
            var baseUnlockAmount = MMGlobalSettings.BaseUnlockAmountSetting
                .FirstOrDefault(p => p.PostType == param.PostType)?.UnlockAmount ?? 0M;

            param.ApplyAmount = param.ApplyAmount ?? baseUnlockAmount;

            return new PostUpsertData
            {
                PostId = postId ?? string.Empty,
                PostType = param.PostType,
                UserId = (int)userId,
                Nickname = nickname,
                MessageId = param.MessageId,
                ApplyAmount = param.ApplyAmount == default(decimal) ? baseUnlockAmount : (decimal)param.ApplyAmount,
                ApplyAdjustPrice = param.ApplyAmount != default(decimal) && param.ApplyAmount != baseUnlockAmount,
                Title = param.Title,
                AreaCode = param.AreaCode,
                Quantity = param.Quantity,
                Age = (byte)param.Age,
                Height = (int)param.Height,
                Cup = (byte)param.Cup,
                BusinessHours = param.BusinessHours,
                LowPrice = param.LowPrice,
                HighPrice = param.HighPrice,
                Address = param.Address,
                ServiceDescribe = param.ServiceDescribe,
                ServiceIds = string.Join(',', param.ServiceIds),
                PhotoIds = string.Join(',', param.PhotoIds),
                VideoIds = param.VideoIds == null ? string.Empty : string.Join(',', param.VideoIds),
                ContactInfos = JsonUtil.ToJsonString(param.ContactInfos)
            };
        }

        /// <summary>
        /// 從01那邊取得用戶資訊
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<ZOUserInfoRes?> GetZeroUserInfo(int userId)
        {
            ZOUserInfoRes? userInfo = null;
            try
            {
                var userInfoResult = await _zeroOneService.GetUserInfo(new ZOUserInfoReq(userId));
                if (userInfoResult != null)
                {
                    userInfo = userInfoResult.DataModel;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}。取得祕色Api的用戶資訊發現異常。");
            }

            return userInfo;
        }

        /// <summary>
        /// 檢查贴子新增/修改參數是否填寫
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="nickname"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static bool CheckPostUpsertDataIsFillIn(int? userId, string? nickname, PostData param)
        {
            param.ContactInfos = param.ContactInfos
                .Where(p => !string.IsNullOrWhiteSpace(p.Contact))?
                .ToArray() ?? Array.Empty<ContactInfo>();

            return userId != null &&
                string.IsNullOrWhiteSpace(nickname) == false &&
                string.IsNullOrWhiteSpace(param.Title) == false &&
                string.IsNullOrWhiteSpace(param.AreaCode) == false &&
                string.IsNullOrWhiteSpace(param.Quantity) == false &&
                string.IsNullOrWhiteSpace(param.BusinessHours) == false &&
                string.IsNullOrWhiteSpace(param.Address) == false &&
                string.IsNullOrWhiteSpace(param.ServiceDescribe) == false &&
                (param.PostType != PostType.Square || (param.PostType == PostType.Square && param.MessageId != 0)) &&
                param.ServiceIds.Any() &&
                param.ContactInfos.Any() &&
                param.PhotoIds.Any();
        }

        /// <summary>
        /// 檢查贴子參數是否符合值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static bool CheckPostParameterIsValid(PostData param)
        {
            return param.LowPrice <= param.HighPrice &&
                param.LowPrice >= 100 &&
                param.HighPrice <= 99999 &&
                param.Title.Length <= 20 &&
                param.Quantity.Length <= 10 &&
                param.BusinessHours.Length <= 20 &&
                param.Address.Length <= 20 &&
                param.ContactInfos.Any(p => p.Contact?.Length > 50) == false &&
                param.ServiceDescribe.Length <= 100 &&
                param.PhotoIds.Length <= 5 &&
                Enum.IsDefined(typeof(PostType), param.PostType) &&
                Enum.IsDefined(typeof(AgeDefined), param.Age) &&
                Enum.IsDefined(typeof(BodyHeightDefined), param.Height) &&
                Enum.IsDefined(typeof(CupDefined), param.Cup);
        }

        /// <summary>
        /// 新增贴子
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">發贴人當下暱稱</param>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> AddPost(int? userId, string? nickname, PostData model)
        {
            return await base.TryCatchProcedure<(int? UserId, string Nickname, PostData Model), BaseReturnModel>(async (param) =>
            {
                var writeUserId = param.UserId;
                var writeNickname = param.Nickname;
                if (param.Model?.UserId != null)
                {
                    var zeroUserInfo = await GetZeroUserInfo((int)(param.Model?.UserId));

                    writeUserId = param.Model?.UserId;
                    writeNickname = zeroUserInfo?.NickName ?? string.Empty;

                    // 如果再抓不到，就從db取歷史資料
                    if (string.IsNullOrWhiteSpace(writeNickname))
                    {
                        var userInfo = await _userInfoRepo.GetUserInfo((int)writeUserId);
                        if (userInfo != null)
                        {
                            writeNickname = userInfo.Nickname ?? string.Empty;
                        }
                    }
                }

                if (!CheckPostUpsertDataIsFillIn(writeUserId, writeNickname, param.Model))
                {
                    return new BaseReturnModel(ReturnCode.MissingNecessaryParameter);
                }

                if (!CheckPostParameterIsValid(param.Model))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                UserInfoData user = await _vipService.GetUserInfoData(writeUserId.Value);

                //会员每日第一次发帖
                bool IsVipPostDay = false;
                if (user.IsVip && user.UserInfo.UserIdentity != (int)IdentityType.Boss && user.UserInfo.UserIdentity != (int)IdentityType.Agent)
                {
                    int postDailyCount = await _postRepo.GetPostDailyCount(writeUserId.Value);
                    IsVipPostDay = postDailyCount > 0 ? false : true;
                }

                PostUpsertData postDto = ComposePostUpsertData(null, (int)writeUserId, writeNickname, param.Model);
                postDto.IsVipPostDay = IsVipPostDay;
                var upsertResult = await _postRepo.PostUpsert(postDto);

                //发帖完成后如果是首次发帖则清除缓存
                if (IsVipPostDay && upsertResult.IsSuccess)
                {
                    await _cache.RemoveCache(_cacheIndexes, string.Format($"{_cacheKey}:{(int)writeUserId}"));
                }

                return new BaseReturnModel(upsertResult.GetReturnCode());
            }, (UserId: userId, Nickname: nickname, Model: model));
        }

        /// <summary>
        /// 取得編輯贴子的資訊
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PostEditData>> GetPostEditData(string postId, int? userId)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var apost = await _postRepo.GetById(param.PostId);
                if (apost == null)
                {
                    return new BaseReturnDataModel<PostEditData>(ReturnCode.DataIsNotExist);
                }

                if (apost?.UserId != userId)
                {
                    return new BaseReturnDataModel<PostEditData>(ReturnCode.NonMatched);
                }

                //取得媒體資源
                var medias = (await _postMediaImageService.Get(SourceType.Post, new string[] { param.PostId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                var videos = (await _postMediaVideoService.Get(SourceType.Post, new string[] { param.PostId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                //取得跟贴相關的服務項目
                var serviceSettings = await _postRepo.GetPostMappingService(new string[] { param.PostId });

                //取得贴相關的聯絡方式
                var contacts = await _postRepo.GetPostContact(new string[] { param.PostId });

                var resultData = new PostEditData()
                {
                    PostType = (PostType)apost.PostType,
                    MessageId = apost.MessageId,
                    ApplyAmount = apost.ApplyAmount,
                    Title = apost.Title,
                    AreaCode = apost.AreaCode,
                    Quantity = apost.Quantity,
                    Age = apost.Age,
                    Height = apost.Height,
                    Cup = apost.Cup,
                    BusinessHours = apost.BusinessHours,
                    LowPrice = apost.LowPrice,
                    HighPrice = apost.HighPrice,
                    Address = apost.Address,
                    ServiceDescribe = apost.ServiceDescribe,
                    ServiceIds = serviceSettings?.Select(p => p.ServiceId).ToArray() ?? Array.Empty<int>(),
                    PhotoSource = medias?
                        .Where(p => p.MediaType == (int)MediaType.Image)?
                        .ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>(),
                    VideoSource = videos?
                        .Where(p => p.MediaType == (int)MediaType.Video)?
                        .ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>(),
                    ContactInfos = Enum.GetValues(typeof(ContactType))
                        .Cast<ContactType>()
                        .Select(p => new ContactInfo
                        {
                            ContactType = p,
                            Contact = contacts?.FirstOrDefault(x => x.ContactType == (byte)p)?.Contact ?? string.Empty
                        }).ToArray() ?? new ContactInfo[0],
                };

                var result = new BaseReturnDataModel<PostEditData>();
                result.DataModel = resultData;
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, (PostId: postId, UserId: userId));
        }

        /// <summary>
        /// 編輯贴子
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">發贴人當下暱稱</param>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> EditPost(string postId, int? userId, string? nickname, PostData model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var writeUserId = param.UserId;
                var writeNickname = param.Nickname;
                if (param.Model?.UserId != null)
                {
                    var zeroUserInfo = await GetZeroUserInfo((int)(param.Model?.UserId));
                    writeUserId = param.Model?.UserId;
                    writeNickname = zeroUserInfo?.NickName ?? string.Empty;

                    // 如果再抓不到，就從db取歷史資料
                    if (string.IsNullOrWhiteSpace(writeNickname))
                    {
                        var userInfo = await _userInfoRepo.GetUserInfo((int)writeUserId);
                        if (userInfo != null)
                        {
                            writeNickname = userInfo.Nickname ?? string.Empty;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(param.PostId) ||
                    !CheckPostUpsertDataIsFillIn(writeUserId, writeNickname, param.Model))
                {
                    return new BaseReturnModel(ReturnCode.MissingNecessaryParameter);
                }

                if (!CheckPostParameterIsValid(param.Model))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                var apost = await _postRepo.GetById(param.PostId);
                if (apost == null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotExist);
                }

                // 非發贴本人則阻檔修改
                if (apost.UserId != writeUserId)
                {
                    return new BaseReturnModel(ReturnCode.IllegalUser);
                }

                var medias = (await _postMediaImageService.Get(SourceType.Post, param.PostId))?.DataModel;

                //取得跟贴相關的服務項目
                var allServiceSettings = await _postRepo.GetPostMappingService(new string[] { param.PostId });

                //取得所有設定
                var options = await _optionRepo.GetPostTypeOptions(param.Model.PostType);

                //組資料
                var resultData = new PostList()
                {
                    PostId = apost.PostId,
                    PostType = apost.PostType,
                    AreaCode = apost.AreaCode,
                    CoverUrl = apost.CoverUrl,
                    Title = apost.Title,
                    Height = Enum.IsDefined(typeof(BodyHeightDefined), apost.Height) ?
                        ((BodyHeightDefined)apost.Height).GetDescription() :
                        BodyHeightDefined.H_Plus.GetDescription(),
                    Age = Enum.IsDefined(typeof(AgeDefined), (int)apost.Age) ?
                        ((AgeDefined)(int)apost.Age).GetDescription() :
                        AgeDefined.Y_Plus.GetDescription(),
                    Cup = Enum.IsDefined(typeof(CupDefined), (int)apost.Cup) ?
                        ((CupDefined)(int)apost.Cup).GetDescription() :
                        CupDefined.Plus.GetDescription(),
                    Job = TryGetOptionValue(apost.MessageId, options),
                    LowPrice = Math.Truncate(apost.LowPrice).ToString(),
                    Favorites = apost.Favorites.ToString(),
                    Comments = apost.Comments.ToString(),
                    Views = apost.Views.ToString(),
                    UpdateTime = (apost.ExamineTime ?? default(DateTime)).ToMMUpdateString(),
                    ServiceItem = MappingServiceItem(apost.PostId, allServiceSettings, options),
                    IsFeatured = apost.IsFeatured ?? false
                };

                PostUpsertData postDto = ComposePostUpsertData(param.PostId, (int)writeUserId, writeNickname, param.Model);
                postDto.OldViewData = JsonUtil.ToJsonString(resultData);

                var upsertResult = await _postRepo.PostUpsert(postDto);

                // 更新成功則刪除雲倉
                if (upsertResult.IsSuccess && medias?.Any() == true)
                {
                    await DeletePostOldMediaFile(param.Model.PhotoIds,
                        param.Model.VideoIds ?? Array.Empty<string>(),
                        medias);
                }

                return new BaseReturnModel(upsertResult.GetReturnCode());
            }, (PostId: postId, UserId: userId, Nickname: nickname, Model: model));
        }

        /// <summary>
        /// 檢舉
        /// </summary>
        /// <param name="complainantUserId">投訴人 UserId</param>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> Report(int? complainantUserId, ReportData model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                param.Model.PhotoIds = param.Model.PhotoIds
                    .Where(p => !string.IsNullOrWhiteSpace(p))?
                    .ToArray() ?? Array.Empty<string>();

                if (param.ComplainantUserId == null ||
                    string.IsNullOrWhiteSpace(param.Model.PostId) ||
                    string.IsNullOrWhiteSpace(param.Model.Describe) ||
                    param.Model.PhotoIds.Any() == false)
                {
                    return new BaseReturnModel(ReturnCode.MissingNecessaryParameter);
                }

                if (Enum.IsDefined(typeof(ReportType), param.Model.ReportType) == false ||
                    param.Model.PhotoIds.Length > 9 ||
                    param.Model.Describe.Length > 150)
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                var reportDto = new ReportCreate
                {
                    ReportType = (byte)param.Model.ReportType,
                    ComplainantUserId = (int)param.ComplainantUserId,
                    PostId = param.Model.PostId,
                    Describe = param.Model.Describe,
                    PhotoIds = string.Join(',', param.Model.PhotoIds)
                };

                var insertResult = await _postRepo.InsertReport(reportDto);

                return new BaseReturnModel(insertResult.GetReturnCode());
            }, (ComplainantUserId: complainantUserId, Model: model));
        }

        /// <summary>
        /// 組成評論資料
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">評論人當下暱稱</param>
        /// <param name="param">輸入參數</param>
        /// <returns></returns>
        private async Task<CommentUpsertData> ComposeCommentUpsertData(string? commentId, int? userId, string? nickname, CommentData param)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var userInfo = await _zeroOneService.GetUserInfo(new ZOUserInfoReq((int)userId));

            var commentDto = new CommentUpsertData()
            {
                CommentId = commentId ?? string.Empty,
                PostId = param.PostId,
                AvatarUrl = userInfo?.DataModel?.Avatar ?? string.Empty,
                UserId = (int)userId,
                Nickname = userInfo?.DataModel?.NickName ?? nickname ?? string.Empty,
                AreaCode = param.AreaCode,
                Status = (int)ReviewStatus.UnderReview,
                Comment = param.Comment,
                SpentTime = param.SpentTime,
                PhotoIds = string.Join(',', param.PhotoIds ?? Array.Empty<string>()),
            };
            return commentDto;
        }

        /// <summary>
        /// 檢查評論用的參數
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">暱稱</param>
        /// <param name="param">用戶參數</param>
        /// <returns></returns>
        private static bool CheckCommentParameterIsFillIn(int? userId, string? nickname, CommentData param)
        {
            param.PhotoIds = param.PhotoIds ?? Array.Empty<string>();

            return userId != null &&
                string.IsNullOrWhiteSpace(nickname) == false &&
                string.IsNullOrWhiteSpace(param.PostId) == false &&
                string.IsNullOrWhiteSpace(param.Comment) == false &&
                string.IsNullOrWhiteSpace(param.AreaCode) == false &&
                param.SpentTime != default(DateTime);
        }

        private static bool CheckCommentParameterIsValid(int? userId, string? nickname, CommentData param)
        {
            return param.PhotoIds?.Length <= 6 &&
                param.Comment.Length <= 100;
        }

        /// <summary>
        /// 評論
        /// </summary>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">評論人當下暱稱</param>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> AddComment(int? userId, string? nickname, CommentData model)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                if (!CheckCommentParameterIsFillIn(param.UserId, param.Nickname, param.Model))
                {
                    return new BaseReturnModel(ReturnCode.MissingNecessaryParameter);
                }

                if (!CheckCommentParameterIsValid(param.UserId, param.Nickname, param.Model))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                CommentUpsertData commentDto = await ComposeCommentUpsertData(null, param.UserId, param.Nickname, param.Model);

                var upsertResult = await _postRepo.CommentUpsert(commentDto);

                return new BaseReturnModel(upsertResult.GetReturnCode());
            }, (UserId: userId, Nickname: nickname, Model: model));
        }

        /// <summary>
        /// 取得編輯評論資料
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <param name="userId">用戶id</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<CommentEditData>> GetCommentEditData(string commentId, int? userId)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var aComment = await _postRepo.GetPostCommentByCommentId(param.CommentId);
                if (aComment == null)
                {
                    return new BaseReturnDataModel<CommentEditData>(ReturnCode.DataIsNotExist);
                }

                if (aComment.UserId != userId)
                {
                    return new BaseReturnDataModel<CommentEditData>(ReturnCode.IllegalUser);
                }

                //取得媒體資源
                var medias = (await _commentMediaImageService.Get(SourceType.Comment, new string[] { aComment.CommentId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                var resultData = new CommentEditData()
                {
                    PostId = aComment.PostId,
                    SpentTime = aComment.SpentTime?.ToString("yyyy-MM-dd") ?? string.Empty,
                    Comment = aComment.Comment,
                    AreaCode = aComment.AreaCode,
                    PhotoSource = medias?
                        .Where(p => p.MediaType == (int)MediaType.Image)?
                        .ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>()
                };

                var result = new BaseReturnDataModel<CommentEditData>();
                result.DataModel = resultData;
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, (CommentId: commentId, UserId: userId));
        }

        /// <summary>
        /// 編輯評論
        /// </summary>
        /// <param name="commentId">評論id</param>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">評論人當下暱稱</param>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> EditComment(string commentId, int? userId, string? nickname, CommentData model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                if (string.IsNullOrWhiteSpace(param.CommentId) ||
                    !CheckCommentParameterIsFillIn(param.UserId, param.Nickname, param.Model))
                {
                    return new BaseReturnModel(ReturnCode.MissingNecessaryParameter);
                }

                if (!CheckCommentParameterIsValid(param.UserId, param.Nickname, param.Model))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                var aComment = await _postRepo.GetPostCommentByCommentId(param.CommentId);
                if (aComment == null)
                {
                    return new BaseReturnDataModel<CommentEditData>(ReturnCode.DataIsNotExist);
                }

                // 比對不到該評在數據庫與傳入參數 postId
                if (aComment.PostId != param.Model.PostId)
                {
                    return new BaseReturnDataModel<CommentEditData>(ReturnCode.NonMatched);
                }

                // 非評論本人
                if (aComment.UserId != param.UserId)
                {
                    return new BaseReturnDataModel<CommentEditData>(ReturnCode.IllegalUser);
                }

                var medias = (await _commentMediaImageService.Get(SourceType.Comment, param.CommentId))?.DataModel;

                CommentUpsertData commentDto = await ComposeCommentUpsertData(param.CommentId, param.UserId, param.Nickname, param.Model);

                var upsertResult = await _postRepo.CommentUpsert(commentDto);

                if (upsertResult.IsSuccess && medias?.Any() == true)
                {
                    var deleteImageId = medias?
                        .Where(p => p.MediaType == (int)MediaType.Image)
                        .Select(p => p.Id)?
                        .ToArray()
                        .Except(param.Model.PhotoIds ?? Array.Empty<string>()) ?? Array.Empty<string>();

                    foreach (var delMediaId in deleteImageId)
                    {
                        //刪除非絕對重要，因此若失敗可以忽略，不影響原流程
                        try
                        {
                            await _commentMediaImageService.DeleteToOss(delMediaId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}。刪除圖檔、視頻發生異常。Media table Id = {delMediaId}");
                        }
                    }
                }

                return new BaseReturnModel(upsertResult.GetReturnCode());
            }, (CommentId: commentId, UserId: userId, Nickname: nickname, Model: model));
        }

        /// <summary>
        /// 評論清單
        /// </summary>
        /// <param name="postId">贴子 id</param>
        /// <param name="pageNo">目前頁數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PageResultModel<CommentList>>> CommentList(string postId, int pageNo)
        {
            Func<MMPostComment, Task<CommentList>> CommentDataFunc = async
                aComment =>
                {
                    var matchPhotos = (await _commentMediaImageService
                        .Get(SourceType.Comment, new string[] { aComment.CommentId }))?
                        .DataModel?
                        .Where(p => p.MediaType == (int)MediaType.Image)?
                        .ToArray() ?? Array.Empty<MediaInfo>();

                    string[] commentPhotos = matchPhotos?
                        .OrderBy(p => p.CreateDate)
                        .Select(p => p.FullMediaUrl)
                        .ToArray() ?? Array.Empty<string>();

                    return new CommentList()
                    {
                        AvatarUrl = aComment.AvatarUrl,
                        Nickname = aComment.Nickname,
                        SpentTime = aComment.SpentTime?.ToString("yyyy年MM月dd日") ?? string.Empty,
                        AreaCode = aComment.AreaCode,
                        Comment = aComment.Comment,
                        PublishTime = aComment.ExamineTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty,
                        PhotoUrls = commentPhotos
                    };
                };

            return await TryCatchProcedure(async (param) =>
            {
                var data = await _postRepo.GetPostComment(param.PostId, new PaginationModel
                {
                    PageNo = param.PageNo,
                    PageSize = GlobalSettings.PageSize
                });

                CommentList[] commentResultData = new CommentList[0];
                if (data?.Data?.Any() == true)
                {
                    //組成回傳資料
                    commentResultData = await Task.WhenAll(data.Data.Select(p => CommentDataFunc(p))) ?? Array.Empty<CommentList>();
                }

                var resultData = new PageResultModel<CommentList>()
                {
                    PageNo = param.PageNo,              //以傳進來的page進行返回
                    PageSize = data?.PageSize ?? GlobalSettings.PageSize,
                    TotalPage = data?.TotalPage ?? 0,
                    Data = commentResultData
                };

                var result = new BaseReturnDataModel<PageResultModel<CommentList>>();
                result.DataModel = resultData;
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, (PostId: postId, PageNo: pageNo));
        }

        /// <summary>
        /// 贴的清單
        /// </summary>
        /// <param name="userId">用戶Id</param>
        /// <param name="model">搜尋參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PostListViewModel>> PostSearch(int? userId, PostSearchParam model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                int queryPage = param.Model.PageNo ?? param.Model.Page ?? 1;
                queryPage = queryPage <= 0 ? 1 : queryPage;

                if (param.Model.PostType.HasValue && Enum.IsDefined(typeof(PostType), param.Model.PostType) == false)
                {
                    param.Model.PostType = null;
                }

                var pageSize = GlobalSettings.PageSize;
                if (param.Model.PageSize.HasValue)
                {
                    if (param.Model.PageSize.Value <= 0)
                    {
                        return new BaseReturnDataModel<PostListViewModel>(ReturnCode.ParameterIsInvalid);
                    }
                    pageSize = param.Model.PageSize.Value > GlobalSettings.MaxPageSize ?
                        GlobalSettings.MaxPageSize :
                        param.Model.PageSize.Value;
                }

                var queryTs = string.IsNullOrWhiteSpace(param.Model.Ts) ? DateTime.Now : Convert.ToDouble(param.Model.Ts).ConvertToDateTime(UnixOfTimeTypes.TotalSeconds);

                (MMPost[] postData, int rowCount) = await _postRepo.PostSearch(
                    param.Model.PostType,
                    param.UserId,
                    param.Model.IsRecommend,
                    param.Model.SortType,
                    param.Model.MessageId,
                    param.Model.LockStatus,
                    param.Model.AreaCode == "00" ? null : param.Model.AreaCode,
                    param.Model.ServiceIds,
                    param.Model.Age,
                    param.Model.Height,
                    param.Model.Cup,
                    param.Model.Price,
                    queryTs,
                    queryPage,
                    pageSize);

                PostList[] resultData = new PostList[0];
                if (postData.Any())
                {
                    var postIds = postData.Select(x => x.PostId).ToArray();

                    var favorites = await _postRepo.GetMMPostFavorite(userId, postIds);

                    //取得跟贴相關的服務項目
                    var allServiceSettings = (await _postRepo.GetPostMappingService(postIds)) ?? Array.Empty<MMPostServiceMapping>();

                    //取得所有設定
                    var options = await _optionRepo.GetPostTypeOptions(param.Model.PostType);

                    //組資料
                    var resultTask = postData.Select(async (p) =>
                    {
                        return new PostList()
                        {
                            PostId = p.PostId,
                            PostType = p.PostType,
                            CoverUrl = await _postMediaImageService.GetFullMediaUrl(
                                new MMMedia()
                                {
                                    FileUrl = p.CoverUrl
                                },
                                postType: p.PostType,
                                isThumbnail: true),
                            Title = p.Title,
                            Height = Enum.IsDefined(typeof(BodyHeightDefined), p.Height) ?
                                ((BodyHeightDefined)p.Height).GetDescription() :
                                BodyHeightDefined.H_Plus.GetDescription(),
                            Age = Enum.IsDefined(typeof(AgeDefined), (int)p.Age) ?
                                ((AgeDefined)(int)p.Age).GetDescription() :
                                AgeDefined.Y_Plus.GetDescription(),
                            Cup = Enum.IsDefined(typeof(CupDefined), (int)p.Cup) ?
                                ((CupDefined)(int)p.Cup).GetDescription() :
                                CupDefined.Plus.GetDescription(),
                            Job = TryGetOptionValue(p.MessageId, options),
                            AreaCode = p.AreaCode,
                            LowPrice = Math.Truncate(p.LowPrice).ToString(),
                            Favorites = p.Favorites.ToString(),
                            Comments = p.Comments.ToString(),
                            Views = (p.Views + p.ViewBaseCount ?? 0).ToString(),
                            UpdateTime = (p.ExamineTime ?? default(DateTime)).ToMMUpdateString(),
                            ServiceItem = MappingServiceItem(p.PostId, allServiceSettings, options)
                                .Take(this.PostSearchMaxTakeServiceItemCount)
                                .ToArray(),
                            IsFeatured = p.IsFeatured ?? false,
                            Unlocks = p.UnlockCount + p.UnlockBaseCount ?? 0,
                            IsFavorite = favorites.Any(x => x.PostId == p.PostId && x.Type == 1)
                        };
                    });

                    resultData = await Task.WhenAll(resultTask) ?? new PostList[0];
                }

                var result = new BaseReturnDataModel<PostListViewModel>();
                result.DataModel = new PostListViewModel()
                {
                    PageNo = queryPage,
                    PageSize = pageSize,
                    TotalPage = (int)Math.Ceiling((decimal)rowCount / pageSize),
                    Data = resultData,
                    TotalCount = rowCount,
                    Ts = queryTs.ToUnixOfTime(UnixOfTimeTypes.TotalSeconds).ToString()
                };
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, (UserId: userId, Model: model));
        }

        /// <summary>
        /// 首页帖子推荐
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<List<PostList>>> RecommendPostList(int? userId)
        {
            return await TryCatchProcedure(async () =>
            {
                var result = new BaseReturnDataModel<List<PostList>>();
                var postWeightList = await _postRepo.GetMMPostWeight();

                if (postWeightList.Any())
                {
                    var agencyPostIds =
                    postWeightList.Where(x => x.PostType == (byte)PostType.Agency && x.Status == 1)?
                    .OrderByDescending(x => x.Weight)
                    .Take(5)
                    .Select(x => x.PostId) ?? new List<string> { };

                    var squarePostIds =
                    postWeightList.Where(x => x.PostType == (byte)PostType.Square && x.Status == 1)?
                    .OrderByDescending(x => x.Weight)
                    .Take(10)
                    .Select(x => x.PostId) ?? new List<string> { };

                    var postIds = agencyPostIds.Union(squarePostIds).ToArray();
                    var postData = await _postRepo.GetPostInfoById(postIds);

                    //取得跟贴相關的服務項目
                    var allServiceSettings = (await _postRepo.GetPostMappingService(postIds)) ?? Array.Empty<MMPostServiceMapping>();

                    var favorites = await _postRepo.GetMMPostFavorite(userId, postIds);

                    //取得所有設定
                    var options = await _optionRepo.GetOptionsByPostTypes(new List<int> { (int)PostType.Square, (int)PostType.Agency });

                    //組資料
                    var resultTask = postData.Select(async (p) =>
                    {
                        var optionDic = options.Where(x => (PostType)x.PostType == p.PostType).ToDictionary(x => x.OptionId, y => y.OptionContent);
                        return new PostList()
                        {
                            PostId = p.PostId,
                            PostType = p.PostType,
                            CoverUrl = await _postMediaImageService.GetFullMediaUrl(
                                new MMMedia()
                                {
                                    FileUrl = p.CoverUrl
                                },
                                postType: p.PostType,
                                isThumbnail: true),
                            Title = p.Title,
                            Height = Enum.IsDefined(typeof(BodyHeightDefined), p.Height) ?
                                ((BodyHeightDefined)p.Height).GetDescription() :
                                BodyHeightDefined.H_Plus.GetDescription(),
                            Age = Enum.IsDefined(typeof(AgeDefined), (int)p.Age) ?
                                ((AgeDefined)(int)p.Age).GetDescription() :
                                AgeDefined.Y_Plus.GetDescription(),
                            Cup = Enum.IsDefined(typeof(CupDefined), (int)p.Cup) ?
                                ((CupDefined)(int)p.Cup).GetDescription() :
                                CupDefined.Plus.GetDescription(),
                            Job = TryGetOptionValue(p.MessageId, optionDic),
                            AreaCode = p.AreaCode,
                            LowPrice = Math.Truncate(p.LowPrice).ToString(),
                            Favorites = p.Favorites.ToString(),
                            Comments = p.Comments.ToString(),
                            Views = (p.Views + p.ViewBaseCount ?? 0).ToString(),
                            UpdateTime = (p.ExamineTime ?? default(DateTime)).ToMMUpdateString(),
                            ServiceItem = MappingServiceItem(p.PostId, allServiceSettings, optionDic)
                                .Take(this.PostSearchMaxTakeServiceItemCount)
                                .ToArray(),
                            IsFeatured = p.IsFeatured ?? false,
                            Unlocks = p.UnlockCount + p.UnlockBaseCount ?? 0,
                            IsFavorite = favorites.Any(x => x.PostId == p.PostId && x.Type == 1)
                        };
                    });

                    result.DataModel = (await Task.WhenAll(resultTask)).ToList() ?? new List<PostList> { };
                }
                else
                {
                    result.DataModel = default;
                }
                result.SetCode(ReturnCode.Success);
                return result;
            });
        }

        /// <summary>
        /// 覓贴詳情
        /// </summary>
        /// <param name="userId">用戶 Id</param>
        /// <param name="postId">贴子 Id</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PostDetail>> PostDetail(int? userId, string postId)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var apost = await _postRepo.GetById(param.PostId);
                if (apost == null || param.UserId == null)
                {
                    return new BaseReturnDataModel<PostDetail>(ReturnCode.DataIsNotCompleted);
                }

                if (apost.Status != ReviewStatus.Approval)
                {
                    return new BaseReturnDataModel<PostDetail>(ReturnCode.DataIsNotExist);
                }

                int userId = param.UserId.Value;

                //取得媒體資源
                var medias = (await _postMediaImageService.Get(SourceType.Post, new string[] { param.PostId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                var videos = (await _postMediaVideoService.Get(SourceType.Post, new string[] { param.PostId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                //取得跟贴相關的服務項目
                var serviceSettings = await _postRepo.GetPostMappingService(new string[] { param.PostId });

                //取得所有設定
                var options = await _optionRepo.GetPostTypeOptions(apost.PostType);

                //取得用戶購買的贴子
                var postTran = await _postTranRepo.GetUserPostTran(userId, param.PostId);
                var isUnlock = postTran != null;

                //取得贴相關的聯絡方式
                var contacts = await _postRepo.GetPostContact(new string[] { param.PostId });

                UserSummaryInfoData user = await _vipService.GetUserSummaryInfoData(userId);

                var cardType = await GetPostCardType(apost.UserId);

                //是否申請調價
                apost.UnlockAmount = apost.ApplyAdjustPrice ? apost.ApplyAmount : apost.UnlockAmount;

                //取得用戶購買狀態，設定預設值，如果未設定福利則以原價呈現
                decimal discount = apost.UnlockAmount * user.Discount(apost.PostType);

                // 免費解鎖次數
                int freeUnlockCount = user.PostFreeUnlockCount(apost.PostType);

                // 即時取得01那邊用戶資訊
                var zeroUserInfo = await GetZeroUserInfo((int)apost.UserId);

                // 用戶註冊時間
                var userRegTime = zeroUserInfo?.CreateTime != default(DateTime) ?
                    zeroUserInfo?.CreateTime.ToString("yyyy/MM/dd") :
                    string.Empty;

                // Review 次數 + 1
                try
                {
                    await _postRepo.UpdatePostViewsCount(param.PostId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}。增加贴子觀看次數失敗。PostId：{param.PostId}");
                }

                var commentData = await _postRepo.GetPostCommentByPostIdAndUserId(param.PostId, userId);

                var commentStatus = CommentStatus.NotYetComment;

                if (isUnlock == false)
                {
                    commentStatus = CommentStatus.PostLock;
                }
                else if (commentData == null)
                {
                    commentStatus = CommentStatus.NotYetComment;
                }
                else
                {
                    commentStatus = (CommentStatus)commentData.Status;
                }

                var postUserInfo = await _userInfoRepo.GetUserInfo((int)apost.UserId);

                var advertisingData = await _advertisingRepo.GetByPostType((AdvertisingContentType)(int)apost.PostType);

                var findAdvertisingType = (int)AdvertisingType.MustSee;
                if (apost.PostType == PostType.Agency)
                {
                    findAdvertisingType = (int)AdvertisingType.MainTip;
                }

                var favorites = await _postRepo.GetMMPostFavorite(postId);
                var isFavorite = false;
                if (favorites.Any())
                    isFavorite = favorites.Any(x => x.UserId == userId && x.Type == 1);

                var mustSeeText = advertisingData
                    .FirstOrDefault(p => p.AdvertisingType == findAdvertisingType)?
                    .AdvertisingContent ?? string.Empty;

                var marqueeText = advertisingData
                    .FirstOrDefault(p => p.AdvertisingType == (int)AdvertisingType.Marquee)?
                    .AdvertisingContent ?? string.Empty;

                var reports = await _postRepo.GetUserReported((int)param.UserId, param.PostId, null);

                var resultData = new PostDetail
                {
                    PostId = param.PostId,
                    PostType = apost.PostType,
                    UserIdentity = (IdentityType)(postUserInfo?.UserIdentity ?? (int)IdentityType.General),
                    EarnestMoney = Math.Truncate(postUserInfo?.EarnestMoney ?? 0M).ToString(),
                    Title = apost.Title,
                    AreaCode = apost.AreaCode,
                    Job = TryGetOptionValue(apost.MessageId, options),
                    Nickname = zeroUserInfo?.NickName ?? apost.Nickname ?? string.Empty,
                    CardType = cardType,
                    AvatarUrl = zeroUserInfo?.Avatar ?? string.Empty,
                    PostUserId= zeroUserInfo?.UserId.ToString(),
                    RegisterTime = userRegTime ?? string.Empty,
                    Favorites = apost.Favorites.ToString(),
                    Comments = apost.Comments.ToString(),
                    Views = (apost.Views + apost.ViewBaseCount ?? 0).ToString(),
                    UnlockAmount = apost.UnlockAmount,
                    Discount = discount,
                    FreeUnlockCount = freeUnlockCount,
                    Unlocks = apost.UnlockCount + apost.UnlockBaseCount ?? 0,
                    PostCommentStatus = commentStatus,
                    HasFreeUnlockAuth = user.HasFreeUnlockAuth(apost.PostType),
                    CommentId = commentStatus == CommentStatus.NotApproved ? commentData?.CommentId ?? string.Empty : string.Empty, //只有評論過且未通過才提供
                    CommentMemo = commentStatus == CommentStatus.NotApproved ? commentData?.Memo ?? string.Empty : string.Empty,    //只有評論過且未通過才提供,
                    IsUnlock = isUnlock,
                    UnlockInfo = new UserUnlockGetInfo()
                    {
                        ContactInfos = isUnlock ? Enum.GetValues(typeof(ContactType))
                            .Cast<ContactType>()
                            .Select(p => new ContactInfo
                            {
                                ContactType = p,
                                Contact = contacts?.FirstOrDefault(x => x.ContactType == (byte)p)?.Contact ?? string.Empty
                            }).ToArray() ?? new ContactInfo[0] : new ContactInfo[0],
                        Address = isUnlock ? apost.Address : string.Empty,
                    },
                    Height = Enum.IsDefined(typeof(BodyHeightDefined), apost.Height) ?
                        ((BodyHeightDefined)apost.Height).GetDescription() :
                        BodyHeightDefined.H_Plus.GetDescription(),
                    Age = Enum.IsDefined(typeof(AgeDefined), (int)apost.Age) ?
                        ((AgeDefined)(int)apost.Age).GetDescription() :
                        AgeDefined.Y_Plus.GetDescription(),
                    Cup = Enum.IsDefined(typeof(CupDefined), (int)apost.Cup) ?
                        ((CupDefined)(int)apost.Cup).GetDescription() :
                        CupDefined.Plus.GetDescription(),
                    Quantity = apost.Quantity,
                    LowPrice = Math.Truncate(apost.LowPrice).ToString(),
                    HighPrice = Math.Truncate(apost.HighPrice).ToString(),
                    BusinessHours = apost.BusinessHours,
                    ServiceDescribe = apost.ServiceDescribe ?? string.Empty,
                    UpdateTime = (apost.ExamineTime ?? default(DateTime)).ToMMUpdateString(),
                    ServiceItem = MappingServiceItem(param.PostId, serviceSettings, options),
                    PhotoUrls = medias?
                        .Where(p => p.MediaType == (int)MediaType.Image)?
                        .Select(p => p.FullMediaUrl)?
                        .ToArray() ?? Array.Empty<string>(),
                    VideoUrl = videos?
                        .Where(p => p.MediaType == (int)MediaType.Video && string.Equals(Path.GetExtension(p.FileUrl).ToLower(), ".m3u8"))?
                        .Select(p => p.FullMediaUrl)?
                        .FirstOrDefault() ?? string.Empty,
                    HasReported = reports.Any(),
                    ReportedCount = reports.Count(),
                    CanReported = postTran?.CreateTime.AddHours(72) > _dateTimeProvider.Now,
                    IsFeatured = apost.IsFeatured ?? false,
                    MustSee = mustSeeText,
                    Marquee = marqueeText,
                    IsFavorite = isFavorite
                };

                var result = new BaseReturnDataModel<PostDetail>();
                result.DataModel = resultData;
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, (UserId: userId, PostId: postId));
        }

        /// <summary>
        /// 取得發贴人 會員狀態
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<int[]> GetPostCardType(int userId)
        {
            var vipType = (int?)(await _vipService.GetUserInfoData(userId)).CurrentVip?.VipType;

            return vipType?.ToEnumerable().ToArray() ?? new int[0];
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<AdminPostDetail>> PostAdminDetail(string postId)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var apost = await _postRepo.GetById(param);
                if (apost == null)
                {
                    return new BaseReturnDataModel<AdminPostDetail>(ReturnCode.DataIsNotCompleted);
                }

                var resultData = JsonUtil.CastByJson<AdminPostDetail>(apost);

                //取得媒體資源
                var medias = (await _postMediaImageService.Get(SourceType.Post, new string[] { param }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                var videos = (await _postMediaVideoService.Get(SourceType.Post, new string[] { param }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                //取得跟贴相關的服務項目
                var serviceSettings = await _postRepo.GetPostMappingService(new string[] { param });

                //取得所有設定
                var options = await _optionRepo.GetPostTypeOptions((PostType)apost.PostType);

                //取得贴相關的聯絡方式
                var contacts = await _postRepo.GetPostContact(new string[] { param });

                var userId = apost.UserId;

                //VIP 會員
                var vip = _vipService.GetUserInfoData(userId).Result.CurrentVip;

                //VIP 會員
                //ResUserEfficientVip? vip =
                //    await _vipService.GetUserEfficientVips(userId)
                //    .GetReturnDataAsync()
                //    .AsEnumerableAsync()
                //    .OrderByDescendingAsync(e => e.EffectiveTime)
                //    .FirstOrDefaultAsync();
                resultData.IsHomePost = false;
                //获取当前贴子权重信息
                var postWeight = await _postRepo.GetMMPostWeight().WhereAsync(a => a.PostId == postId).FirstOrDefaultAsync();
                if (postWeight != null)
                {
                    resultData.Weight = postWeight.Weight;
                    resultData.IsHomePost = true;
                }
                //找尋發贴人的 UserId
                ZOUserInfoRes userInfo = new ZOUserInfoRes();
                var userRegTime = string.Empty;
                //忽略如果打第三方的錯誤
                try
                {
                    var userInfoResult = await _zeroOneService.GetUserInfo(new ZOUserInfoReq(apost.UserId));
                    if (userInfoResult != null)
                    {
                        userInfo = userInfoResult.DataModel;

                        //用戶註冊時間
                        userRegTime = userInfo?.CreateTime != default(DateTime) ?
                            userInfo?.CreateTime.ToString("yyyy/MM/dd") :
                            string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}。取得祕色Api的用戶資訊發現異常。");
                }
                resultData.LowPrice = apost.LowPrice.ToString("F0");
                resultData.HighPrice = apost.HighPrice.ToString("F0");
                resultData.MessageType = TryGetOptionValue(apost.MessageId, options);
                resultData.Nickname = userInfo?.NickName ?? apost.Nickname ?? string.Empty;
                resultData.CardName = vip?.TypeName ?? "-";
                resultData.CardCreateTime = "-";
                resultData.CardEffectiveTime = "-";
                resultData.CoverUrl = userInfo?.Avatar ?? string.Empty;
                resultData.RegisterTime = userRegTime ?? string.Empty;
                resultData.UnlockInfo = new UserUnlockGetInfoForClient()
                {
                    ContactInfos = Enum.GetValues(typeof(ContactType))
                        .Cast<ContactType>()
                        .Select(p => new ContactInfoForClient
                        {
                            ContactType = p,
                            Contact = contacts?.FirstOrDefault(x => x.ContactType == (byte)p)?.Contact ?? string.Empty
                        }).ToArray() ?? new ContactInfoForClient[0],
                    Address = apost.Address,
                };
                resultData.ServiceDescribe = apost.ServiceDescribe ?? string.Empty;
                resultData.ServiceItem = MappingServiceItem(param, serviceSettings, options);
                resultData.PhotoUrls = medias?
                        .Where(p => p.MediaType == (int)MediaType.Image)?
                        .Select(p => p.FullMediaUrl)?
                        .ToArray() ?? Array.Empty<string>();
                resultData.VideoUrl = videos?
                        .Where(p => p.MediaType == (int)MediaType.Video)?
                        .Select(p => p.FullMediaUrl)?
                        .FirstOrDefault() ?? string.Empty;
                resultData.IsFeatured = apost?.IsFeatured ?? false;
                resultData.PhotoSource = medias?
                       .Where(p => p.MediaType == (int)MediaType.Image)?
                       .ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>();
                resultData.VideoSource = videos?
                        .Where(p => p.MediaType == (int)MediaType.Video)?
                        .ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>();

                resultData.UnlockBaseCount = apost.UnlockBaseCount;
                resultData.ViewBaseCount = apost.ViewBaseCount;
                var result = new BaseReturnDataModel<AdminPostDetail>();
                result.DataModel = resultData;
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, (postId));
        }

        /// <summary>
        /// 取得贴子服務項目的文字
        /// </summary>
        /// <param name="postId">贴子 Id</param>
        /// <returns></returns>
        public async Task<string[]> GetServiceItemsText(string postId)
        {
            //取得跟贴相關的服務項目
            var serviceSettings = await _postRepo.GetPostMappingService(new string[] { postId });

            //取得所有設定
            var options = await _optionRepo.GetPostTypeOptions(null);

            return MappingServiceItem(postId, serviceSettings, options);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<PageResultModel<AdminPostList>>> PostSearch(AdminPostListParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<AdminPostList>>();
                var posts = await _postRepo.PostSearch(param);
                result.DataModel = JsonUtil.CastByJson<PageResultModel<AdminPostList>>(posts);
                foreach (var item in result.DataModel.Data)
                {
                    item.IsFeatured = item.IsFeatured ?? false;
                }
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<PageResultModel<AdminOfficialPostList>>> OfficialAdminPostSearch(AdminPostListParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<AdminOfficialPostList>>();
                var posts = await _postRepo.OfficialAdminPostSearch(param);
                result.DataModel = JsonUtil.CastByJson<PageResultModel<AdminOfficialPostList>>(posts);

                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> PostEdit(AdminPostData param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                if (!_acceptStatus.Contains(param.PostStatus))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
                var queryResult = await _postRepo.PostEdit(param);

                return new BaseReturnModel(queryResult.GetReturnCode());
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> PostEditAllData(AdminPostData param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                if (!_acceptStatus.Contains(param.PostStatus))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
                var queryResult = await _postRepo.PostEditAllData(param);
                var medias = (await _postMediaImageService.Get(SourceType.Post, param.PostId))?.DataModel;

                // 更新成功則刪除雲倉
                if (queryResult.IsSuccess && medias?.Any() == true)
                {
                    await DeletePostOldMediaFile(param.PhotoIds.Split(',').ToArray(),
                        param.VideoIds.Split(',').ToArray() ?? Array.Empty<string>(),
                        medias);
                }
                return new BaseReturnModel(queryResult.GetReturnCode());
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> PostBatchEdit(AdminPostBatchData param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var postIds = param.PostIds.Split(",").ToArray();
                var queryResult = await _postRepo.PostBatchEdit(param);

                return new BaseReturnModel(queryResult.GetReturnCode());
            }, param);
        }

        /// <summary>
        /// 解鎖贴子
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<UnlockPostResModel>> UnlockPost(UnlockPostReqModel req)
        {
            BaseReturnDataModel<ResPostTransaction> baseReturn = await _postTransactionService.PostTransaction(new ReqPostTransaction
            {
                PostId = req.PostId,
                UserId = req.UserId,
            });

            if (baseReturn.IsSuccess == false)
            {
                return new BaseReturnDataModel<UnlockPostResModel>(baseReturn);
            }

            return new BaseReturnDataModel<UnlockPostResModel>(baseReturn)
            {
                DataModel = new UnlockPostResModel
                {
                    UnlockInfo = baseReturn.DataModel.UnlockInfo,
                }
            };
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<PageResultModel<AdminCommentList>>> CommentSearch(AdminCommentListParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<AdminCommentList>>();
                var posts = await _postRepo.CommentSearch(param);
                result.DataModel = JsonUtil.CastByJson<PageResultModel<AdminCommentList>>(posts);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<PageResultModel<AdminOfficialCommentList>>> OfficialCommentSearch(AdminCommentListParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<AdminOfficialCommentList>>();
                var posts = await _postRepo.OfficialCommentSearch(param);
                result.DataModel = JsonUtil.CastByJson<PageResultModel<AdminOfficialCommentList>>(posts);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<AdminCommentDetail>> CommentDetail(string commentId)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<AdminCommentDetail>();
                var service = _commentMediaImageService;
                if (service != null)
                {
                    var comment = await _postRepo.CommentDetail(param);
                    if (comment != null)
                    {
                        result.DataModel = JsonUtil.CastByJson<AdminCommentDetail>(comment);
                        //取得媒體資源
                        var medias = (await service.Get(SourceType.Comment, new string[] { param }))?
                        .DataModel?
                        .OrderBy(p => p.CreateDate);
                        result.DataModel.PhotoIds = medias?.Select(p => p.FullMediaUrl)?.ToArray() ?? new string[0];
                        result.SetCode(ReturnCode.Success);
                        return result;
                    }
                    result.SetCode(ReturnCode.DataIsNotExist);
                    return result;
                }
                result.SetCode(ReturnCode.SystemError);
                return result;
            }, commentId);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<AdminOfficialCommentDetail>> OfficialCommentDetail(string commentId)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<AdminOfficialCommentDetail>();
                var service = _commentMediaImageService;
                if (service != null)
                {
                    var comment = await _postRepo.OfficialCommentDetail(param);
                    if (comment != null)
                    {
                        result.DataModel = JsonUtil.CastByJson<AdminOfficialCommentDetail>(comment);
                        result.SetCode(ReturnCode.Success);
                        return result;
                    }
                    result.SetCode(ReturnCode.DataIsNotExist);
                    return result;
                }
                result.SetCode(ReturnCode.SystemError);
                return result;
            }, commentId);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> CommentEdit(AdminCommentData param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                if (!_acceptStatus.Contains(param.Status))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                var comment = await _postRepo.CommentDetail(param.Id);

                var queryResult = await _postRepo.CommentEdit(param);

                try
                {
                    if (string.IsNullOrWhiteSpace(comment?.PostId) == false)
                    {
                        if ((int)param.Status == (int)CommentStatus.Approval)
                        {
                            await _postRepo.UpdatePostCommentsCount(comment.PostId, true);
                            await _userSummaryRepo.IncrementUserQuantity(new IncrementUserSummaryModel
                            {
                                UserId = comment.UserId,
                                Amount = 1,
                                Category = (UserSummaryCategoryEnum)comment.PostType,
                                Type = UserSummaryTypeEnum.Comment
                            });
                        }
                        else if (comment?.Status == (int)CommentStatus.Approval && (int)param.Status != (int)CommentStatus.Approval)
                        {
                            await _postRepo.UpdatePostCommentsCount(comment.PostId, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}。增加贴子評論次數失敗。CommentId：{param.Id}");
                }

                return new BaseReturnModel(queryResult.GetReturnCode());
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> OfficialCommentEdit(AdminCommentData param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                if (!_acceptStatus.Contains(param.Status))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                var comment = await _postRepo.OfficialCommentDetail(param.Id);

                var queryResult = await _postRepo.OfficialCommentEdit(param);

                if (queryResult.IsSuccess == false)
                {
                    return new BaseReturnModel(queryResult.GetReturnCode());
                }

                try
                {
                    if (string.IsNullOrWhiteSpace(comment?.PostId) == false)
                    {
                        if ((int)param.Status == (int)CommentStatus.Approval)
                        {
                            await _postRepo.UpdateOfficialPostCommentsCount(comment.PostId, comment.FacialScore, comment.ServiceQuality, true);
                        }
                        else if (comment?.Status == (int)CommentStatus.Approval && (int)param.Status != (int)CommentStatus.Approval)
                        {
                            await _postRepo.UpdateOfficialPostCommentsCount(comment.PostId, comment.FacialScore, comment.ServiceQuality, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}。增加官方贴子評論次數失敗。CommentId：{param.Id}");
                }

                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<PageResultModel<AdminReportList>>> ReportSearch(AdminReportListParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<AdminReportList>>();
                var posts = await _postRepo.ReportSearch(param);
                result.DataModel = JsonUtil.CastByJson<PageResultModel<AdminReportList>>(posts);
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<AdminReportDetail>> ReportDetail(string reportId)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<AdminReportDetail>();
                var service = _mediaServices.FirstOrDefault(x => x.SourceType == SourceType.Report && x.Type == MediaType.Image);
                if (service != null)
                {
                    var report = await _postRepo.ReportDetail(param);
                    if (report != null)
                    {
                        result.DataModel = JsonUtil.CastByJson<AdminReportDetail>(report);
                        //取得媒體資源
                        var medias = (await service.Get(SourceType.Report, new string[] { param }))?
                            .DataModel?
                            .OrderBy(p => p.CreateDate);
                        result.DataModel.PhotoIds = medias?.Select(p => p.FullMediaUrl)?.ToArray() ?? new string[0];
                        result.SetCode(ReturnCode.Success);
                        return result;
                    }
                    result.SetCode(ReturnCode.DataIsNotExist);
                    return result;
                }
                result.SetCode(ReturnCode.SystemError);
                return result;
            }, reportId);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> ReportEdit(AdminReportData param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                if (!_acceptStatus.Contains(param.Status))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
                var queryResult = await _postRepo.ReportEdit(param);
                if (queryResult.IsSuccess && param.Status == ReviewStatus.Approval)
                {
                    var report = await _postRepo.ReportDetail(param.Id);
                    if (report != null)
                    {
                        var expense = await _incomeExpenseRepo.GetByReport(new AdminIncomeExpenseByReport()
                        {
                            SoruceId = report.PostTranId,
                            Category = (IncomeExpenseCategoryEnum)report.PostType,
                            TransactionType = IncomeExpenseTransactionTypeEnum.Expense
                        });
                        var income = await _incomeExpenseRepo.GetByReport(new AdminIncomeExpenseByReport()
                        {
                            SoruceId = report.PostTranId,
                            Category = (IncomeExpenseCategoryEnum)report.PostType,
                            TransactionType = IncomeExpenseTransactionTypeEnum.Income
                        });
                        if (expense != null && income != null)
                        {
                            income.Status = IncomeExpenseStatusEnum.ReportUnDispatched;
                            await _incomeExpenseRepo.UpdateStatus(income);
                            await _zeroOneService.PointIncome(new ZOPointIncomeExpenseReq(ZOIncomeExpenseCategory.UnlockRefund, expense.UserId, expense.Amount * expense.Rebate, expense.Id));
                        }
                    }
                }
                return new BaseReturnModel(queryResult.GetReturnCode());
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> OfficialReportEdit(AdminReportData param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                if (!_acceptStatus.Contains(param.Status))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
                var queryResult = await _postRepo.ReportEdit(param);
                return new BaseReturnModel(queryResult.GetReturnCode());
            }, param);
        }

        /// <summary>
        /// 贴子收藏新增/刪除
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="postId">贴子id</param>
        /// <param name="type">类型。1：帖子、2：店铺</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> PostFavoriteUpsert(int? userId, string postId, int type)
        {
            return await TryCatchProcedure(async (param) =>
            {
                if (userId == null || string.IsNullOrWhiteSpace(postId))
                {
                    return new BaseReturnModel(ReturnCode.MissingNecessaryParameter);
                }

                var result = await _postRepo.PostFavoriteUpsert((int)param.UserId, param.PostId, type);
                return new BaseReturnModel(result.GetReturnCode());
            }, (UserId: userId, PostId: postId));
        }

        public async Task<BaseReturnDataModel<IEnumerable<PostWeightResult>>> GetMMPostWeight()
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<PostWeightResult>>();
                result.DataModel = await _postRepo.GetMMPostWeight();
                result.SetCode(ReturnCode.Success);
                return result;
            }, String.Empty);
        }

        public async Task<BaseReturnDataModel<IEnumerable<GoldStoreResult>>> GetMMGoldStore()
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<GoldStoreResult>>();
                result.DataModel = await _postRepo.GetMMGoldStore();
                result.SetCode(ReturnCode.Success);
                return result;
            }, String.Empty);
        }

        public async Task<BaseReturnDataModel<IEnumerable<MMPostFavorite>>> GetMMPostFavorite(string postId)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<MMPostFavorite>>();
                result.DataModel = await _postRepo.GetMMPostFavorite(postId);
                result.SetCode(ReturnCode.Success);
                return result;
            }, String.Empty);
        }

        public async Task<BaseReturnModel> InsertMMPostWeight(MMPostWeight param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var insertResult = await _postRepo.InsertMMPostWeight(param);
                BaseReturnModel result = new BaseReturnModel();
                result.SetCode(new ReturnCode(insertResult.Code));
                return result;
            }, param);
        }

        public async Task<BaseReturnModel> DeleteMMPostWeight(int id)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var repo = await _postRepo.DeleteMMPostWeight(id);
                BaseReturnModel result = new BaseReturnModel();
                result.SetCode(new ReturnCode(repo.Code));
                return result;
            }, id);
        }

        public async Task<BaseReturnModel> UpdateMMPostWeight(UpdateMMPostParam param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var repo = await _postRepo.UpdateMMPostWeight(param);
                BaseReturnModel result = new BaseReturnModel();
                result.SetCode(new ReturnCode(repo.Code));
                return result;
            }, param);
        }

        public async Task<BaseReturnModel> UpdateMMGoldStore(List<UpdateMMGoldStoreParam> param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var repo = await _postRepo.UpdateMMGoldStore(param);
                BaseReturnModel result = new BaseReturnModel();
                result.SetCode(new ReturnCode(repo.Code));
                return result;
            }, param);
        }

        /// <summary>
        /// 後台權重表批量刪除
        /// </summary>
        /// <param name="post">修改後的Model</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> PostWeightBatchRemove(AdminPostBatchData param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var queryResult = await _postRepo.PostWeightBatchRemove(param);
                return new BaseReturnModel(queryResult.GetReturnCode());
            }, param);
        }

        /// <summary>
        /// 檢查官方贴子新增/修改參數是否填寫
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="nickname"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static bool CheckOfficialPostUpsertDataIsFillIn(int? userId, string? nickname, ReqOfficialPostData param)
        {
            // 套餐-價格 最低價格不可低於100, 限正整數字，5位數內
            param.Combo = param.Combo
                .Where(p => p.ComboPrice >= 100 &&
                    p.ComboPrice < 100000 &&
                    !string.IsNullOrWhiteSpace(p.ComboName) &&
                    !string.IsNullOrWhiteSpace(p.Service))?
                .ToArray() ?? Array.Empty<ComboData>();

            return userId != null &&
                string.IsNullOrWhiteSpace(nickname) == false &&
                string.IsNullOrWhiteSpace(param.Title) == false &&
                string.IsNullOrWhiteSpace(param.AreaCode) == false &&
                string.IsNullOrWhiteSpace(param.BusinessHours) == false &&
                string.IsNullOrWhiteSpace(param.ServiceDescribe) == false &&
                param.ServiceIds.Any() &&
                param.PhotoIds.Any() &&
                param.Combo.Any();
        }

        /// <summary>
        /// 檢查贴子參數是否符合值
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private static bool CheckOfficialPostParameterIsValid(ReqOfficialPostData param)
        {
            // 與pm確認過，套餐內僅判斷有無填充，若超過則截斷
            return
                param.Title.Length <= 20 &&
                param.BusinessHours.Length <= 20 &&
                param.Address.Length <= 20 &&
                param.ServiceDescribe.Length <= 100 &&
                param.PhotoIds.Length <= 5 &&
                Enum.IsDefined(typeof(AgeDefined), param.Age) &&
                Enum.IsDefined(typeof(BodyHeightDefined), param.Height) &&
                Enum.IsDefined(typeof(CupDefined), param.Cup);
        }

        /// <summary>
        /// 合成贴子新增或編輯的資訊
        /// </summary>
        /// <param name="postId">贴子id</param>
        /// <param name="userId">用戶id</param>
        /// <param name="nickname">用戶暱稱</param>
        /// <param name="param">新增/修改參數</param>
        /// <returns></returns>
        private static OfficialPostUpsertData ComposeOfficialPostUpsertData(string? postId,
            int userId, string nickname, ReqOfficialPostData param)
        {
            var lowPrice = 0;
            var highPrice = 0;
            if (param.Combo.Any())
            {
                lowPrice = (int)param.Combo.Min(p => p.ComboPrice);
                highPrice = (int)param.Combo.Max(p => p.ComboPrice);
            }

            return new OfficialPostUpsertData
            {
                PostId = postId ?? string.Empty,
                UserId = (int)userId,
                Nickname = nickname,
                Title = param.Title,
                AreaCode = param.AreaCode,
                Age = (byte)param.Age,
                Height = (short)param.Height,
                Cup = (byte)param.Cup,
                BusinessHours = param.BusinessHours,
                LowPrice = lowPrice,
                HighPrice = highPrice,
                Address = param.Address,
                ServiceDescribe = param.ServiceDescribe,
                ServiceIds = string.Join(',', param.ServiceIds),
                PhotoIds = string.Join(',', param.PhotoIds),
                VideoIds = param.VideoIds == null ? string.Empty : string.Join(',', param.VideoIds),
                Combo = JsonUtil.ToJsonString(param.Combo)
            };
        }

        /// <summary>
        /// 新增官方贴子
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> AddOfficialPost(ReqOfficialPostData model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var writeUserId = param.UserId;
                var writeNickname = param.Nickname;
                if (param.UserId != null)
                {
                    var zeroUserInfo = await GetZeroUserInfo((int)(param.UserId));

                    writeUserId = param.UserId;
                    writeNickname = zeroUserInfo?.NickName ?? string.Empty;

                    // 如果再抓不到，就從db取歷史資料
                    if (string.IsNullOrWhiteSpace(writeNickname))
                    {
                        var userInfo = await _userInfoRepo.GetUserInfo((int)writeUserId);
                        if (userInfo != null)
                        {
                            writeNickname = userInfo.Nickname ?? string.Empty;
                        }
                    }
                }

                if (!CheckOfficialPostUpsertDataIsFillIn(writeUserId, writeNickname, param))
                {
                    return new BaseReturnModel(ReturnCode.MissingNecessaryParameter);
                }

                if (!CheckOfficialPostParameterIsValid(param))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                OfficialPostUpsertData postDto = ComposeOfficialPostUpsertData(null, (int)writeUserId, writeNickname, param);

                var upsertResult = await _postRepo.OfficialPostUpsert(postDto);

                return new BaseReturnModel(upsertResult.GetReturnCode());
            }, model);
        }

        /// <summary>
        /// 取得編輯官方贴子的資訊
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OfficialPostEditData>> GetOfficialPostEditData(ReqPostIdUserId model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var apost = await _postRepo.GetOfficialPostById(param.PostId);
                if (apost == null)
                {
                    return new BaseReturnDataModel<OfficialPostEditData>(ReturnCode.DataIsNotExist);
                }

                if (apost?.UserId != param.UserId)
                {
                    return new BaseReturnDataModel<OfficialPostEditData>(ReturnCode.NonMatched);
                }

                //取得媒體資源
                var medias = (await _postMediaImageService.Get(SourceType.Post, new string[] { param.PostId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                var videos = (await _postMediaVideoService.Get(SourceType.Post, new string[] { param.PostId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                //取得跟贴相關的服務項目
                var serviceSettings = await _postRepo.GetPostMappingService(new string[] { param.PostId });

                //取得官方贴套餐組合
                var officialPrice = await _postRepo.GetOfficialPostPrice(new string[] { param.PostId });

                var resultData = new OfficialPostEditData()
                {
                    Title = apost.Title,
                    AreaCode = apost.AreaCode,
                    Age = apost.Age,
                    Height = apost.Height,
                    Cup = apost.Cup,
                    BusinessHours = apost.BusinessHours,
                    Address = apost.Address,
                    ServiceDescribe = apost.ServiceDescribe,
                    ServiceIds = serviceSettings?.Select(p => p.ServiceId).ToArray() ?? Array.Empty<int>(),
                    PhotoSource = medias?
                        .Where(p => p.MediaType == (int)MediaType.Image)?
                        .ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>(),
                    VideoSource = videos?
                        .Where(p => p.MediaType == (int)MediaType.Video)?
                        .ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>(),
                    Combo = officialPrice?.Select(p => new ComboData()
                    {
                        ComboName = p.ComboName,
                        ComboPrice = p.ComboPrice,
                        Service = p.Service
                    }).ToArray() ?? Array.Empty<ComboData>(),
                };

                var result = new BaseReturnDataModel<OfficialPostEditData>();
                result.DataModel = resultData;
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, model);
        }

        /// <summary>
        /// 編輯官方贴子
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> EditOfficialPost(ReqOfficialPostData model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var writeUserId = param.UserId;
                var writeNickname = param.Nickname;
                if (param.UserId != null)
                {
                    var zeroUserInfo = await GetZeroUserInfo((int)(param.UserId));
                    writeUserId = param.UserId;
                    writeNickname = zeroUserInfo?.NickName ?? string.Empty;

                    // 如果再抓不到，就從db取歷史資料
                    if (string.IsNullOrWhiteSpace(writeNickname))
                    {
                        var userInfo = await _userInfoRepo.GetUserInfo((int)writeUserId);
                        if (userInfo != null)
                        {
                            writeNickname = userInfo.Nickname ?? string.Empty;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(param.PostId) ||
                    !CheckOfficialPostUpsertDataIsFillIn(writeUserId, writeNickname, param))
                {
                    return new BaseReturnModel(ReturnCode.MissingNecessaryParameter);
                }

                if (!CheckOfficialPostParameterIsValid(param))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                var apost = await _postRepo.GetOfficialPostById(param.PostId);
                if (apost == null || apost.IsDelete)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotExist);
                }

                // 非發贴本人則阻檔修改
                if (apost.UserId != writeUserId)
                {
                    return new BaseReturnModel(ReturnCode.IllegalUser);
                }

                var medias = (await _postMediaImageService.Get(SourceType.Post, param.PostId))?.DataModel;

                //取得跟贴相關的服務項目
                var allServiceSettings = await _postRepo.GetPostMappingService(new string[] { param.PostId });

                //取得所有設定
                var options = await _optionRepo.GetPostTypeOptions(PostType.Official);

                //取得官方贴套餐組合
                var officialPrice = await _postRepo.GetOfficialPostPrice(new string[] { param.PostId });

                //組資料
                var resultData = new OfficialPostList()
                {
                    PostId = apost.PostId,
                    PostType = PostType.Official,
                    AreaCode = apost.AreaCode,
                    CoverUrl = apost.CoverUrl,
                    Title = apost.Title,
                    Height = Enum.IsDefined(typeof(BodyHeightDefined), (int)apost.Height) ?
                        ((BodyHeightDefined)apost.Height).GetDescription() :
                        BodyHeightDefined.H_Plus.GetDescription(),
                    Age = Enum.IsDefined(typeof(AgeDefined), (int)apost.Age) ?
                        ((AgeDefined)(int)apost.Age).GetDescription() :
                        AgeDefined.Y_Plus.GetDescription(),
                    Cup = Enum.IsDefined(typeof(CupDefined), (int)apost.Cup) ?
                        ((CupDefined)(int)apost.Cup).GetDescription() :
                        CupDefined.Plus.GetDescription(),
                    LowPrice = Math.Truncate(apost.LowPrice).ToString(),
                    FacialScore = apost.FacialScore?.ToString() ?? "0",
                    UpdateTime = (apost.ExamineTime ?? default(DateTime)).ToMMUpdateString()
                };

                OfficialPostUpsertData postDto = ComposeOfficialPostUpsertData(param.PostId, (int)writeUserId, writeNickname, param);
                postDto.OldViewData = JsonUtil.ToJsonString(resultData);

                var upsertResult = await _postRepo.OfficialPostUpsert(postDto);

                // 更新成功則刪除雲倉
                if (upsertResult.IsSuccess && medias?.Any() == true)
                {
                    await DeletePostOldMediaFile(param.PhotoIds,
                        param.VideoIds ?? Array.Empty<string>(),
                        medias);
                }

                return new BaseReturnModel(upsertResult.GetReturnCode());
            }, model);
        }

        /// <summary>
        /// 編輯官方贴子(Admin)
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> AdminEditOfficialPost(AdminOfficialPostData model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var writeUserId = param.UserId;
                var writeNickname = param.ExamineMan;
                if (param.UserId != null)
                {
                    var zeroUserInfo = await GetZeroUserInfo((int)(param.UserId));
                    writeUserId = param.UserId;
                    writeNickname = zeroUserInfo?.NickName ?? string.Empty;

                    // 如果再抓不到，就從db取歷史資料
                    if (string.IsNullOrWhiteSpace(writeNickname))
                    {
                        var userInfo = await _userInfoRepo.GetUserInfo((int)writeUserId);
                        if (userInfo != null)
                        {
                            writeNickname = userInfo.Nickname ?? string.Empty;
                        }
                    }
                }

                var apost = await _postRepo.GetOfficialPostById(param.PostId);
                if (apost == null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotExist);
                }

                var upsertResult = await _postRepo.AdminOfficialPostUpdate(model);
                return new BaseReturnModel(upsertResult.GetReturnCode());
            }, model);
        }

        /// <summary>
        /// 編輯官方贴编辑锁定状态(Admin)
        /// </summary>
        /// <param name="postId">輸入參數</param>
        /// <param name="status">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OfficialPostEditLock>> AdminOfficialPostEditLock(string postId, bool status)
        {
            return await TryCatchProcedure(async (param) =>
            {
                await _postRepo.AdminOfficialPostEditLock(param, status);

                var postInfo = await _postRepo.GetOfficialPostById(param);

                return new BaseReturnDataModel<OfficialPostEditLock>(ReturnCode.Success)
                {
                    DataModel = new OfficialPostEditLock()
                    {
                        LockStatus = postInfo?.LockStatus ?? false
                    }
                };
            }, postId);
        }

        /// <summary>
        /// 刪除舊贴子的上傳圖片及視頻
        /// </summary>
        /// <param name="photoIds">照片id</param>
        /// <param name="videoIds">視頻id</param>
        /// <param name="medias">媒體資訊</param>
        /// <returns></returns>
        private async Task DeletePostOldMediaFile(string[] photoIds, string[] videoIds, MediaInfo[]? medias)
        {
            var deleteImageId = medias?
                .Where(p => p.MediaType == (int)MediaType.Image)
                .Select(p => p.Id)?
                .ToArray()
                .Except(photoIds) ?? Array.Empty<string>();

            var deleteVideoId = medias?
                 .Where(p => p.MediaType == (int)MediaType.Video)
                 .Select(p => p.Id)?
                 .ToArray()
                 .Except(videoIds) ?? Array.Empty<string>();

            foreach (var delMediaId in deleteImageId.Union(deleteVideoId))
            {
                //刪除非絕對重要，因此若失敗可以忽略，不影響原流程
                try
                {
                    await _postMediaImageService.DeleteToOss(delMediaId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}。刪除圖檔、視頻發生異常。Media table Id = {delMediaId}");
                }
            }
        }

        /// <summary>
        /// 根据发帖人取得官方贴
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<MMOfficialPost[]> GetOfficialPostByUserId(int userId)
        {
            return await _postRepo.GetOfficialPostByUserId(userId);
        }

        /// <summary>
        /// 官方贴的清單
        /// </summary>
        /// <param name="model">搜尋參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OfficialPostListViewModel>> OfficialPostSearch(ReqOfficialPostSearchParam model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                if (param.UserIdentity != null &&
                    param.UserIdentity != IdentityType.Boss &&
                    param.UserIdentity != IdentityType.Girl)
                {
                    param.UserIdentity = null;
                }

                int queryPage = param.PageNo ?? 1;
                queryPage = queryPage <= 0 ? 1 : queryPage;

                var pageSize = GlobalSettings.PageSize;
                if (param.PageSize.HasValue)
                {
                    if (param.PageSize.Value <= 0)
                    {
                        return new BaseReturnDataModel<OfficialPostListViewModel>(ReturnCode.ParameterIsInvalid);
                    }

                    pageSize = param.PageSize.Value > GlobalSettings.MaxPageSize ?
                        GlobalSettings.MaxPageSize :
                        param.PageSize.Value;
                }

                var queryTs = string.IsNullOrWhiteSpace(param.Ts) ?
                    DateTime.Now :
                    Convert.ToDouble(param.Ts).ConvertToDateTime(UnixOfTimeTypes.TotalSeconds);

                (MMOfficialPost[] postData, int rowCount) = await _postRepo
                    .OfficialPostSearch(new PostSearchCondition()
                    {
                        PostType = PostType.Official,
                        UserId = param.UserId,
                        IsRecommend = param.IsRecommend,
                        SortType = param.SortType,
                        AreaCode = param.AreaCode == "00" ? null : param.AreaCode,
                        ServiceIds = param.ServiceIds,
                        Age = param.Age,
                        Height = param.Height,
                        Cup = param.Cup,
                        Price = param.Price,
                        BookingStatus = param.BookingStatus,
                        UserIdentity = param.UserIdentity,
                        QueryTs = queryTs,
                        PageNo = queryPage,
                        PageSize = pageSize
                    });

                OfficialPostList[] resultData = new OfficialPostList[0];
                if (postData.Any())
                {
                    var postUserIds = postData.Select(x => x.UserId).ToArray();
                    var allPostUserInfo = (await _userInfoRepo.GetUserInfos(postUserIds)) ?? Array.Empty<MMUserInfo>();

                    //組資料
                    var resultTask = postData.Select(async (p) =>
                    {
                        return new OfficialPostList()
                        {
                            PostId = p.PostId,
                            PostType = PostType.Official,
                            CoverUrl = await _postMediaImageService.GetFullMediaUrl(
                                new MMMedia()
                                {
                                    FileUrl = p.CoverUrl
                                },
                                postType: PostType.Official,
                                isThumbnail: true),
                            Title = p.Title,
                            Height = Enum.IsDefined(typeof(BodyHeightDefined), (int)p.Height) ?
                                ((BodyHeightDefined)p.Height).GetDescription() :
                                BodyHeightDefined.H_Plus.GetDescription(),
                            Age = Enum.IsDefined(typeof(AgeDefined), (int)p.Age) ?
                                ((AgeDefined)(int)p.Age).GetDescription() :
                                AgeDefined.Y_Plus.GetDescription(),
                            Cup = Enum.IsDefined(typeof(CupDefined), (int)p.Cup) ?
                                ((CupDefined)(int)p.Cup).GetDescription() :
                                CupDefined.Plus.GetDescription(),
                            AreaCode = p.AreaCode,
                            LowPrice = Math.Truncate(p.LowPrice).ToString(),
                            FacialScore = (p.FacialScore ?? 0).ToString(),
                            UpdateTime = (p.ExamineTime ?? default(DateTime)).ToMMUpdateString(),
                            IsOpen = allPostUserInfo
                                .Where(x => x.UserId == p.UserId)?
                                .FirstOrDefault()?.IsOpen ?? true
                        };
                    });

                    resultData = await Task.WhenAll(resultTask) ?? new OfficialPostList[0];
                }

                var result = new BaseReturnDataModel<OfficialPostListViewModel>();
                result.DataModel = new OfficialPostListViewModel()
                {
                    PageNo = queryPage,
                    PageSize = pageSize,
                    TotalPage = (int)Math.Ceiling((decimal)rowCount / pageSize),
                    Data = resultData,
                    TotalCount = rowCount,
                    Ts = queryTs.ToUnixOfTime(UnixOfTimeTypes.TotalSeconds).ToString()
                };
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, model);
        }

        /// <summary>
        /// 获取我的官方帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OfficialPostListViewModel>> GetMyOfficialPostList(ReqMyOfficialStorePost model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                int queryPage = model.Page;
                queryPage = queryPage <= 0 ? 1 : queryPage;

                var pageSize = GlobalSettings.PageSize;

                var posts = await _postRepo.MyOfficialPostList(param);

                OfficialPostList[] resultData = new OfficialPostList[0];
                if (posts.Data.Any())
                {
                    //組資料
                    var resultTask = posts.Data.Select(async (p) =>
                    {
                        return new OfficialPostList()
                        {
                            PostId = p.PostId,
                            CoverUrl = await _postMediaImageService.GetFullMediaUrl(
                                new MMMedia()
                                {
                                    FileUrl = p.CoverUrl
                                },
                                postType: PostType.Official,
                                isThumbnail: true),
                            Title = p.Title,
                            Height = Enum.IsDefined(typeof(BodyHeightDefined), (int)p.Height) ?
                                ((BodyHeightDefined)p.Height).GetDescription() :
                                BodyHeightDefined.H_Plus.GetDescription(),
                            Age = Enum.IsDefined(typeof(AgeDefined), (int)p.Age) ?
                                ((AgeDefined)(int)p.Age).GetDescription() :
                                AgeDefined.Y_Plus.GetDescription(),
                            Cup = Enum.IsDefined(typeof(CupDefined), (int)p.Cup) ?
                                ((CupDefined)(int)p.Cup).GetDescription() :
                                CupDefined.Plus.GetDescription(),
                            LowPrice = Math.Truncate(p.LowPrice).ToString(),
                            ViewBaseCount = p.ViewBaseCount,
                            AreaCode = p.AreaCode,
                        };
                    });

                    resultData = await Task.WhenAll(resultTask) ?? new OfficialPostList[0];
                }

                var result = new BaseReturnDataModel<OfficialPostListViewModel>();
                result.DataModel = new OfficialPostListViewModel()
                {
                    Page = queryPage,
                    PageSize = pageSize,
                    TotalPage = (int)Math.Ceiling((decimal)posts.TotalCount / pageSize),
                    Data = resultData,
                    TotalCount = posts.TotalCount
                };
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, model);
        }

        /// <summary>
        /// 官方店铺帖子列表
        /// </summary>
        /// <param name="model">搜尋參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OfficialPostListViewModel>> OfficialPostList(ReqOfficialStorePost model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                int queryPage = model.Page;
                queryPage = queryPage <= 0 ? 1 : queryPage;

                var boss = await _identityApplyRepo.Detail(param.ApplyId);

                param.UserId = boss?.UserId;
                var posts = await _postRepo.OfficialPostList(param);

                OfficialPostList[] resultData = new OfficialPostList[0];
                if (posts.Data.Any())
                {
                    //組資料
                    var resultTask = posts.Data.Select(async (p) =>
                    {
                        return new OfficialPostList()
                        {
                            PostId = p.PostId,
                            CoverUrl = await _postMediaImageService.GetFullMediaUrl(
                                new MMMedia()
                                {
                                    FileUrl = p.CoverUrl
                                },
                                postType: PostType.Official,
                                isThumbnail: true),
                            Title = p.Title,
                            Height = Enum.IsDefined(typeof(BodyHeightDefined), (int)p.Height) ?
                                ((BodyHeightDefined)p.Height).GetDescription() :
                                BodyHeightDefined.H_Plus.GetDescription(),
                            Age = Enum.IsDefined(typeof(AgeDefined), (int)p.Age) ?
                                ((AgeDefined)(int)p.Age).GetDescription() :
                                AgeDefined.Y_Plus.GetDescription(),
                            Cup = Enum.IsDefined(typeof(CupDefined), (int)p.Cup) ?
                                ((CupDefined)(int)p.Cup).GetDescription() :
                                CupDefined.Plus.GetDescription(),
                            LowPrice = Math.Truncate(p.LowPrice).ToString(),
                            ViewBaseCount = p.ViewBaseCount,
                            AreaCode = p.AreaCode,
                            Views = p.Views
                        };
                    });

                    resultData = await Task.WhenAll(resultTask) ?? new OfficialPostList[0];
                }

                var result = new BaseReturnDataModel<OfficialPostListViewModel>();
                result.DataModel = new OfficialPostListViewModel()
                {
                    Page = queryPage,
                    PageSize = param.PageSize,
                    TotalPage = (int)Math.Ceiling((decimal)posts.TotalCount / param.PageSize),
                    Data = resultData,
                    TotalCount = posts.TotalCount
                };
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, model);
        }

        /// <summary>
        /// 官方覓贴詳情
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OfficialPostDetail>> OfficialPostDetail(ReqPostIdUserId model)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var apost = await _postRepo.GetOfficialPostById(param.PostId);
                if (apost == null || param.UserId == default(int))
                {
                    return new BaseReturnDataModel<OfficialPostDetail>(ReturnCode.DataIsNotCompleted);
                }

                if (apost.IsDelete)
                {
                    return new BaseReturnDataModel<OfficialPostDetail>(ReturnCode.DataIsNotExist);
                }

                int userId = param.UserId;

                //取得媒體資源
                var medias = (await _postMediaImageService.Get(SourceType.Post, new string[] { param.PostId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                var videos = (await _postMediaVideoService.Get(SourceType.Post, new string[] { param.PostId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);

                //取得跟贴相關的服務項目
                var serviceSettings = await _postRepo.GetPostMappingService(new string[] { param.PostId });

                //取得所有設定
                var options = await _optionRepo.GetPostTypeOptions(PostType.Official);

                var cardType = await GetPostCardType(apost.UserId);

                // 即時取得01那邊用戶資訊
                var zeroUserInfo = await GetZeroUserInfo((int)apost.UserId);

                var postUserInfo = await _userInfoRepo.GetUserInfo((int)apost.UserId);

                /*
                1. 會員未預約過該贴(有付過預約金即算有預約過)，顯示錯誤訊息"成功預約后才可以投诉哦"
                2. 該會員已針對該贴投訴過1次，顯示錯誤訊息"该贴您已投诉过"
                3. 該贴子預約後超過72小時，顯示錯誤訊息"預約超过72小时后不可投诉"
                每一張預約單個別計算
                */
                var booking = await _bookingRepo.GetBookingPost(new BookingFilter()
                {
                    UserId = param.UserId,
                    PostIds = new string[] { param.PostId }
                });

                var reports = await _postRepo.GetUserReported(param.UserId, param.PostId, null);
                var reportStatus = ViewOfficialReportStatus.CanReport;
                if (reports.Any())
                {
                    reportStatus = ViewOfficialReportStatus.HasReported;
                }
                else if (!booking.Any())
                {
                    reportStatus = ViewOfficialReportStatus.NoAppointment;
                }
                else if (booking.All(x => x.Status == BookingStatus.TransactionCompleted || x.Status == BookingStatus.RefundSuccessful))
                {
                    reportStatus = ViewOfficialReportStatus.AllAppointmentsFinished;
                }
                else if (!booking.Any(p => p.BookingTime.AddHours(72) >= DateTime.Now))
                {
                    reportStatus = ViewOfficialReportStatus.Overtime;
                }

                bool unfinishedBooking = false;
                if (booking.Any())
                {
                    unfinishedBooking = booking.Any(p =>
                        p.Status == BookingStatus.InService ||
                        p.Status == BookingStatus.RefundInProgress);
                }

                // Review 次數 + 1
                try
                {
                    await _postRepo.UpdateOfficialPostViewsCount(param.PostId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{MethodBase.GetCurrentMethod()}。增加官方贴子觀看次數失敗。PostId：{param.PostId}");
                }

                var resultData = new OfficialPostDetail
                {
                    PostId = param.PostId,
                    PostUserId = zeroUserInfo?.UserId,
                    PostType = PostType.Official,
                    PostStatus = apost.Status,
                    LockStatus = apost.LockStatus,
                    UserIdentity = (IdentityType)(postUserInfo?.UserIdentity ?? (int)IdentityType.General),
                    Title = apost.Title,
                    AreaCode = apost.AreaCode,
                    Nickname = zeroUserInfo?.NickName ?? apost.Nickname ?? string.Empty,
                    FacialScore = (apost.FacialScore ?? 0).ToString(),
                    Address = apost.Address,
                    CardType = cardType,
                    AvatarUrl = zeroUserInfo?.Avatar ?? string.Empty,
                    Height = Enum.IsDefined(typeof(BodyHeightDefined), (int)apost.Height) ?
                        ((BodyHeightDefined)(int)apost.Height).GetDescription() :
                        BodyHeightDefined.H_Plus.GetDescription(),
                    Age = Enum.IsDefined(typeof(AgeDefined), (int)apost.Age) ?
                        ((AgeDefined)(int)apost.Age).GetDescription() :
                        AgeDefined.Y_Plus.GetDescription(),
                    Cup = Enum.IsDefined(typeof(CupDefined), (int)apost.Cup) ?
                        ((CupDefined)(int)apost.Cup).GetDescription() :
                        CupDefined.Plus.GetDescription(),
                    LowPrice = Math.Truncate(apost.LowPrice).ToString(),
                    HighPrice = Math.Truncate(apost.HighPrice).ToString(),
                    BusinessHours = apost.BusinessHours,
                    ServiceDescribe = apost.ServiceDescribe ?? string.Empty,
                    ServiceItem = MappingServiceItem(param.PostId, serviceSettings, options),
                    PhotoUrls = medias?
                        .Where(p => p.MediaType == (int)MediaType.Image)?
                        .Select(p => p.FullMediaUrl)?
                        .ToArray() ?? Array.Empty<string>(),
                    VideoUrl = videos?
                        .Where(p => p.MediaType == (int)MediaType.Video && string.Equals(Path.GetExtension(p.FileUrl).ToLower(), ".m3u8"))?
                        .Select(p => p.FullMediaUrl)?
                        .FirstOrDefault() ?? string.Empty,
                    ReportStatus = reportStatus,
                    ReportedCount = reports.Count(),
                    AvgFacialScore = apost.Comments == 0M ? "5.0" : (apost.TotalFacialScore / (apost.Comments == 0M ? 1M : apost.Comments)).ToString(".#"),
                    AvgServiceQuality = apost.Comments == 0M ? "5.0" : (apost.TotalServiceQuality / (apost.Comments == 0M ? 1M : apost.Comments)).ToString(".#"),
                    Comments = apost.Comments.ToString(),
                    AppointmentCount = apost.AppointmentCount.ToString(),
                    HaveUnfinishedBooking = unfinishedBooking,
                    ShopName = postUserInfo?.Nickname
                };

                var result = new BaseReturnDataModel<OfficialPostDetail>();
                result.DataModel = resultData;
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, model);
        }

        /// <summary>
        /// 官方覓贴詳情
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<AdminOfficialPostDetail>> AdminOfficialPostDetail(ReqPostIdUserId model)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var apost = await _postRepo.GetOfficialPostById(param.PostId);
                if (apost == null || param.UserId == default(int))
                {
                    return new BaseReturnDataModel<AdminOfficialPostDetail>(ReturnCode.DataIsNotCompleted);
                }

                //取得媒體資源
                var videos = (await _postMediaVideoService.Get(SourceType.Post, new string[] { param.PostId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);
                var images = (await _postMediaImageService.Get(SourceType.Post, new string[] { param.PostId }))?
                    .DataModel?
                    .OrderBy(p => p.CreateDate);
                //取得跟贴相關的服務項目
                var serviceSettings = await _postRepo.GetPostMappingService(new string[] { param.PostId });

                //取得所有設定
                var options = await _optionRepo.GetPostTypeOptions(PostType.Official);

                var cardType = await GetPostCardType(apost.UserId);
                //VIP 會員
                var vip = _vipService.GetUserInfoData(apost.UserId).Result.CurrentVip;

                //取得官方贴套餐組合
                var officialPrice = await _postRepo.GetOfficialPostPrice(new string[] { param.PostId });

                // 即時取得01那邊用戶資訊
                var zeroUserInfo = await GetZeroUserInfo((int)apost.UserId);

                var postUserInfo = await _userInfoRepo.GetUserInfo((int)apost.UserId);

                /*
                1. 會員未預約過該贴(有付過預約金即算有預約過)，顯示錯誤訊息"成功預約后才可以投诉哦"
                2. 該會員已針對該贴投訴過1次，顯示錯誤訊息"该贴您已投诉过"
                3. 該贴子預約後超過72小時，顯示錯誤訊息"預約超过72小时后不可投诉"
                每一張預約單個別計算
                */
                var booking = await _bookingRepo.GetBookingPost(new BookingFilter()
                {
                    UserId = param.UserId,
                    PostIds = new string[] { param.PostId }
                });

                var reportStatus = ViewOfficialReportStatus.CanReport;
                if (await _postRepo.UserHasReported((int)param.UserId, param.PostId))
                {
                    reportStatus = ViewOfficialReportStatus.HasReported;
                }

                bool isHomePost = false;
                int weight = 0;
                //获取当前贴子权重信息
                var postWeight = await _postRepo.GetMMOfficialPostWeight().WhereAsync(a => a.PostId == apost.PostId).FirstOrDefaultAsync();
                if (postWeight != null)
                {
                    weight = postWeight.Weight;
                    isHomePost = true;
                }

                var resultData = new AdminOfficialPostDetail
                {
                    PostId = param.PostId,
                    PostType = PostType.Official,
                    UserIdentity = (IdentityType)(postUserInfo?.UserIdentity ?? (int)IdentityType.General),
                    Title = apost.Title,
                    AreaCode = apost.AreaCode,
                    Nickname = zeroUserInfo?.NickName ?? apost.Nickname ?? string.Empty,
                    FacialScore = (apost.FacialScore ?? 0).ToString(),
                    Address = apost.Address,
                    CardType = cardType,
                    AvatarUrl = zeroUserInfo?.Avatar ?? string.Empty,
                    Height = Enum.IsDefined(typeof(BodyHeightDefined), (int)apost.Height) ?
                        ((BodyHeightDefined)(int)apost.Height).GetDescription() :
                        BodyHeightDefined.H_Plus.GetDescription(),
                    Age = Enum.IsDefined(typeof(AgeDefined), (int)apost.Age) ?
                        ((AgeDefined)(int)apost.Age).GetDescription() :
                        AgeDefined.Y_Plus.GetDescription(),
                    Cup = Enum.IsDefined(typeof(CupDefined), (int)apost.Cup) ?
                        ((CupDefined)(int)apost.Cup).GetDescription() :
                        CupDefined.Plus.GetDescription(),
                    LowPrice = Math.Truncate(apost.LowPrice).ToString(),
                    HighPrice = Math.Truncate(apost.HighPrice).ToString(),
                    BusinessHours = apost.BusinessHours,
                    ServiceDescribe = apost.ServiceDescribe ?? string.Empty,
                    ServiceItem = MappingServiceItem(param.PostId, serviceSettings, options),
                    PhotoUrls = images?
                        .Where(p => p.MediaType == (int)MediaType.Image)?
                        .Select(p => p.FullMediaUrl)?
                        .ToArray() ?? Array.Empty<string>(),
                    VideoUrl = videos?
                        .Where(p => p.MediaType == (int)MediaType.Video)?
                        .Select(p => p.FullMediaUrl)?
                        .FirstOrDefault() ?? string.Empty,
                    ReportStatus = reportStatus,
                    AvgFacialScore = apost.Comments == 0M ? "5.0" : (apost.TotalFacialScore / apost.Comments).ToString(".#"),
                    AvgServiceQuality = apost.Comments == 0M ? "5.0" : (apost.TotalServiceQuality / apost.Comments).ToString(".#"),
                    Comments = apost.Comments.ToString(),
                    IsHomePost = isHomePost,
                    Weight = weight,
                    Memo = apost.Memo,
                    UserId = apost.UserId,
                    CreateTime = apost.CreateTime,
                    UpdateTime = apost.UpdateTime,
                    ExamineTime = apost.ExamineTime,
                    IsDelete = apost.IsDelete,
                    IsOpen = postUserInfo?.IsOpen,
                    Status = apost.Status,
                    CompletedOrder = booking.Where(a => a.Status == BookingStatus.TransactionCompleted).Count(),
                    CardEffectiveTime = null,
                    Combo = officialPrice?.Select(p => new AdminComboData()
                    {
                        ComboName = p.ComboName,
                        ComboPrice = p.ComboPrice,
                        Service = p.Service
                    }).ToArray() ?? Array.Empty<AdminComboData>(),
                    PhotoSource = images?
                        .Where(p => p.MediaType == (int)MediaType.Image)?
                        .ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>(),
                    VideoSource = videos?
                        .Where(p => p.MediaType == (int)MediaType.Video)?
                        .ToDictionary(p => p.Id, p => p.FullMediaUrl) ?? new Dictionary<string, string>(),
                    ViewBaseCount = apost.ViewBaseCount
                };

                var result = new BaseReturnDataModel<AdminOfficialPostDetail>();
                result.DataModel = resultData;
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, model);
        }

        /// <summary>
        /// 官方私信
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OfficialDM>> GetOfficialDM(ReqPostIdUserId model)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var apost = await _postRepo.GetOfficialPostById(param.PostId);
                if (apost == null)
                {
                    return new BaseReturnDataModel<OfficialDM>(ReturnCode.DataIsNotCompleted);
                }

                if (apost.Status != ReviewStatus.Approval)
                {
                    return new BaseReturnDataModel<OfficialDM>(ReturnCode.DataIsNotExist);
                }

                var booking = await _bookingRepo.GetBookingPost(new BookingFilter()
                {
                    UserId = param.UserId,
                    PostIds = new string[] { param.PostId }
                });

                if (!booking.Any())
                {
                    return new BaseReturnDataModel<OfficialDM>(ReturnCode.NoAppointment);
                }

                var user = await _userInfoRepo.GetUserInfo(apost.UserId);

                var allAdvertising = await _advertisingRepo.Get();

                var result = new BaseReturnDataModel<OfficialDM>();
                result.DataModel = new OfficialDM()
                {
                    DownloadTip = allAdvertising?
                        .Where(p => p.AdvertisingType == (int)AdvertisingType.MailSetting)
                        .FirstOrDefault()?.AdvertisingContent ?? string.Empty,
                    DownloadUrl = allAdvertising?
                        .Where(p => p.AdvertisingType == (int)AdvertisingType.MailSettingUrl)
                        .FirstOrDefault()?.AdvertisingContent ?? string.Empty,
                    Contact = user?.Contact ?? string.Empty
                };
                result.SetCode(ReturnCode.Success);

                return await Task.FromResult(result);
            }, model);
        }

        /// <summary>
        /// 檢查評價參數
        /// </summary>
        /// <param name="param"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        private static BaseReturnModel CheckOfficialCommentParam(ReqOfficialCommentData param, bool isEdit)
        {
            if ((string.IsNullOrWhiteSpace(param.CommentId) && isEdit) ||
                string.IsNullOrWhiteSpace(param.Nickname) ||
                string.IsNullOrWhiteSpace(param.Comment))
            {
                return new BaseReturnModel(ReturnCode.MissingNecessaryParameter);
            }

            if (param.Comment.Length > 100 ||
                param.FacialScore < 1 ||
                param.FacialScore > 5 ||
                param.ServiceQuality < 1 ||
                param.ServiceQuality > 5)
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>
        /// 評論
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> AddOfficialComment(ReqOfficialCommentData model)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var checkResult = CheckOfficialCommentParam(param, false);
                if (!checkResult.IsSuccess)
                {
                    return checkResult;
                }

                var userInfo = await _zeroOneService.GetUserInfo(new ZOUserInfoReq((int)param.UserId));

                // 綁定預約單
                MMBooking? bookingInfo = await _bookingRepo.GetById(param.BookingId);
                if (bookingInfo == null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotExist);
                }

                // 只有服務完成且在待評價階段才能評論
                //if (bookingInfo.Status != BookingStatus.PendingEvaluation)
                //{
                //    return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
                //}

                // 該預約單非評論者本人
                if (bookingInfo.UserId != param.UserId)
                {
                    return new BaseReturnModel(ReturnCode.IllegalUser);
                }

                // 評論過不得再評論
                var aComment = await _postRepo.GetOfficialPostCommentByBookingId(bookingInfo.BookingId);
                if (aComment != null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsExist);
                }

                //bookingInfo.Status = BookingStatus.EvaluationReview;

                var commentDto = new MMOfficialPostComment()
                {
                    BookingId = param.BookingId,
                    PostId = bookingInfo.PostId,
                    AvatarUrl = userInfo?.DataModel?.Avatar ?? string.Empty,
                    UserId = param.UserId,
                    Nickname = userInfo?.DataModel?.NickName ?? param.Nickname ?? string.Empty,
                    FacialScore = param.FacialScore,
                    ServiceQuality = param.ServiceQuality,
                    Comment = param.Comment,
                    Status = (int)ReviewStatus.UnderReview,
                    CreateTime = DateTime.Now
                };

                OfficialCommentModel officialComment = new()
                {
                    Comment = commentDto,
                    Booking = bookingInfo
                };

                var upsertResult = await _postRepo.OfficialCommentUpsert(officialComment);

                return new BaseReturnModel(upsertResult ?
                    new BaseReturnModel(ReturnCode.Success) :
                    new BaseReturnModel(ReturnCode.RunSQLFail));
            }, model);
        }

        /// <summary>
        /// 取得編輯評論資料
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<OfficialCommentEditData>> GetOfficialCommentEditData(ReqCommentIdUserId model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var aComment = await _postRepo.GetOfficialPostCommentByCommentId(param.CommentId);
                if (aComment == null)
                {
                    return new BaseReturnDataModel<OfficialCommentEditData>(ReturnCode.DataIsNotExist);
                }

                if (aComment.UserId != param.UserId)
                {
                    return new BaseReturnDataModel<OfficialCommentEditData>(ReturnCode.IllegalUser);
                }

                var resultData = new OfficialCommentEditData()
                {
                    CommentId = aComment.CommentId,
                    FacialScore = aComment.FacialScore,
                    ServiceQuality = aComment.ServiceQuality,
                    Comment = aComment.Comment
                };

                var result = new BaseReturnDataModel<OfficialCommentEditData>(ReturnCode.Success)
                {
                    DataModel = resultData
                };

                return await Task.FromResult(result);
            }, model);
        }

        /// <summary>
        /// 編輯評論
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnModel> EditOfficialComment(ReqOfficialCommentData model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var checkResult = CheckOfficialCommentParam(param, true);
                if (!checkResult.IsSuccess)
                {
                    return checkResult;
                }

                var aComment = await _postRepo.GetOfficialPostCommentByCommentId(param.CommentId);
                if (aComment == null)
                {
                    return new BaseReturnDataModel<CommentEditData>(ReturnCode.DataIsNotExist);
                }

                // 比對不到該評論在數據庫與傳入參數 BookingId
                if (aComment.BookingId != param.BookingId)
                {
                    return new BaseReturnDataModel<CommentEditData>(ReturnCode.NonMatched);
                }

                // 非評論本人
                if (aComment.UserId != param.UserId)
                {
                    return new BaseReturnDataModel<CommentEditData>(ReturnCode.IllegalUser);
                }

                MMBooking? bookingInfo = await _bookingRepo.GetById(aComment.BookingId);
                if (bookingInfo == null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotExist);
                }

                // 編輯評論只能是在評論未通過時才能評論
                //if (bookingInfo.Status != BookingStatus.EvaluationRejected)
                //{
                //    return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
                //}

                //bookingInfo.Status = BookingStatus.EvaluationReview;

                aComment.FacialScore = param.FacialScore;
                aComment.ServiceQuality = param.ServiceQuality;
                aComment.Comment = param.Comment;
                aComment.UpdateTime = DateTime.Now;
                aComment.Status = (byte)ReviewStatus.UnderReview;

                OfficialCommentModel officialComment = new()
                {
                    Comment = aComment,
                    Booking = bookingInfo
                };

                var upsertResult = await _postRepo.OfficialCommentUpsert(officialComment);

                return new BaseReturnModel(upsertResult ?
                    new BaseReturnModel(ReturnCode.Success) :
                    new BaseReturnModel(ReturnCode.RunSQLFail));
            }, model);
        }

        /// <summary>
        /// 評論清單
        /// </summary>
        /// <param name="model">輸入參數</param>
        /// <returns></returns>
        public async Task<BaseReturnDataModel<PageResultModel<OfficialCommentList>>> OfficialCommentList(ReqPostIdPageNo model)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var data = await _postRepo.GetOfficialPostComment(param.PostId, new PaginationModel
                {
                    PageNo = param.PageNo,
                    PageSize = GlobalSettings.PageSize
                });

                //組成回傳資料
                OfficialCommentList[] commentResultData = data?.Data?.Select(p => new OfficialCommentList()
                {
                    AvatarUrl = p.AvatarUrl,
                    Nickname = p.Nickname,
                    FacialScore = p.FacialScore,
                    ServiceQuality = p.ServiceQuality,
                    Comment = p.Comment,
                    PublishTime = p.ExamineTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty,
                }).ToArray() ?? new OfficialCommentList[0];

                var result = new BaseReturnDataModel<PageResultModel<OfficialCommentList>>(ReturnCode.Success);
                result.DataModel = new PageResultModel<OfficialCommentList>()
                {
                    PageNo = param.PageNo,              //以傳進來的page進行返回
                    PageSize = data?.PageSize ?? GlobalSettings.PageSize,
                    TotalPage = data?.TotalPage ?? 0,
                    Data = commentResultData
                }; ;

                return await Task.FromResult(result);
            }, model);
        }
    }
}