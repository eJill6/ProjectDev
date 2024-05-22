using IMBGDataBase.Enums;
using IMBGDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;
using System.Collections.Generic;

namespace IMBGDataBase.Merchant
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

            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.IMBGTransferService).AccountPrefixCode;
            string[] userCodes = new string[]
            {
                $"jx{accountPrefixCode}_69778",
                $"cts{accountPrefixCode}_3",
                $"cts{accountPrefixCode}_36",
                $"msl{accountPrefixCode}_588",
                $"msl{accountPrefixCode}_888",
            };

            for (int i = 0; i < userCodes.Length; i++)
            {
                string userCode = userCodes[i];

                result.Data.List.Add(new IMBGBetLog()
                {
                    UserCode = userCode,
                    GameId = 1001,
                    AllBills = "100",
                    OpenTime = (DateTime.Now.AddMinutes(-10).ToUnixOfTime() / 1000).ToString(),
                    EndTime = (DateTime.Now.ToUnixOfTime() / 1000).ToString(),
                    WinLost = "10",
                    EffectBet = "123",
                    DealId = DateTime.Now.ToUnixOfTime().ToString(),
                    Id = DateTime.Now.AddSeconds(i).ToUnixOfTime(),
                });
            }

            return new BetLogResponseInfo() { ApiResult = result.ToJsonString() };
        }
    }
}