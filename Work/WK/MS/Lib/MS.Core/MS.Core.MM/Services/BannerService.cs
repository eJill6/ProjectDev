using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.Redis;
using MS.Core.MM.Model.Banner;
using MS.Core.MM.Model.Banner.Enums;
using MS.Core.MM.Model.Media;
using MS.Core.MM.Models;
using MS.Core.MM.Models.Media;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Media.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Services;
using MS.Core.Utils;

namespace MS.Core.MM.Service
{
    /// <inheritdoc cref="IBannerService"/>
    public class BannerService : BaseService, IBannerService
    {
        /// <inheritdoc cref="IEnumerable{IMediaService}"/>
        private readonly IEnumerable<IMediaService> _medias = null;

        /// <inheritdoc cref="IBannerRepo"/>
        private readonly IBannerRepo _repo = null;

        /// <inheritdoc cref="IRedisService"/>
        private readonly IRedisService _cache;

        /// <summary>
        /// 快取用的CacheKey
        /// </summary>
        private readonly string _cacheKey = "MMService:BannerCacheKey";

        /// <summary>
        /// 快取用的Db Index
        /// </summary>
        private readonly int _cacheIndexes = 10;

        /// <inheritdoc />
        public BannerService(IEnumerable<IMediaService> medias,
            IBannerRepo repo,
            IRedisService cache,
            ILogger<BannerService> logger) : base(logger)
        {
            _medias = medias;
            _repo = repo;
            _cache = cache;
        }

        private IMediaService GetMedia => _medias.FirstOrDefault(m => m.SourceType == SourceType.Banner && m.Type == MediaType.Image);

        /// <inheritdoc />
        public async Task<BaseReturnModel> Create(SaveBannerParam param)
        {
            return await TryCatchProcedure<SaveBannerParam, BaseReturnModel>(async (param) =>
            {
                if (param.Media == null)
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                var now = DateTime.Now;
                param.Id = await _repo.CreateNewSEQID();

                // 設定媒體資料
                param.Media.RefId = param.Id;
                param.Media.SourceType = (int)SourceType.Banner;
                if (param.Media.CreateDate == default(DateTime))
                {
                    param.Media.CreateDate = now;
                }

                // 設定banner資料
                if (param.CreateDate == default(DateTime))
                {
                    param.CreateDate = now;
                }

                // 設定banner資料
                if (param.StartDate == default(DateTime))
                {
                    param.StartDate = now;
                }

                var checkResult = await CheckParam(param);

                if (!checkResult.IsSuccess)
                {
                    return checkResult;
                }

                var mediaResult = await GetMedia.CreateToOss(param.Media);
                if (!mediaResult.IsSuccess)
                {
                    return mediaResult;
                }

                if (!(await _repo.Create(param)))
                {
                    return new BaseReturnModel(ReturnCode.OperationFailed);
                }

                await _cache.RemoveCache(_cacheIndexes, _cacheKey);
                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <inheritdoc />
        public async Task<BaseReturnModel> Delete(string seqId)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var banner = await _repo.Get(seqId);
                if (banner == null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotExist);
                }

                var medias = await GetMedia.Get(SourceType.Banner, seqId);
                foreach (var media in medias.DataModel)
                {
                    await GetMedia.DeleteToOss(media.Id);
                }
                await _repo.Delete(seqId, medias.DataModel.Select(x => x.Id).ToArray());
                await _cache.RemoveCache(_cacheIndexes, _cacheKey);
                return new BaseReturnModel(ReturnCode.Success);
            }, seqId);
        }

        /// <inheritdoc />
        public async Task<BaseReturnDataModel<BannerInfo[]>> Get()
        {
            return await TryCatchProcedure<object, BaseReturnDataModel<BannerInfo[]>>(async (param) =>
            {
                var result = new BaseReturnDataModel<BannerInfo[]>() { };
                result.DataModel = await _cache.GetCache<BannerInfo, BannerInfo[]>(
                    _cacheIndexes,
                    _cacheKey,
                    true,
                    60 * 60,
                    false,
                    async () =>
                    {
                        try
                        {
                            var banners = await _repo.Get();

                            if (banners.Length > 0)
                            {
                                // 有banner才找Media
                                var refIds = banners.Select(x => x.Id).ToArray();
                                var medias = await GetMedia.Get(SourceType.Banner, refIds);
                                if (!medias.IsSuccess)
                                {
                                    result.SetModel(medias);
                                    return new BannerInfo[0];
                                }

                                return banners.Select(x =>
                                {
                                    var banner = JsonUtil.CastByJson<BannerInfo>(x);
                                    banner.Media = medias.DataModel.FirstOrDefault(m => m.RefId == banner.Id);
                                    return banner;
                                }).ToArray();
                            }

                            return new BannerInfo[0];
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Get fails, param:{JsonUtil.ToJsonString(param)}");
                            result.SetCode(ReturnCode.SystemError);
                            return new BannerInfo[0];
                        }
                    });

                result.SetCode(ReturnCode.Success);
                return result;
            }, null);
        }

        /// <inheritdoc />
        public async Task<BaseReturnDataModel<BannerInfo[]>> Get(DateTime dateTime)
        {
            return await TryCatchProcedure<DateTime, BaseReturnDataModel<BannerInfo[]>>(async (param) =>
            {
                var result = await Get();
                if (result.IsSuccess)
                {
                    result.DataModel = result.DataModel
                        .Where(x => x.IsActive && ((x.StartDate == null ? true : dateTime >= x.StartDate) && (x.EndDate == null ? true : dateTime < x.EndDate)))
                        .OrderByDescending(x => x.Sort)
                        .ToArray();
                }
                return result;
            }, dateTime);
        }

        /// <inheritdoc />
        public async Task<BaseReturnModel> Update(SaveBannerParam param)
        {
            return await TryCatchProcedure<SaveBannerParam, BaseReturnModel>(async (param) =>
            {
                if (string.IsNullOrEmpty(param.Id))
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                var setting = JsonUtil.CastByJson<SaveBannerParam>(await _repo.Get(param.Id));

                if (setting == null)
                {
                    return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
                }

                var checkResult = await CheckParam(param);

                if (!checkResult.IsSuccess)
                {
                    return checkResult;
                }

                var mediaResult = await GetMedia.Get(SourceType.Banner, param.Id);
                if (!mediaResult.IsSuccess && mediaResult.DataModel.Length == 0)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
                }

                MediaInfo[] others = null;
                if (param.Media != null)
                {
                    if (!string.IsNullOrEmpty(param.Media.Id) && mediaResult.DataModel.Length > 1)
                    {
                        // 防呆刪除多餘的貼圖
                        others = mediaResult.DataModel.Where(x => x.Id != param.Media.Id).ToArray();
                    }

                    var updateResult = await GetMedia.UpdateToOss(param.Media);
                    if (!updateResult.IsSuccess)
                    {
                        return updateResult;
                    }
                }

                setting.Title = param.Title;
                setting.StartDate = param.StartDate;
                setting.EndDate = param.EndDate;
                setting.Sort = param.Sort;
                setting.IsActive = param.IsActive;
                setting.LinkType = param.LinkType;
                setting.RedirectUrl = param.RedirectUrl;
                setting.ModifyUser = param.ModifyUser;
                setting.ModifyDate = param.ModifyDate;
                setting.LocationType = param.LocationType;
                if (param.Media != null)
                {
                    setting.Media = param.Media;
                }
                else
                {
                    setting.Media = JsonUtil.CastByJson<SaveMediaToOssParam>(mediaResult.DataModel.FirstOrDefault());
                }

                if (!(await _repo.Update(setting)))
                {
                    return new BaseReturnModel(ReturnCode.UpdateFailed);
                }

                if (others != null)
                {
                    foreach (var other in others)
                    {
                        await GetMedia.Delete(other.Id);
                    }
                }
                await _cache.RemoveCache(_cacheIndexes, _cacheKey);
                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }

        /// <summary>
        /// 檢查參數資料
        /// </summary>
        /// <param name="param">參數資料</param>
        /// <param name="isAdd">是否為新增</param>
        /// <returns>回傳結果給前端</returns>
        private async Task<BaseReturnModel> CheckParam(SaveBannerParam param)
        {
            //不同位置类型的图片大小不一样，已在后台进行限制 2023年11月6日14:45:03 Edison
            //if (param.Media != null)
            //{
            //    var media = await GetMedia.CheckParam(param.Media);
            //    if (!media.IsSuccess)
            //    {
            //        return media;
            //    }
            //}

            List<string> validRequiredValues = new List<string>
            {
                param.Title,
                param.CreateUser
            };

            if (!ParamUtil.IsValidRequired(validRequiredValues.ToArray()))
            {
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            if (!Enum.IsDefined(typeof(LinkType), Convert.ToInt32(param.LinkType)))
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }
            if (!Enum.IsDefined(typeof(LocationType), Convert.ToInt32(param.LocationType)))
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }
            if (param.StartDate >= param.EndDate)
            {
                return new BaseReturnModel(MMReturnCode.DateIncorrect);
            }

            if (param.Sort < 1 || param.Sort > 9999)
            {
                return new BaseReturnModel(MMReturnCode.SortLimit);
            }

            //if (await _repo.HasDuplicateSort(param))
            //{
            //    return new BaseReturnModel(MMReturnCode.SortIsUsed);
            //}

            if (param.IsActive && param.EndDate < DateTime.Now)
            {
                return new BaseReturnModel(MMReturnCode.DateIsExpired);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }
    }
}