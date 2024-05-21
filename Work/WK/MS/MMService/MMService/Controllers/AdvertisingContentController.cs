using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Service;
using MS.Core.MM.Services.interfaces;
using MS.Core.Models;

namespace MMService.Controllers
{
    public class AdvertisingContentController : ApiControllerBase
    {
        private readonly IAdvertisingContentService _service = null;
        private readonly IMapper _mapper = null;
        public AdvertisingContentController(IAdvertisingContentService service,
            IMapper mapper,
            ILogger logger) : base(logger)
        {
            _mapper = mapper;
            _service = service;
            _mapper = mapper;
        }
        /// <summary>
        /// 取得全部宣傳文字
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMAdvertisingContent[]>();
                var query = await _service.Get();
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<MMAdvertisingContent[]>(query.DataModel);
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
        /// 取得私信页设定
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMailSetting()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMAdvertisingContent[]>();
                var query = await _service.Get();
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<MMAdvertisingContent[]>(query.DataModel.Where(q => q.AdvertisingType == 5 || q.AdvertisingType == 6));
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
        /// 取得單筆宣傳文字
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.Get(param);
            }, id));
        }
        /// <summary>
        /// 刪除宣傳文字
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.Delete(param);
            }, id));
        }
        /// <summary>
        /// 更新宣傳文字
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Update(UpdateAdvertisingContentParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.Update(param);
            }, param));
        }
    }
}
