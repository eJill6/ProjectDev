using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MS.Core.Extensions;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Models.Entities.HomeAnnouncement;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.Models;
using System.Runtime.ConstrainedExecution;

namespace MMService.Controllers
{
    public class HomeAnnouncementController : ApiControllerBase
    {
        private readonly IHomeAnnouncementService _service;

        private readonly IMapper _mapper = null;

        public HomeAnnouncementController(IHomeAnnouncementService service, IMapper mapper, ILogger logger) : base(logger)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// 後台首頁公告表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<MMHomeAnnouncement>>();
                var query = await _service.Get();
                if (query.IsSuccess)
                {
                    result.DataModel = _mapper.Map<IEnumerable<MMHomeAnnouncement>>(query.DataModel);
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
        /// 前台首頁公告跑马灯，按权重取第一条
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> FrontsideDetail()
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMHomeAnnouncement>();
                var query = await _service.Get();
                if (query.IsSuccess)
                {
                    var item = query.DataModel.Where(x => x.IsActive && x.Type == 1 
                    && (x.StartTime <= DateTime.Now || x.StartTime == null)
                    && (x.EndTime >= DateTime.Now || x.StartTime == null))
                    .OrderByDescending(x => x.Weight).FirstOrDefault();
                    if (item != null)
                    {
                        item.HomeContent = item.HomeContent.StripHtmlTags();
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
        /// 後台首頁公告Detail
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ApiResponse<MMHomeAnnouncement>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Detail(int id)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMHomeAnnouncement>();
                var query = await _service.Get();
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
        /// 後台首頁公告Update
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Update(MMHomeAnnouncement param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<MMHomeAnnouncement>>();
                var query = await _service.Get();
                if (query.IsSuccess)
                {
                    var data = _mapper.Map<IEnumerable<MMHomeAnnouncement>>(query.DataModel);

                    if (!data.Where(a => a.Weight == param.Weight && param.Type == a.Type && a.Id != param.Id).Any())
                    {
                        return await _service.Update(param);
                    }
                    else
                    {
                        BaseReturnModel returnModel = new BaseReturnModel();
                        returnModel.SetCode(new ReturnCode(ReturnCode.UpdateFailed.Code, false, "排序不可重复"));
                        return returnModel;
                    }
                }
                else
                {
                    BaseReturnModel returnModel = new BaseReturnModel();
                    returnModel.SetCode(new ReturnCode(ReturnCode.UpdateFailed.Code, false, "修改失败"));
                    return returnModel;
                }
            }, param));
        }

        [HttpPost]
        public async Task<IActionResult> Create(MMHomeAnnouncement param)
        {
            return ApiResult(await TryCatchProcedure<MMHomeAnnouncement, BaseReturnModel>(async (param) =>
            {
                if (string.IsNullOrWhiteSpace(User?.Nickname()))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }
                param.Operator = User?.Nickname();
                param.CreateDate = DateTime.Now;
                return await _service.Create(param);
            }, param));
        }

        /// <summary>
        ///  後台首頁公告删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return ApiResult(await TryCatchProcedure<int, BaseReturnModel>(async (param) =>
            {
                return await _service.Delete(param);
            }, id));
        }
    }
}