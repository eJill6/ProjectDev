using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using ProductTransferService.SportDataBase.BLL;
using ProductTransferService.SportDataBase.Merchant;
using ProductTransferService.SportDataBase.Model;
using UnitTestN6;

namespace UnitTestProject
{
    public class SportBetDetailMSLMockService : SportBetDetailMSLService
    {
        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(SportApiParamModel apiParam)
        {
            return MockDataUtil.GetRemoteBetDetailApiResult(apiParam);
        }

        //protected override string GetRemoteBetDetailApiResult(SportApiParamModel apiParam)
        //{
        //    return "{\"error_code\":0,\"message\":\"\",\"Data\":{\"last_version_key\":6026559,\"BetDetails\":[{\"trans_id\":113600127819,\"vendor_member_id\":\"qatest03\",\"operator_id\":\"Hecbet\",\"league_id\":102499,\"leaguename\":[{\"lang\":\"en\",\"name\":\"*NBA Championship 2022 Winner\"},{\"lang\":\"cs\",\"name\":\"*2022美国职业篮球赛总冠军\"}],\"match_id\":45047104,\"home_id\":17892,\"hometeamname\":[{\"lang\":\"en\",\"name\":\"The Field\"},{\"lang\":\"cs\",\"name\":\"其它\"}],\"away_id\":168769,\"awayteamname\":[{\"lang\":\"en\",\"name\":\"Brooklyn Nets\"},{\"lang\":\"cs\",\"name\":\"布鲁克林篮网\"}],\"match_datetime\":\"2022-02-24T20:05:00\",\"sport_type\":2,\"sportname\":[{\"lang\":\"en\",\"name\":\"Basketball\"},{\"lang\":\"cs\",\"name\":\"篮球\"}],\"bet_type\":1,\"bettypename\":[{\"lang\":\"en\",\"name\":\"Handicap\"},{\"lang\":\"cs\",\"name\":\"让球\"}],\"parlay_ref_no\":0,\"odds\":0.2900,\"stake\":2.0000,\"transaction_time\":\"2021-10-19T02:23:08.443\",\"ticket_status\":\"won\",\"winlost_amount\":0.5800,\"after_amount\":99352.1857,\"currency\":20,\"winlost_datetime\":\"2022-04-25T00:00:00\",\"odds_type\":1,\"bet_team\":\"h\",\"isLucky\":\"False\",\"home_hdp\":0.0000,\"away_hdp\":0.0000,\"hdp\":0.0,\"betfrom\":\"x\",\"islive\":\"0\",\"home_score\":null,\"away_score\":null,\"settlement_time\":\"2022-04-25T23:48:50.2\",\"customInfo1\":\"\",\"customInfo2\":\"\",\"customInfo3\":\"\",\"customInfo4\":\"\",\"customInfo5\":\"\",\"ba_status\":\"0\",\"version_key\":6026558,\"ParlayData\":null},{\"trans_id\":113693264828,\"vendor_member_id\":\"twqa06\",\"operator_id\":\"Hecbet\",\"league_id\":102499,\"leaguename\":[{\"lang\":\"en\",\"name\":\"*NBA Championship 2022 Winner\"},{\"lang\":\"cs\",\"name\":\"*2022美国职业篮球赛总冠军\"}],\"match_id\":45047104,\"home_id\":17892,\"hometeamname\":[{\"lang\":\"en\",\"name\":\"The Field\"},{\"lang\":\"cs\",\"name\":\"其它\"}],\"away_id\":168769,\"awayteamname\":[{\"lang\":\"en\",\"name\":\"Brooklyn Nets\"},{\"lang\":\"cs\",\"name\":\"布鲁克林篮网\"}],\"match_datetime\":\"2022-02-24T20:05:00\",\"sport_type\":2,\"sportname\":[{\"lang\":\"en\",\"name\":\"Basketball\"},{\"lang\":\"cs\",\"name\":\"篮球\"}],\"bet_type\":1,\"bettypename\":[{\"lang\":\"en\",\"name\":\"Handicap\"},{\"lang\":\"cs\",\"name\":\"让球\"}],\"parlay_ref_no\":0,\"odds\":0.3100,\"stake\":1.0000,\"transaction_time\":\"2021-10-28T04:11:42.943\",\"ticket_status\":\"half won\",\"winlost_amount\":0.3100,\"after_amount\":99993.9400,\"currency\":20,\"winlost_datetime\":\"2022-04-25T00:00:00\",\"odds_type\":1,\"bet_team\":\"h\",\"isLucky\":\"False\",\"home_hdp\":0.0000,\"away_hdp\":0.0000,\"hdp\":0.0,\"betfrom\":\"z\",\"islive\":\"0\",\"home_score\":null,\"away_score\":null,\"settlement_time\":\"2022-04-25T23:48:50.2\",\"customInfo1\":\"\",\"customInfo2\":\"\",\"customInfo3\":\"\",\"customInfo4\":\"\",\"customInfo5\":\"\",\"ba_status\":\"0\",\"version_key\":6026559,\"ParlayData\":null}]}}";
        //}
    }

    public class MockDataUtil
    {
        public static BetLogResponseInfo GetRemoteBetDetailApiResult(SportApiParamModel apiParam)
        {
            var apiResult = new ApiResult<BetResult>
            {
                Data = new BetResult()
                {
                    last_version_key = DateTime.Now.ToUnixOfTime().ToString(),
                    BetDetails = new List<BetDetails>()
                }
            };

            decimal[] testOddses = new decimal[] { 1.5m, 1.65m, 1.8m };

            TestUtil.DoPlayerJobs(                
                recordCount: 5000,
                job: (playerId, betId) =>
                {
                    DateTime transactionTime = DateTime.UtcNow.AddDays(-1);

                    apiResult.Data.BetDetails.Add(CreateMockBetResult(
                        playerId,
                        SabaTicketStatus.HalfLose,
                        ticketExtraStatus: null,
                        betType: "1",
                        SabaSportHandicap.China,
                        testOddses.First(),
                        betMoney: 100,
                        winMoney: 0,
                        betId: betId.ToString(),
                        transactionTime: transactionTime.ToFormatDateTimeString(),
                        settlementTime: transactionTime.AddHours(1).ToFormatDateTimeString()));

                    betId = TestUtil.CreateId();

                    apiResult.Data.BetVirtualSportDetails.Add(CreateMockBetResult(
                        playerId,
                        SabaTicketStatus.Draw,
                        ticketExtraStatus: null,
                        betType: "1",
                        SabaSportHandicap.Europe,
                        testOddses.First(),
                        betMoney: 666,
                        winMoney: 0,
                        betId.ToString(),
                        transactionTime: transactionTime.ToFormatDateTimeString(),
                        settlementTime: transactionTime.AddHours(-1).ToFormatDateTimeString()).CastByJson<BetVirtualSportDetails>());

                    betId = TestUtil.CreateId();

                    apiResult.Data.BetCasinoDetails.Add(CreateMockBetResult(
                        playerId,
                        SabaTicketStatus.Draw,
                        ticketExtraStatus: null,
                        betType: "1",
                        SabaSportHandicap.America,
                        testOddses.First(),
                        betMoney: 888,
                        winMoney: 0,
                        betId.ToString(),
                        transactionTime: transactionTime.ToFormatDateTimeString()).CastByJson<BetCasinoDetails>());
                });

            return new BetLogResponseInfo() { ApiResult = apiResult.ToJsonString() };
        }

        private static BetDetails CreateMockBetResult(string playerId, SabaTicketStatus sabaTicketStatus, string ticketExtraStatus, string betType,
            SabaSportHandicap handicap, decimal odds, decimal betMoney, decimal winMoney, string betId, string transactionTime, string settlementTime = "")
        {
            string winlostAmount = winMoney.ToString();

            if (winMoney > 0)
            {
                winlostAmount = "+" + winlostAmount;
            }
            if (settlementTime.IsNullOrEmpty())
            {
                settlementTime = string.Empty;
            }

            return new BetDetails()
            {
                Bet_type = betType,
                BettypeName = new List<LangName>() { new LangName() { Lang = "cs", Name = "Quarter 1 Over / Under" } },
                Sport_type = "2",
                SportName = new List<LangName>() { new LangName() { Lang = "cs", Name = "篮球" } },
                League_id = "93817",
                LeagueName = new List<LangName>() { new LangName() { Lang = "cs", Name = "美国职业篮球赛总冠军 获胜队" } },
                Home_id = "667804",
                HometeamName = new List<LangName>() { new LangName() { Lang = "cs", Name = "成都.AG" } },
                Away_id = "714227",
                AwayteamName = new List<LangName>() { new LangName() { Lang = "cs", Name = "洛杉矶湖人" } },
                After_amount = "10.98",
                Currency = "20",
                Match_datetime = DateTime.Now.AddHours(-12).ToFormatDateTimeString(),
                Winlost_datetime = DateTime.Now.AddHours(-12).ToFormatDateTimeString(),
                Transaction_time = transactionTime,
                Settlement_time = settlementTime,
                Match_id = "39101673",
                Odds = odds.ToString(),
                Odds_type = handicap.Value,
                Stake = betMoney.ToString(),
                Ticket_status = sabaTicketStatus.Value,
                Ticket_extra_status = ticketExtraStatus,
                Version_key = "5378705",
                Winlost_amount = winlostAmount,
                Vendor_member_id = playerId,
                Trans_id = betId,
                ParlayData = new List<SabaParlay>()
                {
                    new SabaParlay()
                    {
                        Parlay_id = betId,
                        Bet_type = betType,
                        BettypeName = new List<LangName>() { new LangName() { Lang = "cs", Name = "Quarter 1 Over / Under" } },
                        Sport_type = "2",
                        SportName = new List<LangName>() { new LangName() { Lang = "cs", Name = "篮球" } },
                        League_id = "93817",
                        LeagueName = new List<LangName>() { new LangName() { Lang = "cs", Name = "美国职业篮球赛总冠军 获胜队" } },
                        Home_id = "667804",
                        HometeamName = new List<LangName>() { new LangName() { Lang = "cs", Name = "成都.AG" } },
                        Away_id = "714227",
                        AwayteamName = new List<LangName>() { new LangName() { Lang = "cs", Name = "洛杉矶湖人" } },
                        Match_id = "39101673",
                        Match_datetime = DateTime.Now.AddHours(-12).ToFormatDateTimeString(),
                        Bet_team = "",
                        Home_hdp = "",
                        Away_hdp = "",
                        Hdp = "",
                        Islive = "",
                        Home_score = "",
                        Away_score = "",
                        Ticket_status = sabaTicketStatus.Value,
                        Winlost_datetime = DateTime.Now.AddHours(-12).ToFormatDateTimeString(),
                        Odds = odds.ToString(),
                    },
                    new SabaParlay()
                    {
                        Parlay_id = betId,
                        Bet_type = betType,
                        BettypeName = new List<LangName>() { new LangName() { Lang = "cs", Name = "Quarter 1 Over / Under" } },
                        Sport_type = "2",
                        SportName = new List<LangName>() { new LangName() { Lang = "cs", Name = "篮球" } },
                        League_id = "93817",
                        LeagueName = new List<LangName>() { new LangName() { Lang = "cs", Name = "美国职业篮球赛总冠军 获胜队" } },
                        Home_id = "667804",
                        HometeamName = new List<LangName>() { new LangName() { Lang = "cs", Name = "成都.AG" } },
                        Away_id = "714227",
                        AwayteamName = new List<LangName>() { new LangName() { Lang = "cs", Name = "洛杉矶湖人" } },
                        Match_id = "39101673",
                        Match_datetime = DateTime.Now.AddHours(-12).ToFormatDateTimeString(),
                        Bet_team = "",
                        Home_hdp = "",
                        Away_hdp = "",
                        Hdp = "",
                        Islive = "",
                        Home_score = "",
                        Away_score = "",
                        Ticket_status = sabaTicketStatus.Value,
                        Winlost_datetime = DateTime.Now.AddHours(-12).ToFormatDateTimeString(),
                        Odds = odds.ToString(),
                    }
                }
            };
        }
    }
}