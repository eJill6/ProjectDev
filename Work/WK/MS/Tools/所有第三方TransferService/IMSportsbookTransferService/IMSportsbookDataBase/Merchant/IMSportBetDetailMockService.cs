using IMSportsbookDataBase.Common;
using IMSportsbookDataBase.Enums;
using IMSportsbookDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ThirdParty.IMSport;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;
using System.Collections.Generic;

namespace IMBGDataBase.Merchant
{
    public class IMSportBetDetailMockService : IMSportBetDetailMSLService
    {
        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMSportApiParamModel apiParam)
        {
            //return MockDataUtil.GetMcokRemoteBetDetailApiResult(apiParam);
            return MockDataUtil.GetFromTpBetDetailApiResult(apiParam);
        }

        public class MockDataUtil
        {
            public static BetLogResponseInfo GetMcokRemoteBetDetailApiResult(IMSportApiParamModel apiParam)
            {
                var result = new BetLogResult<List<BetResult>>
                {
                    Code = (int)APIErrorCode.Success,
                    Result = new List<BetResult>()
                };

                string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.IMSportTransferService).AccountPrefixCode;
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
                    decimal[] testOddses = new decimal[] { 1.5m, 1.65m, 1.8m };

                    foreach (IMSportWagerType imSportWagerType in IMSportWagerType.GetAll())
                    {
                        foreach (decimal testOdds in testOddses)
                        {
                            //Win
                            betIdByTime += 1 + i;

                            result.Result.Add(CreateMockBetResult(
                                playerId,
                                IMSportBetStatus.Win,
                                imSportWagerType,
                                IMOneHandicap.EURO,
                                testOdds,
                                betMoney: 100,
                                winMoney: 10,
                                isCashout: false,
                                betIdByTime.ToString()));

                            //Cashout
                            betIdByTime += 1 + i;

                            result.Result.Add(CreateMockBetResult(
                                playerId,
                                betStatus: null,
                                imSportWagerType,
                                IMOneHandicap.EURO,
                                testOdds,
                                betMoney: 100,
                                winMoney: 10,
                                isCashout: true,
                                betIdByTime.ToString()));
                        }
                    }

                    //Lose
                    betIdByTime += 1 + i;

                    result.Result.Add(CreateMockBetResult(
                        playerId,
                        betStatus: IMSportBetStatus.Lose,
                        IMSportWagerType.Single,
                        IMOneHandicap.EURO,
                        odds: 1.5m,
                        betMoney: 100,
                        winMoney: -10,
                        isCashout: false,
                        betIdByTime.ToString()));

                    //HalfLose
                    betIdByTime += 1 + i;

                    result.Result.Add(CreateMockBetResult(
                        playerId,
                        betStatus: IMSportBetStatus.HalfLose,
                        IMSportWagerType.Single,
                        IMOneHandicap.EURO,
                        odds: 1.5m,
                        betMoney: 100,
                        winMoney: -10,
                        isCashout: false,
                        betIdByTime.ToString()));

                    //HalfWin
                    betIdByTime += 1 + i;

                    result.Result.Add(CreateMockBetResult(
                        playerId,
                        betStatus: IMSportBetStatus.HalfWin,
                        IMSportWagerType.Single,
                        IMOneHandicap.EURO,
                        odds: 1.5m,
                        betMoney: 100,
                        winMoney: 10,
                        isCashout: false,
                        betIdByTime.ToString()));

                    //Draw
                    betIdByTime += 1 + i;

                    result.Result.Add(CreateMockBetResult(
                        playerId,
                        betStatus: IMSportBetStatus.Draw,
                        IMSportWagerType.Single,
                        IMOneHandicap.EURO,
                        odds: 1.5m,
                        betMoney: 100,
                        winMoney: 0,
                        isCashout: false,
                        betIdByTime.ToString()));
                }

                return new BetLogResponseInfo() { ApiResult = result.ToJsonString() };
            }

            private static BetResult CreateMockBetResult(string playerId, IMSportBetStatus betStatus, IMSportWagerType wagerType,
                IMOneHandicap handicap, decimal odds, decimal betMoney, decimal winMoney, bool isCashout, string betId)
            {
                string betTradeStatus = null;
                string resultStatus = null;
                decimal betTradeBuybackAmount = 0;

                if (isCashout)
                {
                    betTradeStatus = SingleBetInfo.s_cashOutTradeStatus;
                    betTradeBuybackAmount = winMoney;
                    winMoney = 0;
                }
                else
                {
                    resultStatus = betStatus.Value;
                }

                return new BetResult()
                {
                    PlayerId = playerId,
                    WagerCreationDateTime = DateTime.Now.ToFormatDateTimeString(),
                    LastUpdatedDate = DateTime.Now.ToFormatDateTimeString(),
                    StakeAmount = betMoney.ToString(),
                    WinLoss = winMoney.ToString(),
                    ResultStatus = resultStatus,
                    BetTradeStatus = betTradeStatus,
                    WagerType = wagerType.Value,
                    OddsType = handicap.Value,
                    BetTradeBuybackAmount = betTradeBuybackAmount,
                    BetId = betId,
                    IsSettled = 1,
                    DetailItems = new List<DetailItem>()
                {
                    CreateMockDetailItem(odds),
                    CreateMockDetailItem(odds),
                }
                };
            }

            private static DetailItem CreateMockDetailItem(decimal odds)
            {
                return new DetailItem()
                {
                    Market = "Live",
                    EventName = "Crucero Del Norte vs Argentinos Juniors",
                    EventDateTime = DateTime.Now.ToFormatDateString(),
                    CompetitionName = "",
                    HomeTeamName = "",
                    AwayTeamName = "",
                    FavTeam = "A",
                    BetType = "AH",
                    BetTypeDesc = "Asian Handicap",
                    Selection = "H",
                    Odds = odds.ToString(),
                    HomeTeamHTScore = "",
                    AwayTeamHTScore = "",
                    HomeTeamFTScore = "",
                    AwayTeamFTScore = "",
                    WagerHomeTeamScore = "0",
                    WagerAwayTeamScore = "0",
                    Handicap = "-0.2500",
                    IsWagerItemCancelled = "0",
                    SportsName = "Soccer",
                };
            }


            public static BetLogResponseInfo GetFromTpBetDetailApiResult(IMSportApiParamModel apiParam)
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