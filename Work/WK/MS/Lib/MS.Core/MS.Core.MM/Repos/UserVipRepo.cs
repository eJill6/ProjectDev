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
    public class UserVipRepo : BaseInlodbRepository<MMUserVip>, IUserVipRepo
    {
        public UserVipRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger, IDateTimeProvider dateTimeProvider) : base(setting, provider, logger)
        {
            DateTimeProvider = dateTimeProvider;
        }
        IDateTimeProvider DateTimeProvider { get; }
        public async Task<MMUserVip[]> GetUserVips(int userId)
        {
            DateTime now = DateTimeProvider.Now;
            return await ReadDb.QueryTable<MMUserVip>()
                .Where(e => e.UserId == userId)
                .Where(e => e.EffectiveTime > now.AddDays(-e.ExtendDay))
                .QueryAsync()
                .ToArrayAsync();
        }

        public async Task<MMUserVip[]> GetUserVips(IEnumerable<int> userIds)
        {
            DateTime now = DateTimeProvider.Now;
            return await ReadDb.QueryTable<MMUserVip>()
                .Where(e => userIds.Contains(e.UserId))
                .Where(e => e.EffectiveTime > now.AddDays(-e.ExtendDay))
                .QueryAsync()
                .ToArrayAsync();
        }

        public async Task<MMUserVip[]> GetUserEfficientVipsByType(VipType vipId)
        {
            DateTime time = DateTimeProvider.Now;
            return (await ReadDb.QueryTable<MMUserVip>()
                .Where(e => e.VipType == vipId)
                .Where(e => e.EffectiveTime >= time)
                .QueryAsync()).ToArray();
        }
    }
}
