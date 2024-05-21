using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Repos.interfaces;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class OfficialPostPriceRepo : BaseInlodbRepository<MMOfficialPostPrice>, IOfficialPostPriceRepo
    {
        public OfficialPostPriceRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<IEnumerable<MMOfficialPostPrice>> GetByPostId(string postId)
        {
            return await ReadDb.QueryTable<MMOfficialPostPrice>().Where(e => e.PostId == postId).QueryAsync();
        }

        public async Task<MMOfficialPostPrice> GetById(int id)
        {
            return await ReadDb.QueryTable<MMOfficialPostPrice>().Where(e => e.Id == id).QueryAsync().FirstOrDefaultAsync();
        }
    }
}