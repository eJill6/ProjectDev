using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MMService.Services;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Post;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminPost;
using MS.Core.Models;

namespace MMService.Controllers
{
    public class PostWeightController : ApiControllerBase
    {
        /// <summary>
        /// 贴子相關服務
        /// </summary>
        private readonly IPostService _service;

        private readonly IMapper _mapper = null;

        public PostWeightController(IPostService service, IMapper mapper,
    ILogger logger) : base(logger)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// 後台贴權重表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<PostWeightResult>>();
                var query = await _service.GetMMPostWeight();
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<IEnumerable<PostWeightResult>>(query.DataModel);
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    result.SetModel(query);
                }
                return result;
            }, String.Empty));
        }

        /// <summary>
        /// 後台贴權重表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ApiResponse<PostWeightResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Detail(int id)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PostWeightResult>();
                var query = await _service.GetMMPostWeight();
                if (query.IsSuccess)
                {
                    var item = query.DataModel.FirstOrDefault(x => x.Id == id);
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
            }, String.Empty));
        }

        /// <summary>
        /// 後台贴權重表新增
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(MMPostWeight param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.InsertMMPostWeight(param);
            }, param));
        }

        /// <summary>
        /// 後台贴權重表刪除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.DeleteMMPostWeight(id);
            }, id));
        }

        /// <summary>
        /// 後台贴權重表修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Update(UpdateMMPostParam param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.UpdateMMPostWeight(param);
            }, param));
        }

        /// <summary>
        /// 後台權重表批量刪除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostWeightBatchRemove(AdminPostBatchData param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                return await _service.PostWeightBatchRemove(param);
            }, param));
        }        
    }
}