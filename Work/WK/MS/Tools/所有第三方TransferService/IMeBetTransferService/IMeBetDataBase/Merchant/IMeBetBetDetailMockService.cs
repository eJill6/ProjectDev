using IMeBetDataBase.Common;
using IMeBetDataBase.Enums;
using IMeBetDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;
using System.Collections.Generic;

namespace IMBGDataBase.Merchant
{
    public class IMeBetBetDetailMockService : IMeBetBetDetailMSLService
    {
        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMeBetApiParamModel apiParam)
        {
            //return MockDataUtil.GetMockRemoteBetDetailApiResult(apiParam);
            return MockDataUtil.GetFromTpBetDetailApiResult(apiParam);
        }

        public class MockDataUtil
        {
            public static BetLogResponseInfo GetMockRemoteBetDetailApiResult(IMeBetApiParamModel apiParam)
            {
                var result = new BetLogResult<List<BetResult>>
                {
                    Code = (int)APIErrorCode.Success,
                    Result = new List<BetResult>()
                };

                string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.IMeBetTransferService).AccountPrefixCode;
                string[] playerIds = new string[]
                {
                    $"jx{accountPrefixCode}_69778",
                    $"cts{accountPrefixCode}_3",
                    $"cts{accountPrefixCode}_36",
                    $"msl{accountPrefixCode}_588",
                    $"msl{accountPrefixCode}_888",
                };

                for (int i = 0; i < playerIds.Length; i++)
                {
                    string playerId = playerIds[i];

                    result.Result.Add(new BetResult()
                    {
                        PlayerId = playerId,
                        GameName = "test",
                        BetDate = DateTime.Now.ToFormatDateTimeString(),
                        ReportingDate = DateTime.Now.ToFormatDateTimeString(),
                        BetAmount = 1000000,
                        ValidBet = 1000000,
                        WinLoss = "10",
                        BetId = DateTime.Now.AddSeconds(i).ToUnixOfTime().ToString(),
                        Status = IMLotteryOrderStatus.Settled.Value
                    });

                    result.Result.Add(new BetResult()
                    {
                        PlayerId = playerId,
                        GameName = "testtest",
                        BetDate = DateTime.Now.ToFormatDateTimeString(),
                        ReportingDate = DateTime.Now.ToFormatDateTimeString(),
                        BetAmount = 9999,
                        ValidBet = 888,
                        WinLoss = "10",
                        BetId = DateTime.Now.AddSeconds(i).ToUnixOfTime().ToString(),
                        Status = IMLotteryOrderStatus.Settled.Value
                    });
                }

                return new BetLogResponseInfo() { ApiResult = result.ToJsonString() };
            }

            public static BetLogResponseInfo GetFromTpBetDetailApiResult(IMeBetApiParamModel apiParam)
            {
                var betLogResult = new BetLogResult<List<BetResult>>();
                string apiResult = ApiClient.GetBetLog(apiParam);

                if (!string.IsNullOrEmpty(apiResult))
                {
                    betLogResult = apiResult.Deserialize<BetLogResult<List<BetResult>>>();
                    //No Data Found
                    if (betLogResult.Code == (int)APIErrorCode.NoDataFound)
                    {
                        return new BetLogResponseInfo() { ApiResult = betLogResult.ToJsonString() };
                    }

                    // 大於1頁5000筆
                    if (betLogResult.Pagination != null && betLogResult.Pagination.TotalPage > 1)
                    {
                        for (int i = 2; i <= betLogResult.Pagination.TotalPage; i++)
                        {
                            apiParam.Page = i;
                            apiResult = ApiClient.GetBetLog(apiParam);
                            BetLogResult<List<BetResult>> pagedResult = apiResult.Deserialize<BetLogResult<List<BetResult>>>();
                            betLogResult.Result.AddRange(pagedResult.Result);
                        }
                    }
                }

                return new BetLogResponseInfo() { ApiResult = betLogResult.ToJsonString() };
            }
        }
    }
}