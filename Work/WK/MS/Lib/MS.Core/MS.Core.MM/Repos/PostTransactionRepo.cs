using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Exceptions;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Extensions;
using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MM.Models;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Repos.interfaces;
using MS.Core.Models;
using MS.Core.Repos;

namespace MS.Core.MM.Repos
{
    public class PostTransactionRepo : BaseInlodbRepository<MMPostTransactionModel>, IPostTransactionRepo
    {
        public PostTransactionRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger, IUserSummaryRepo userSummaryRepo) : base(setting, provider, logger)
        {
            UserSummaryRepo = userSummaryRepo;
        }

        private IUserSummaryRepo UserSummaryRepo { get; }

        /// <summary>
        /// 解鎖贴子
        /// </summary>
        /// <param name="postTrans">解鎖贴子</param>
        /// <param name="income">收益人交易紀錄</param>
        /// <param name="expense">消費紀錄</param>
        /// <returns></returns>
        public async Task UnlockPost(UnlockPostInfoModel unlockPost)
        {
            DapperComponent dapperComponent = UnlockPostDC(unlockPost);
            try
            {
                await dapperComponent.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //支出單號 失敗
                throw new MSSqlException(ReturnCode.RunSQLFail, $"UnlockPost:{unlockPost.Expense.Id},{ex.Message}");
            }
        }

        /// <summary>
        /// 使用點數解鎖贴子
        /// </summary>
        /// <param name="postTrans"></param>
        /// <param name="income"></param>
        /// <param name="expense"></param>
        /// <param name="userPointExpense"></param>
        /// <returns></returns>
        public async Task UseUserPointUnlockPost(UseUserPointUnlockPostModel unlockPost)
        {
            IncrementUserSummaryModel freeUnlock = new()
            {
                Type = UserSummaryTypeEnum.FreeUnlock,
                UserId = unlockPost.Expense.UserId,
                Amount = 1,
                Category = unlockPost.UserPointExpense.VipType.ConvertToUserSummaryCategory(),
            };

            DapperComponent db = UnlockPostDC(unlockPost);

            UserSummaryRepo.IncrementUserSummary(db, freeUnlock);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new MSSqlException(ReturnCode.SystemError, $"解锁贴子失败,{ex.Message}");
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<QueryUserPostUnlockCount>> PostUnlockCountByUser(int userId)
        {
            string querySql = @"
SELECT [PostType]
	,COUNT(1) AS Count
FROM [Inlodb].[dbo].[MMPostTransaction] WITH(NOLOCK)
WHERE PostUserId = @UserId
GROUP BY [PostType]";

            return await ReadDb.QueryAsync<QueryUserPostUnlockCount>(querySql, new { userId });
        }

        /// <summary>
        /// 取得使用者已購買的贴子
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetExpensePosts(int userId)
        {
            return await ReadDb.QueryTable<MMPostTransactionModel>().Where(e => e.UserId == userId).OrderByDescending(c=>c.CreateTime).SelectQueryAsync(e => e.PostId);
        }

        public async Task<MMPostTransactionModel?> GetUserPostTran(int userId, string postId)
        {
            return await WriteDb.QueryTable<MMPostTransactionModel>()
                .Where(e => e.UserId == userId && e.PostId == postId)
                .QueryAsync()
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 使用者是否購買贴子
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<bool> IsUserBuyPost(int userId, string postId)
        {
            return await ReadDb.QueryTable<MMPostTransactionModel>()
                .Where(e => e.UserId == userId)
                .Where(e => e.PostId == postId)
                .IsExistsAsync();
        }

        /// <inheritdoc/>
        public async Task<int> GetUnlockCount(int userId)
        {
            return await ReadDb.QueryTable<MMPostTransactionModel>()
                .Where(e => e.UserId == userId)
                .CountAsync();
        }
        public async Task<IEnumerable<string>> GetUnlockPostId(int userId)
        {
            return (await ReadDb.QueryTable<MMPostTransactionModel>()
              .Where(e => e.UserId == userId).QueryAsync()).Select(c => c.PostId);
        }

        /// <inheritdoc/>
        public async Task<MMPostTransactionModel[]> List(string[] unlockIds, int idType)
        {
            var component = ReadDb.QueryTable<MMPostTransactionModel>();
            switch (idType)
            {
                case 0:
                    component.Where(x => unlockIds.Contains(x.Id));
                    break;

                case 1:
                    component.Where(x => unlockIds.Contains(x.PostId));
                    break;

                //case 2:
                //    component.Where(x => unlockIds.Contains(x.ReportId));
                //    break;

                default:
                    return new MMPostTransactionModel[0];
            }
            return await component.QueryAsync()
                .ToArrayAsync();
        }

        private DapperComponent UnlockPostDC(UnlockPostInfoModel unlockPost)
        {
            IncrementUserSummaryModel userQuantity = new()
            {
                Type = UserSummaryTypeEnum.UnLock,
                UserId = unlockPost.Expense.UserId,
                Amount = 1,
                Category = unlockPost.PostTransaction.PostType.ConvertToUserSummaryCategory(),
            };

            IncrementUserSummaryModel unlocked = new()
            {
                Type = UserSummaryTypeEnum.Unlocked,
                UserId = unlockPost.Income.UserId,
                Amount = 1,
                Category = unlockPost.PostTransaction.PostType.ConvertToUserSummaryCategory(),
            };

            UserSummaryRepo.IncrementUserSummary(WriteDb, userQuantity);
            UserSummaryRepo.IncrementUserSummary(WriteDb, unlocked);

            string sql = @"
UPDATE MMUserInfo
SET RewardsPoint = RewardsPoint + 20
WHERE UserId = @UserId";

            string addPostUnlockSql = @"
                UPDATE MMPost
                SET UnlockCount = UnlockCount + 1
                WHERE PostId = @PostId; ";

            DapperComponent dapperComponent =
                WriteDb
                .Insert(unlockPost.PostTransaction)
                .Insert(unlockPost.Income)
                .Insert(unlockPost.Expense)
                .AddExecuteSQL(sql, new { unlockPost.Income.UserId })
                .AddExecuteSQL(addPostUnlockSql, new { unlockPost.PostTransaction.PostId });

            return dapperComponent;
        }
    }
}