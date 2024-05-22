using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.OB.OBFI;
using JxBackendService.Model.ThirdParty.OB.OBSP;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TPGameOBSPApiMLSMockService : TPGameOBSPApiMSLService
    {
        public TPGameOBSPApiMLSMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var obspBetLogs = new List<OBSPBetLog>();

            var responseModel = new OBSPApiResponse<OBSPQueryBetListData>()
            {
                Data = new OBSPQueryBetListData()
                {
                    List = obspBetLogs
                }
            };

            long betIdByTime = DateTime.Now.ToUnixOfTime();
            decimal[] testOddses = new decimal[] { 1.5m, 1.65m, 1.8m };
            string playerId = "ctsD_3";

            foreach (WagerType wagerType in WagerType.GetAll())
            {
                foreach (decimal testOdds in testOddses)
                {
                    //Win
                    betIdByTime++;

                    obspBetLogs.Add(CreateMockBetResult(
                        playerId,
                        OBSPOutcome.Win,
                        wagerType,
                        OBSPHandicap.EU,
                        testOdds,
                        betMoney: 100,
                        winMoney: 10,
                        betIdByTime.ToString()));

                    //Cashout
                    betIdByTime++;

                    obspBetLogs.Add(CreateMockBetResult(
                        playerId,
                        OBSPOutcome.RefundCapital,
                        wagerType,
                        OBSPHandicap.EU,
                        testOdds,
                        betMoney: 100,
                        winMoney: 10,
                        betIdByTime.ToString()));
                }
            }

            //Lose
            betIdByTime++;

            obspBetLogs.Add(CreateMockBetResult(
                playerId,
                OBSPOutcome.Lose,
                WagerType.Single,
                OBSPHandicap.EU,
                1.5m,
                betMoney: 100,
                winMoney: -10,
                betIdByTime.ToString()));

            //HalfLose
            betIdByTime++;

            obspBetLogs.Add(CreateMockBetResult(
               playerId,
               OBSPOutcome.HalfLose,
               WagerType.Single,
               OBSPHandicap.EU,
               1.5m,
               betMoney: 100,
               winMoney: -10,
               betIdByTime.ToString()));

            //HalfWin
            betIdByTime++;

            obspBetLogs.Add(CreateMockBetResult(
               playerId,
               OBSPOutcome.HalfWin,
               WagerType.Single,
               OBSPHandicap.EU,
               1.5m,
               betMoney: 100,
               winMoney: 10,
               betIdByTime.ToString()));

            //Draw
            betIdByTime++;

            obspBetLogs.Add(CreateMockBetResult(
              playerId,
              OBSPOutcome.NoResult,
              WagerType.Single,
              OBSPHandicap.EU,
              1.5m,
              betMoney: 100,
              winMoney: 0,
              betIdByTime.ToString()));

            string[] responses = new string[] { responseModel.ToJsonString() };

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            {
                RequestBody = "1",
                ResponseContent = responses.ToJsonString()
            });
        }

        private OBSPBetLog CreateMockBetResult(string playerId, OBSPOutcome outcome, WagerType wagerType,
            OBSPHandicap handicap, decimal odds, decimal betMoney, decimal winMoney, string betId)
        {
            var obspBetLog = new OBSPBetLog()
            {
                UserName = playerId,
                OrderStatus = OBSPOrderStatus.Done.Value,
                Outcome = outcome.Value,
                OrderAmount = betMoney,
                ProfitAmount = winMoney,
                Memo = "test",
                OrderNo = betId,
                CreateTime = DateTime.Now.ToUnixOfTime(),
                SettleTime = DateTime.Now.ToUnixOfTime(),
                BetCount = 2,
                DetailList = new List<OBSPBetLogDetail>()
                {
                    new OBSPBetLogDetail()
                    {
                        SportName = "足球",
                        MatchName="土耳其联赛U19",
                        MatchInfo="班迪马士邦U19 v 埃尔祖鲁姆U19",
                        PlayName = "全场独赢",
                        PlayOptionName="班迪马士邦U19",
                        MarketType = handicap.Value,
                        OddsValue = odds.ToString(),
                    },
                    new OBSPBetLogDetail()
                    {
                        SportName = "足球",
                        MatchName="土耳其联赛U19",
                        MatchInfo="班迪马士邦U19 v 埃尔祖鲁姆U19",
                        PlayName = "全场独赢",
                        PlayOptionName="班迪马士邦U19",
                        MarketType = handicap.Value,
                        OddsValue = odds.ToString(),
                    }
                }
            };

            if (wagerType == WagerType.Single)
            {
                obspBetLog.BetCount = 1;
                obspBetLog.DetailList.RemoveRange(1, obspBetLog.DetailList.Count - 1);
            }

            return obspBetLog;
        }
    }
}