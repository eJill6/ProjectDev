using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using LCDataBase.Enums;
using LCDataBase.Model;

namespace LCDataBase.Merchant
{
    public class LCBetDetailMockService : LCBetDetailMSLService
    {
        protected override BetLogResponseInfo GetRemoteBetDetailApiResult(LCApiParamModel apiParam)
        {
            var result = new ApiResult<BetResult>()
            {
                Data = new BetResult()
                {
                    Code = (int)APIErrorCode.Success,
                    BetDetails = new BetDetails()
                }
            };

            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode(JxApplication.LCTransferService).AccountPrefixCode;
            string[] userCodes = new string[]
            {
                $"jx{accountPrefixCode}_69778",
                $"cts{accountPrefixCode}_3",
                $"cts{accountPrefixCode}_36",
                $"msl{accountPrefixCode}_588",
                $"msl{accountPrefixCode}_888",
            };

            string agentID = ConfigurationManager.AppSettings["AgentID"].Trim();
            string linecode = ConfigurationManager.AppSettings["LCLinecode"].Trim();
            string lcUserHeader = agentID + "_";
            string jxPlayerHeader = agentID + "_" + linecode;

            List<BetDetail> betDetails = new List<BetDetail>();

            for (int i = 0; i < userCodes.Length; i++)
            {
                string userCode = userCodes[i];

                betDetails.Add(new BetDetail
                {
                    Accounts = lcUserHeader + userCode,
                    LineCode = jxPlayerHeader,
                    CellScore = "50",
                    AllBet = "100",
                    Profit = "10",
                    Revenue = "10",
                    GameStartTime = DateTime.Now.ToFormatDateTimeString(),
                    GameEndTime = DateTime.Now.ToFormatDateTimeString(),
                    KindID = (int)GameKind.K220,
                    ServerID = (int)RoomType.R2201,
                    GameID = DateTime.Now.AddSeconds(i).ToUnixOfTime().ToString(),
                    TableID = 0,
                    ChairID = 0,
                    UserCount = 0,
                    CardValue = "0",
                    ChannelID = 0
                });
            }

            result.Data.BetDetails = new BetDetails()
            {
                Accounts = betDetails.Select(s => s.Accounts).ToArray(),
                LineCode = betDetails.Select(s => s.LineCode).ToArray(),
                CellScore = betDetails.Select(s => s.CellScore).ToArray(),
                AllBet = betDetails.Select(s => s.AllBet).ToArray(),
                Profit = betDetails.Select(s => s.Profit).ToArray(),
                Revenue = betDetails.Select(s => s.Revenue).ToArray(),
                GameStartTime = betDetails.Select(s => s.GameStartTime).ToArray(),
                GameEndTime = betDetails.Select(s => s.GameEndTime).ToArray(),
                KindID = betDetails.Select(s => s.KindID).ToArray(),
                ServerID = betDetails.Select(s => s.ServerID).ToArray(),
                GameID = betDetails.Select(s => s.GameID).ToArray(),
                TableID = betDetails.Select(s => s.TableID).ToArray(),
                ChairID = betDetails.Select(s => s.ChairID).ToArray(),
                UserCount = betDetails.Select(s => s.UserCount).ToArray(),
                CardValue = betDetails.Select(s => s.CardValue).ToArray(),
                ChannelID = betDetails.Select(s => s.ChannelID).ToArray(),
            };

            return new BetLogResponseInfo() { ApiResult = result.ToJsonString() };
        }
    }

    public class BetDetail
    {
        /// <summary>
        /// 使用者id
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 游戏局号列表
        /// </summary>
        public string GameID { get; set; }

        /// <summary>
        /// 玩家帐号列表
        /// </summary>
        public string Accounts { get; set; }

        /// <summary>
        /// 房间 ID 列表
        /// </summary>
        public int ServerID { get; set; }

        /// <summary>
        /// 游戏 ID 列表
        /// </summary>
        public int KindID { get; set; }

        /// <summary>
        /// 桌子号列表
        /// </summary>
        public long TableID { get; set; }

        /// <summary>
        /// 椅子号列表
        /// </summary>

        public int ChairID { get; set; }

        /// <summary>
        /// 玩家数量列表
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// 有效下注
        /// </summary>
        public string CellScore { get; set; }

        /// <summary>
        /// 总下注列表
        /// </summary>
        public string AllBet { get; set; }

        /// <summary>
        /// 盈利列表
        /// </summary>
        public string Profit { get; set; }

        /// <summary>
        /// 抽水列表
        /// </summary>
        public string Revenue { get; set; }

        /// <summary>
        /// 游戏开始时间列表
        /// </summary>
        public string GameStartTime { get; set; }

        /// <summary>
        /// 游戏结束时间列表
        /// </summary>
        public string GameEndTime { get; set; }

        /// <summary>
        /// 手牌公共牌
        /// </summary>
        public string CardValue { get; set; }

        /// <summary>
        /// 渠道 ID 列表
        /// </summary>
        public int ChannelID { get; set; }

        /// <summary>
        /// 游戏结果对应玩家所属站点
        /// </summary>
        public string LineCode { get; set; }
    }
}