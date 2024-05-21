using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MMService.Attributes;

using MMService.Models.My;
using MMService.Services;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Models.Entities.HomeAnnouncement;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Models.User;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.MMModel.Models.HomeAnnouncement;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;

using MMModel = MS.Core.MMModel.Models;

namespace MMService.Controllers
{
    /// <summary>
    /// 個人中心
    /// </summary>
    public class MyController : ApiControllerBase
    {
        /// <summary>
        /// 個人相關服務
        /// </summary>
        private readonly IMyService _my;

        /// <summary>
        /// 設定相關服務
        /// </summary>
        private readonly ISettingsService _settingService;

        /// <summary>
        /// 適配器
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 個人中心
        /// </summary>
        /// <param name="logger">log</param>
        /// <param name="my">個人相關服務</param>
        /// <param name="settingService">設定相關服務</param>
        public MyController(ILogger<MyController> logger,
            IMapper mapper,
            IMyService my,
            ISettingsService settingService) : base(logger)
        {
            _my = my;
            _mapper = mapper;
            _settingService = settingService;
        }

        /// <summary>
        /// 個人中心頁
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<CenterInfo>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Center()
        {
            return ApiResult(await TryCatchProcedure(async () =>{
                var result = new BaseReturnDataModel<CenterViewModel>();

                int userId = User.GetUserId();

                var query = await _my.Center(userId);

                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<CenterViewModel>(query.DataModel);
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetCode(ReturnCode.SystemError);
                }

                return result;
            }));
        }

        /// <summary>
        /// 總覽
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(MMModel.ApiResponse<BaseReturnDataModel<Overview>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Overview()
        {
            return ApiResult(await TryCatchProcedure<BaseReturnDataModel<Overview>>(async () =>
            {
                int? userId = User.UserId();
                if (!userId.HasValue)
                {
                    return new BaseReturnDataModel<Overview>(ReturnCode.ParameterIsInvalid);
                }

                string nickname = User.GetNickname();

                var result = await _my.Overview(userId.Value, nickname);

                return result;
            }));
        }
        /// <summary>
        /// 获取我的收藏
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<MyFavorite>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyFavorite(MyFavoriteQueryParamForClient param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                if (!User.UserId().HasValue)
                {
                    return new BaseReturnDataModel<PageResultModel<MyFavorite>>(ReturnCode.ParameterIsInvalid);
                }

                param.UserId = User.GetUserId();
                return await _my.GetMyFavorite(param);
            }, param));
        }
        /// <summary>
        /// 發贴管理
        /// </summary>
        /// <param name="model">贴子類型</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<MyPostList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ManagePost(MyPostQueryParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                if (!User.UserId().HasValue)
                {
                    return new BaseReturnDataModel<PageResultModel<MyPostList>>(ReturnCode.ParameterIsInvalid);
                }

                param.UserId = User.GetUserId();
                return await _my.ManagePost(param);
            }, model));
        }

        /// <summary>
        /// 觅老板查询自己的官方帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<MyOfficialPostList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> OfficialBossPostPage(MyOfficialQueryParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                if (!User.UserId().HasValue)
                {
                    return new BaseReturnDataModel<PageResultModel<MyOfficialPostList>>(ReturnCode.ParameterIsInvalid);
                }
                param.UserId = User.GetUserId();
                return await _my.OfficialBossPostPageAsync(param);
            }, model));
        }

        /// <summary>
        /// 用户已读消息记录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<MyMessageList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UserToMessageOperation(MessageOperationParamForClient param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                if (!User.UserId().HasValue)
                {
                    return new BaseReturnDataModel<PageResultModel<MyMessageList>>(ReturnCode.ParameterIsInvalid);
                }
                param.UserId= User.GetUserId();
                return await _my.UserToMessageAll(param);
            }, param));
        }
        /// <summary>
        /// 获取我的消息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<MyMessageList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MyMessage(MyMessageQueryParamForClient param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                if (!User.UserId().HasValue && param.MessageInfoType.Equals(MessageType.ComplaintPost))
                {
                    return new BaseReturnDataModel<PageResultModel<MyMessageList>>(ReturnCode.ParameterIsInvalid);
                }
                param.UserId = User.GetUserId();
                return await _my.MyMessage(param);
            }, param));
        }
        //GetAnnouncementById
        /// <summary>
        /// 公告消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> GetAnnouncementById(string Id)
        {
            return ApiResult(await TryCatchProcedure(async (Id) =>
            {
                if (!int.TryParse(Id,out int result))
                {
                    return new BaseReturnDataModel<MyAnnouncementViewModel>(ReturnCode.ParameterIsInvalid);
                }
                return await _my.GetAnnouncementById(result);
            },Id));
        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="favoriteId"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("{favoriteId}")]
        public async Task<IActionResult> CanCelFavorite(string favoriteId)
        {
            return ApiResult(await TryCatchProcedure(async (favoriteId) =>
            {
                if (string.IsNullOrEmpty(favoriteId))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
                return await _my.CanCelFavorite(favoriteId);
            }, favoriteId));
        }
        /// <summary>
        /// 投诉詳情
        /// </summary>
        /// <param name="reportId">投诉Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{reportId}")]
        public async Task<IActionResult> ReportDetail(string reportId)
        {
            return ApiResult(await TryCatchProcedure(async (reportId) =>
            {
                if (string.IsNullOrEmpty(reportId))
                {
                    return new BaseReturnDataModel<ReportDetailViewModel>(ReturnCode.ParameterIsInvalid);
                }

                return await _my.ReportDetail(reportId);

            }, reportId));
        }
        /// <summary>
        /// 解鎖的贴子
        /// </summary>
        /// <param name="param">查詢參數</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(MMModel.ApiResponse<MyUnlockPostListViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UnlockPost(MyUnlockQueryParam param)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                if (!User.UserId().HasValue)
                {
                    return new BaseReturnDataModel<MyUnlockPostListViewModel>(ReturnCode.ParameterIsInvalid);
                }

                param.UserId = User.GetUserId();

                var resultData = (await _my.UnlockPost(param)).DataModel;

                var tip = await _settingService.GetTip(param.PostType);

                var result = new BaseReturnDataModel<MyUnlockPostListViewModel>();
                result.SetCode(ReturnCode.Success);
                result.DataModel = new MyUnlockPostListViewModel()
                {
                    PageNo = resultData.PageNo,
                    TotalPage = resultData.TotalPage,
                    PageSize = resultData.PageSize,
                    Data = resultData.Data,
                    Tip = tip.DataModel
                };
                return result;
            }));
        }

        /// <summary>
        /// User資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<UserInfoViewModel>), StatusCodes.Status200OK)]
        [UserFrequency(seconds: 1, times: 1)]
        public async Task<IActionResult> UserInfo()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                UserInfoReq req = new()
                {
                    UserId = User.GetUserId(),
                };
                BaseReturnDataModel<UserInfoRes> userInfo = await _my.GetUserInfo(req);
                return new BaseReturnDataModel<UserInfoViewModel>(ReturnCode.Success)
                {
                    DataModel = _mapper.Map<UserInfoViewModel>(userInfo.DataModel),
                };
            }));
        }

        /// <summary>
        /// 商店營業開關
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ShopOpenClosed>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ShopOpen()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                return await _my.ShopOpen(new UserInfoReq()
                {
                    UserId = User.GetUserId(),
                });
            }));
        }

        /// <summary>
        /// 刪除官方贴
        /// </summary>
        /// <param name="model">刪除資料</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteOfficialPost(OfficialPostDelete model)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                var dm = _mapper.Map<ReqOfficialPostDelete>(model);
                dm.UserId = User.GetUserId();

                return await _my.DeleteOfficialPost(dm);
            }));
        }

        /// <summary>
        /// 上架官方帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SetShelfOfficialPost(OfficialPostShelfOfficial model)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                var dm = _mapper.Map<ReqSetShelfOfficialPost>(model);
                dm.UserId = User.GetUserId();

                return await _my.SetShelfOfficialPost(dm);
            }));
        }
        /// <summary>
        /// 上架官方帖子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditShopDoBusinessTime(EditDoBusinessTimeParamter model)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                return await _my.EditShopDoBusinessTime(model);
            }));
        }
    }
}