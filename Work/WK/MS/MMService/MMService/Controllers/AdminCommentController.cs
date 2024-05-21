using Microsoft.AspNetCore.Mvc;
using MMService.Services;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminComment;
using MS.Core.Models;
using MS.Core.Models.Models;

namespace MMService.Controllers
{
    public class AdminCommentController : ApiControllerBase
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

        public AdminCommentController(IPostService service,
            IUserInfoRepo user,
            IVipService vip,
            ICityService cityService,
            ILogger logger) : base(logger)
        {
            _service = service;
            _user = user;
            _vip = vip;
            _cityService = cityService;
        }

        /// <summary>
        /// 评价记录
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminCommentList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(AdminCommentListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = await _service.CommentSearch(model);
                if (result.IsSuccess)
                {
                    result.SetCode(ReturnCode.Success);
                    return result;
                }
                result.SetModel(result);
                return result;
            }, model));
        }

        /// <summary>
        /// 评价审核
        /// </summary>
        /// <param name="commentId">评价Id</param>
        /// <param name="model">發佈資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{commentId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit(string commentId, AdminCommentData model)
        {
            model.Id = commentId;
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var nickname = User?.Nickname();
                if (string.IsNullOrEmpty(nickname))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
                param.ExamineMan = nickname;
                return await _service.CommentEdit(param);
            }, model));
        }



        /// <summary>
        /// 评价详情
        /// </summary>
        /// <param name="commentId">评价Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{commentId}")]
        [ProducesResponseType(typeof(ApiResponse<AdminCommentDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Detail(string commentId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = await _service.CommentDetail(commentId);
                if (result.IsSuccess)
                {
                    result.DataModel.AreaCode = _cityService.Get(result.DataModel.AreaCode);
                }
                return result;
            }, commentId));
        }
    }
}
