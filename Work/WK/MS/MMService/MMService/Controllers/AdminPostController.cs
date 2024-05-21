using Microsoft.AspNetCore.Mvc;
using MMService.Attributes;
using MMService.Services;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using Newtonsoft.Json;
using System.Reflection;
using ReturnCode = MS.Core.Models.ReturnCode;

namespace MMService.Controllers
{
    public class AdminPostController : ApiControllerBase
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

        /// <summary>
        /// 媒體服務
        /// </summary>
        private readonly IEnumerable<IMediaService> _mediaServices;

        /// <summary>
        /// 設定取得贴子影片
        /// </summary>
        private IMediaService _postMediaVideoService => _mediaServices
            .FirstOrDefault(m => m.SourceType == SourceType.Post && m.Type == MediaType.Video);

        public AdminPostController(IPostService service,
            IUserInfoRepo user,
            IVipService vip,
            ICityService cityService,
            IEnumerable<IMediaService> mediaServices,
            ILogger logger) : base(logger)
        {
            _service = service;
            _user = user;
            _vip = vip;
            _cityService = cityService;
            _mediaServices = mediaServices;
        }

        /// <summary>
        /// 查找贴子。適用首頁、廣場
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminPostList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(AdminPostListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = await _service.PostSearch(model);
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

                                post.UserType = EnumExtension.GetDescription((IdentityType)user.UserIdentity);
                            }
                            else
                            {
                                _logger.LogError($"{MethodInfo.GetCurrentMethod()?.Name ?? string.Empty} {nameof(MMPost)} not have {nameof(MMUserInfo)} item:{JsonConvert.SerializeObject(post)}");
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
        /// 审核發佈贴子
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="model">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit(string postId, AdminPostData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                param.PostId = postId;
                param.ExamineMan = User?.Nickname();
                var result = await _service.PostEdit(param);
                if (model.PostStatus == ReviewStatus.Approval &&
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
        /// 批量編輯發佈贴子
        /// </summary>
        /// <param name="model">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> BatchReview(AdminPostBatchData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                param.ExamineMan = User?.Nickname();
                var result = await _service.PostBatchEdit(param);
                if (param.PostStatus == (int)ReviewStatus.Approval &&
                    result.IsSuccess &&
                    _postMediaVideoService != null)
                {
                    foreach (var id in param.PostIds.Split(",").ToArray())
                    {
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            var videos = await _postMediaVideoService.Get(SourceType.Post, id);
                            if (videos.IsSuccess)
                            {
                                var videoDatas = videos.DataModel.Where(x => x.MediaType == (int)MediaType.Video).ToArray();
                                if (videoDatas.Length > 0)
                                {
                                    foreach (var item in videoDatas)
                                    {
                                        var itemResult = await _postMediaVideoService.NotifyVideoProcess(item.Id);
                                        if (!itemResult.IsSuccess)
                                        {
                                            _logger.LogError($"BatchReview NotifyVideoProcess fail item:{JsonConvert.SerializeObject(item)}, itemResult:{JsonConvert.SerializeObject(itemResult)}");
                                        }
                                        await Task.Delay(1000);
                                    }
                                }
                                else
                                {
                                    _logger.LogError($"BatchReview videoDatas.Length < 0 videoDatas:{JsonConvert.SerializeObject(videoDatas)}");
                                }
                            }
                            else
                            {
                                _logger.LogError($"BatchReview get media fail videos:{JsonConvert.SerializeObject(videos)}");
                            }
                        }
                    }
                }
                else
                {
                    _logger.LogError($"BatchReview:param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}, _postMediaVideoService != null:{_postMediaVideoService != null}");
                }

                return result;
            }, model));
        }

        /// <summary>
        /// 覓贴詳情
        /// </summary>
        /// <param name="postId">贴子Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<AdminPostDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Detail(string postId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = await _service.PostAdminDetail(param);
                if (result.IsSuccess)
                {
                    var user = (await _user.GetUserInfos(new int[] { result.DataModel.UserId })).FirstOrDefault(x => x.UserId == result.DataModel.UserId);
                    if (user == null)
                    {
                        return new MS.Core.Models.BaseReturnDataModel<AdminPostDetail>(ReturnCode.DataIsNotCompleted);
                    }
                    else
                    {
                        result.DataModel.UserType = ((IdentityType)user.UserIdentity).GetDescription();
                    }
                    result.DataModel.AreaCode = _cityService.Get(result.DataModel.AreaCode);
                }

                return result;
            }, postId));
        }

        /// <summary>
        /// 編輯發佈的贴
        /// </summary>
        /// <param name="postId">贴 Id</param>
        /// <param name="model">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditPost(string postId, AdminPostData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                param.PostId = postId;
                param.ExamineMan = User?.Nickname();
                var result = await _service.PostEditAllData(param);
                if (model.PostStatus == ReviewStatus.Approval &&
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
                            }
                        }
                    }
                }
                return result;
            }, model));
        }
    }
}