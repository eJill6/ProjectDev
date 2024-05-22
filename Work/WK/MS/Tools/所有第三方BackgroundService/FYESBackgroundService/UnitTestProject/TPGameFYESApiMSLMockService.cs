using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.FYES;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;

namespace UnitTestProject
{
    public class TPGameFYESApiMSLMockService : TPGameFYESApiMSLService
    {
        public TPGameFYESApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode().AccountPrefixCode.ToLower();
            string merchant = OBEBSharedAppSetting.MerchantCode.ToLower();
            string[] playerIds = new string[]
            {
                $"{merchant}jx{accountPrefixCode}_69778",
                $"{merchant}cts{accountPrefixCode}_3",
                $"{merchant}cts{accountPrefixCode}_36",
                $"{merchant}msl{accountPrefixCode}_588",
                $"{merchant}msl{accountPrefixCode}_888",
            };

            var requestBodys = new List<string>();
            var responseContents = new List<FYESGetBetLogResponseModel>();
            var list = new List<FYESBetLog>();

            foreach (string tpUserName in playerIds)
            {
                // 单关类型
                list.Add(GetSingle(tpUserName));

                // 串关类型
                list.Add(GetCombo(tpUserName));

                // 趣味游戏类型
                list.Add(GetSmart(tpUserName));

                // 主播类型
                list.Add(GetAnchor(tpUserName));

                // 虚拟电竞类型（3.0新增）
                list.Add(GetVisualSport(tpUserName));
            }

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
        private FYESBetLog GetSingle(string tpUserName)
        {
            return new FYESBetLog()
            {
                OrderID = "18337",
                UserName = tpUserName,
                Category = "League of legends",
                BetAmount = 10.0000m,
                BetMoney = 0.0000m,
                Money = 0.0000m,
                Status = GetRandomFYESBetLogStatusString(),
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
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
        private FYESBetLog GetCombo(string tpUserName)
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
                StartAt = DateTime.Now,
                EndAt = DateTime.Now,
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
                StartAt = DateTime.Now,
                EndAt = DateTime.Now,
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
                UserName = tpUserName,
                Status = GetRandomFYESBetLogStatusString(),
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
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
        private FYESBetLog GetSmart(string tpUserName)
        {
            return new FYESBetLog()
            {
                OrderID = "18360",
                Type = "Smart",
                UserName = tpUserName,
                Code = "LOL",
                CreateAt = DateTime.Now,
                RewardAt = DateTime.Now,
                UpdateAt = DateTime.Now,
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
        private FYESBetLog GetAnchor(string tpUserName)
        {
            return new FYESBetLog()
            {
                OrderID = "122325",
                UserName = tpUserName,
                Category = "英雄联盟",
                BetAmount = 10.00m,
                BetMoney = 0.00m,
                Money = 0.00m,
                Status = GetRandomFYESBetLogStatusString(),
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                Timestamp = 1614004381806,
                ResultAt = DateTime.Now,
                RewardAt = DateTime.Now,
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
        private FYESBetLog GetVisualSport(string tpUserName)
        {
            return new FYESBetLog()
            {
                OrderID = "505786572051965320",
                UserName = tpUserName,
                Type = "VisualSport",
                Status = GetRandomFYESBetLogStatusString(),
                CreateAt = DateTime.Now,
                ResultAt = DateTime.Parse("1900/1/1 0:00:00"),
                RewardAt = DateTime.Parse("1900/1/1 0:00:00"),
                UpdateAt = DateTime.Now,
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