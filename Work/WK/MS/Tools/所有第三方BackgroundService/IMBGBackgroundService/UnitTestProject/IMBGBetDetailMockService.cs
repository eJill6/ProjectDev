using IdGen;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using ProductTransferService.IMBGDataBase.Enums;
using ProductTransferService.IMBGDataBase.Merchant;
using ProductTransferService.IMBGDataBase.Model;
using UnitTestN6;

namespace UnitTestProject
{
    public class IMBGBetDetailMockService : IMBGBetDetailMSLService
    {
        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMBGApiParamModel apiParam)
        {
            return MockDataUtil.GetRemoteBetDetailApiResult(apiParam);
        }
    }

    public class MockDataUtil
    {
        private static readonly IdGenerator s_idGenerator = new IdGenerator(0);

        public static BetLogResponseInfo GetRemoteBetDetailApiResult(IMBGApiParamModel apiParam)
        {
            var result = new IMBGResp<IMBGBetList<IMBGBetLog>>()
            {
                Data = new IMBGBetList<IMBGBetLog>()
                {
                    Code = (int)APIErrorCode.Success,
                    List = new List<IMBGBetLog>()
                }
            };

            TestUtil.DoPlayerJobs(                
                recordCount: 10000,
                job: (userCode, betId) =>
                {
                    result.Data.List.Add(new IMBGBetLog()
                    {
                        UserCode = userCode,
                        GameId = 1001,
                        AllBills = "100",
                        OpenTime = (DateTime.Now.AddMinutes(-10).ToUnixOfTime() / 1000).ToString(),
                        EndTime = (DateTime.Now.ToUnixOfTime() / 1000).ToString(),
                        WinLost = "10",
                        EffectBet = "123",
                        DealId = betId.ToString(),
                        Id = betId,
                    });
                });

            return new BetLogResponseInfo() { ApiResult = result.ToJsonString() };
        }

        private static long CreateId()
        {
            while (true)
            {
                if (s_idGenerator.TryCreateId(out long betId))
                {
                    return betId;
                }

                TaskUtil.DelayAndWait(1000);
            }
        }
    }
}