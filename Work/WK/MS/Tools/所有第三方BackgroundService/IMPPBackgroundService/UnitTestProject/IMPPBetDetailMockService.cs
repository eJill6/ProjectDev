using IMBGDataBase.Merchant;
using IMPPDataBase.Common;
using IMPPDataBase.Enums;
using IMPPDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using UnitTestN6;

namespace UnitTestProject
{
    public class IMPPBetDetailMockService : IMPPBetDetailMSLService
    {
        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMPPApiParamModel apiParam)
        {
            return MockDataUtil.GetMockRemoteBetDetailApiResult(apiParam);
            //return MockDataUtil.GetFromTpBetDetailApiResult(apiParam);
        }

        public class MockDataUtil
        {
            public static BetLogResponseInfo GetMockRemoteBetDetailApiResult(IMPPApiParamModel apiParam)
            {
                var betLogs = new List<BetResult>();

                TestUtil.DoPlayerJobs(
                    recordCount: 10000,
                    job: (playerId, betId) =>
                    {
                        betLogs.Add(new BetResult()
                        {
                            PlayerId = playerId,
                            GameName = "test",
                            DateCreated = DateTimeOffset.Now,
                            GameDate = DateTimeOffset.Now,
                            LastUpdatedDate = DateTimeOffset.Now,
                            BetAmount = 100,
                            WinLoss = 10,
                            RoundId = betId.ToString(),
                            Status = "Closed",
                            Provider = IMOneProviderType.IMSE.Value
                        });
                    });

                var result = new BetLogResult<List<BetResult>>
                {
                    Code = (int)APIErrorCode.Success,
                    Result = betLogs
                };

                return new BetLogResponseInfo() { ApiResult = result.ToJsonString() };
            }

            public static BetLogResponseInfo GetFromTpBetDetailApiResult(IMPPApiParamModel apiParam)
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