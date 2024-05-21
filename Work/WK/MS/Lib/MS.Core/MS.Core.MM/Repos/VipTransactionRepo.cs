using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Models.Models;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class BuyVipCardModel
    {
        /// <summary>
        /// Vip 購買紀錄
        /// </summary>
        public MMVipTransaction VipTransaction { get; set; } = null!;
        /// <summary>
        /// 支出交易紀錄
        /// </summary>
        public MMIncomeExpenseModel Expense { get; set; } = null!;
    }
    public class VipTransactionRepo : BaseInlodbRepository<MMVipTransaction>, IVipTransactionRepo
    {
        public VipTransactionRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public async Task BuyVipCard(BuyVipCardModel buyVipCard)
        {
            await WriteDb
                .Insert(buyVipCard.VipTransaction)
                .Insert(buyVipCard.Expense)
                .SaveChangesAsync();
        }

        public async Task<int> GetUserVipLogCount(int userId, VipType vipType)
        {
            return await ReadDb.QueryTable<MMVipTransaction>()
                .Where(e => e.UserId == userId)
                .Where(e => e.Type == vipType)
                .CountAsync();
        }

        public async Task<IEnumerable<MMVipTransaction>> GetUserVipLogs(int userId)
        {
            return await ReadDb.QueryTable<MMVipTransaction>()
                .Where(e => e.UserId == userId)
                .QueryAsync();
        }

        public async Task<IEnumerable<int>> GetVipUserIds(IEnumerable<int> userIds)
        {
            DateTime time = DateTimeExtension.GetCurrentTime();
            return await ReadDb.QueryTable<MMVipTransaction>()
                .Where(e => userIds.Contains(e.UserId))
                .Where(e => e.EffectiveTime >= time)
                .SelectQueryAsync(e => new { e.UserId }).SelectAsync(e => e.UserId);
        }

        public async Task<MMVipTransaction[]> GetUserEfficientVipsByType(int vipId)
        {
            DateTime time = DateTimeExtension.GetCurrentTime();
            return (await ReadDb.QueryTable<MMVipTransaction>()
                .Where(e => e.VipId == vipId)
                .Where(e => e.EffectiveTime >= time)
                .QueryAsync()).ToArray();
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMVipTransaction>> GetVips(AdminUserManagerUserCardsListParam param)
        {
            var component = ReadDb.QueryTable<MMVipTransaction>();
            if (!string.IsNullOrEmpty(param.Id))
            {
                component.Where(e => e.Id == param.Id);
            }

            if (param.UserId.HasValue)
            {
                component.Where(e => e.UserId == param.UserId);
            }

            if (param.VipId.HasValue)
            {
                component.Where(e => e.VipId == param.VipId);
            }

            if (param.PayType.HasValue)
            {
                component.Where(e => e.PayType == param.PayType);
            }

            var begin = param.BeginDate;
            var end = param.EndDate;
            return await component.Where(x => x.CreateTime >= begin && x.CreateTime < end)
                .OrderByDescending(x => x.CreateTime)
                .QueryPageResultAsync(param);
        }
    }
}
