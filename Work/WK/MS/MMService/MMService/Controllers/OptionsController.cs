using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Post;
using MS.Core.MM.Services.Bases;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;

namespace MMService.Controllers
{
    /// <summary>
    /// 選項 dropdownlist 內容
    /// </summary>
    public class OptionsController : ApiControllerBase
    {
        /// <summary>
        /// 選項服務
        /// </summary>
        private readonly IOptionsService _service;

        /// <summary>
        /// 適配器
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 選項 dropdownlist 內容
        /// </summary>
        /// <param name="service">選項服務</param>
        /// <param name="mapper">適配器</param>
        /// <param name="logger">log</param>
        public OptionsController(IOptionsService service,
             IMapper mapper,
             ILogger logger) : base(logger)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// 新增項目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateOptionsParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.Create(param);
            }, param));
        }

        /// <summary>
        /// 刪除項目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{OptionId}")]
        public async Task<IActionResult> Delete(int OptionId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.Delete(OptionId);
            }, OptionId));
        }

        /// <summary>
        /// 修改項目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Update(UpdateOptionsParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.Update(param);
            }, param));
        }

        /// <summary>
        /// 取得項目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOptionByPostType(int postType)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMOptions[]>();
                var query = await _service.GetOptionByPostType(postType);
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<MMOptions[]>(query.DataModel);
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, postType));
        }

        /// <summary>
        /// 取得項目(後台)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOptionsByPostTypeAndOptionType(PostType postType, OptionType optionType)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMOptions[]>();
                var query = await _service.GetOptionsByPostTypeAndOptionType(postType, optionType, null);
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<MMOptions[]>(query.DataModel);
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, postType));
        }

        /// <summary>
        /// 取得單筆項目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{OptionId}")]
        public async Task<IActionResult> GetOptionsByOptionId(int OptionId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMOptions>();
                var query = await _service.GetOptionsByPostTypeAndOptionType(0, 0, OptionId);
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<MMOptions>(query.DataModel.FirstOrDefault());
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, OptionId));
        }
    }
}