using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Model.Banner;
using MS.Core.MM.Model.Entities.Banner;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Repos.interfaces;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class BannerRepo : BaseInlodbRepository<MMBanner>, IBannerRepo
    {
        /// <summary>
        /// 媒體相關repository
        /// </summary>
        private readonly IMediaRepo _media = null;

        public BannerRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger, IMediaRepo media) : base(setting, provider, logger)
        {
            _media = media;
        }

        /// <inheritdoc/>
        public async Task<bool> Create(SaveBannerParam param)
        {
            await _media.Create(WriteDb, param.Media).Insert<MMBanner>(param).SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public async Task<string> CreateNewSEQID()
        {
            return await GetSequenceIdentity("SEQ_MMBanner_Id");
        }

        /// <inheritdoc/>
        public async Task Delete(string seqId, string[] strings)
        {

            await _media.Delete(WriteDb, strings).Delete(new MMBanner()
            {
                Id = seqId
            }).SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<MMBanner[]> Get()
        {
            return (await ReadDb.QueryTable<MMBanner>()
                .QueryAsync()).ToArray();
        }

        /// <inheritdoc/>
        public async Task<MMBanner> Get(string id)
        {
            return (await ReadDb.QueryTable<MMBanner>().Where(x => x.Id == id).QueryAsync()).FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<bool> HasDuplicateSort(SaveBannerParam param)
        {
            var query = (await ReadDb.QueryTable<MMBanner>()
                .Where(x => x.Sort == param.Sort && x.Type == param.Type)
                .QueryAsync()).FirstOrDefault();
            if (query != null && query.Id != param.Id)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> Update(SaveBannerParam param)
        {
            await _media.Update(WriteDb, param.Media).Update<MMBanner>(param).SaveChangesAsync();
            return true;
        }
    }
}
