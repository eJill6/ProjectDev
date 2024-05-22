using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.FYES;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using System;
using System.Collections.Generic;

namespace UnitTestProject
{
    public class TPGameFYESApiMSLMockService : TPGameFYESApiMSLService
    {
        private readonly string _userName_jxd = "jxd_69778";

        private readonly string _UserName_ctsd = "ctsd_3";

        public TPGameFYESApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var requestBodys = new List<string>();
            var responseContents = new List<FYESGetBetLogResponseModel>();
            var list = new List<FYESBetLog>();

            // 单关类型
            list.Add(GetSingle());

            // 串关类型
            list.Add(GetCombo());

            // 趣味游戏类型
            list.Add(GetSmart());

            // 主播类型
            list.Add(GetAnchor());

            // 虚拟电竞类型（3.0新增）
            list.Add(GetVisualSport());

            FYESGetBetLogResponseModel model = new FYESGetBetLogResponseModel()
            {
                success = 1,
                msg = "6ms",
                info = new GetBetLogResponseModel()
                {
                    RecordCount = 124,
                    PageIndex = 1,
                    PageSize = 20,
                    list = list,
                },
            };

            string[] responses = new string[] { model.ToJsonString() };

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, new RequestAndResponse()
            {
                RequestBody = requestBodys.ToJsonString(),
                ResponseContent = responses.ToJsonString(),
            });
        }

        /// <summary>
        /// 单关类型
        /// </summary>
        /// <returns></returns>
        private FYESBetLog GetSingle()
        {
            return new FYESBetLog()
            {
                OrderID = "18337",
                UserName = _userName_jxd,
                Category = "League of legends",
                BetAmount = 10.0000m,
                BetMoney = 0.0000m,
                Money = 0.0000m,
                Status = GetRandomFYESBetLogStatusString(),
                CreateAt = DateTime.Parse("2020/4/11 15:15:41"),
                UpdateAt = DateTime.Parse("2020/4/12 6:31:44"),
                ResultAt = DateTime.Parse("1900/1/1 0:00:00"),
                RewardAt = DateTime.Parse("1900/1/1 0:00:00"),
                OddsType = "EU",
                Odds = 1.7312m,
                IP = "175.45.179.254",
                Language = "ENG",
                Platform = new List<string>() { "PC", "Windows" },
                Type = "Single",
                IsTest = false,
                Timestamp = 1588678163509,
                Currency = "CNY",
            };
        }

        /// <summary>
        /// 串关类型
        /// </summary>
        /// <returns></returns>
        private FYESBetLog GetCombo()
        {
            var details = new List<FYESBetLogDetail>();

            details.Add(new FYESBetLogDetail()
            {
                DetailID = "18361",
                CateID = "2",
                Category = "英雄联盟",
                LeagueID = "1",
                League = "2020 韩国职业联赛 春季赛",
                MatchID = "451",
                Match = "IMT Academy VS FlyQuest Academy",
                StartAt = DateTime.Parse("2020/4/12 0:00:00"),
                EndAt = DateTime.Parse("2020/4/12 3:00:00"),
                BetID = "12778",
                Bet = "{round}-两队皆击杀男爵",
                Content = "IMT.A",
                ResultAt = DateTime.Parse("1900/1/1 0:00:00"),
                Result = "",
                OddsType = "EU",
                Odds = 1.8796m,
                Status = GetRandomFYESBetLogStatusString(),
            });

            details.Add(new FYESBetLogDetail()
            {
                DetailID = "18362",
                CateID = "2",
                Category = "英雄联盟",
                LeagueID = "1",
                League = "2020 韩国职业联赛 春季赛",
                MatchID = "451",
                Match = "IMT Academy VS FlyQuest Academy",
                StartAt = DateTime.Parse("2020/4/12 0:00:00"),
                EndAt = DateTime.Parse("2020/4/12 3:00:00"),
                BetID = "12778",
                Bet = "{round}-两队皆击杀男爵",
                Content = "IMT.A",
                ResultAt = DateTime.Parse("1900/1/1 0:00:00"),
                Result = "",
                OddsType = "EU",
                Odds = 1.8796m,
                Status = GetRandomFYESBetLogStatusString(),
            });

            return new FYESBetLog()
            {
                Type = "Combo",
                OrderID = "124771",
                UserName = _userName_jxd,
                Status = GetRandomFYESBetLogStatusString(),
                CreateAt = DateTime.Parse("2020/03/31 22:57:01"),
                UpdateAt = DateTime.Parse("2020/03/31 22:57:03"),
                RewardAt = null,
                BetAmount = 10.0000m,
                BetMoney = 0.0000m,
                Money = 0.0000m,
                OddsType = "EU",
                Odds = 29.7263m,
                IP = "113.81.27.154",
                Language = "CHN",
                Platform = new List<string>() { "Mobile", "Android" },
                IsTest = true,
                Timestamp = 1588678163509,
                Currency = "CNY",
                Details = details,
            };
        }

        /// <summary>
        /// 趣味游戏类型
        /// </summary>
        /// <returns></returns>
        private FYESBetLog GetSmart()
        {
            return new FYESBetLog()
            {
                OrderID = "18360",
                Type = "Smart",
                UserName = _userName_jxd,
                Code = "LOL",
                CreateAt = DateTime.Parse("2020/4/11 17:47:26"),
                RewardAt = DateTime.Parse("2020/4/11 17:47:40"),
                UpdateAt = DateTime.Parse("2020/4/12 7:16:57"),
                BetAmount = 10.0000m,
                BetMoney = 10.0000m,
                Money = -10.0000m,
                Status = GetRandomFYESBetLogStatusString(),
                IsTest = true,
                IP = "13.73.17.192",
                Odds = 2.7600m,
                Currency = "CNY",
            };
        }

        /// <summary>
        /// 主播类型
        /// </summary>
        /// <returns></returns>
        private FYESBetLog GetAnchor()
        {
            return new FYESBetLog()
            {
                OrderID = "122325",
                UserName = _userName_jxd,
                Category = "英雄联盟",
                BetAmount = 10.00m,
                BetMoney = 0.00m,
                Money = 0.00m,
                Status = GetRandomFYESBetLogStatusString(),
                CreateAt = DateTime.Parse("2021/2/21 14:19:26"),
                UpdateAt = DateTime.Parse("2021/2/22 22:33:01"),
                Timestamp = 1614004381806,
                ResultAt = DateTime.Parse("2021/2/22 22:32:59"),
                RewardAt = DateTime.Parse("2021/2/22 22:32:59"),
                OddsType = "EU",
                Odds = 1.8600m,
                IP = "119.131.143.178",
                Language = "CHN",
                Platform = new List<string>() { "PC", "Windows" },
                Type = "Anchor",
                IsTest = true,
                Currency = "CNY"
            };
        }

        /// <summary>
        /// 虚拟电竞类型（3.0新增）
        /// </summary>
        /// <returns></returns>
        private FYESBetLog GetVisualSport()
        {
            return new FYESBetLog()
            {
                OrderID = "505786572051965320",
                UserName = _userName_jxd,
                Type = "VisualSport",
                Status = GetRandomFYESBetLogStatusString(),
                CreateAt = DateTime.Parse("2021/10/31 17:48:40"),
                ResultAt = DateTime.Parse("1900/1/1 0:00:00"),
                RewardAt = DateTime.Parse("1900/1/1 0:00:00"),
                UpdateAt = DateTime.Parse("2021/10/31 17:48:40"),
                Timestamp = 1635673720519,
                BetAmount = 10.0000m,
                BetMoney = 0.0000m,
                Money = 0.0000m,
                Language = "CHN",
                IP = "116.204.216.232",
                Platform = new List<string>() { "PC", "Windows" },
                Currency = "CNY",
                IsTest = true,
                Category = "街头霸王",
                OddsType = "MY",
                Odds = -0.306m
            };
        }

        private string GetRandomFYESBetLogStatusString()
        {
            var list = new List<FYESBetLogStatus>() {
                FYESBetLogStatus.Win,
                FYESBetLogStatus.Lose,
            };

            Random random = new Random();

            int randomIndex = random.Next(0, list.Count);

            return list[randomIndex].Value;
        }
    }
}