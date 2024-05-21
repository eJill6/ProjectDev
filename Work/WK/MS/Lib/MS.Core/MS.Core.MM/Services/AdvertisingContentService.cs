using Microsoft.Extensions.Logging;
using MS.Core.Infrastructure.Redis;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.Models;
using MS.Core.Services;

namespace MS.Core.MM.Service
{
    /// <inheritdoc cref="IBannerService"/>
    public class AdvertisingContentService : BaseService, IAdvertisingContentService
    {
        /// <inheritdoc cref="IEnumerable{IMediaService}"/>
        private readonly IEnumerable<IMediaService> _medias = null;

        /// <inheritdoc cref="IAdvertisingContentRepo"/>
        private readonly IAdvertisingContentRepo _repo = null;

        /// <inheritdoc cref="IRedisService"/>
        private readonly IRedisService _cache;

        /// <inheritdoc />
        public AdvertisingContentService(IEnumerable<IMediaService> medias,
            IAdvertisingContentRepo repo,
            IRedisService cache,
            ILogger<BannerService> logger) : base(logger)
        {
            _medias = medias;
            _repo = repo;
            _cache = cache;
        }
        /// <inheritdoc />
        public async Task<BaseReturnModel> Delete(int id)
        {
            return await TryCatchProcedure<int, BaseReturnModel>(async (param) =>
            {
                await _repo.Delete(id);
                return new BaseReturnModel(ReturnCode.Success);
            }, id);
        }

        /// <inheritdoc />
        public async Task<BaseReturnModel> Update(MMAdvertisingContent param)
        {
            return await TryCatchProcedure<MMAdvertisingContent, BaseReturnModel>(async (param) =>
            {
                if (!(await _repo.Update(param)))
                {
                    return new BaseReturnModel(ReturnCode.UpdateFailed);
                }
                return new BaseReturnModel(ReturnCode.Success);
            }, param);
        }


        /// <inheritdoc />
        public async Task<BaseReturnDataModel<MMAdvertisingContent[]>> Get()
        {
            return await TryCatchProcedure<object, BaseReturnDataModel<MMAdvertisingContent[]>>(async (param) =>
            {
                var result = new BaseReturnDataModel<MMAdvertisingContent[]>();
                result.DataModel = await _repo.Get();
                result.SetCode(ReturnCode.Success);
                return result;
            }, null);
        }

        /// <inheritdoc />
        public async Task<BaseReturnDataModel<MMAdvertisingContent>> Get(int id)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<MMAdvertisingContent>();
                result.DataModel = await _repo.Get(id);
                result.SetCode(ReturnCode.Success);
                return result;
            }, id);
        }
    }
}