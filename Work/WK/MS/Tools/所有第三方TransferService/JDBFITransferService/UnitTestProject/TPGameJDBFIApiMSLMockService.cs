using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.JDB;
using JxBackendService.Model.ThirdParty.JDB.JDBFI;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TPGameJDBFIApiMSLMockService : TPGameJDBFIApiMSLService
    {
        public TPGameJDBFIApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            JDBFIGetBetLogRequestModel request = GetJDBFIBetLogRequest(lastSearchToken);
            var betLogs = new List<JDBFIBetLog>();
            int[] userIds = { 6251 };
            string gameDate = request.Starttime.ToDateTime(JDBResponseDateTimeFormat).AddSeconds(25).ToString(JDBResponseDateTimeFormat);

            foreach (int userId in userIds)
            {
                betLogs.Add(new JDBFIBetLog()
                {
                    SeqNo = DateTime.Now.ToUnixOfTime().ToString(),
                    PlayerId = $"msl{EnvLoginUser.EnvironmentCode.AccountPrefixCode.ToLower()}{userId}",
                    MType = "7001",
                    GameDate = gameDate,
                    Bet = -62.5m,
                    Win = 19.75m,
                    Total = -42.75m,
                    LastModifyTime = gameDate
                });
            }

            var response = new JDBFIBetLogResponseModel()
            {
                Status = JDBFIResponseCode.Success.Value,
                Data = betLogs,
                Err_text = null,
            };

            return new BaseReturnDataModel<RequestAndResponse>(
                ReturnCode.Success,
                new RequestAndResponse()
                {
                    RequestBody = request.ToJsonString(isCamelCaseNaming: true),
                    ResponseContent = response.ToJsonString(isCamelCaseNaming: true)
                });
        }
    }
}