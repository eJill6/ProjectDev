using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Core.Extensions;
using MS.Core.Infrastructures.DBTools;
using MS.Core.Infrastructures.DBTools.Models;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Model.Entities.Media;
using MS.Core.MM.Models;
using MS.Core.MM.Models.Booking;
using MS.Core.MM.Models.Booking.Enums;
using MS.Core.MM.Models.Booking.Res;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models.AdminBooking;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.Refund;
using MS.Core.MMModel.Models.User.Enums;
using MS.Core.Models.Models;
using MS.Core.Repos;
using System.Data;

namespace MS.Core.MM.Repos
{
    public class BookingRepo : BaseInlodbRepository<MMBooking>, IBookingRepo
    {
        private IUserSummaryRepo UserSummaryRepo { get; }

        public BookingRepo(IOptionsMonitor<MsSqlConnections> setting, IRequestIdentifierProvider provider, ILogger logger
            , IUserSummaryRepo userSummaryRepo) : base(setting, provider, logger)
        {
            UserSummaryRepo = userSummaryRepo;
        }

        public async Task SetBookingCompleted(ResBookingCompleted resBookingCompleted)
        {
            await WriteDb.Update(resBookingCompleted.Bookings)
                 .Update(resBookingCompleted.Posts)
                 .Update(resBookingCompleted.UpdateUserSummary)
                 .Insert(resBookingCompleted.InsertUserSummary)
                 .SaveChangesAsync();
        }

        public async Task Booking(PostBookingModel postBooking)
        {
            await WriteDb.Insert(postBooking.MMBooking)
                .Insert(postBooking.MMIncomeExpenseModel)
                .SaveChangesAsync();
        }

        public async Task Refund(RefundModel refund)
        {
            string sql = @"
             UPDATE [dbo].[MMBooking]
                SET Status = @Status, CancelTime =@CancelTime
             WHERE [BookingId] = @BookingId AND Status = @OriginalStatus

             IF @@ROWCOUNT = 0
             BEGIN
                 ;THROW 50000, 'No records were updated.', 1;
             END
             ";

            string applyRefundSql = @"
             IF EXISTS (SELECT TOP 1 1 FROM [dbo].[MMApplyRefund] WITH(NOLOCK) WHERE RefundId = @RefundId AND Status = 0)
             UPDATE [dbo].[MMApplyRefund] SET Status = @Status, ExamineMan = @ExamineMan, ExamineTime = GETDATE(), Memo = @Memo
             WHERE RefundId = @RefundId AND Status = 0
            ";
            await WriteDb
                .AddExecuteSQL(sql, refund.Booking)
                .AddExecuteSQL(applyRefundSql, refund.ApplyRefundModel, false)
                .Insert(refund.MMIncomeExpenseModel)
                .SaveChangesAsync();
        }

        /// <summary>
        /// 后台拒绝退款
        /// </summary>
        /// <param name="refundModel"></param>
        /// <returns></returns>
        public async Task RefuseRefund(MMApplyRefund refundModel, AdminRefuseRefundUpdateModel refuseRefundModel)
        {
            string applyRefundSql = @"UPDATE [dbo].[MMApplyRefund] SET Status = 2, ExamineMan = @ExamineMan, ExamineTime = @ExamineTime, Memo = @Memo
                                    WHERE [RefundId] = @RefundId AND Status = 0
                                    IF @@ROWCOUNT = 0
                                    BEGIN
                                        ;THROW 50000, 'No records were updated.', 1;
                                    END";

            string bookingSql = @"UPDATE [dbo].[MMBooking] SET Status = @Status, ScheduledTime = @ScheduledTime
                                WHERE [BookingId] = @BookingId
                                IF @@ROWCOUNT = 0
                                BEGIN
                                    ;THROW 50000, 'No records were updated.', 1;
                                END";

            await WriteDb
                .AddExecuteSQL(applyRefundSql, refundModel)
                .AddExecuteSQL(bookingSql, refuseRefundModel)
                .SaveChangesAsync();
        }

        public async Task Accept(BookingAccept accept)
        {
            string sql = @"
UPDATE [dbo].[MMBooking]
SET Status = @Status, AcceptTime =@AcceptTime, ScheduledTime=@ScheduledTime
WHERE [BookingId] = @BookingId AND Status = @OriginalStatus

IF @@ROWCOUNT = 0
BEGIN
    ;THROW 50000, 'No records were updated.', 1;
END
";
            await WriteDb.AddExecuteSQL(sql, accept).SaveChangesAsync();
        }

        public async Task Update(MMBooking booking)
        {
            await WriteDb.Update(booking).SaveChangesAsync();
        }

        /// <summary>
        /// 根据用户ID和发帖人ID获取状态不为完成的预约订单数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postUserId"></param>
        /// <returns></returns>
        public async Task<int> GetUserAndPostUserNotCompletedCount(int userId, int postUserId)
        {
            return await WriteDb
                .QueryTable<MMBooking>()
                .Where(c => c.UserId == userId && c.PostUserId == postUserId && c.Status != BookingStatus.TransactionCompleted && c.Status != BookingStatus.RefundSuccessful)
                .CountAsync();
        }

        public async Task Distribute(BookingDistributeModel bookingDistribute)
        {
            //            string sql = @"
            //UPDATE [dbo].[MMBooking]
            //SET IncomeId = @IncomeId
            //WHERE [BookingId] = @BookingId AND Status IN @Statuses AND IncomeId IS NULL

            //IF @@ROWCOUNT = 0
            //BEGIN
            //    ;THROW 50000, 'No records were updated.', 1;
            //END
            //";

            string sql = @"
            UPDATE [dbo].[MMBooking]
            SET IncomeId = @IncomeId,[Status]=@Status
            WHERE [BookingId] = @BookingId AND IncomeId IS NULL

            IF @@ROWCOUNT = 0
            BEGIN
                ;THROW 50000, 'No records were updated.', 1;
            END
            ";

            IncrementUserSummaryModel incrementUserSummary = new()
            {
                Amount = Math.Floor(bookingDistribute.Income.Amount * bookingDistribute.Income.Rebate),
                Category = UserSummaryCategoryEnum.Official,
                Type = UserSummaryTypeEnum.Income,
                UserId = bookingDistribute.Income.UserId,
            };

            await WriteDb.AddExecuteSQL(sql, bookingDistribute.Booking)
                .AddRepoFunction(dc => UserSummaryRepo.IncrementUserSummary(dc, incrementUserSummary))
                .Insert(bookingDistribute.Income)
                .Update(bookingDistribute.Expense)
                .SaveChangesAsync();
        }

        public async Task Done(BookingDone bookDone)
        {
            string updatePostSql = @" UPDATE [dbo].[MMOfficialPost] SET AppointmentCount = AppointmentCount+1  WHERE PostId = @PostId ";

            ///统计表内增加用户预约次数
            IncrementUserSummaryModel incrementUserSummary = new()
            {
                Amount = 1,
                Category = UserSummaryCategoryEnum.Official,
                Type = UserSummaryTypeEnum.BookingCount,
                UserId = bookDone.Booking.UserId,
            };

            ///统计表内增加发帖用户被预约次数
            IncrementUserSummaryModel incrementPostUserSummary = new()
            {
                Amount = 1,
                Category = UserSummaryCategoryEnum.Official,
                Type = UserSummaryTypeEnum.BookedCount,
                UserId = bookDone.Booking.PostUserId,
            };

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@PostId", bookDone.Booking.PostId, DbType.String);

            //ReadDb.QueryTable<MMOfficialPost>

            await WriteDb.Update(bookDone.Booking)
                .AddExecuteSQL(updatePostSql, dynamicParameters)
                .AddRepoFunction(dc => UserSummaryRepo.IncrementUserSummary(dc, incrementUserSummary))
                .AddRepoFunction(dc => UserSummaryRepo.IncrementUserSummary(dc, incrementPostUserSummary))
                .SaveChangesAsync();
        }

        public async Task<MMBooking?> GetById(string bookingId, bool isWriteDb = false)
        {
            if (isWriteDb)
            {
                var queryComponentFromWriteDb = WriteDb.QueryTable<MMBooking>();
                return await queryComponentFromWriteDb.Where(e => e.BookingId == bookingId).QueryAsync().FirstOrDefaultAsync();
            }
            var queryComponent = ReadDb.QueryTable<MMBooking>();
            return await queryComponent.Where(e => e.BookingId == bookingId).QueryAsync().FirstOrDefaultAsync();
        }

        public async Task<MMApplyRefund?> GetRefundById(string refundId)
        {
            var queryComponent = ReadDb.QueryTable<MMApplyRefund>();
            return await queryComponent.Where(e => e.RefundId == refundId).QueryAsync().FirstOrDefaultAsync();
        }

        public async Task<PageResultModel<MMBooking>> GetPageByFilter(PageBookingFilter filter)
        {
            DapperQueryComponent<MMBooking> queryComponent = QueryByFilter(filter);

            return await queryComponent.OrderByDescending(e => e.BookingTime).QueryPageResultAsync(filter.Pagination);
        }

        public async Task<PageResultModel<MMBooking>> GetBookingPageByFilter(MyOrderPageBookingFilter filter)
        {
            DapperQueryComponent<MMBooking> queryComponent = GetMyOrderQueryByFilter(filter);

            return await queryComponent.OrderByDescending(e => e.BookingTime).QueryPageResultAsync(filter.Pagination);
        }

        private DapperQueryComponent<MMBooking> GetMyOrderQueryByFilter(MyOrderBookingFilter filter)
        {
            var queryComponent = WriteDb.QueryTable<MMBooking>();
            queryComponent = queryComponent.Where(e => e.Status == filter.Status);

            if (filter.UserId.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.UserId == filter.UserId);
            }

            if (filter.PostUserId.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.PostUserId == filter.PostUserId);
            }

            if (filter.PostIds.IsNotEmpty())
            {
                queryComponent = queryComponent.Where(e => filter.PostIds.Contains(e.PostId));
            }

            if (filter.IsDistribute == false)
            {
                queryComponent = queryComponent.Where(e => e.IncomeId == null);
            }

            if (filter.IsDistribute == true)
            {
                queryComponent = queryComponent.Where(e => e.IncomeId != null);
            }

            if (filter.PaymentType.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.PaymentType == filter.PaymentType);
            }

            if (filter.IsDelete.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.IsDelete == filter.IsDelete);
            }
            if (filter.IsRunOutOfTime == true)
            {
                queryComponent = queryComponent.Where(e => DateTime.Now > e.ScheduledTime);
            }

            return queryComponent;
        }

        private DapperQueryComponent<MMBooking> QueryByFilter(BookingFilter filter)
        {
            var queryComponent = WriteDb.QueryTable<MMBooking>();

            if (filter.UserId.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.UserId == filter.UserId);
            }

            if (filter.Statuses.IsNotEmpty())
            {
                queryComponent = queryComponent.Where(e => filter.Statuses.Contains(e.Status));
            }

            if (filter.PostUserId.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.PostUserId == filter.PostUserId);
            }

            if (filter.PostIds.IsNotEmpty())
            {
                queryComponent = queryComponent.Where(e => filter.PostIds.Contains(e.PostId));
            }

            if (filter.IsDistribute == false)
            {
                queryComponent = queryComponent.Where(e => e.IncomeId == null);
            }

            if (filter.IsDistribute == true)
            {
                queryComponent = queryComponent.Where(e => e.IncomeId != null);
            }

            if (filter.PaymentType.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.PaymentType == filter.PaymentType);
            }

            if (filter.IsDelete.HasValue)
            {
                queryComponent = queryComponent.Where(e => e.IsDelete == filter.IsDelete);
            }
            if (filter.IsRunOutOfTime == true)
            {
                queryComponent = queryComponent.Where(e => DateTime.Now > e.ScheduledTime);
            }

            return queryComponent;
        }

        /// <summary>
        /// 取得預約的贴子
        /// </summary>
        /// <param name="filter">篩選條件</param>
        /// <returns></returns>
        public async Task<MMBooking[]> GetBookingPost(BookingFilter filter)
        {
            DapperQueryComponent<MMBooking> queryComponent = QueryByFilter(filter);

            return (await queryComponent.QueryAsync())?.ToArray() ?? new MMBooking[0];
        }

        /// <summary>
        /// 取得預約的贴子
        /// </summary>
        /// <param name="filter">篩選條件</param>
        /// <returns></returns>
        public async Task<int> GetBookingCount(BookingFilter filter)
        {
            DapperQueryComponent<MMBooking> queryComponent = QueryByFilter(filter);

            return await queryComponent.CountAsync();
        }

        /// <summary>
        /// 取得預約的贴子數量
        /// </summary>
        /// <param name="filter">篩選條件</param>
        /// <returns></returns>
        public async Task<int> GetBookingPostCount(BookingFilter filter)
        {
            DapperQueryComponent<MMBooking> queryComponent = QueryByFilter(filter);

            return await queryComponent.CountAsync();
        }

        /// <summary>
        /// 從預約單號查詢退費申請單
        /// </summary>
        /// <param name="bookingIds">預約單id</param>
        /// <returns></returns>
        public async Task<MMApplyRefund[]> GetApplyRefundWithBookingId(string[] bookingIds)
        {
            return (await ReadDb.QueryTable<MMApplyRefund>()
                .Where(p => bookingIds.Contains(p.BookingId)).QueryAsync()).ToArray();
        }

        /// <summary>
        /// 申請退費
        /// </summary>
        /// <param name="bookingEntity">預約資料</param>
        /// <param name="applyRefundEntity">申請退費資料</param>
        /// <param name="mediaEntity">上傳圖檔</param>
        /// <returns></returns>
        public async Task<bool> ApplyRefund(MMBooking bookingEntity, MMApplyRefund applyRefundEntity, MMMedia[] mediaEntity)
        {
            try
            {
                // 這邊與廣場的產id進行共用，切勿修改
                applyRefundEntity.RefundId = await GetSequenceIdentity<MMApplyRefund>();

                mediaEntity.ToList().ForEach(p => p.RefId = applyRefundEntity.RefundId);

                await WriteDb.Update(bookingEntity)
                    .Insert(applyRefundEntity)
                    .Update(mediaEntity)
                    .SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 取得用戶預約的贴子
        /// </summary>
        /// <param name="filter">篩選條件</param>
        /// <returns></returns>
        public async Task<MMBooking[]> GetUserBookingPost(BookingFilter filter)
        {
            DapperQueryComponent<MMBooking> queryComponent = QueryByFilter(filter);

            return (await queryComponent.QueryAsync()).ToArray();
        }

        /// <summary>
        /// 获取预约中次数
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public async Task<int> GetInProgressBookingCount(int userId)
        {
            var sql = @"
SELECT COUNT(1) FROM MMBooking T1 WITH(NOLOCK)
JOIN MMOfficialPost T2 WITH(NOLOCK) ON T1.PostId = T2.PostId
WHERE T1.UserId = @UserId AND
T1.Status IN (0,1,8,10)";
            return await ReadDb.QueryScalarAsync<int>(sql, new { UserId = userId });
        }

        /// <summary>
        /// 根据预约单Id获取贴子Id
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> List(string[] bookingIds)
        {
            var component = ReadDb.QueryTable<MMBooking>();
            component.Where(x => bookingIds.Contains(x.BookingId));
            return (await component.QueryAsync()
                .ToArrayAsync()).ToDictionary(x => x.BookingId, x => x.PostId);
        }

        /// <summary>
        /// 查询预定单列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<MMBooking>> List(AdminBookingListParam param)
        {
            var component = ReadDb.QueryTable<MMBooking>();
            var beginDate = param.BeginDate;
            var endDate = param.EndDate;
            switch (param.DateTimeType)
            {
                case 1:
                    component.Where(x => x.BookingTime >= beginDate && x.BookingTime <= endDate);
                    break;

                case 2:
                    component.Where(x => x.AcceptTime >= beginDate && x.AcceptTime <= endDate);
                    break;

                case 3:
                    component.Where(x => x.FinishTime >= beginDate && x.FinishTime <= endDate);
                    break;

                case 4:
                    component.Where(x => x.CancelTime >= beginDate && x.CancelTime <= endDate);
                    break;

                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(param.BookingId))
            {
                component.Where(x => x.BookingId == param.BookingId);
            }
            if (param.UserId.HasValue)
            {
                component.Where(x => x.UserId == param.UserId);
            }
            if (param.PaymentType.HasValue)
            {
                if (param.PaymentType == 1)
                {
                    component.Where(x => x.PaymentType == BookingPaymentType.Booking && x.Status != BookingStatus.RefundSuccessful);
                }
                else if (param.PaymentType == 2)
                {
                    component.Where(x => x.PaymentType == BookingPaymentType.Full && x.Status != BookingStatus.RefundSuccessful);
                }
                else if (param.PaymentType == 3)
                {
                    component.Where(x => x.PaymentType == BookingPaymentType.Booking && x.Status == BookingStatus.RefundSuccessful);
                }
                else if (param.PaymentType == 4)
                {
                    component.Where(x => x.PaymentType == BookingPaymentType.Full && x.Status == BookingStatus.RefundSuccessful);
                }
            }
            if (param.BookingStatus.HasValue)
            {
                if (param.BookingStatus == 2)
                {
                    component.Where(x => new int[] { 2, 3, 4, 5, 10 }.Contains((int)x.Status));
                }
                else
                {
                    component.Where(x => (int)x.Status == param.BookingStatus);
                }
            }
            if (param.OrderStatus.HasValue)
            {
                component.Where(x => (int)x.Status == param.OrderStatus);
            }
            if(param.UserIdentity.HasValue)
            {
                component.Where(x => x.CurrentIdentity == (IdentityType)param.UserIdentity);
            }

            return await component.OrderByDescending(x => x.BookingTime)
                .QueryPageResultAsync(param);
        }

        //根据预约单Id批量查询 预约单发帖人的身份
        public async Task<Dictionary<string, int?>> GetBookingIdentityByIds(string[] bookingIds)
        {
            var component = ReadDb.QueryTable<MMBooking>();
            component.Where(x => bookingIds.Contains(x.BookingId));
            return (await component.QueryAsync()
           .ToArrayAsync()).ToDictionary(x => x.BookingId, x => x.CurrentIdentity == null ? null : (int?)x.CurrentIdentity, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 查询预定单退款申请列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PageResultModel<MMApplyRefundAndBookingModel>> RefundApplyList(AdminBookingRefundListParam param)
        {
            int startRowNum = (param.Page <= 1) ? 1 : 1 + (param.Page - 1) * param.PageSize;
            int endRowNum = (startRowNum - 1) + param.PageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@BookingId", param.BookingId, DbType.String);
            parameters.Add("@UserId", param.UserId, DbType.String);
            parameters.Add("@PaymentType", param.PaymentType, DbType.Int32);
            parameters.Add("@ReasonType", param.ReasonType, DbType.Int32);
            parameters.Add("@UserIdentity", param.UserIdentity, DbType.Int32);
            parameters.Add("@StartNo", startRowNum);
            parameters.Add("@EndNo", endRowNum);
            parameters.Add("@BeginDate", param.BeginDate, DbType.DateTime);
            parameters.Add("@EndDate", param.EndDate, DbType.DateTime);
            parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string querSql = @" DROP TABLE IF EXISTS #TMP_MMApplyRefund
			SELECT 
                MA.[RefundId],MA.[UserId],MA.[BookingId],MA.[ReasonType],MA.[Reason],MA.[ApplyTime],MA.[Status],MA.[ExamineMan],MA.[ExamineTime],MA.[Memo],MB.[PostUserId],
                ISNULL(MB.[CurrentIdentity],MU.[UserIdentity]) AS CurrentIdentity,MB.[PaymentType],MB.[Discount],MB.[PostId],MB.[BookingTime],MB.[PaymentMoney]
			    INTO #TMP_MMApplyRefund
			    FROM MMApplyRefund AS MA WITH(NOLOCK) LEFT JOIN [dbo].[MMBooking] AS MB WITH(NOLOCK) ON MA.[BookingId]=MB.[BookingId] 
			    INNER JOIN MMUserInfo AS MU ON MB.PostUserId=MU.UserId ";

            querSql += " SELECT * INTO #TMP_MMApplyRefundData FROM #TMP_MMApplyRefund WHERE  [ApplyTime] >= @BeginDate AND [ApplyTime] < @EndDate ";

            if (!string.IsNullOrWhiteSpace(param.BookingId))
                querSql += " AND [BookingId]=@BookingId ";
            if (param.UserId.HasValue)
                querSql += " AND [UserId]=@UserId ";
            if (param.PaymentType.HasValue)
                querSql += " AND [PaymentType]=@PaymentType ";
            if (param.ReasonType.HasValue)
                querSql += " AND [ReasonType]=@ReasonType ";
            if (param.UserIdentity.HasValue)
                querSql += " AND [CurrentIdentity]=@UserIdentity ";
            //if(param.ReasonType.HasValue)
            //    querSql += " AND [CurrentIdentity]=@UserIdentity ";

            querSql += @"  
            
            SET @TotalCount=(SELECT COUNT(*) FROM #TMP_MMApplyRefundData );
            --分页
            SELECT
                *
            FROM(
                SELECT *,ROW_NUMBER() OVER(ORDER BY [ApplyTime] DESC) AS RowNumber
                FROM  #TMP_MMApplyRefundData
            ) AS T
            WHERE T.RowNumber BETWEEN @StartNo AND @EndNo";

            var result = await ReadDb.QueryAsync<MMApplyRefundAndBookingModel>(querSql, parameters);

            var totalCount = parameters.Get<int>("@TotalCount");

            var totalPage = (int)Math.Ceiling((decimal)totalCount / param.PageSize);

            return new PageResultModel<MMApplyRefundAndBookingModel>
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