using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MMService.Attributes;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Models.Vip;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Vip;
using MS.Core.Models;

namespace MMService.Controllers
{
    /// <summary>
    /// 會員卡相關
    /// </summary>
    public class VipController : ApiControllerBase
    {
        /// <summary>
        /// 會員卡相關
        /// </summary>
        /// <param name="logger">log</param>
        /// <param name="vipService">會員相關服務</param>
        /// <param name="mapper">適配器</param>
        public VipController(ILogger logger, IVipService vipService, IMapper mapper) : base(logger)
        {
            VipService = vipService;
            Mapper = mapper;
        }

        private IVipService VipService { get; }
        private IMapper Mapper { get; }

        /// <summary>
        /// 購買會員卡
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFrequency(seconds: 3, times: 1)]
        [ProducesResponseType(typeof(ApiResponse<BaseReturnModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BuyVipTransaction(BuyVipData model)
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                return new BaseReturnModel(ReturnCode.CustomizedMessage, "服务已关闭");
            }));
        }

        /// <summary>
        /// 販售的會員卡
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<BaseReturnDataModel<VipViewModel[]>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListedVips()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                var vips = await VipService.GetListedVips();

                if (vips.IsSuccess == false)
                {
                    return new BaseReturnDataModel<VipViewModel[]>(vips) { };
                }

                var result = Mapper.Map<VipViewModel[]>(vips.DataModel);

                return new BaseReturnDataModel<VipViewModel[]>(ReturnCode.Success)
                {
                    DataModel = result
                };
            }));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<BaseReturnDataModel<UserVipTransLogViewModel[]>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UserVipTransLogs()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                var vips = await VipService.GetUserVipTransLogs(new ReqUserVipTransLog
                {
                    UserId = User.GetUserId(),
                });

                if (vips.IsSuccess == false)
                {
                    return new BaseReturnDataModel<UserVipTransLogViewModel[]>(vips) { };
                }

                var result = Mapper.Map<UserVipTransLogViewModel[]>(vips.DataModel);

                return new BaseReturnDataModel<UserVipTransLogViewModel[]>(ReturnCode.Success)
                {
                    DataModel = result
                };
            }));
        }
    }
}