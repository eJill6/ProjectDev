using ImageMagick;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class VIPWelfareRepo : BaseInlodbRepository<MMVipWelfare>, IVIPWelfareRepo
    {
        public VIPWelfareRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task<IEnumerable<MMVipWelfare>> GetVipWelfares(IEnumerable<VipType> vipTypes, VIPWelfareTypeEnum type)
        {
            DapperQueryComponent<MMVipWelfare> dapper = GetByFilter(new VipWelfareFilter
            {
                Type = type,
                VipTypes = vipTypes,
            });
            return await dapper.QueryAsync();
        }

        public async Task<IEnumerable<MMVipWelfare>> GetVipWelfaresByFilter(VipWelfareFilter filter)
        {
            DapperQueryComponent<MMVipWelfare> dapper = GetByFilter(filter);

            return await dapper.QueryAsync();
        }

        private DapperQueryComponent<MMVipWelfare> GetByFilter(VipWelfareFilter filter)
        {
            DapperQueryComponent<MMVipWelfare> dapper = ReadDb.QueryTable<MMVipWelfare>();

            if (filter.VipTypes.IsNotEmpty())
            {
                dapper.Where(e => filter.VipTypes.Contains(e.VipType));
            }

            if (filter.Type.HasValue)
            {
                dapper.Where(e => e.Type == filter.Type);
            }

            return dapper;
        }
    }
}
