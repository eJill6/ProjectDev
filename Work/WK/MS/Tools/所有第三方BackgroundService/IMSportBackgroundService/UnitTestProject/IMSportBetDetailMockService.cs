using IMBGDataBase.Merchant;
using IMSportDataBase.Common;
using IMSportDataBase.Enums;
using IMSportDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ThirdParty.IMSport;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using UnitTestN6;

namespace UnitTestProject
{
    public class IMSportBetDetailMockService : IMSportBetDetailMSLService
    {
        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(IMSportApiParamModel apiParam)
        {
            return MockDataUtil.GetMcokRemoteBetDetailApiResult(apiParam);
            //return MockDataUtil.GetFromTpBetDetailApiResult(apiParam);
        }

        public class MockDataUtil
        {
            public static BetLogResponseInfo GetMcokRemoteBetDetailApiResult(IMSportApiParamModel apiParam)
            {
                //decimal[] testOddses = new decimal[] { 1.5m, 1.65m, 1.8m };
                //foreach (IMSportWagerType imSportWagerType in IMSportWagerType.GetAll())
                //{
                //    foreach (decimal testOdds in testOddses)
                //    {
                //        Win
                //        result.Result.Add(CreateMockBetResult(
                //            playerId,
                //            IMSportBetStatus.Win,
                //            imSportWagerType,
                //            IMOneHandicap.EURO,
                //            testOdds,
                //            betMoney: 100,
                //            winMoney: 10,
                //            isCashout: false,
                //            idGeneratorService.CreateId().ToString()));

                //        Cashout
                //        result.Result.Add(CreateMockBetResult(
                //            playerId,
                //            betStatus: null,
                //            imSportWagerType,
                //            IMOneHandicap.EURO,
                //            testOdds,
                //            betMoney: 100,
                //            winMoney: 10,
                //            isCashout: true,
                //            idGeneratorService.CreateId().ToString()));
                //    }
                //}

                //Lose
                //result.Result.Add(CreateMockBetResult(
                //    playerId,
                //    betStatus: IMSportBetStatus.Lose,
                //    IMSportWagerType.Single,
                //    IMOneHandicap.EURO,
                //    odds: 1.5m,
                //    betMoney: 100,
                //    winMoney: -10,
                //    isCashout: false,
                //    idGeneratorService.CreateId().ToString()));

                //HalfLose
                //result.Result.Add(CreateMockBetResult(
                //    playerId,
                //    betStatus: IMSportBetStatus.HalfLose,
                //    IMSportWagerType.Single,
                //    IMOneHandicap.EURO,
                //    odds: 1.5m,
                //    betMoney: 100,
                //    winMoney: -10,
                //    isCashout: false,
                //    idGeneratorService.CreateId().ToString()));

                //HalfWin
                //result.Result.Add(CreateMockBetResult(
                //    playerId,
                //    betStatus: IMSportBetStatus.HalfWin,
                //    IMSportWagerType.Single,
                //    IMOneHandicap.EURO,
                //    odds: 1.5m,
                //    betMoney: 100,
                //    winMoney: 10,
                //    isCashout: false,
                //    idGeneratorService.CreateId().ToString()));

                //Draw
                //result.Result.Add(CreateMockBetResult(
                //    playerId,
                //    betStatus: IMSportBetStatus.Draw,
                //    IMSportWagerType.Single,
                //    IMOneHandicap.EURO,
                //    odds: 1.5m,
                //    betMoney: 100,
                //    winMoney: 0,
                //    isCashout: false,
                //    idGeneratorService.CreateId().ToString()));

                var betLogs = new List<BetResult>();

                TestUtil.DoPlayerJobs(
                    recordCount: 10000,
                    job: (playerId, betId) =>
                    {
                        betLogs.Add(CreateMockBetResult(
                            playerId,
                            betStatus: IMSportBetStatus.Draw,
                            IMSportWagerType.Single,
                            IMOneHandicap.EURO,
                            odds: 1.5m,
                            betMoney: 100,
                            winMoney: 0,
                            isCashout: false,
                            betId.ToString()));
                    });

                var result = new BetLogResult<List<BetResult>>
                {
                    Code = (int)APIErrorCode.Success,
                    Result = betLogs
                };
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