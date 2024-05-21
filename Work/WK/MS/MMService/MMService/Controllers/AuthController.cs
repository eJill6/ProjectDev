using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMService.Services;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Models.Auth;
using MS.Core.MM.Models.Auth.Enums;
using MS.Core.MM.Models.Auth.ServiceReq;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Auth;
using MS.Core.MMModel.Models.Post;
using MS.Core.Models;

namespace MMService.Controllers
{
    /// <summary>
    /// 權限相關
    /// </summary>
    public class AuthController : ApiControllerBase
    {
        /// <summary>
        /// 權限相關服務
        /// </summary>
        private readonly IAuthService _auth;

        /// <summary>
        /// 適配器
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 個人中心
        /// </summary>
        /// <param name="logger">log</param>
        /// <param name="auth">權限相關服務</param>
        /// <param name="mapper">適配器</param>
        public AuthController(ILogger<AuthController> logger,
            IAuthService auth,
            IMapper mapper) : base(logger)
        {
            _auth = auth;
            _mapper = mapper;
        }

        /// <summary>
        /// 權限登入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<SignInResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SignIn(SignInData model)
        {
            return ApiResult(await TryCatchProcedure<SignInData, BaseReturnDataModel<SignInResponse>>(async (param) =>
            {
                return await _auth.GenerateToken(param, LoginType.FrontendSide);
            }, model));
        }

        /// <summary>
        /// 後台登入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<SignInResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BackendSignIn(SignInData model)
        {
            return ApiResult(await TryCatchProcedure<SignInData, BaseReturnDataModel<SignInResponse>>(async (param) =>
            {
                return await _auth.GenerateToken(param, LoginType.BackendSide);
            }, model));
        }

        /// <summary>
        /// 認證資訊
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CertificationInfo()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                return await _auth.CertificationInfo(new ReqUserId()
                {
                    UserId = User.GetUserId()
                });
            }));
        }

        ///// <summary>
        ///// 覓經紀申請
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        //public async Task<IActionResult> AgentIdentityApply()
        //{
        //    return ApiResult(await TryCatchProcedure(async () =>
        //    {
        //        return await _auth.AgentIdentityApply(new ReqUserId()
        //        {
        //            UserId = User.GetUserId()
        //        });
        //    }));
        //}

        /// <summary>
        /// 覓經紀申請 包含contact信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AgentIdentityApply(AgentIdentityApplyData model)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
				var dm = _mapper.Map<ReqAgentIdentityApplyData>(model);
				dm.UserId = (int)User?.UserId();

				return await _auth.AgentIdentityApply(dm);
            }));
        }

        /// <summary>
        /// 覓老闆申請
        /// </summary>
        /// <param name="model">申請資料</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> BossIdentityApply(BossIdentityApplyData model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var dm = _mapper.Map<ReqBossIdentityApplyData>(model);
                dm.UserId = (int)User?.UserId();

                return await _auth.BossIdentityApply(dm);
            }, model));
        }

        /// <summary>
        /// 覓老闆申請或者更新资料
        /// </summary>
        /// <param name="model">申請資料</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> BossIdentityApplyOrUpdate(OfficialShopDetailForclient model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var dm = _mapper.Map<ReqBossApplyOrUpdateData>(model);
                dm.UserId = (int)User?.UserId();

                return await _auth.BossIdentityApplyOrUpdate(dm);
            }, model));
        }

        /// <summary>
        /// 覓女郎申請
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GirlIdentityApply()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                return await _auth.GirlIdentityApply(new ReqUserId()
                {
                    UserId = User.GetUserId()
                });
            }));
        }
    }
}