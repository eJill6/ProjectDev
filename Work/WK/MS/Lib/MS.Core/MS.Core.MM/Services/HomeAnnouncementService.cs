using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructure.Redis;
using MS.Core.MM.Models;
using MS.Core.MM.Models.Entities.HomeAnnouncement;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Service;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Services
{
    public class HomeAnnouncementService : BaseService, IHomeAnnouncementService
    {
        /// <inheritdoc cref="IEnumerable{IMediaService}"/>
        private readonly IEnumerable<IMediaService> _medias = null;

        /// <inheritdoc cref="IAdvertisingContentRepo"/>
        private readonly IHomeAnnouncementRepo _repo = null;

        /// <inheritdoc cref="IRedisService"/>
        private readonly IRedisService _cache;

        /// <inheritdoc />
        public HomeAnnouncementService(IEnumerable<IMediaService> medias,
            IHomeAnnouncementRepo repo,
            IRedisService cache,
            ILogger<BannerService> logger) : base(logger)
        {
            _medias = medias;
            _repo = repo;
            _cache = cache;
        }

        public async Task<BaseReturnDataModel<IEnumerable<MMHomeAnnouncement>>> Get()
        {
            return await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<IEnumerable<MMHomeAnnouncement>>();
                result.DataModel = await _repo.Get();
                result.SetCode(ReturnCode.Success);
                return result;
            }, String.Empty);
        }

        public async Task<BaseReturnModel> Update(MMHomeAnnouncement param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var repo = await _repo.Update(param);
                BaseReturnModel result = new BaseReturnModel();
                result.SetCode(new ReturnCode(repo.Code));
                return result;
            }, param);
        }

        public async Task<BaseReturnModel> Create(MMHomeAnnouncement param)
        {
            if (await _repo.HasDuplicateSort(param))
            {
                return new BaseReturnModel(MMReturnCode.WeightIsUsed);
            }
            return await TryCatchProcedure(async (param) =>
            {
                var repo = await _repo.Create(param);
                BaseReturnModel result = new BaseReturnModel();
                result.SetCode(new ReturnCode(repo.Code));
                return result;
            }, param);
        }

        /// <inheritdoc />
        public async Task<BaseReturnModel> Delete(int id)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var homeAnnouncement = await _repo.Get().WhereAsync(a => a.Id == id);
                if (homeAnnouncement == null)
                {
                    return new BaseReturnModel(ReturnCode.DataIsNotExist);
                }
                await _repo.Delete(id);

                return new BaseReturnModel(ReturnCode.Success);
            }, id);
        }
    }
}