using IMDataBase.Common;
using IMDataBase.Enums;
using IMDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;
using System.Collections.Generic;

namespace IMBGDataBase.Merchant
{
    public class IMBetDetailMSLMockService : IMBetDetailMSLService
    {
        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMApiParamModel apiParam)
        {
            return MockDataUtil.GetFromTpBetDetailApiResult(apiParam);
            //return MockDataUtil.GetRemoteBetDetailApiResult(apiParam);
        }
    }

    public class MockDataUtil
    {
        public static BetLogResponseInfo GetMockRemoteBetDetailApiResult(IMApiParamModel apiParam)
        {
            if (apiParam.ProductCode == IMProductCode.ESportsBulls)
            {
                return GetSuccessResponse();
                //return GetNoDataResponse();
                //return GetFailedResponse();
                //return null;
            }
            else if (apiParam.ProductCode == IMProductCode.EsportsVirtual)
            {
                return GetSuccessResponse();
                //return GetNoDataResponse();
                //return GetFailedResponse();
                //return null;
            }
            else if (apiParam.ProductCode == IMProductCode.PKTenLegends)
            {
                return GetSuccessResponse();
                //return GetNoDataResponse();
                //return GetFailedResponse();
                //return null;
            }

            throw new NotSupportedException();
        }

        private static BetLogResponseInfo GetSuccessResponse()
        {
            var result = new BetLogResult<List<BetResult>>
            {
                Code = (int)APIErrorCode.Success,
                Result = new List<BetResult>()
            };

            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.IMTransferService).AccountPrefixCode;
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
                long betIdByTime = DateTime.Now.ToUnixOfTime();
                betIdByTime += 1 + i;

                result.Result.Add(new BetResult()
                {
                    PlayerId = playerId,
                    WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                    SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                    StakeAmount = "100",
                    WinLoss = "10",
                    BetId = betIdByTime.ToString(),
                    IsSettled = 1,
                    IsCancelled = 0,
                    OddsType = IMOneHandicap.EURO.Value,
                    DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.5" } }
                });

                betIdByTime += 1 + i;

                result.Result.Add(new BetResult()
                {
                    PlayerId = playerId,
                    WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                    SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                    StakeAmount = "100",
                    WinLoss = "10",
                    BetId = betIdByTime.ToString(),
                    IsSettled = 1,
                    IsCancelled = 0,
                    OddsType = IMOneHandicap.EURO.Value,
                    DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.65" } }
                });

                betIdByTime += 1 + i;

                result.Result.Add(new BetResult()
                {
                    PlayerId = playerId,
                    WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                    SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                    StakeAmount = "100",
                    WinLoss = "10",
                    BetId = betIdByTime.ToString(),
                    IsSettled = 1,
                    IsCancelled = 0,
                    OddsType = IMOneHandicap.EURO.Value,
                    DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.8" } }
                });

                betIdByTime += 1 + i;

                result.Result.Add(new BetResult()
                {
                    PlayerId = playerId,
                    WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                    SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                    StakeAmount = "100",
                    WinLoss = "-10",
                    BetId = betIdByTime.ToString(),
                    IsSettled = 1,
                    IsCancelled = 0,
                    OddsType = IMOneHandicap.EURO.Value,
                    DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.8" } }
                });

                betIdByTime += 1 + i;

                result.Result.Add(new BetResult()
                {
                    PlayerId = playerId,
                    WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                    SettlementDateTime = DateTime.Now.ToFormatDateTimeString(),
                    StakeAmount = "100",
                    WinLoss = "0",
                    BetId = betIdByTime.ToString(),
                    IsSettled = 1,
                    IsCancelled = 0,
                    OddsType = IMOneHandicap.EURO.Value,
                    DetailItems = new List<DetailItem>() { new DetailItem() { CompetitionName = "StreetFighter", Odds = "1.8" } }
                });
            }

            return new BetLogResponseInfo() { ApiResult = result.ToJsonString() };
        }

        private static BetLogResult<List<BetResult>> GetNoDataResponse()
        {
            var result = new BetLogResult<List<BetResult>>
            {
                Code = (int)APIErrorCode.NoDataFound,
                Result = new List<BetResult>()
            };

            return result;
        }

        private static BetLogResult<List<BetResult>> GetFailedResponse()
        {
            var result = new BetLogResult<List<BetResult>>
            {
                Code = (int)APIErrorCode.SystemHasFailedYourRequest,
                Result = new List<BetResult>()
            };

            return result;
        }

        public static BetLogResponseInfo GetFromTpBetDetailApiResult(IMApiParamModel apiParam)
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