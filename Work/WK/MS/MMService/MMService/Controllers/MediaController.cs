using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Models.Media;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using System.Linq;

namespace MMService.Controllers
{
    public class MediaController : ApiControllerBase
    {
        private readonly IEnumerable<IMediaService> _services = null;
        private readonly IMapper _mapper = null;
        public MediaController(IEnumerable<IMediaService> services,
            IMapper mapper,
            ILogger logger) : base(logger)
        {
            _services = services;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateImages(SaveBossApplyMedia applyMedia)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<List<MediaViewModel>>();
                if(!applyMedia.paramForClients.Any())
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);

                var sourceTypes = applyMedia.paramForClients.Select(c => c.SourceType);

                foreach (var paramItem in applyMedia.paramForClients)
                {
                    var createParam = _mapper.Map<MS.Core.MM.Model.Media.SaveMediaToOssParam>(paramItem);
                    var serviceItem =  _services.First(c => c.SourceType == paramItem.SourceType);
                    if (serviceItem == null)
                    {
                        result.SetCode(ReturnCode.ParameterIsInvalid);
                        continue;
                    }
                    
                    var checkResult = await serviceItem.CheckParam(createParam);
                    if (!checkResult.IsSuccess)
                    {
                        result.SetModel(checkResult);
                        continue;
                    }

                    var createResult = await serviceItem.Create(createParam);
                    if (!createResult.IsSuccess)
                    {
                        result.SetModel(createResult);
                        continue;
                    }
                    else
                    {
                        result.DataModel.Add(new MediaViewModel()
                        {
                            Id = createParam.Id,
                            FullMediaUrl = createParam.FullMediaUrl,
                        });
                        result.SetCode(ReturnCode.Success);
                    }
                }
                return result;

            }, applyMedia));
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveMediaToOssParamForClient param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MediaViewModel>();
                var service = _services.FirstOrDefault(x => x.SourceType == param.SourceType &&
                (int)x.Type == param.MediaType);

                if (service == null)
                {
                    result.SetCode(ReturnCode.ParameterIsInvalid);
                }
                else
                {
                    var createParam = _mapper.Map<MS.Core.MM.Model.Media.SaveMediaToOssParam>(param);
                    var checkResult = await service.CheckParam(createParam);
                    if (!checkResult.IsSuccess)
                    {
                        result.SetModel(checkResult);
                        return result;
                    }
                    var createResult = await service.Create(createParam);
                    if (!createResult.IsSuccess)
                    {
                        result.SetModel(createResult);
                        return result;
                    }
                    else
                    {
                        result.DataModel = new MediaViewModel()
                        {
                            Id = createParam.Id,
                            FullMediaUrl = createParam.FullMediaUrl,
                        };
                        result.SetCode(ReturnCode.Success);
                    }
                }

                return result;
            }, param));
        }

        [HttpPost]
        public async Task<IActionResult> SplitUpload(SaveMediaToOssParamForClient param)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<string>();
                var service = _services.FirstOrDefault(x => x.SourceType == param.SourceType &&
                (int)x.Type == param.MediaType);

                if (service == null)
                {
                    result.SetCode(ReturnCode.ParameterIsInvalid);
                }
                else
                {
                    var createParam = _mapper.Map<MS.Core.MM.Model.Media.SaveMediaToOssParam>(param);
                    var checkResult = await service.CheckParam(createParam);
                    if (!checkResult.IsSuccess)
                    {
                        result.SetModel(checkResult);
                        return result;
                    }
                    var createResult = await service.CreateSplit(createParam);
                    if (!createResult.IsSuccess)
                    {
                        result.SetModel(createResult);
                        return result;
                    }
                    else
                    {
                        result.DataModel = createResult.DataModel;
                        result.SetCode(ReturnCode.Success);
                    }
                }

                return result;
            }, param));
        }

        [HttpPost]
        public async Task<IActionResult> MergeUpload(MergeUpload request)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MediaViewModel>();
                var service = _services.FirstOrDefault(
                    x => x.SourceType == param.SourceType &&
                    x.Type == param.MediaType);

                if (service == null){
                    result.SetCode(ReturnCode.ParameterIsInvalid);
                }
                else{

                    var createResult = await service.CreateMerge(param);
                    if (!createResult.IsSuccess){
                        result.SetModel(createResult);
                        return result;
                    }
                    else{
                        result.DataModel = new MediaViewModel(){
                            Id = createResult.DataModel.Id,
                            FullMediaUrl = createResult.DataModel.FullMediaUrl,
                        };
                        result.SetCode(ReturnCode.Success);
                    }
                }

                return result;
            }, request));
        }

        /// <summary>
        /// 通知ZeroOne做影像處理
        /// </summary>
        /// <param name="req">傳輸參數</param>
        /// <returns>非同步Task</returns>
        [HttpPost]
        public async Task<IActionResult> NotifyVideoProcess(string mediaId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnModel();
                var service = _services.FirstOrDefault(x => x.SourceType == SourceType.Post &&
                x.Type == MediaType.Video);

                if (service == null)
                {
                    result.SetCode(ReturnCode.ParameterIsInvalid);
                }
                else
                {
                    return await service.NotifyVideoProcess(mediaId);
                }

                return result;
            }, mediaId));
        }



        /// <summary>
        /// 測試取得贴子下的媒體資訊
        /// </summary>
        /// <param name="sourceType">來源類型</param>
        /// <param name="mediaType">媒體類型</param>
        /// <param name="refId">贴子、頻論、舉報、banner的編號</param>
        /// <returns>非同步Task</returns>
        [HttpGet]
        public async Task<IActionResult> Get(int sourceType, int mediaType, string refId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MediaInfo[]>();
                var service = _services.FirstOrDefault(x => x.SourceType == (SourceType)param.Item1 &&
                x.Type == (MediaType)param.Item2);

                if (service == null)
                {
                    result.SetCode(ReturnCode.ParameterIsInvalid);
                }
                else
                {
                    result = await service.Get((SourceType)param.Item1, param.Item3);
                }

                return result;
            }, new Tuple<int, int, string>(sourceType, mediaType, refId)));
        }

        /// <summary>
        /// 取得上傳影片的Url
        /// </summary>
        /// <returns>非同步Task</returns>
        [HttpGet]
        public async Task<IActionResult> GetUploadVideoUrl()
        {
            return ApiResult(await TryCatchProcedure(async () =>
            {
                var result = new BaseReturnDataModel<VideoUrlModel>();
                var service = _services.FirstOrDefault(x => x.SourceType == SourceType.Post &&
                x.Type == MediaType.Video);

                if (service == null)
                {
                    result.SetCode(ReturnCode.ParameterIsInvalid);
                }
                else
                {
                    result = await service.GetUploadVideoUrl();
                }

                return result;
            }));
        }

    }
}
