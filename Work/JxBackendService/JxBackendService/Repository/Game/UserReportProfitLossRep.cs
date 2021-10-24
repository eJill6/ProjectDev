using JxBackendService.Interface.Repository;
using JxBackendService.Model.Enums;
using JxBackendService.Model.StoredProcedureParam;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Repository.Game
{
    public class UserReportProfitLossRep : BaseDbRepository, IUserReportProfitLossRep
    {

        public UserReportProfitLossRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public UserReportProfitLossResult GetUserReportProfitloss(ProGetUserReportProfitlossParam param)
        {
            var userReportProfitLossResult = new UserReportProfitLossResult();

            string sql = $"{InlodbType.Inlodb}.dbo.Pro_GetUserReport_Profitloss";
            DbHelper.QueryMultiple(sql, param, CommandType.StoredProcedure, (gridReader) =>
            {
                if (param.UserType == 1)
                {
                    userReportProfitLossResult.Type1Table0Result = gridReader.ReadSingle<UserReportProfitlossType1Table0Result>();
                    userReportProfitLossResult.Type1Table1Results = gridReader.Read<UserReportProfitlossType1Table1Result>().ToList();
                }
                else if (param.UserType == 2)
                {
                    IEnumerable<UserReportProfitlossType2Table0Result> type2Table0Result = null;

                    try
                    {
                        type2Table0Result = gridReader.Read<UserReportProfitlossType2Table0Result>();
                    }
                    catch (InvalidOperationException)
                    {
                        //ignore;
                        //sp內有判斷,所以有可能沒有回傳資料集,dapper會直接出錯
                    }

                    if (type2Table0Result != null)
                    {
                        userReportProfitLossResult.Type2Table0Result = type2Table0Result.Single();
                        userReportProfitLossResult.Type2Table1Results = gridReader.Read<UserReportProfitlossType2Result>().ToList();
                        userReportProfitLossResult.Type2Table2Results = gridReader.Read<UserReportProfitlossType2Result>().ToList();
                        userReportProfitLossResult.Type2Table3Results = gridReader.Read<UserReportProfitlossType2Result>().ToList();
                    }
                    else
                    {
                        userReportProfitLossResult.Type2Table0Result = new UserReportProfitlossType2Table0Result();
                        userReportProfitLossResult.Type2Table1Results = new List<UserReportProfitlossType2Result>();
                        userReportProfitLossResult.Type2Table2Results = new List<UserReportProfitlossType2Result>();
                        userReportProfitLossResult.Type2Table3Results = new List<UserReportProfitlossType2Result>();
                    }
                }
            });

            return userReportProfitLossResult;
        }
    }
}
