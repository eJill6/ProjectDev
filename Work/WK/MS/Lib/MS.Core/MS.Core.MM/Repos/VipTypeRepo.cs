using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Repos.interfaces;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class VipTypeRepo : BaseInlodbRepository<MMVipType>, IVipTypeRepo
    {
        public VipTypeRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<MMVipType[]> GetAll()
        {
            return await ReadDb.QueryTable<MMVipType>().QueryAsync().ToArrayAsync();
        }
    }
}
