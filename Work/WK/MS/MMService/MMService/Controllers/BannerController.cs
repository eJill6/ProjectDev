using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Model.Banner;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.Models;

namespace MMService.Controllers
{
    public class BannerController : ApiControllerBase
    {
        private readonly IBannerService _service = null;
        private readonly IMapper _mapper = null;

        public BannerController(IBannerService service,
            IMapper mapper,
            ILogger logger) : base(logger)
        {
            _mapper = mapper;
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveBannerParam param)
        {
            return ApiResult(await TryCatchProcedure<SaveBannerParam, BaseReturnModel>(async (param) =>
            {
                if (string.IsNullOrWhiteSpace(User?.Nickname()))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                param.CreateDate = DateTime.Now;
                return await _service.Create(param);
            }, param));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string seqId)
        {
            return ApiResult(await TryCatchProcedure<string, BaseReturnModel>(async (param) =>
            {
                return await _service.Delete(param);
            }, seqId));
        }

        [HttpPost]
        public async Task<IActionResult> Update(SaveBannerParam param)
        {
            return ApiResult(await TryCatchProcedure<SaveBannerParam, BaseReturnModel>(async (param) =>
            {
                if (string.IsNullOrWhiteSpace(User?.Nickname()))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                param.ModifyUser = User.Nickname();
                param.ModifyDate = DateTime.Now;
                return await _service.Update(param);
            }, param));
        }

        /// <summary>
        /// Banner
        /// </summary>
        /// <param name="bannerId">Banner id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{bannerId}")]
        [ProducesResponseType(typeof(ApiResponse<BannerInfo>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Detail(string bannerId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<BannerInfo>();
                var query = await _service.Get();
                if (query.IsSuccess)
                {
                    var item = query.DataModel.FirstOrDefault(x => x.Id == param);
                    if (item != null)
                    {
                        result.DataModel = item;
                        result.SetCode(ReturnCode.Success);
                    }
                    else
                    {
                        result.SetCode(ReturnCode.DataIsNotExist);
                    }
                }
                else
                {
                    result.SetModel(query);
                }

                return result;
            }, bannerId));
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                return await _service.Get();
            }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<BannerViewModel[]>();
                var query = await _service.Get(DateTime.Now);
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<BannerViewModel[]>(query.DataModel);
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }

                return result;
            }, DateTime.Now));
        }

        /// <summary>
        /// 快捷入口列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ShortcutList()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<BannerViewModel[]>();
                var query = await _service.Get(DateTime.Now);
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<BannerViewModel[]>(query.DataModel.Where(a => a.LocationType == 1).Take(8));
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }

                return result;
            }, DateTime.Now));
        }

        /// <summary>
        /// 官方店铺banner（官方首页）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> OfficialBanner()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<BannerViewModel[]>();
                var query = await _service.Get(DateTime.Now);
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<BannerViewModel[]>(query.DataModel.Where(a => a.LocationType == 3).Take(4));
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }

                return result;
            }, DateTime.Now));
        }
    }
}