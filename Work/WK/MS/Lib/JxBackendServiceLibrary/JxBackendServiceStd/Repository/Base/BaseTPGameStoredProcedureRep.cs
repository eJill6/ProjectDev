using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
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
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace JxBackendService.Repository.Base
{
    public abstract class BaseTPGameStoredProcedureRep : BaseDbRepository, ITPGameStoredProcedureRep
    {
        protected static readonly string _transferSpSuccessCode = "2";

        private static readonly int _minRecheckOrderMinutes = -3;

        protected static string OrderIdPrefix => SharedAppSettings.PlatformMerchant.Value; //各商戶代碼

        private static readonly string s_depositActionCode = "D";

        private static readonly string s_withdrawActionCode = "W";

        private readonly Lazy<IPlatformProductService> _platformProductService;

        public static string DepositActionCode => s_depositActionCode;

        public static string WithdrawActionCode => s_withdrawActionCode;

        public abstract PlatformProduct Product { get; } //先預留之後可能會用到

        protected string ProductName => _platformProductService.Value.GetName(Product.Value);

        public BaseTPGameStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(envLoginUser.Application, SharedAppSettings.PlatformMerchant);
        }

        #region abstract

        public abstract string DWDailyProfitLossTableName { get; }

        protected abstract string SearchUnprocessedMoneyInInfoSpName { get; }

        protected abstract string SearchUnprocessedMoneyOutInfoSpName { get; }

        public abstract string MoneyInInfoTableName { get; }

        public abstract string MoneyOutInfoTableName { get; }

        protected abstract string TransferSuccessSpName { get; }

        protected abstract string TransferRollbackSpName { get; }

        protected abstract string TransferInSpName { get; }

        protected abstract string TransferOutSpName { get; }

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
                    IsOperateByBackSide = true //sp用來阻擋一次只能有一張處理中的轉回單
                },
                CommandType.StoredProcedure);

            return errorMsg;
        }

        protected virtual string GetMoneyInIdSequence() => GetMoneyInIdSequenceIdentity();

        protected virtual string GetMoneyOutIdSequence() => GetMoneyOutIdSequenceIdentity();

        public List<BaseReturnDataModel<string>> AddMultipleProductProfitLossAndPlayInfo(List<InsertTPGameProfitlossParam> tpGameProfitlosses)
        {
            string tpUserInfoTableName = GetTPUserInfoTableName();

            string sql = $@"
                DECLARE @SystemHandler NVARCHAR(50) = N'系统';

                DROP TABLE IF EXISTS #Param
                DROP TABLE IF EXISTS #SaveResult
                DROP TABLE IF EXISTS #SeqKY
                DROP TABLE IF EXISTS #ExistPlayID
                DROP TABLE IF EXISTS #NotExistUserID
                DROP TABLE IF EXISTS #ParamStat

                CREATE TABLE #SaveResult(
	                KeyID NVARCHAR(200),
                    ErrorMsg NVARCHAR(500)
                );

                CREATE TABLE #Param(
                    RowID INT IDENTITY(1,1),
                    KeyId NVARCHAR(200),
	                UserID INT, --用戶ID
                    BetTime DATETIME, --投注時間
                    ProfitLossTime DATETIME, --開獎時間
                    ProfitLossType NVARCHAR(50), --亏赢类型，包真人和体育
                    ProfitLossMoney DECIMAL(18, 4), --下注額
	                AllBetMoney DECIMAL(18, 4), --總投注額
                    WinMoney DECIMAL(18, 4), --虧盈
                    PrizeMoney DECIMAL(18, 4), --下注額+虧盈
                    Memo NVARCHAR(500), --備註
                    PlayID NVARCHAR(50), --订单编号
                    GameType NVARCHAR(50), --遊戲類型
                    BetResultType INT, --1:贏,0:輸,-1和
                );

                INSERT INTO #Param (
	                KeyId,
	                UserID,
	                BetTime,
	                ProfitLossTime,
	                ProfitLossType,
	                ProfitLossMoney,
	                AllBetMoney,
	                WinMoney,
	                PrizeMoney,
	                Memo,
	                PlayID,
	                GameType,
	                BetResultType)
                SELECT
	                KeyId,
	                UserID,
	                BetTime,
	                ProfitLossTime,
	                ProfitLossType,
	                ProfitLossMoney,
	                AllBetMoney,
	                WinMoney,
	                PrizeMoney,
	                Memo,
	                PlayID,
	                GameType,
	                BetResultType
                FROM OPENJSON(@ParamJson)
                WITH(
	                KeyId NVARCHAR(200) '$.KeyId',
	                UserID INT '$.UserID',
	                BetTime DATETIMEOFFSET '$.BetTime',
	                ProfitLossTime DATETIMEOFFSET '$.ProfitLossTime',
	                ProfitLossType NVARCHAR(50) '$.ProfitLossType',
	                ProfitLossMoney DECIMAL(18, 4) '$.ProfitLossMoney',
	                AllBetMoney DECIMAL(18, 4) '$.AllBetMoney',
	                WinMoney DECIMAL(18, 4) '$.WinMoney',
	                PrizeMoney DECIMAL(18, 4) '$.PrizeMoney',
	                Memo NVARCHAR(500) '$.Memo',
	                PlayID NVARCHAR(50) '$.PlayID',
	                GameType NVARCHAR(50) '$.GameType',
	                BetResultType INT '$.BetResultType')

                SELECT
	                PlayID
                INTO #ExistPlayID
                FROM {InlodbType.Inlodb}.dbo.{ProfitlossTableName} WITH(NOLOCK)
                WHERE PlayID IN (SELECT PlayID FROM #Param)

                INSERT INTO #SaveResult(
	                KeyID,
	                ErrorMsg)
                SELECT
	                KeyId,
	                N'单号已存在'
                FROM #Param
                WHERE PlayID IN (SELECT PlayID FROM #ExistPlayID)

                DELETE FROM #Param
                WHERE PlayID IN (SELECT PlayID FROM #ExistPlayID)

                --加入 ProfitLossID 自產序號
                SELECT
	                KeyId,
	                CONVERT(VARCHAR(20), GETDATE(), 112) + RIGHT(REPLICATE('0', 8) + CAST(NEXT VALUE FOR {InlodbType.Inlodb}.dbo.SEQ_{ProfitlossTableName}_ProfitLossID AS VARCHAR(20)), 8) AS SeqID_KY
                INTO #SeqKY
                FROM #Param

                DECLARE @NowDate DATETIME = GETDATE()
                DECLARE @ErrorMsg NVARCHAR(50)

                BEGIN TRY
                    BEGIN TRANSACTION

	                --插入亏盈
                    INSERT INTO {InlodbType.Inlodb}.dbo.{ProfitlossTableName} (
                        ProfitLossID,
                        UserID,
                        BetTime,
                        ProfitLossTime,
                        ProfitLossType,
                        ProfitLossMoney,
                        WinMoney,
                        PrizeMoney,
                        Memo,
                        PlayID,
                        GameType,
                        BetResultType,
                        AllBetMoney)
                    SELECT
                        SK.SeqID_KY,
                        PA.UserID,
                        PA.BetTime,
                        PA.ProfitLossTime,
                        PA.ProfitLossType,
                        PA.ProfitLossMoney,
                        PA.WinMoney,
                        PA.PrizeMoney,
                        PA.Memo,
                        PA.PlayID,
                        PA.GameType,
                        PA.BetResultType,
                        PA.AllBetMoney
	                FROM #Param PA
	                INNER JOIN #SeqKY SK ON PA.KeyId = SK.KeyId

	                --寫入注單
	                INSERT INTO {InlodbType.Inlodb}.dbo.{PlayInfoTableName}
	                (
		                PlayInfoID,
		                UserID,
		                PlayID,
		                BetTime,
		                ProfitLossTime,
		                BetMoney,
		                WinMoney,
		                GameType,
		                Memo,
		                RefID,
		                SaveTime,
		                BetResultType,
		                AllBetMoney)
	                SELECT
		                SK.SeqID_KY,
		                PA.UserID,
		                PA.PlayID,
		                PA.BetTime,
		                PA.ProfitLossTime,
		                PA.ProfitLossMoney,
		                PA.WinMoney,
		                PA.GameType,
		                PA.Memo,
		                SK.SeqID_KY,
		                @NowDate,
		                PA.BetResultType,
		                PA.AllBetMoney
	                FROM #Param PA
	                INNER JOIN #SeqKY SK ON PA.KeyId = SK.KeyId

	                SELECT DISTINCT
		                UserID
	                INTO #NotExistUserID
	                FROM #Param

	                DELETE FROM #NotExistUserID WHERE UserID IN (
		                SELECT
			                UserID
		                FROM {InlodbType.Inlodb}.dbo.{tpUserInfoTableName} WITH(NOLOCK)
		                WHERE UserID IN (SELECT UserID FROM #Param)
	                )

	                --因為是多筆的關係，需要先建立空資料
	                IF EXISTS(SELECT TOP 1 1 FROM #NotExistUserID)
	                BEGIN
		                INSERT INTO {InlodbType.Inlodb}.dbo.{tpUserInfoTableName}(
			                UserID,
			                TransferIn,
			                TransferOut,
			                WinOrLoss,
			                Rebate,
			                AvailableScores,
			                FreezeScores,
			                LastUpdateTime)
		                SELECT
			                UserID,
			                0,
			                0,
			                0,
			                0,
			                0,
			                0,
			                @NowDate
		                FROM #NotExistUserID
	                END

	                --修改第三方的積分
                    SELECT
		                UserID,
		                MAX(RowID) AS MaxRowID,
		                SUM(WinMoney) AS TotalWinMoney
	                INTO #ParamStat
	                FROM #Param
	                GROUP BY UserID

	                UPDATE TPU
	                SET
		                WinOrLoss = ISNULL(WinOrLoss, 0) + PS.TotalWinMoney,
		                LastUpdateTime = @NowDate
	                FROM {InlodbType.Inlodb}.dbo.{tpUserInfoTableName} TPU WITH(NOLOCK)
	                INNER JOIN #ParamStat PS ON TPU.UserID = PS.UserID
	                INNER JOIN #Param PA ON PS.MaxRowID = PA.RowID

	                --執行
                    COMMIT TRAN

                END TRY
                BEGIN CATCH
	                IF @@TRANCOUNT > 0
	                BEGIN
		                ROLLBACK TRAN
		                EXEC {InlodbType.Inlodb}.dbo.Pro_LogErrorProcedure

                        SET @ErrorMsg = N'未知异常'
	                END
                END CATCH

                INSERT INTO #SaveResult(
			        KeyID,
			        ErrorMsg)
		        SELECT
			        KeyId,
			        @ErrorMsg
		        FROM #Param

                SELECT KeyID, ErrorMsg FROM #SaveResult ";

            string paramJson = tpGameProfitlosses.Select(s =>
                new
                {
                    s.KeyId,
                    s.UserID,
                    BetTime = s.BetTime.ToFormatDateTimeMillisecondsString(),
                    ProfitLossTime = s.ProfitLossTime.ToFormatDateTimeMillisecondsString(),
                    s.ProfitLossType,
                    s.ProfitLossMoney,
                    s.AllBetMoney,
                    s.WinMoney,
                    s.PrizeMoney,
                    s.Memo,
                    s.PlayID,
                    s.GameType,
                    s.BetResultType,
                })
                .ToJsonString();

            List<SaveTPGameProfitlossResult> saveTPGameProfitlossResults = DbHelper.QueryList<SaveTPGameProfitlossResult>(
                sql,
                new { ParamJson = paramJson.ToNVarchar(-1) });

            return saveTPGameProfitlossResults.Select(s => new BaseReturnDataModel<string>(s.ErrorMsg, s.KeyId)).ToList();
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

        private string GetTPUserInfoTableName()
        {
            ITPGameUserInfoService tpGameUserInfoService = DependencyUtil.ResolveJxBackendService<ITPGameUserInfoService>(
                Product,
                SharedAppSettings.PlatformMerchant,
                EnvLoginUser,
                DBConnectionType).Value;

            string tpUserInfoTableName = ModelUtil.GetTableName(tpGameUserInfoService.GetUserInfoType());

            return tpUserInfoTableName;
        }
    }
}