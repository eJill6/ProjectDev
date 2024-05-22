using IMPTDataBase.Common;
using IMPTDataBase.Enums;
using IMPTDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;
using System.Collections.Generic;

namespace IMBGDataBase.Merchant
{
    public class IMPTBetDetailMSLMockService : IMPTBetDetailMSLService
    {
        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMPTApiParamModel apiParam)
        {
            //return MockDataUtil.GetMockRemoteBetDetailApiResult(apiParam);
            return MockDataUtil.GetFromTpBetDetailApiResult(apiParam);
        }
    }

    public class MockDataUtil
    {
        public static BetLogResponseInfo GetMockRemoteBetDetailApiResult(IMPTApiParamModel apiParam)
        {
            var result = new BetLogResult<List<BetResult>>
            {
                Code = (int)APIErrorCode.Success,
                Result = new List<BetResult>()
            };

            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.IMPTTransferService).AccountPrefixCode;
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

                //0 = 常规旋转
                result.Result.Add(new BetResult()
                {
                    PlayerName = playerId,
                    GameName = "test",
                    GameDate = DateTime.Now.ToFormatDateTimeString(),
                    Bet = 100,
                    Win = 110,
                    GameCode = DateTime.Now.AddSeconds(i).ToUnixOfTime().ToString(),
                    BonusType = "0"
                });

                //BonusType = 1 = 免费游戏 (包括自由旋转)
                result.Result.Add(new BetResult()
                {
                    GameType = "POP Slots",
                    PlayerName = playerId,
                    GameName = "test",
                    GameDate = DateTime.Now.ToFormatDateTimeString(),
                    Bet = 0,
                    Win = 666,
                    GameCode = DateTime.Now.AddSeconds(i + 1).ToUnixOfTime().ToString(),
                    BonusType = "1"
                });

                //BonusType = 2 = 重新旋转
                result.Result.Add(new BetResult()
                {
                    GameType = "Progressive Slot Machines",
                    PlayerName = playerId,
                    GameName = "test",
                    GameDate = DateTime.Now.ToFormatDateTimeString(),
                    Bet = 0,
                    Win = 0,
                    GameCode = DateTime.Now.AddSeconds(i + 2).ToUnixOfTime().ToString(),
                    BonusType = "2"
                });

                //BonusType = 3 = 红利奖金 (旋转触发免费游戏)
                result.Result.Add(new BetResult()
                {
                    GameType = "Slot Machines",
                    PlayerName = playerId,
                    GameName = "test",
                    GameDate = DateTime.Now.ToFormatDateTimeString(),
                    Bet = 0,
                    Win = 0,
                    GameCode = DateTime.Now.AddSeconds(i + 3).ToUnixOfTime().ToString(),
                    BonusType = "3"
                });

                //BonusType = 4 = 自由旋转（促销）
                result.Result.Add(new BetResult()
                {
                    GameType = "Live Games",
                    PlayerName = playerId,
                    GameName = "test",
                    GameDate = DateTime.Now.ToFormatDateTimeString(),
                    Bet = 0,
                    Win = 20,
                    GameCode = DateTime.Now.AddSeconds(i + 4).ToUnixOfTime().ToString(),
                    BonusType = "4"
                });

                //BonusType = 5 =金牌
                result.Result.Add(new BetResult()
                {
                    GameType = "123123",
                    PlayerName = playerId,
                    GameName = "test",
                    GameDate = DateTime.Now.ToFormatDateTimeString(),
                    Bet = 0,
                    Win = 15,
                    GameCode = DateTime.Now.AddSeconds(i + 5).ToUnixOfTime().ToString(),
                    BonusType = "5"
                });

                result.Result.Add(new BetResult()
                {
                    PlayerName = playerId,
                    GameName = "test13123",
                    GameDate = DateTime.Now.ToFormatDateTimeString(),
                    Bet = 888,
                    Win = 999,
                    GameCode = DateTime.Now.AddSeconds(i).ToUnixOfTime().ToString(),
                    BonusType = "0",
                });
            }

            return new BetLogResponseInfo() { ApiResult = result.ToJsonString() };
        }

        public static BetLogResponseInfo GetFromTpBetDetailApiResult(IMPTApiParamModel apiParam)
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