using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.Models;

namespace MS.Core.Repos
{
    public abstract class BaseInlodbRepository : BaseRepository
    {
        protected BaseInlodbRepository(
            IOptionsMonitor<MsSqlConnections> setting, 
            IRequestIdentifierProvider provider, 
            ILogger logger) : base(setting, "InloDb_Read", "InloDb_Write", provider, logger)
        {
        }
    }

    public abstract class BaseInlodbRepository<T> : BaseInlodbRepository where T : BaseDBModel
    {
        protected BaseInlodbRepository(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<string> GetSequenceIdentity()
        {
            return await GetSequenceIdentity<T>();
        }
    }
}