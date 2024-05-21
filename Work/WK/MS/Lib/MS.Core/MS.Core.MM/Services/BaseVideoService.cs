using Microsoft.Extensions.Logging;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Model.Media;
using MS.Core.MM.Models;
using MS.Core.MM.Models.Media;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Services;
using MS.Core.Utils;
using Newtonsoft.Json;
using System.Reflection;

namespace MS.Core.MM.Service
{
    /// <summary>
    /// 媒體服務的基礎型別
    /// </summary>
    public abstract class BaseVideoService : BaseService, IMediaService
    {
        /// <inheritdoc cref="IMediaRepo"/>
        private readonly IMediaRepo _repo = null;

        /// <inheritdoc/>
        public abstract SourceType SourceType { get; }

        /// <inheritdoc/>
        public MediaType Type => MediaType.Video;

        private IZeroOneApiService _zeroOne = null;

        private readonly IPostRepo _postRepo = null;

        public BaseVideoService(
            IMediaRepo repo,
            IZeroOneApiService zeroOne,
            IPostRepo postRepo,
            ILogger logger) : base(logger)
        {
            _repo = repo;
            _zeroOne = zeroOne;
            _postRepo = postRepo;
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> Create(SaveMediaToOssParam param)
        {
            var uploadResult = await CreateToOss(param);
            if (!uploadResult.IsSuccess)
            {
                return uploadResult;
            }

            return await TryCatchProcedure<SaveMediaToOssParam, BaseReturnModel>(async (param) =>
            {
                await _repo.Create(param);
                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> Delete(string seqId)
        {
            var deleteOssResult = await DeleteToOss(seqId);
            if (!deleteOssResult.IsSuccess)
            {
                return deleteOssResult;
            }

            return await TryCatchProcedure<string, BaseReturnModel>(async (param) =>
            {
                if (!(await _repo.Delete(param)))
                {
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }
                return new BaseReturnModel(ReturnCode.Success);
            }, seqId);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> DeleteToOss(string seqId)
        {
            return await TryCatchProcedure<string, BaseReturnModel>(async (param) =>
            {
                var media = await _repo.Get(param);
                if (media == null)
                {
                    return new BaseReturnModel(MMReturnCode.DateIncorrect);
                }

                return new BaseReturnModel(ReturnCode.Success);
            }, seqId);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<MediaInfo[]>> Get(SourceType type, string refId)
        {
            return await Get(type, new string[] { refId });
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<MediaInfo[]>> Get(SourceType type, string[] refIds)
        {
            if (refIds.Length <= 0)
            {
                _logger.LogError($"{MethodInfo.GetCurrentMethod()} fail, refIds Length is 0");
                return await Task.FromResult(new BaseReturnDataModel<MediaInfo[]>(ReturnCode.MissingNecessaryParameter));
            }
            return await TryCatchProcedure<Tuple<SourceType, string[]>, BaseReturnDataModel<MediaInfo[]>>(async (param) =>
            {
                var result = new BaseReturnDataModel<MediaInfo[]>();
                var queryResult = await _repo.Get((int)Type, (int)param.Item1, param.Item2);
                var items = new List<MediaInfo>();
                foreach (var query in queryResult)
                {
                    var item = JsonUtil.CastByJson<MediaInfo>(query);
                    item.FullMediaUrl = await GetFullMediaUrl(item);
                    items.Add(item);
                }
                result.DataModel = items.ToArray();
                result.SetCode(ReturnCode.Success);
                return result;
            }, new Tuple<SourceType, string[]>(type, refIds));
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<MediaInfo[]>> GetByIds(SourceType type, string[] ids)
        {
            if (ids.Length <= 0)
            {
                _logger.LogError($"{MethodInfo.GetCurrentMethod()} fail, ids Length is 0");
                return await Task.FromResult(new BaseReturnDataModel<MediaInfo[]>(ReturnCode.MissingNecessaryParameter));
            }
            return await TryCatchProcedure<Tuple<SourceType, string[]>, BaseReturnDataModel<MediaInfo[]>>(async (param) =>
            {
                var result = new BaseReturnDataModel<MediaInfo[]>();
                var queryResult = await _repo.GetByIds((int)Type, (int)param.Item1, param.Item2);
                var items = new List<MediaInfo>();
                foreach (var query in queryResult)
                {
                    var item = JsonUtil.CastByJson<MediaInfo>(query);
                    item.FullMediaUrl = await GetFullMediaUrl(item);
                    items.Add(item);
                }
                result.DataModel = items.ToArray();
                result.SetCode(ReturnCode.Success);
                return result;
            }, new Tuple<SourceType, string[]>(type, ids));
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> Update(SaveMediaToOssParam param)
        {
            var updateResult = await UpdateToOss(param);
            if (!updateResult.IsSuccess)
            {
                return updateResult;
            }
            return await TryCatchProcedure<SaveMediaToOssParam, BaseReturnModel>(async (param) =>
            {
                if (!await _repo.Update(param))
                {
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }
                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> UpdateToOss(SaveMediaToOssParam param)
        {
            return await TryCatchProcedure<SaveMediaToOssParam, BaseReturnModel>(async (param) =>
            {
                var media = await _repo.Get(param.Id);
                if (media.FileUrl == param.FileUrl)
                {
                    // 圖片一樣就不做事
                    return new BaseReturnModel(ReturnCode.Success);
                }

                param.ModifyDate = DateTime.Now;

                // 先上傳新的圖檔
                var createResult = await CreateToOss(param);

                if (!createResult.IsSuccess)
                {
                    return createResult;
                }

                // 在刪除不一樣的圖檔
                //if (!(await _oos.DeleteObject(ConvertToOosUrl(media.FileUrl))))
                //{
                //    return new BaseReturnModel(ReturnCode.OperationFailed);
                //}

                return createResult;
            }, param);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> CheckParam(SaveMediaToOssParam param)
        {
            if (param == null)
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            List<string> validRequiredValues = new List<string>
            {
                param.FileName,
            };

            if (!ParamUtil.IsValidRequired(validRequiredValues.ToArray()))
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            return await ChildCheckParam(param);
        }

        /// <summary>
        /// 由子類別來實作相對應的檢查
        /// </summary>
        /// <param name="param">輸入參數</param>
        /// <returns>驗證結果</returns>
        protected abstract Task<BaseReturnModel> ChildCheckParam(SaveMediaToOssParam param);

        /// <inheritdoc/>
        public async Task<BaseReturnModel> CreateToOss(SaveMediaToOssParam param)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                if (string.IsNullOrEmpty(param.Id))
                {
                    param.Id = await _repo.CreateNewSEQID();
                }

                var extRaw = Path.GetExtension(param.FileName);
                var ext = extRaw.ToLower();

                var result = await _zeroOne.MediaUpload(new ZOMediaUploadReq()
                {
                    FileBody = param.Bytes,
                    FileName = param.FileName.Replace(ext, string.Empty),
                    FileNameExtension = ext,
                });

                if (!result.IsSuccess)
                {
                    _logger.LogError($"Upload file fail, result:{JsonUtil.ToJsonString(result)}");
                    return new BaseReturnModel(ReturnCode.ThirdPartyApiNotSuccess);
                }

                param.FileUrl = result.DataModel;
                param.FullMediaUrl = await GetFullMediaUrl(param);

                if (param.CreateDate == default(DateTime))
                {
                    param.CreateDate = DateTime.Now;
                }

                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <inheritdoc/>
        public async Task<string> GetFullMediaUrl(MMMedia param, bool isThumbnail = false, PostType postType = PostType.Square)
        {
            return await _zeroOne.GetFullMediaUrl(param.FileUrl);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnModel> NotifyVideoProcess(string mediaId)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                if (string.IsNullOrEmpty(param))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                var media = await _repo.Get(param);
                if (media == null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotExist);
                }

                if (Path.GetExtension(media.FileUrl).ToLower() == ".m3u8" ||
                    media.MediaType != (int)MediaType.Video)
                {
                    return new BaseReturnModel(ReturnCode.NonMatched);
                }

                await _zeroOne.NotifyVideoProcess(media.FileUrl, media.Id, 2);
                return new BaseReturnModel(ReturnCode.Success);
            }, mediaId);
        }

        /// <inheritdoc/>
        public async Task Encrypt(DateTime begin, DateTime end, DateTime finish)
        {
            // 交給01那邊去處理
            try
            {
                var totalPage = 1;
                var index = 1;
                for (; index <= totalPage; index++)
                {
                    var pageResult = await _repo.GetUnencrypt(Type, SourceType, begin, end, index, 20);
                    var medias = pageResult.Data.Where(x => !string.Equals(Path.GetExtension(x.FileUrl).ToLower(), ".m3u8")).ToArray();
                    foreach (var media in medias)
                    {
                        await Encrypt(media.Id);
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        _logger.LogInformation("VideoEncrypt {begin}, {end}, {Id} success", begin, end, media.Id);
                    }
                    totalPage = pageResult.TotalPage;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "VideoEncrypt Fail");
            }
        }

        /// <summary>
        /// 將原有資訊加密
        /// </summary>
        /// <param name="id">媒體編號</param>
        /// <returns>非同步任務</returns>
        private async Task<BaseReturnModel> Encrypt(string id)
        {
            return await TryCatchProcedure(async (id) =>
            {
                var media = await _repo.Get(id);

                if (media == null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotExist);
                }

                var mmPost = await _postRepo.GetById(media.RefId);
                var officialPost = await _postRepo.GetOfficialPostById(media.RefId);

                if ((mmPost == null && officialPost == null) ||
                    (mmPost?.Status != ReviewStatus.Approval && officialPost?.Status != ReviewStatus.Approval))
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotExist);
                }

                var extRaw = Path.GetExtension(media.FileUrl);
                var ext = extRaw.ToLower();

                if (string.IsNullOrEmpty(ext) || ext.Equals(".m3u8", StringComparison.OrdinalIgnoreCase) ||
                    media.MediaType != (int)MediaType.Video)
                {
                    // 视频已经加密就不做事
                    return new BaseReturnModel(ReturnCode.Success);
                }
                await _zeroOne.NotifyVideoProcess(media.FileUrl, media.Id, 2);
                return new BaseReturnModel(ReturnCode.Success);
            }, id);
        }

        /// <inheritdoc/>
        public Task Decrypt(DateTime begin, DateTime end, DateTime finish, bool isOnlyThumbnailResize)
        {
            // 交給01那邊去處理
            throw new NotImplementedException();
        }

        public async Task<BaseReturnDataModel<string>> CreateSplit(SaveMediaToOssParam createParam)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var extRaw = Path.GetExtension(param.FileName);
                var ext = extRaw.ToLower();

                var result = await _zeroOne.MediaSplitUpload(new ZOMediaUploadReq()
                {
                    FileBody = param.Bytes,
                    FileName = param.FileName.Replace(ext, string.Empty),
                    FileNameExtension = ext,
                });

                if (!result.IsSuccess)
                {
                    _logger.LogError($"SplieUpload file fail, result:{JsonUtil.ToJsonString(result)}");
                    return result;
                }

                return result;
            }, createParam);
        }

        public async Task<BaseReturnDataModel<MediaInfo>> CreateMerge(MergeUpload req)
        {
            return await base.TryCatchProcedure(async (param) =>
            {
                var result = await _zeroOne.MediaMergeUpload(param.Paths, param.Suffix);

                if (!result.IsSuccess)
                {
                    _logger.LogError($"SplieUpload file fail, result:{JsonUtil.ToJsonString(result)}");
                    return new BaseReturnDataModel<MediaInfo>(ReturnCode.ThirdPartyApiNotSuccess);
                }

                var media = new MediaInfo()
                {
                    Id = await _repo.CreateNewSEQID(),
                };

                media.FileUrl = result.DataModel;
                media.FullMediaUrl = await GetFullMediaUrl(media);
                media.CreateDate = DateTime.Now;
                media.MediaType = (int)param.MediaType;
                media.SourceType = (int)param.SourceType;

                if (await _repo.Create(media))
                {
                    return new BaseReturnDataModel<MediaInfo>(ReturnCode.Success)
                    {
                        DataModel = media,
                    };
                }
                else
                {
                    return new BaseReturnDataModel<MediaInfo>(ReturnCode.OperationFailed);
                }
            }, req);
        }

        /// <inheritdoc/>
        public Task<BaseReturnModel> UploadThumbnail(SaveMediaToOssParam param, PostType postType, bool isForceToOss = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<VideoUrlModel>> GetUploadVideoUrl()
        {
            return await base.TryCatchProcedure(async () =>
            {
                return await _zeroOne.GetUploadVideoUrl();
            });
        }
    }
}