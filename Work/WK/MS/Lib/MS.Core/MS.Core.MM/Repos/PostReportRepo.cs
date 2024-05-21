using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Repos.interfaces;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class PostReportRepo : BaseInlodbRepository, IPostReportRepo
    {
        public PostReportRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<IEnumerable<MMPostReport>> QueryByFilter(PostReportFilter filter)
        {
            var comm = ReadDb.QueryTable<MMPostReport>();
            if(filter.PostTranIds.IsNotEmpty())
            {
                comm.Where(e => filter.PostTranIds.Contains(e.PostTranId));
            }
            if (filter.PostIds.IsNotEmpty())
            {
                comm.Where(e => filter.PostIds.Contains(e.PostId));
            }
            return await comm.QueryAsync();
        }
    }
}