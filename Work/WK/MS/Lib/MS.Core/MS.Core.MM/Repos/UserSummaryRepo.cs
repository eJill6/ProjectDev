using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Repos.interfaces;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class UserSummaryRepo : BaseInlodbRepository<MMUserSummary>, IUserSummaryRepo
    {
        public UserSummaryRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger) : base(setting, provider, logger)
        {
        }

        public DapperComponent IncrementUserSummary(DapperComponent writeDb, IncrementUserSummaryModel entity)
        {
            string sql = @"
IF EXISTS (SELECT (1) FROM [MMUserSummary] WHERE [UserId] = @UserId AND [Type] = @Type AND [Category] = @Category)
  UPDATE [MMUserSummary] SET [Amount] = [Amount] + @Amount WHERE [UserId] = @UserId AND [Type] = @Type AND [Category] = @Category
ELSE
  INSERT INTO [MMUserSummary] ([UserId],[Category], [Type], [Amount]) VALUES (@UserId, @Category, @Type, @Amount)
";
            return writeDb.AddExecuteSQL(sql, new 
            {
                entity.UserId, 
                Type = (byte)entity.Type, 
                Category = (byte)entity.Category,
                entity.Amount
            });
        }

        public DapperComponent IncrementUserSummary(DapperComponent writeDb, IEnumerable<IncrementUserSummaryModel> entities)
        {
            string sql = @"
IF EXISTS (SELECT (1) FROM [MMUserSummary] WHERE [UserId] = @UserId AND [Type] = @Type AND [Category] = @Category)
  UPDATE [MMUserSummary] SET [Amount] = [Amount] + @Amount WHERE [UserId] = @UserId AND [Type] = @Type AND [Category] = @Category
ELSE
  INSERT INTO [MMUserSummary] ([UserId],[Category], [Type], [Amount]) VALUES (@UserId, @Category, @Type, @Amount)
";
            var param = entities.Select(entity => new
            {
                entity.UserId,
                Type = (byte)entity.Type,
                Category = (byte)entity.Category,
                entity.Amount
            });
            return writeDb.AddExecuteSQL(sql, param);
        }

        public async Task IncrementUserQuantity(IncrementUserSummaryModel entity)
        {
            await IncrementUserSummary(WriteDb, entity).SaveChangesAsync();
        }

        public async Task<IEnumerable<MMUserSummary>> GetUserSummaries(IEnumerable<int> userIds)
        {
            return await ReadDb.QueryTable<MMUserSummary>().Where(e => userIds.Contains(e.UserId)).QueryAsync().ToArrayAsync();
        }

        public async Task<decimal> GetUserAmount(int userId, UserSummaryTypeEnum type, UserSummaryCategoryEnum category)
        {
            var query =
                await ReadDb.QueryTable<MMUserSummary>()
                .Where(e => e.UserId == userId)
                .Where(e => e.Type == type)
                .Where(e => e.Category == category)
                .SelectQueryAsync(e => new { e.Amount })
                .FirstOrDefaultAsync();
            return query?.Amount ?? 0;
        }

        public async Task RestSetAmount(UserSummaryTypeEnum type, decimal amount, DateTime time)
        {
            var sql = @"
UPDATE [MMUserSummary]
SET Amount = @Amount, RestSetTime = @Time
WHERE Type = @Type";

            await WriteDb
                .AddExecuteSQL(sql, new { Type = type, Amount = amount, Time = time })
                .SaveChangesAsync();
        }
    }
}