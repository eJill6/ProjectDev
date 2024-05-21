using EasyNetQ;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Helpers.RestRequestHelpers;
using MS.Core.Infrastructure.Redis;
using MS.Core.Infrastructures.Exceptions;
using MS.Core.Infrastructures.Providers;
using MS.Core.Infrastructures.ZeroOne.Models;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZeroOne.Models.Responses;
using MS.Core.Models;
using MS.Core.Models.Models;
using System.Security.Cryptography;
using System.Text;

namespace MS.Core.Infrastructures.ZoneOne
{
    public class ZeroOneApiService : ZeroOneApiServiceBase, IZeroOneApiService
    {
        IMemoryCache MemoryCache { get; }

        public ZeroOneApiService(
            IDateTimeProvider dateTimeProvider,
            IOptions<ZeroOneSettings> zeroOneSettingsOptions,
            IRequestIdentifierProvider provider,
            IMemoryCache memoryCache,
            ILogger logger) : base(dateTimeProvider,
                zeroOneSettingsOptions,
                provider,
                logger)
        {
            MemoryCache = memoryCache;
        }

        public async Task<BaseReturnDataModel<ZOUserInfoRes>> GetUserInfo(ZOUserInfoReq req)
        {
            var result = await base.GetAsync<ZOUserInfoRes>(ZeroOneApi.UserInfo, req);
            if (result.IsSuccess)
            {
                var urlDomain = (await GetUrlDomain())?.Cdn?.FirstOrDefault();

                if (result.DataModel != null)
                {
                    result.DataModel.Avatar = $"{urlDomain}{result.DataModel.Avatar}";
                }
                else
                {
                    result.DataModel = new ZOUserInfoRes();
                }
            }
            return result;
        }

        public async Task<BaseReturnDataModel<bool>> GetPermission(ZOVipPermissionReq req)
        {
            var result = await base.GetAsync<bool>(ZeroOneApi.Permission, req);
            if (result.IsSuccess)
            {
                return result;
            }
            return new BaseReturnDataModel<bool>(ReturnCode.SystemError);
        }


        public async Task<BaseReturnModel> CashExpense(ZOCashIncomeExpenseReq req)
        {
            ZOCashDapiReq dapiReq = new(req.CategoryId, req.UserId, -req.Amount, req.Memo);
            var result = await base.PostAsync(ZeroOneApi.Cash, dapiReq);
            if (result.IsSuccess == false)
            {
                throw new ZeroOneException(result.ReturnCode);
            }
            return result;
        }

        public async Task<BaseReturnModel> CashIncome(ZOCashIncomeExpenseReq req)
        {
            ZOCashDapiReq dapiReq = new(req.CategoryId, req.UserId, req.Amount, req.Memo);
            var result = await base.PostAsync(ZeroOneApi.Cash, dapiReq);
            if (result.IsSuccess == false)
            {
                throw new ZeroOneException(result.ReturnCode);
            }
            return result;
        }

        public async Task<BaseReturnModel> PointExpense(ZOPointIncomeExpenseReq req)
        {
            ZOPointDapiReq dapiReq = new(req.CategoryId, req.UserId, -req.Point, req.Memo);
            var result = await base.PostAsync(ZeroOneApi.Point, dapiReq);
            if (result.IsSuccess == false)
            {
                throw new ZeroOneException(result.ReturnCode);
            }
            return result;
        }

        public async Task<BaseReturnModel> PointIncome(ZOPointIncomeExpenseReq req)
        {
            if (req.Point == 0)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            ZOPointDapiReq dapiReq = new(req.CategoryId, req.UserId, req.Point, req.Memo);
            var result = await base.PostAsync(ZeroOneApi.Point, dapiReq);
            if (result.IsSuccess == false)
            {
                throw new ZeroOneException(result.ReturnCode);
            }
            return result;
        }

        private async Task<LuoznzUrlModel?> GetUrlDomain()
        {
            LuoznzUrlModel? result =
                await MemoryCache.GetOrSetAsync(MemoryCacheKey.UrlDomain, getUrlDomain, 1000 * 60 * 10);

            return result;

            async Task<LuoznzUrlModel?> getUrlDomain()
            {
                return (await RestRequestHelper.Request(ZeroOneSettings.UrlDomain)
                       .Get(e => e.AddHeader("x-id", ZeroOneSettings.Xid))
                       .ResponseAsync<LuoznzUrlModel>())?.Result;
            }
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<string>> MediaUpload(ZOMediaUploadReq req)
        {
            var url = await GetUrlDomain();
            var domain = url?.MediaConvert?.FirstOrDefault();
            if (domain == null)
            {
                return new BaseReturnDataModel<string>(ReturnCode.ThirdPartyApi);
            }

            return await base.MediaUploadAsync(domain, ZeroOneApi.MediaUpload, req);
        }

        /// <inheritdoc/>
        public async Task<string> GetFullMediaUrl(string fileUrl)
        {
            var url = await GetUrlDomain();
            var extRaw = Path.GetExtension(fileUrl);
            var ext = extRaw.ToLower();
            if (ext == ".m3u8")
            {
                var uri = new Uri(new Uri(url?.VideoPro?.FirstOrDefault() ?? string.Empty), fileUrl);

                var t = new DateTimeOffset(DateTimeProvider.Now).ToUnixTimeSeconds();
                var sign = $"{ZeroOneSettings.M3U8Key}{fileUrl}{t}";
                using (var md5 = MD5.Create())
                {
                    sign = ToHexString(md5
                        .ComputeHash(Encoding.UTF8.GetBytes(sign)))
                        .ToLower();
                }

                return $"{uri.AbsoluteUri}?sign={sign}&t={t}";
            }
            else
            {
                var uri = new Uri(new Uri(url?.MediaConvert?.FirstOrDefault() ?? string.Empty), fileUrl);
                return await Task.FromResult(uri.AbsoluteUri);
            }
        }

        /// <inheritdoc/>
        public async Task NotifyVideoProcess(string path, string id, int orientation)
        {
            const string ExchangeKey = "shark.topic";
            const string RoutingKey = "backend.video.media_convert";
            const string QueueKey = "backend.video.media_convert.queue";
            var obj = new
            {
                path = path,
                model = "only-slice-video",
                screen_orientation = orientation,
                id = Convert.ToInt64(id),
            };
            _logger.LogError($"NotifyVideoProcess, param:{obj}");
            using (var bus = RabbitHutch.CreateBus(ZeroOneSettings.RabbitMqConnection))
            {
                if (!bus.Advanced.IsConnected)
                {
                    _logger.LogError($"NotifyVideoProcess Mq Disconnect and reconnect, param:{obj}");
                    await bus.Advanced.ConnectAsync();
                }
                //var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
                //await bus.Advanced.PublishAsync(new Exchange(ExchangeKey, "topic"), RoutingKey, false, new MessageProperties() { Expiration = TimeSpan.FromSeconds(5) }, bytes);
                await bus.SendReceive.SendAsync(QueueKey, obj);
            }
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<string>> MediaSplitUpload(ZOMediaUploadReq req)
        {
            var url = await GetUrlDomain();
            var domain = url?.MediaConvert?.FirstOrDefault();
            if (domain == null)
            {
                return new BaseReturnDataModel<string>(ReturnCode.ThirdPartyApi);
            }
            return await base.MediaSplitUpload(domain, ZeroOneApi.MediaUpload, req);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<string>> MediaMergeUpload(string[] paths, string suffix)
        {
            var url = await GetUrlDomain();
            var domain = url?.MediaConvert?.FirstOrDefault();
            if (domain == null)
            {
                return new BaseReturnDataModel<string>(ReturnCode.ThirdPartyApi);
            }

            return await base.MediaMergeUpload(domain, paths, suffix);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<VideoUrlModel>> GetUploadVideoUrl()
        {
            var url = await GetUrlDomain();
            var domain = url?.MediaConvert?.FirstOrDefault();
            if (domain == null)
            {
                return new BaseReturnDataModel<VideoUrlModel>(ReturnCode.ThirdPartyApi);
            }
            return await base.GetUploadVideoUrl(domain, ZeroOneApi.MediaUpload);
        }
    }

}