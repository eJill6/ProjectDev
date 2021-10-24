using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Repository.Game
{
    public class UserCommissionStoredProcedureRep : BaseDbRepository, IUserCommissionStoredProcedureRep
    {

        public UserCommissionStoredProcedureRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public List<SpUserCommissionSelBackendResult> GetUserCommissionForBackSide(int? userId, DateTime startDate)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_UserCommission_SelBackend";
            return DbHelper.QueryList<SpUserCommissionSelBackendResult>(sql, new { userId, startDate }, CommandType.StoredProcedure);
        }

        public BaseReturnDataModel<double> GetCommissionPayBySystem(int yearMonth)
        {
            string sql = $@"
                DECLARE @SumCommissionAmount MONEY,
	                    @ErrCode VARCHAR(5),		
		                @ErrMsg NVARCHAR(100)

                EXEC {InlodbType.Inlodb}.dbo.Pro_UserCommissioninfo_SelSummaryCommissionAmount
	                @ProcessMonth = @yearMonth,
	                @SumCommissionAmount = @SumCommissionAmount OUTPUT,
	                @ErrCode = @ErrCode	OUTPUT,
	                @ErrMsg = @ErrMsg OUTPUT

                SELECT	@SumCommissionAmount AS SumCommissionAmount,
		                @ErrCode AS ErrCode,
		                @ErrMsg AS ErrMsg ";

            var spSelSummaryCommissionAmountResult = DbHelper.QueryList<SpSelSummaryCommissionAmountResult>(sql, new { yearMonth }).Single();

            if (!spSelSummaryCommissionAmountResult.ErrMsg.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<double>(spSelSummaryCommissionAmountResult.ErrMsg, 0);
            }
            else
            {
                return new BaseReturnDataModel<double>(ReturnCode.Success, spSelSummaryCommissionAmountResult.SumCommissionAmount);
            }
        }

        public List<SpUserCommissionSelFrontSideResult> GetUserCommissionForFrontSide(int userId, string commissionType, DateTime startDate, DateTime endDate, int type)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_UserCommission_SelCommissionList";
            return DbHelper.QueryList<SpUserCommissionSelFrontSideResult>(
                sql,
                new { userId, commissionType, startDate, endDate, type },
                CommandType.StoredProcedure);
        }
    }
}
