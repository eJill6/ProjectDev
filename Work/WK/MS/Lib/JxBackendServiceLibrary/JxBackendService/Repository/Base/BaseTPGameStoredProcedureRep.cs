using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.db;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Date;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseTPGameStoredProcedureRep : BaseDbRepository, ITPGameStoredProcedureRep
    {
        protected static readonly string _transferSpSuccessCode = "2";

        private static readonly int _minRecheckOrderMinutes = -3;

        protected static readonly string OrderIdPrefix = SharedAppSettings.PlatformMerchant.Value; //各商戶代碼

        private static readonly string s_depositActionCode = "D";

        private static readonly string s_withdrawActionCode = "W";

        private readonly IPlatformProductService _platformProductService;

        public static string DepositActionCode => s_depositActionCode;

        public static string WithdrawActionCode => s_withdrawActionCode;

        public abstract PlatformProduct Product { get; } //先預留之後可能會用到

        protected string ProductName => _platformProductService.GetName(Product.Value);

        public BaseTPGameStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
        }

        #region abstract

        public abstract string DWDailyProfitLossTableName { get; }

        protected abstract string SearchUnprocessedMoneyInInfoSpName { get; }

        protected abstract string SearchUnprocessedMoneyOutInfoSpName { get; }

        protected abstract string MoneyInInfoTableName { get; }

        protected abstract string MoneyOutInfoTableName { get; }

        protected abstract string TransferSuccessSpName { get; }

        protected abstract string TransferRollbackSpName { get; }

        protected abstract string TransferInSpName { get; }

        protected abstract string TransferOutSpName { get; }

        protected abstract string AddProfitlossAndPlayInfoSpName { get; }

        public abstract string PlayInfoTableName { get; }

        public abstract string ProfitlossTableName { get; }

        public virtual List<SqlSelectColumnInfo> PlayInfoSelectColumnInfos
        {
            get
            {
                List<SqlSelectColumnInfo> selectColumnInfos = ModelUtil.GetAllColumnInfos<TPGamePlayInfoRowModel>();

                //一律設定成identity,讓底層再建立temp table的時候做轉型
                selectColumnInfos
                    .Where(w => w.AliasName == nameof(TPGamePlayInfoRowModel.PlayInfoID)).Single()
                    .IsIdentity = true;

                return selectColumnInfos;
            }
        }

        public virtual List<SqlSelectColumnInfo> ProfitLossSelectColumnInfos
        {
            get
            {
                List<SqlSelectColumnInfo> selectColumnInfos = ModelUtil.GetAllColumnInfos<TPGameProfitLossRowModel>();
                //一律設定成identity,讓底層再建立temp table的時候做轉型
                selectColumnInfos.Where(w => w.AliasName == nameof(TPGameProfitLossRowModel.ProfitLossID)).Single().IsIdentity = true;

                return selectColumnInfos;
            }
        }

        public TPGameProfitLossRowModelCompareTime ProfitLossCompareTimeProperty => TPGameProfitLossRowModelCompareTime.ProfitLossTime;

        #endregion abstract

        private string GetMoneyInfoTableKey(bool isMoneyIn)
        {
            if (isMoneyIn)
            {
                return "MoneyInID";
            }
            else
            {
                return "MoneyOutID";
            }
        }

        public virtual string GetTransferSpActionType(bool isMoneyIn)
        {
            if (isMoneyIn)
            {
                return "1";
            }
            else
            {
                return "2";
            }
        }

        public List<TPGameMoneyInInfo> GetTPGameUnprocessedMoneyInInfo()
        {
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{SearchUnprocessedMoneyInInfoSpName}";
            return DbHelper.QueryList<TPGameMoneyInInfo>(sql, null, CommandType.StoredProcedure);
        }

        public List<TPGameMoneyInInfo> GetTPGameProcessingMoneyInInfo()
        {
            string tableKey = GetMoneyInfoTableKey(true);
            string processingMoneyInSql = SearchTPGameProcessingMoneyInfo(tableKey, MoneyInInfoTableName);

            return DbHelper.QueryList<TPGameMoneyInInfo>(
                processingMoneyInSql,
                new
                {
                    ProcessingStatus = TPGameMoneyInOrderStatus.Processing.Value,
                });
        }

        public List<TPGameMoneyOutInfo> GetTPGameProcessedMoneyOutInfo(DateTime startDate, DateTime endDate, List<int> userIds)
        {
            string tableKey = GetMoneyInfoTableKey(true);
            string processedMoneyInSql = SearchTPGameProcessedMoneyOutByUserIds(tableKey, MoneyInInfoTableName);

            return DbHelper.QueryList<TPGameMoneyOutInfo>(
                processedMoneyInSql,
                new
                {
                    ProcessedStatus = TPGameMoneyOutOrderStatus.Success.Value,
                    StartDate = startDate,
                    EndDate = endDate,
                    UserIds = userIds
                });
        }

        public List<TPGameMoneyOutInfo> GetTPGameUnprocessedMoneyOutInfo()
        {
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{SearchUnprocessedMoneyOutInfoSpName}";

            return DbHelper.QueryList<TPGameMoneyOutInfo>(sql, null, CommandType.StoredProcedure);
        }

        public List<TPGameMoneyOutInfo> GetTPGameProcessingMoneyOutInfo()
        {
            string tableKey = GetMoneyInfoTableKey(false);
            string processingMoneyInSql = SearchTPGameProcessingMoneyInfo(tableKey, MoneyOutInfoTableName);

            return DbHelper.QueryList<TPGameMoneyOutInfo>(
                processingMoneyInSql,
                new
                {
                    ProcessingStatus = TPGameMoneyOutOrderStatus.Processing.Value,
                });
        }

        public TPGameMoneyInInfo GetTPGameMoneyInInfo(string moneyId)
        {
            string tableKey = GetMoneyInfoTableKey(isMoneyIn: true);
            string sql = GetTPGameMoneyInfoSql(tableKey, MoneyInInfoTableName);

            return DbHelper.QuerySingleOrDefault<TPGameMoneyInInfo>(sql, new { moneyId = moneyId.ToVarchar(32) });
        }

        public TPGameMoneyOutInfo GetTPGameMoneyOutInfo(string moneyId)
        {
            string tableKey = GetMoneyInfoTableKey(isMoneyIn: false);
            string sql = GetTPGameMoneyInfoSql(tableKey, MoneyOutInfoTableName);

            return DbHelper.QuerySingleOrDefault<TPGameMoneyOutInfo>(sql, new { moneyId = moneyId.ToVarchar(32) });
        }

        private string SearchTPGameProcessingMoneyInfo(string tableKey, string tableName)
        {
            string sql = $@"
                -- 撈取設定分鐘前為處理中的訂單
                DECLARE @ProcessingDate DATETIME = DATEADD(MINUTE, {_minRecheckOrderMinutes}, GETDATE())
                DECLARE	@StartDate DATETIME = @ProcessingDate - 3,
		                @EndDate DATETIME =  @ProcessingDate

                SELECT  {tableKey}, Amount, OrderID, OrderTime, Handle,
                        HandTime, UserID, [Status], Memo
                FROM    {InlodbType.Inlodb.Value}.dbo.{tableName} WITH(NOLOCK)
                WHERE   ([Status] = @ProcessingStatus AND OrderTime >= @StartDate AND OrderTime <= @EndDate) ";

            return sql;
        }

        private string SearchTPGameProcessedMoneyOutByUserIds(string tableKey, string tableName)
        {
            string sql = $@"
                SELECT  {tableKey}, Amount, OrderID, OrderTime, Handle,
                        HandTime, UserID, [Status], Memo
                FROM    {InlodbType.Inlodb.Value}.dbo.{tableName} WITH(NOLOCK)
                WHERE   [Status] = @ProcessedStatus AND
                        HandTime >= @StartDate AND HandTime <= @EndDate AND
                        UserID IN @UserIds ";

            return sql;
        }

        private string GetTPGameMoneyInfoSql(string tableKey, string tableName)
        {
            string sql = $@"
SELECT  {tableKey}, Amount, OrderID, OrderTime, Handle,
        HandTime, UserID, [Status], Memo
FROM    {InlodbType.Inlodb.Value}.dbo.{tableName} WITH(NOLOCK)
WHERE   {tableKey} = @moneyId ";

            return sql;
        }

        public virtual bool DoTransferSuccess(bool isMoneyIn, BaseTPGameMoneyInfo tpGameMoneyInfo, UserScore userScore)
        {
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{TransferSuccessSpName}";

            return DbHelper.ExecuteScalar<string>(
                sql,
                new
                {
                    TransferID = tpGameMoneyInfo.GetMoneyID().ToVarchar(32),
                    ActionType = GetTransferSpActionType(isMoneyIn).ToNVarchar(20),
                    userScore.AvailableScores,
                    userScore.FreezeScores,
                    ProductName
                },
                CommandType.StoredProcedure) == _transferSpSuccessCode;
        }

        public virtual bool DoTransferRollback(bool isMoneyIn, BaseTPGameMoneyInfo tpGameMoneyInfo, string msg)
        {
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{TransferRollbackSpName}";

            return DbHelper.ExecuteScalar<string>(
                sql,
                new
                {
                    TransferID = tpGameMoneyInfo.GetMoneyID().ToVarchar(32),
                    ProductName = ProductName.ToNVarchar(50),
                    ActionType = GetTransferSpActionType(isMoneyIn).ToNVarchar(20),
                    msg = msg.ToNVarchar(1024)
                },
                CommandType.StoredProcedure) == _transferSpSuccessCode;
        }

        public virtual TPGameTransferMoneyResult CreateMoneyInOrder(int userId, decimal amount, string tpGameAccount, TPGameMoneyInOrderStatus transferInStatus)
        {
            string errorMsg = ExecTransferInSp(userId, amount, tpGameAccount, transferInStatus, out string moneyId);

            TPGameMoneyInInfo tpGameMoneyOutInfo = null;

            if (errorMsg.IsNullOrEmpty())
            {
                tpGameMoneyOutInfo = GetTPGameMoneyInInfo(moneyId);
            }

            var result = new TPGameTransferMoneyResult()
            {
                TPGameMoneyInfo = tpGameMoneyOutInfo,
                ErrorMsg = errorMsg
            };

            return result;
        }

        protected virtual string ExecTransferInSp(int userId, decimal amount, string tpGameAccount, TPGameMoneyInOrderStatus transferInStatus, out string moneyId)
        {
            const bool isDeposit = true;
            moneyId = GetMoneyInIdSequence();
            string orderId = CreateOrderId(isDeposit, moneyId, userId, tpGameAccount);
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{TransferInSpName}";

            return DbHelper.ExecuteScalar<string>(
                sql,
                new
                {
                    userId,
                    amount,
                    ProductName,
                    moneyId,
                    orderId,
                    OrderStatus = transferInStatus.Value
                },
                CommandType.StoredProcedure);
        }

        public TPGameTransferMoneyResult CreateMoneyOutOrder(CreateTransferOutOrderParam param)
        {
            string errorMsg = ExecTransferOutSp(param, out string moneyId);

            TPGameMoneyOutInfo tpGameMoneyOutInfo = null;

            if (errorMsg.IsNullOrEmpty())
            {
                tpGameMoneyOutInfo = GetTPGameMoneyOutInfo(moneyId);
            }

            var result = new TPGameTransferMoneyResult()
            {
                TPGameMoneyInfo = tpGameMoneyOutInfo,
                ErrorMsg = errorMsg
            };

            return result;
        }

        protected virtual string ExecTransferOutSp(CreateTransferOutOrderParam param, out string moneyId)
        {
            const bool isDeposit = false;
            moneyId = GetMoneyOutIdSequence();
            string orderId = CreateOrderId(isDeposit, moneyId, param.UserId, param.TPGameAccount);
            string sql = $"{InlodbType.Inlodb.Value}.dbo.{TransferOutSpName}";

            string errorMsg = DbHelper.ExecuteScalar<string>(
                sql,
                new
                {
                    param.UserId,
                    param.Amount,
                    moneyId,
                    orderId,
                    OrderStatus = param.TransferOutStatus.Value,
                    IsOperateByBackSide = param.IsOperateByBackSide
                },
                CommandType.StoredProcedure);

            return errorMsg;
        }

        protected virtual string GetMoneyInIdSequence() => GetMoneyInIdSequenceIdentity();

        protected virtual string GetMoneyOutIdSequence() => GetMoneyOutIdSequenceIdentity();

        public BaseReturnModel AddProductProfitLossAndPlayInfo(InsertTPGameProfitlossSpParam tpGameProfitloss)
        {
            string sql = $@"
                EXEC {InlodbType.Inlodb.Value}.dbo.{AddProfitlossAndPlayInfoSpName}
                    @UserID = @UserID,
                    @BetTime = @BetTime,
                    @ProfitLossTime = @ProfitLossTime,
                    @ProfitLossType = @ProfitLossType,
                    @ProfitLossMoney = @ProfitLossMoney,
                    @AllBetMoney = @AllBetMoney,
                    @WinMoney = @WinMoney,
                    @PrizeMoney = @PrizeMoney,
                    @Memo = @Memo,
                    @PlayID = @PlayID,
                    @GameType = @GameType,
                    @BetResultType = @BetResultType,
                    @AvailableScores = @AvailableScores,
                    @FreezeScores = @FreezeScores ";

            string errorMsg = DbHelper.ExecuteScalar<string>(
                sql,
                tpGameProfitloss);

            BaseReturnModel returnModel;

            if (!errorMsg.IsNullOrEmpty())
            {
                returnModel = new BaseReturnModel(errorMsg);
            }
            else
            {
                returnModel = new BaseReturnModel(ReturnCode.Success);
            }

            return returnModel;
        }

        public PagedResultModel<TPGameMoneyInInfo> GetMoneyInInfoList(SearchTPGameMoneyInfoParam searchParam)
        {
            return SearchMoneyInfoList<TPGameMoneyInInfo>(searchParam, MoneyInInfoTableName);
        }

        public PagedResultModel<TPGameMoneyOutInfo> GetMoneyOutInfoList(SearchTPGameMoneyInfoParam searchParam)
        {
            return SearchMoneyInfoList<TPGameMoneyOutInfo>(searchParam, MoneyOutInfoTableName);
        }

        private PagedResultModel<T> SearchMoneyInfoList<T>(SearchTPGameMoneyInfoParam searchParam, string basicTableName) where T : BaseTPGameMoneyInfo, new()
        {
            TableSearchDateRange dateRange = GetTableSearchDateRange(searchParam.SearchOrderStartDate, searchParam.SearchOrderEndDate);

            string commonFilter = null;

            if (!searchParam.UserID.HasValue)
            {
                commonFilter += $"AND UserID = (SELECT TOP 1 UserID FROM {InlodbType.Inlodb}.dbo.UserInfo WITH(NOLOCK) WHERE UserID = @UserID) ";
            }

            if (searchParam.OrderStatus.HasValue)
            {
                commonFilter += "AND [Status] = @OrderStatus ";
            }

            searchParam.InitSortModelsBySortField();

            if (!searchParam.SortModels.AnyAndNotNull())
            {
                searchParam.SortModels = new List<SortModel>()
                {
                    new SortModel()
                    {
                        ColumnName = nameof(BaseTPGameMoneyInfo.OrderTime),
                        Sort = SortOrder.Descending
                    }
                };
            }

            List<SqlSelectColumnInfo> selectColumnInfos = ModelUtil.GetAllColumnInfos<T>();
            //一律設定成identity,讓底層再建立temp table的時候做轉型
            string moneyIdColumnName = new T().GetPrimaryKeyColumnName();
            selectColumnInfos.Where(w => w.AliasName == moneyIdColumnName).Single().IsIdentity = true;

            return GetCrossDbQueryList<T>(new JxCrossDbQueryParam()
            {
                BasicTableName = basicTableName,
                SelectColumnInfos = selectColumnInfos,
                InlodbFilters = $"OrderTime >= @{nameof(dateRange.InlodbStartDate)} AND OrderTime < @{nameof(dateRange.SmallThanInlodbEndDate)} " + commonFilter,
                InlodbBakFilters = $"OrderTime >= @{nameof(dateRange.InlodbBakStartDate)} AND OrderTime < @{nameof(dateRange.SmallThanInlodbBakEndDate)} " + commonFilter,
                OrderBy = searchParam.ToOrderByText(),
                PageNo = searchParam.PageNo,
                PageSize = searchParam.PageSize,
                Parameters = new
                {
                    searchParam.UserID,
                    searchParam.OrderStatus,
                    searchParam.PageNo,
                    searchParam.PageSize,
                    dateRange.InlodbStartDate,
                    dateRange.SmallThanInlodbEndDate,
                    dateRange.InlodbBakStartDate,
                    dateRange.SmallThanInlodbBakEndDate,
                }
            });
        }

        protected virtual string CreateOrderId(bool isDeposit, string moneyId, int userId, string tpGameAccount)
        {
            string productCode = Product.Value;
            string actionCode = ConvertToActionCode(isDeposit);

            return $"{OrderIdPrefix}{productCode}{actionCode}{EnvLoginUser.EnvironmentCode.OrderPrefixCode}{moneyId}";
        }

        protected string ConvertToActionCode(bool isDeposit)
        {
            string actionCode = DepositActionCode;

            if (!isDeposit)
            {
                actionCode = WithdrawActionCode;
            }

            return actionCode;
        }

        protected virtual string GetProfitLossTypeFilter(string profitLossType)
        {
            if (profitLossType.IsNullOrEmpty())
            {
                return null;
            }

            return "AND ProfitLossType = @ProfitLossType ";
        }

        private string GetMoneyInIdSequenceIdentity() => GetSequenceIdentity($"SEQ_{Product.Value}MoneyInInfo_MoneyInID");

        private string GetMoneyOutIdSequenceIdentity() => GetSequenceIdentity($"SEQ_{Product.Value}MoneyOutInfo_MoneyOutID");
    }
}