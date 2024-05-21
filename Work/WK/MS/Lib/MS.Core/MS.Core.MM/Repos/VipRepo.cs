using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class VipRepo : BaseInlodbRepository<MMVip>, IVipRepo
    {
        public VipRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<MMVip?> GetVipAsync(int vipId)
        {
            return await ReadDb.QueryTable<MMVip>().Where(e => e.Status == VipStatus.Listed && e.Id == vipId).QueryAsync().FirstOrDefaultAsync();
        }

        public async Task<MMVip[]> GetVipsAsync()
        {
            return await ReadDb.QueryTable<MMVip>().QueryAsync().ToArrayAsync();
        }

        public async Task<MMVip[]> GetListedVipsAsync()
        {
            return await ReadDb.QueryTable<MMVip>().Where(e => e.Status == VipStatus.Listed).QueryAsync().ToArrayAsync();
        }
    }
}
