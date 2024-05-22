using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AWCSP;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TPGameAWCSPApiMSLMockService : TPGameAWCSPApiMSLService
    {
        public TPGameAWCSPApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            AWCSPGetBetLogRequestModel request = GetAWCSPBetLogRequest(lastSearchToken, AWCSPPlatform.HORSEBOOK.Value);
            var betLogs = new List<AWCSPBetLog>();
            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.AWCSPTransferService).AccountPrefixCode;
            string[] playerIds = new string[]
            {
                $"jx{accountPrefixCode}_69778",
                $"cts{accountPrefixCode}_3",
                $"cts{accountPrefixCode}_36",
                $"msl{accountPrefixCode}_588",
                $"msl{accountPrefixCode}_888",
                $"msl{accountPrefixCode}_6251",
            };

            string gameDate = request.StartTime;

            foreach (string playerId in playerIds)
            {
                betLogs.Add(new AWCSPBetLog()
                {
                    PlatformTxId = DateTime.Now.ToUnixOfTime().ToString(),
                    UserId = playerId,
                    GameType = "7001",
                    BetTime = gameDate,
                    BetAmount = -62.5m,
                    WinAmount = 19.75m,
                    UpdateTime = gameDate
                });
            }

            var response = new AWCSPBetLogResponseModel()
            {
                Status = AWCSPResponseCode.Success.Value,
                Transactions = betLogs,
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