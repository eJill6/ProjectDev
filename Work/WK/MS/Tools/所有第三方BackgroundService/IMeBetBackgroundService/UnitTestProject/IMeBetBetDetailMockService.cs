using IMBGDataBase.Merchant;
using IMeBetDataBase.Common;
using IMeBetDataBase.Enums;
using IMeBetDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.IM;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using UnitTestN6;

namespace UnitTestProject
{
    public class IMeBetBetDetailMockService : IMeBetBetDetailMSLService
    {        

        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMeBetApiParamModel apiParam)
        {
            return MockDataUtil.GetMockRemoteBetDetailApiResult(apiParam);
            //return MockDataUtil.GetFromTpBetDetailApiResult(apiParam);
        }

        public class MockDataUtil
        {
            public static BetLogResponseInfo GetMockRemoteBetDetailApiResult(IMeBetApiParamModel apiParam)
            {
                var betLogs = new List<BetResult>();

                TestUtil.DoPlayerJobs(                
                recordCount: 10000,
                job: (playerId, betId) =>
                {
                    betLogs.Add(new BetResult()
                    {
                        PlayerId = playerId,
                        GameName = "testtest",
                        BetDate = DateTime.Now.ToFormatDateTimeString(),
                        ReportingDate = DateTime.Now.ToFormatDateTimeString(),
                        BetAmount = 9999,
                        ValidBet = 888,
                        WinLoss = "10",
                        BetId = betId.ToString(),
                        Status = IMLotteryOrderStatus.Settled.Value
                    });
                });

                var result = new BetLogResult<List<BetResult>>
                {
                    Code = (int)APIErrorCode.Success,
                    Result = betLogs
                };

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