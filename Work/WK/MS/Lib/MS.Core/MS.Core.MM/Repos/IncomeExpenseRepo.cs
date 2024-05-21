using Amazon.S3.Model;
using Dapper;
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
using MS.Core.MM.Models.Booking;
using MS.Core.MM.Models.Booking.Enums;
using MS.Core.MM.Models.Entities.IncomeExpense;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.Post.ServiceReq;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.AdminPostTransaction;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.My;
using MS.Core.MMModel.Models.OperationOverview;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using MS.Core.Repos;
using Newtonsoft.Json;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace MS.Core.MM.Repos
{
    public class IncomeExpenseRepo : BaseInlodbRepository<MMIncomeExpenseModel>, IIncomeExpenseRepo
    {
        private static List<IncomeExpenseStatusEnum> DispatchedStatus;

        private IUserSummaryRepo UserSummaryRepo { get; }

        private IBookingRepo BookingRepo { get; }

        public IncomeExpenseRepo(IOptionsMonitor<MsSqlConnections> setting,
            IRequestIdentifierProvider provider,
            ILogger logger,
            IUserSummaryRepo userSummaryRepo,
            IBookingRepo bookingRepo
            )
            : base(setting, provider, logger)
        {
            UserSummaryRepo = userSummaryRepo;
            BookingRepo = bookingRepo;
        }

        static IncomeExpenseRepo()
        {
            DispatchedStatus = new List<IncomeExpenseStatusEnum>
            {
                IncomeExpenseStatusEnum.Approved,
                IncomeExpenseStatusEnum.Dispatched
            };
        }

        /// <inheritdoc/>
        public async Task<decimal> GetMonthIncome(int userId, DateTime now)
        {
            // 計算本月月初日期
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

            // 計算本月月底日期
            DateTime lastDayOfMonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59);

            var data = await ReadDb.QueryTable<MMIncomeExpenseModel>()
                  .Where(x => x.UserId == userId && x.DistributeTime >= firstDayOfMonth && x.DistributeTime < lastDayOfMonth)
                  .Where(x => x.TransactionType == IncomeExpenseTransactionTypeEnum.Income)
                  .Where(x => x.PayType == IncomeExpensePayType.Amount)
                  .Where(x => x.Status == IncomeExpenseStatusEnum.Dispatched || x.Status == IncomeExpenseStatusEnum.Approved).QueryAsync();

            decimal sum = 0;
            foreach (var item in data)
            {
                sum += Math.Floor(item.Amount * item.Rebate);
            }

            return sum;
        }

        public async Task<MMIncomeExpenseModel> GetBySourceId(string sourceId)
        {
            return await ReadDb.QueryTable<MMIncomeExpenseModel>().Where(x => x.SourceId == sourceId).QueryFirstAsync();
        }

        /// <inheritdoc/>
        public async Task<decimal> GetFreezeIncome(int userId)
        {
            return await ReadDb.QueryTable<MMIncomeExpenseModel>()
                .Where(x => x.UserId == userId)
                .Where(x => x.Status == IncomeExpenseStatusEnum.UnDispatched || x.Status == IncomeExpenseStatusEnum.Unusual)
                .Where(x => x.TransactionType == IncomeExpenseTransactionTypeEnum.Income)
                .SumAsync<decimal>(x => x.Amount * x.Rebate);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MMIncomeExpenseModel>> GetTransactionByFilter(IncomeExpenseFilter filter)
        {
            DapperQueryComponent<MMIncomeExpenseModel> queryComponent = GetByFilter(filter);

            return await queryComponent.QueryAsync();
        }

        public async Task<IEnumerable<MMIncomeExpenseModel>> GetTransactionByFilterOrderByDescending(IncomeExpenseFilter filter)
        {
            DapperQueryComponent<MMIncomeExpenseModel> queryComponent = GetByFilter(filter);

            return await queryComponent.OrderByDescending(e => e.CreateTime).QueryAsync();
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMIncomeExpenseModel>> GetPageTransactionByFilter(PageIncomeExpenseFilter filter)
        {
            DapperQueryComponent<MMIncomeExpenseModel> queryComponent = GetByFilter(filter);

            return await queryComponent.OrderByDescending(e => e.CreateTime).QueryPageResultAsync(filter.Pagination);
        }
        public async Task<PageResultModel<AdminUserManagerOfficialIncomeExpensesList>> GetPageIncomeExpenseData(QueryIncomeExpensePageParamter paramter)
        {
            int startRowNum = (paramter.Page <= 1) ? 1 : 1 + (paramter.Page - 1) * paramter.PageSize;
            int endRowNum = (startRowNum - 1) + paramter.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@Id", paramter.Id, DbType.String);
            parameters.Add("@UserId", paramter.UserId, DbType.Int32);
            parameters.Add("@PostType", paramter.PostType.HasValue ? (int)paramter.PostType : 0, DbType.Int32);
            parameters.Add("@BeginDate", paramter.BeginDate, DbType.DateTime);
            parameters.Add("@EndDate", paramter.EndDate, DbType.DateTime);
            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            string querySql = @" DROP TABLE IF EXISTS #TMP_MMIncomeExpense ";
            querySql += @"
                        SELECT 
	                         ME.[Id]
	                        ,ME.[TransactionType]
	                        ,ME.[Category]
	                        ,ME.[SourceId]
	                        ,ME.[TargetId]
	                        ,ME.[UserId]
	                        ,ME.[Title]
	                        ,ME.[Memo]
	                        ,ME.[Amount]
	                        ,ME.[Status]
	                        ,ME.[CreateTime]
	                        ,ME.[DistributeTime]
	                        ,ME.[PayType]
	                        ,ME.[Rebate]
	                        ,ME.[UnusualMemo]
	                        ,ME.[UpdateTime]
	                        ,ISNULL(MB.[PaymentType],0) AS PaymentType
                        INTO #TMP_MMIncomeExpense
                        FROM [dbo].[MMIncomeExpense] AS ME WITH(NOLOCK)
                        LEFT JOIN [dbo].[MMBooking] AS MB WITH(NOLOCK) ON ME.SourceId=MB.BookingId  WHERE 1=1 ";



            if (paramter.Category.HasValue)
            {
                switch (paramter.Category.Value)
                {
                    case AdminIncomeExpensesCategory.PostUnLock:
                        //帖子解锁
                        querySql += "AND ME.[TransactionType]=1 AND ME.[Category]!=3";
                        break;
                    case AdminIncomeExpensesCategory.PostIncome:
                        //帖子收益
                        querySql += "AND ME.[TransactionType]=2 ";
                        break;
                    case AdminIncomeExpensesCategory.UnLockRefund:
                        //帖子解锁退款
                        querySql += "AND ME.[TransactionType]=3 AND ME.[Category]!=3";
                        break;
                }
            }

            if(paramter.PostType.HasValue)
            {
                querySql += " AND ME.[Category]=@PostType ";
            }


            if (paramter.UserId.HasValue)
            {
                querySql += " AND ME.[UserId]=@UserId ";
            }
            if (paramter.BeginDate.HasValue)
            {
                // queryComponent = queryComponent.Where(e => e.CreateTime >= filter.StartTime);

                querySql += " AND ME.[CreateTime]>=@BeginDate ";
            }

            if (paramter.EndDate.HasValue)
            {
                //queryComponent = queryComponent.Where(e => e.CreateTime < filter.EndTime);
                querySql += " AND ME.[CreateTime]<@EndDate ";
            }
            if (!string.IsNullOrEmpty(paramter.Id))
            {
                querySql += " AND ME.[Id]=@Id ";
            }
            querySql += @"

                SET @TotalCount=(SELECT COUNT(*) FROM #TMP_MMIncomeExpense);
                 --分页
                SELECT
                    *
                FROM(
                    SELECT *,ROW_NUMBER() OVER(ORDER BY [CreateTime] DESC) AS RowNumber
                    FROM  #TMP_MMIncomeExpense
                ) AS T
                WHERE T.RowNumber BETWEEN @StartNo AND @EndNo
            ";

            var result = await ReadDb.QueryAsync<AdminUserManagerOfficialIncomeExpensesList>(querySql, parameters);
            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / paramter.PageSize);

            return new PageResultModel<AdminUserManagerOfficialIncomeExpensesList>
            {
                Page = paramter.Page,
                PageSize = paramter.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };

        }
        public async Task<PageResultModel<AdminUserManagerOfficialIncomeExpensesList>> GetPageOfficialIncomeExpenseData(QueryIncomeExpensePageParamter paramter)
        {
            int startRowNum = (paramter.Page <= 1) ? 1 : 1 + (paramter.Page - 1) * paramter.PageSize;
            int endRowNum = (startRowNum - 1) + paramter.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@Id", paramter.Id, DbType.String);
            parameters.Add("@UserId", paramter.UserId, DbType.Int32);
            parameters.Add("@PostType", paramter.PostType.HasValue ? (int)paramter.PostType : 0, DbType.Int32);
            parameters.Add("@BeginDate", paramter.BeginDate, DbType.DateTime);
            parameters.Add("@EndDate", paramter.EndDate, DbType.DateTime);

            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);


            string querySql = @" DROP TABLE IF EXISTS #TMP_MMIncomeExpense ";
            querySql += @"
                        SELECT 
	                         ME.[Id]
	                        ,ME.[TransactionType]
	                        ,ME.[Category]
	                        ,ME.[SourceId]
	                        ,ME.[TargetId]
	                        ,ME.[UserId]
	                        ,ME.[Title]
	                        ,ME.[Memo]
	                        ,ME.[Amount]
	                        ,ME.[Status]
	                        ,ME.[CreateTime]
	                        ,ME.[DistributeTime]
	                        ,ME.[PayType]
	                        ,ME.[Rebate]
	                        ,ME.[UnusualMemo]
	                        ,ME.[UpdateTime]
	                        ,MB.[PaymentType] AS PaymentType
                        INTO #TMP_MMIncomeExpense
                        FROM [dbo].[MMIncomeExpense] AS ME WITH(NOLOCK)
                        INNER JOIN [dbo].[MMBooking] AS MB WITH(NOLOCK) ON ME.SourceId=MB.BookingId  WHERE 1=1 ";

            if (paramter.Category.HasValue)
            {
                switch (paramter.Category.Value)
                {
                    case AdminIncomeExpensesCategory.Booking:
                        //支付预约金
                        querySql += "AND ME.[TransactionType]=1 AND MB.[PaymentType]=1 AND ME.[Category]=3";
                        break;
                    case AdminIncomeExpensesCategory.UnBooking:
                        //退回预约金
                        querySql += "AND ME.[TransactionType]=3 AND MB.[PaymentType]=1 AND ME.[Category]=3";
                        break;
                    case AdminIncomeExpensesCategory.PayInFull:
                        //支付全额
                        querySql += "AND ME.[TransactionType]=1 AND MB.[PaymentType]=2 AND ME.[Category]=3";
                        break;
                    case AdminIncomeExpensesCategory.FullRefund:
                        //退回全额
                        querySql += "AND ME.[TransactionType]=3 AND MB.[PaymentType]=2 AND ME.[Category]=3";
                        break;
                    case AdminIncomeExpensesCategory.PostIncome:
                        //帖子收益
                        querySql += "AND ME.[TransactionType]=2  ";
                        break;
                }
            }

            //if (paramter.PostType.HasValue)
            //{
            //    querySql += " AND ME.[Category]=@PostType ";
            //}

            if (paramter.UserId.HasValue)
            {
                querySql += " AND ME.[UserId]=@UserId ";
            }
            if (paramter.BeginDate.HasValue)
            {
                // queryComponent = queryComponent.Where(e => e.CreateTime >= filter.StartTime);

                querySql += " AND ME.[CreateTime]>=@BeginDate ";
            }

            if (paramter.EndDate.HasValue)
            {
                //queryComponent = queryComponent.Where(e => e.CreateTime < filter.EndTime);
                querySql += " AND ME.[CreateTime]<@EndDate ";
            }
            if (!string.IsNullOrEmpty(paramter.Id))
            {
                querySql += " AND ME.[Id]=@Id ";
            }
            querySql += @"

                SET @TotalCount=(SELECT COUNT(*) FROM #TMP_MMIncomeExpense);
                 --分页
                SELECT
                    *
                FROM(
                    SELECT *,ROW_NUMBER() OVER(ORDER BY [CreateTime] DESC) AS RowNumber
                    FROM  #TMP_MMIncomeExpense
                ) AS T
                WHERE T.RowNumber BETWEEN @StartNo AND @EndNo
            ";

            var result = await ReadDb.QueryAsync<AdminUserManagerOfficialIncomeExpensesList>(querySql, parameters);
            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / paramter.PageSize);

            return new PageResultModel<AdminUserManagerOfficialIncomeExpensesList>
            {
                Page = paramter.Page,
                PageSize = paramter.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }

        /// <inheritdoc/>
        public async Task<decimal> GetSumTransactionByFilter(IncomeExpenseFilter filter)
        {
            DapperQueryComponent<MMIncomeExpenseModel> queryComponent = GetByFilter(filter);

            var list = await queryComponent.QueryAsync();
            var expenseAmount = list.Where(e => e.TransactionType == IncomeExpenseTransactionTypeEnum.Expense && e.Status == IncomeExpenseStatusEnum.Approved).Sum(e => e.Amount * e.Rebate);
            var incomeAmount = list.Where(e => e.TransactionType == IncomeExpenseTransactionTypeEnum.Refund && e.Status == IncomeExpenseStatusEnum.Refund).Sum(e => e.Amount * e.Rebate);
            return expenseAmount - incomeAmount;
        }

        public async Task<decimal> GetSumGetIncomeByFilter(IncomeExpenseFilter filter)
        {
            DapperQueryComponent<MMIncomeExpenseModel> queryComponent = GetByFilter(filter);

            var list = await queryComponent.QueryAsync();

            var data = list.Where(x => x.PayType == IncomeExpensePayType.Amount)
                .Where(x => x.Status == IncomeExpenseStatusEnum.Dispatched || x.Status == IncomeExpenseStatusEnum.Approved);

            decimal sum = 0;
            foreach (var item in data)
            {
                sum += Math.Floor(item.Amount * item.Rebate);
            }

            return sum;
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMIncomeExpenseModel>> List(AdminPostTransactionListParam param)
        {
            var component = ReadDb.QueryTable<MMIncomeExpenseModel>();
            var beginDate = param.BeginDate;
            var endDate = param.EndDate;
            component.Where(x => x.CreateTime >= beginDate && x.CreateTime < endDate);

            if (!string.IsNullOrWhiteSpace(param.Id))
            {
                component.Where(x => x.SourceId == param.Id);
            }
            if (param.PostType.HasValue)
            {
                var category = (IncomeExpenseCategoryEnum)param.PostType;
                component.Where(x => x.Category == category);
            }
            else
            {
                IncomeExpenseCategoryEnum[] categories = new[]
                {
                    IncomeExpenseCategoryEnum.Agency,
                    IncomeExpenseCategoryEnum.Square,
                    IncomeExpenseCategoryEnum.Experience
                };
                component.Where(x => categories.Contains(x.Category));
            }
            if (param.UnlockType.HasValue)
            {
                switch (param.UnlockType)
                {
                    case 0:
                        component.Where(x => x.Rebate == 1.0M);
                        break;

                    case 1:
                        component.Where(x => x.Rebate == 0.0M);
                        break;

                    case 2:
                        component.Where(x => x.Rebate > 0.0M && x.Rebate < 1.0M);
                        break;

                    default:
                        break;
                }
            }
            if (param.UserId.HasValue)
            {
                component.Where(x => x.UserId == param.UserId);
            }
            return await component.Where(e => e.TransactionType == IncomeExpenseTransactionTypeEnum.Expense)
                .OrderByDescending(x => x.CreateTime)
                .QueryPageResultAsync(param);
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMIncomeExpenseModel>> List(AdminIncomeExpenseListByTypeParam param)
        {
            var component = ReadDb.QueryTable<MMIncomeExpenseModel>()
            .Where(x => x.TransactionType == param.Type);
            switch (param.IdType)
            {
                case 0:
                    component.Where(x => param.Ids.Contains(x.SourceId));
                    break;

                case 1:
                    component.Where(x => param.Ids.Contains(x.TargetId));
                    break;

                default:
                    return new PageResultModel<MMIncomeExpenseModel>()
                    {
                        Data = new MMIncomeExpenseModel[0]
                    };
            }
            return await component.OrderByDescending(x => x.CreateTime)
                .QueryPageResultAsync(param);
        }

        private DapperQueryComponent<MMIncomeExpenseModel> GetByFilter(IncomeExpenseFilter filter)
        {
            var queryComponent = WriteDb.QueryTable<MMIncomeExpenseModel>();

            if (filter.UserId.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.UserId == filter.UserId);
            }

            if (filter.TransactionTypes.IsNotEmpty())
            {
                queryComponent = queryComponent.Where(e => filter.TransactionTypes.Contains(e.TransactionType));
            }

            if (filter.Categories.IsNotEmpty())
            {
                queryComponent = queryComponent.Where(e => filter.Categories.Contains(e.Category));
            }
            if (filter.StartTime.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.CreateTime >= filter.StartTime);
            }

            if (filter.EndTime.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.CreateTime < filter.EndTime);
            }

            if (filter.PayType.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.PayType == filter.PayType);
            }

            if (filter.Ids.IsNotEmpty())
            {
                queryComponent = queryComponent.Where(e => filter.Ids.Contains(e.Id));
            }

            if (filter.SourceIds.IsNotEmpty())
            {
                queryComponent = queryComponent.Where(e => filter.SourceIds.Contains(e.SourceId));
            }

            if (filter.TargetIds.IsNotEmpty())
            {
                queryComponent = queryComponent.Where(e => filter.TargetIds.Contains(e.TargetId));
            }

            if (filter.Status.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.Status == filter.Status);
            }

            if (filter.IsZeroAmount.HasValue)
            {
                if (filter.IsZeroAmount.Value)
                {
                    queryComponent = queryComponent.Where(e => e.Amount == 0);
                }
                else
                {
                    queryComponent = queryComponent.Where(e => e.Amount > 0);
                }
            }

            return queryComponent;
        }

        public async Task<MMIncomeExpenseModel?> GetByReport(AdminIncomeExpenseByReport param)
        {
            return (await ReadDb.QueryTable<MMIncomeExpenseModel>()
                .Where(x => x.TransactionType == param.TransactionType &&
                        x.SourceId == param.SoruceId &&
                        x.Category == param.Category
                        )
                .QueryAsync()).FirstOrDefault();
        }

        public async Task<DBResult> Update(AdminIncomeExpenseUpdateStatusParam param)
        {
            try
            {
                var result = await WriteDb.QueryFirstAsync<DBResult>(
                    "[dbo].[Pro_MMIncomeExpenseAdminUpdate]",
                    paras: param,
                    commandTimeout: 30,
                    CommandType.StoredProcedure);
                if (!result.IsSuccess)
                {
                    Logger.LogError($"{nameof(Update)} fail, param:{JsonConvert.SerializeObject(param)}, result:{JsonConvert.SerializeObject(result)}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"{nameof(Update)} fail, param:{JsonConvert.SerializeObject(param)}");
            }

            return new DBResult(ReturnCode.SystemError);
        }

        /// <inheritdoc/>
        public async Task<PageResultModel<MMIncomeExpenseModel>> List(AdminIncomeListParam param)
        {
            var component = ReadDb.QueryTable<MMIncomeExpenseModel>();

            if (!string.IsNullOrEmpty(param.Id))
            {
                var id = param.Id;
                component.Where(x => x.Id == id);
            }
            if (param.UserId.HasValue)
            {
                var id = param.UserId;
                component.Where(x => x.UserId == id);
            }

            if (param.PostType.HasValue)
            {
                var id = param.PostType.Value.ConvertToIncomeExpenseCategory();
                component.Where(x => x.Category == id);
            }

            if (param.BookingPaymentType != 0)
            {
                MMBooking[] bookings = await BookingRepo.GetUserBookingPost(new BookingFilter
                {
                    PaymentType = (BookingPaymentType)param.BookingPaymentType
                });
                component.Where(x => bookings.Select(y => y.BookingId).Contains(x.SourceId));
            }

            if (param.SourceIds.Any())
            {
                component.Where(x => param.SourceIds.Contains(x.SourceId));
            }

            if (param.LockType.HasValue)
            {
                var now = DateTime.Now;
                switch (param.LockType)
                {
                    case 0:
                        component.Where(x => x.CreateTime.AddHours(MMGlobalSettings.BaseDispatchHours) <= now);
                        break;

                    case 1:
                        component.Where(x => x.CreateTime.AddHours(MMGlobalSettings.BaseDispatchHours) > now);
                        break;
                }
            }

            if (param.Status.HasValue)
            {
                component.Where(x => x.Status == param.Status);
            }

            var begin = param.BeginDate;
            var end = param.EndDate;
            if (param.DateTimeType == 1)
            {
                begin = begin.AddHours(-MMGlobalSettings.BaseDispatchHours);
                end = end.AddHours(-MMGlobalSettings.BaseDispatchHours);
            }
            return await component.Where(x => x.CreateTime >= begin && x.CreateTime < end)
                .Where(x => x.TransactionType == IncomeExpenseTransactionTypeEnum.Income)
                .OrderByDescending(x => x.CreateTime)
                .QueryPageResultAsync(param);
        }

        public async Task<DBResult> UpdateStatus(MMIncomeExpenseModel incomeExpense)
        {
            return await UpdateStatus(incomeExpense.ToEnumerable());
        }

        public async Task<DBResult> Dispatched(MMIncomeExpenseModel incomeExpense)
        {
            return await Dispatched(incomeExpense.ToEnumerable());
        }

        public async Task<DBResult> Dispatched(IEnumerable<MMIncomeExpenseModel> incomeExpenses)
        {
            var incomeDispatched = incomeExpenses.Where(e => DispatchedStatus.Contains(e.Status)).ToArray();

            var incrementUserSummaries = incomeDispatched.Where(e => e.Category.IsUserSummaryCategory()).Select(e => new IncrementUserSummaryModel
            {
                Amount = e.Amount * e.Rebate,
                Category = e.Category.ConvertToUserSummaryCategory().Value,
                Type = UserSummaryTypeEnum.Income,
                UserId = e.UserId,
            });

            if (incrementUserSummaries.IsNotEmpty())
            {
                UserSummaryRepo.IncrementUserSummary(WriteDb, incrementUserSummaries);
            }

            try
            {
                await UpdateStatus(WriteDb, incomeExpenses).SaveChangesAsync();
            }
            catch (MMException ex)
            {
                string ids = string.Join(",", incomeExpenses.Select(e => e.Id));
                throw new MSSqlException(ReturnCode.RunSQLFail, $"UpdateIncomeExpenseStatus:[{ids}],{ex.Message}");
            }
            return new DBResult(ReturnCode.Success);
        }

        public async Task<DBResult> UpdateStatus(IEnumerable<MMIncomeExpenseModel> incomeExpenses)
        {
            UpdateStatus(WriteDb, incomeExpenses);

            try
            {
                await WriteDb.SaveChangesAsync();
            }
            catch (MMException ex)
            {
                string ids = string.Join(",", incomeExpenses.Select(e => e.Id));
                throw new MSSqlException(ReturnCode.RunSQLFail, $"UpdateIncomeExpenseStatus:[{ids}],{ex.Message}");
            }

            return new DBResult(ReturnCode.Success);
        }

        private DapperComponent UpdateStatus(DapperComponent writeDb, IEnumerable<MMIncomeExpenseModel> incomeExpenses)
        {
            string sql = @"
UPDATE [Inlodb].[dbo].[MMIncomeExpense]
SET Status = @Status,
    UnusualMemo = @UnusualMemo,
    UpdateTime = @UpdateTime,
    DistributeTime = @DistributeTime
WHERE Id = @Id";

            return writeDb.AddExecuteSQL(sql, incomeExpenses);
        }

        public async Task<DBResult> UpdateZeroAmountStatus()
        {
            string sql = $@"
UPDATE MMIncomeExpense
SET DistributeTime = @Time, Status = @NewStatus
WHERE TransactionType = @TransactionType
	AND Status = @Status
	AND Amount = 0
	AND PayType = @PayType
	AND CreateTime < DATEADD(HOUR, -120, @Time)";

            try
            {
                await WriteDb.AddExecuteSQL(sql, new
                {
                    Time = DateTimeExtension.GetCurrentTime(),
                    NewStatus = IncomeExpenseStatusEnum.Dispatched,
                    TransactionType = IncomeExpenseTransactionTypeEnum.Income,
                    Status = IncomeExpenseStatusEnum.UnDispatched,
                    PayType = IncomeExpensePayType.Amount,
                }).SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new MSSqlException(ReturnCode.RunSQLFail, $"UpdateZeroAmountStatus,{ex.Message}");
            }

            return new DBResult(ReturnCode.Success);
        }

        public async Task<PageResultModel<AdminIncomeList>> GetIncomeExpenseList(AdminIncomeListParam param)
        {
            int startRowNum = (param.Page <= 1) ? 1 : 1 + (param.Page - 1) * param.PageSize;
            int endRowNum = (startRowNum - 1) + param.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@Id", param.Id, DbType.String);
            parameters.Add("@PostId", param.PostId, DbType.String);
            parameters.Add("@UserId", param.UserId, DbType.Int32);
            parameters.Add("@PostType", param.PostType, DbType.Int32);
            parameters.Add("@Status", param.Status, DbType.Int32);
            parameters.Add("@UserIdentity", param.ApplyIdentity, DbType.Int32);
            parameters.Add("@DateTimeType", param.DateTimeType, DbType.Int32);
            parameters.Add("@BeginDate", param.BeginDate, DbType.DateTime);
            parameters.Add("@EndDate", param.EndDate, DbType.DateTime);

            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string querySql = @"
            SELECT
                ME.[Id], ME.[TransactionType], ME.[Category], ME.[SourceId], ME.[TargetId], ME.[UserId], ME.[Title], ME.[Memo],
                ME.[Amount], ME.[Status], ME.[CreateTime], ME.[DistributeTime], ME.[PayType], ME.[Rebate], ME.[UnusualMemo], ME.[UpdateTime],ISNULL(MB.[CurrentIdentity],MU.[UserIdentity]) AS CurrentIdentity
                INTO #TMP_MMIncomeExpense
                FROM MMIncomeExpense ME WITH(NOLOCK) LEFT JOIN  MMBooking MB WITH(NOLOCK) ON ME.[SourceId]=MB.[BookingId] LEFT JOIN MMUserInfo MU WITH(NOLOCK)  ON ME.[UserId]=MU.[UserId]
            WHERE ME.TransactionType = 2 ";

            if (!string.IsNullOrEmpty(param.Id))
                querySql += "AND ME.Id = @Id";
            if (param.UserId.HasValue)
                querySql += " AND ME.UserId = @UserId";
            if (param.PostType.HasValue)
                querySql += " AND ME.Category = @PostType";
            if (param.Status.HasValue)
                querySql += " AND ME.[Status] = @Status";
          
            if (param.DateTimeType == 0)
                querySql += " AND ME.CreateTime >= @BeginDate AND ME.CreateTime < @EndDate";
            else if (param.DateTimeType == 1)
                querySql += " AND ME.CreateTime >= DATEADD(MINUTE, -120, @BeginDate) AND ME.CreateTime < DATEADD(MINUTE, -120, @EndDate)";

            //根据筛选后的临时表数据查询相关联的投诉/预定/解锁单记录
            querySql += $@"
            SELECT *
            INTO #TMP_InIncomeDATA
            FROM (
            SELECT DISTINCT
                    IE.Id,
                    CASE
                        WHEN PT.PostId IS NOT NULL THEN PT.PostId
                        ELSE BK.PostId
                    END AS PostId,
                    IE.Category,
                    IE.UserId,
                    PT.Id AS TargetId,
                    BK.BookingId,
                    IE.CreateTime,
                    IE.DistributeTime,
                    IE.[Status],
                    IE.Amount,
                    IE.Rebate,
                    IE.UnusualMemo,
                    IE.SourceId,
                    PR.ReportId,
                    PR.[Status] AS ReportStatus,
                    IE.[CurrentIdentity] AS CurrentIdentity
            FROM #TMP_MMIncomeExpense IE
            LEFT JOIN (
                SELECT DISTINCT
                    T2.[Id],
                    T2.[PostId],
                    T2.[ReportId]
                FROM MMPostTransaction T2 WITH(NOLOCK)
            ) PT ON IE.SourceId = PT.Id
            LEFT JOIN (
                SELECT DISTINCT
                    T2.[ReportId],
                    T2.[Status]
                FROM MMPostReport T2 WITH(NOLOCK)
            ) PR ON PT.ReportId = PR.ReportId
            LEFT JOIN (
                SELECT DISTINCT
                    T2.[BookingId],
                    T2.[PostId]
                FROM MMBooking T2 WITH(NOLOCK)
            ) BK ON IE.SourceId = BK.BookingId AND IE.Category = 3) list
            WHERE (@PostId IS NULL OR list.PostId = @PostId) { (param.ApplyIdentity.HasValue? " AND list.[CurrentIdentity]=@UserIdentity " : "") };

            SET @TotalCount=(SELECT COUNT(*) FROM #TMP_InIncomeDATA {(param.ApplyIdentity.HasValue ? " WHERE [CurrentIdentity]=@UserIdentity " : "")})

            --分页
            SELECT
                *
            FROM(
                SELECT *,ROW_NUMBER() OVER(ORDER BY [CreateTime] DESC) AS RowNumber
                FROM  #TMP_InIncomeDATA
            ) AS T
            WHERE T.RowNumber BETWEEN @StartNo AND @EndNo

            DROP TABLE IF EXISTS #TMP_MMIncomeExpense;
            DROP TABLE IF EXISTS #TMP_InIncomeDATA;";



            var result = await ReadDb.QueryAsync<AdminIncomeList>(querySql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<AdminIncomeList>
            {
                Page = param.Page,
                PageSize = param.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }
    }
}