using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MMService.Attributes;
using MMService.Services;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Models.AdminPost;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using Newtonsoft.Json;
using System.Reflection;
using ReturnCode = MS.Core.Models.ReturnCode;

namespace MMService.Controllers
{
    /// <summary>
    /// 后台官方贴子
    /// </summary>
    public class AdminOfficialPostController : ApiControllerBase
    {
        /// <summary>
        /// 贴子相關服務
        /// </summary>
        private readonly IPostService _service;

        /// <summary>
        /// MM使用者的Repo
        /// </summary>
        private readonly IUserInfoRepo _user;

        /// <summary>
        /// 會員卡的服務
        /// </summary>
        private readonly IVipService _vip;

        /// <summary>
        /// 城市服務
        /// </summary>
        private readonly ICityService _cityService;

        private readonly IUserVipRepo _userVip;

        /// <summary>
        /// 媒體服務
        /// </summary>
        private readonly IEnumerable<IMediaService> _mediaServices;

        /// <summary>
        /// 個人相關服務
        /// </summary>
        private readonly IMyService _my;

        /// <summary>
        /// 適配器
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 設定取得帖子影片
        /// </summary>
        private IMediaService _postMediaVideoService => _mediaServices
            .FirstOrDefault(m => m.SourceType == SourceType.Post && m.Type == MediaType.Video);

        public AdminOfficialPostController(IPostService service,
            IUserInfoRepo user,
            IVipService vip,
            ICityService cityService,
            IUserVipRepo userVipRepo,
            IEnumerable<IMediaService> mediaServices,
            IMyService my,
            IMapper mapper,
            ILogger logger) : base(logger)
        {
            _service = service;
            _user = user;
            _vip = vip;
            _cityService = cityService;
            _userVip = userVipRepo;
            _my = my;
            _mapper = mapper;
            _mediaServices = mediaServices;
        }

        /// <summary>
        /// 查找官方贴子（Admin）
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminOfficialPostList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(AdminPostListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = await _service.OfficialAdminPostSearch(model);
                if (result.IsSuccess &&
                    result?.DataModel?.Data != null)
                {
                    if (result.DataModel.Data.Length > 0)
                    {
                        var userIds = result.DataModel.Data.Select(x => x.UserId);
                        var users = await _user.GetUserInfos(userIds);
                        var userDic = users.GroupBy(x => x.UserId).ToDictionary(x => x.Key, x => x.First());

                        var vipTypeDic = (await _vip.GetVipTypes()).ToDictionary(e => e.Id, e => e.Name);
                        foreach (var post in result.DataModel.Data)
                        {
                            if (userDic.ContainsKey(post.UserId))
                            {
                                var user = userDic[post.UserId];
                                if (user.UserIdentity == 0)
                                {
                                    post.UserType = "一般";
                                }
                                post.IsOpen = user.IsOpen;
                                post.UserIdentity = (MS.Core.MMModel.Models.User.Enums.IdentityType)user.UserIdentity;
                            }
                            else
                            {
                                _logger.LogError($"{MethodInfo.GetCurrentMethod()?.Name ?? string.Empty} {nameof(MMOfficialPost)} not have {nameof(MMUserInfo)} item:{JsonConvert.SerializeObject(post)}");
                                post.UserType = "一般";
                            }
                            var userVipInfo = await _vip.GetUserInfoData(post.UserId);
                            if (userVipInfo.IsVip)
                            {
                                post.VipCard = string.Join("，", userVipInfo.Vips.Select(x => vipTypeDic.GetValueOrDefault(x.VipType)));
                            }
                            else
                            {
                                post.VipCard = "-";
                            }
                        }
                    }
                    result.SetCode(ReturnCode.Success);
                    return result;
                }
                result.SetModel(result);
                return result;
            }, model));
        }

        /// <summary>
        /// 官方覓贴詳情（Admin）
        /// </summary>
        /// <param name="postId">官方贴Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<AdminOfficialPostDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Detail(string postId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = await _service.AdminOfficialPostDetail(new ReqPostIdUserId()
                {
                    UserId = (int)User?.UserId(),
                    PostId = postId
                });
                if (result.IsSuccess)
                {
                    //移除会员卡信息
                    //List<string> userTypes = result.DataModel.CardType.Select(item => ((VipType)item).GetDescription()).ToList();
                    //result.DataModel.CardName = string.Join(",", userTypes);
                    result.DataModel.AreaCode = _cityService.Get(result.DataModel.AreaCode);
                }

                return result;
            }, postId));
        }

        /// <summary>
        /// 編輯發佈的官方贴（Admin）
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="model">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}")]
        [UserFrequency(seconds: 3, times: 1)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit(string postId, AdminOfficialPostData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                param.PostId = postId;
                param.ExamineMan = User?.Nickname();
                var result = await _service.AdminEditOfficialPost(param);
                if (param.PostStatus == (int)ReviewStatus.Approval &&
                    result.IsSuccess &&
                    _postMediaVideoService != null)
                {
                    var videos = await _postMediaVideoService.Get(SourceType.Post, model.PostId);
                    if (videos.IsSuccess)
                    {
                        var videoDatas = videos.DataModel.Where(x => x.MediaType == (int)MediaType.Video).ToArray();
                        if (videoDatas.Length > 0)
                        {
                            foreach (var item in videoDatas)
                            {
                                await _postMediaVideoService.NotifyVideoProcess(item.Id);
                                await Task.Delay(1000);
                            }
                        }
                    }
                }
                return result;
            }, model));
        }

        /// <summary>
        /// 編輯官方贴编辑锁定状态（Admin）
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="parame">编辑状态</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditLock(string postId, AdminOfficialPostEditLockData parame)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                return await _service.AdminOfficialPostEditLock(postId, parame.LockStatus);
            }));
        }

        /// <summary>
        /// 刪除官方贴(Admin)
        /// </summary>
        /// <param name="model">刪除資料</param>
        /// <param name="userId">会员ID</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteOfficialPost(OfficialPostDelete model)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                var dm = _mapper.Map<ReqOfficialPostDelete>(model);

                return await _my.ModifyDeleteStatusOfficialPost(dm);
            }));
        }
    }
}