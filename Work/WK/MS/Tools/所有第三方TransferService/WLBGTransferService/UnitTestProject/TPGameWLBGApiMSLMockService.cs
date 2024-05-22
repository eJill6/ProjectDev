using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.WLBG;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TPGameWLBGApiMSLMockService : TPGameWLBGApiMSLService
    {
        public TPGameWLBGApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            WLBGGetBetLogRequestModel request = GetWLBGBetLogRequest(lastSearchToken);

            var betLogs = new WLBGBetRecordResult();
            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.WLBGTransferService).AccountPrefixCode;
            string[] playerIds = new string[]
            {
                $"jx{accountPrefixCode}_69778",
                $"cts{accountPrefixCode}_3",
                $"msl{accountPrefixCode}_9530",
                $"msl{accountPrefixCode}_888"
            };

            string gameDate = request.From.ToDateTime("yyyyMMddHHmmss").AddSeconds(25).ToFormatDateTimeString();

            foreach (string player in playerIds)
            {
                betLogs = new WLBGBetRecordResult()
                {
                    From = request.From.ToDateTime("yyyyMMddHHmmss"),
                    Until = request.Until.ToDateTime("yyyyMMddHHmmss"),
                    Count = 1,
                    HasMore = false,
                    List = new WLBGBetRecordLogs
                    {
                        Uid = new List<string>() { player },
                        Game = new List<string>() { "13" },
                        Profit = new List<string>() { "0.3" },
                        Balance = new List<string>() { "356.3" },
                        Bet = new List<string>() { "0.5" },
                        ValidBet = new List<string>() { "0.5" },
                        Tax = new List<string>() { "0" },
                        GameStartTime = new List<string>() { gameDate },
                        RecordTime = new List<string>() { gameDate },
                        GameId = new List<string>() { DateTime.Now.ToUnixOfTime().ToString() },
                        RecordId = new List<string>() { DateTime.Now.ToUnixOfTime().ToString() },
                        Category = new List<string>() { "4" }
                    }
                };
            }
            /*
           {\"code\":0,\"data\":{\"from\":\"2023-01-31 17:02:01\",\"until\":\"2023-01-31 17:05:02\",\"count\":2,\"hasMore\":false,\"list\":{\"uid\":[\"mslD_9530\",\"mslD_9530\"],\"game\":[13,22],\"profit\":[\"0.3\",\"5\"],\"balance\":[\"356.3\",\"351.3\"],\"bet\":[\"0.5\",\"5\"],\"validBet\":[\"0.5\",\"5\"],\"tax\":[\"0\",\"0\"],\"gameStartTime\":[\"2023-01-31 17:03:13\",\"2023-01-31 17:03:43\"],\"recordTime\":[\"2023-01-31 17:03:34\",\"2023-01-31 17:03:46\"],\"gameId\":[\"0DK230131170312B1041\",\"16R230131170343V1037\"],\"recordId\":[\"1098190-393\",\"1098190-394\"],\"category\":[4,4]}}}
           */
            var response = new WLBGBetLogResponseModel()
            {
                Code = 0,
                Data = betLogs
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