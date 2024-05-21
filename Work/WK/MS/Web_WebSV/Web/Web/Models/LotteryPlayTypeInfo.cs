using JxLottery.Services.BonusService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.SpaPlayConfig;
using JxBackendService.Interface.Model.User;

namespace Web.Models
{
    public static class LotteryPlayTypeInfoConst
    {
        public static readonly int[] SplitPlayTypeRadioId = new int[] { 6186 };
    }

    public class LotteryPlayTypeInfo
    {
        /// <summary>
        /// 結果
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 資料
        /// </summary>
        public PlayConfigData Data { get; set; }
    }

    /// <summary>
    /// 設定資料
    /// </summary>
    public class PlayConfigData
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="depositUrl">存款Url</param>
        /// <param name="infos">玩法資訊</param>
        /// <param name="service">彩票服務</param>
        /// <param name="lotteryUserData">userInfo</param>
        public PlayConfigData(string depositUrl, IList<PlayMode<PlayTypeInfoApiResult>> infos, IBonusService service, ILotteryUserData lotteryUserData)
        {
            DepositUrl = depositUrl;
            PlayConfigs = infos.Select(x => new PlayConfig(x, service, lotteryUserData)).ToArray();
        }

        /// <summary>
        /// 存款網址
        /// </summary>
        public string DepositUrl { get; set; }

        /// <summary>
        /// 玩法資訊
        /// </summary>
        public PlayConfig[] PlayConfigs { get; set; }
    }

    /// <summary>
    /// 玩法資訊
    /// </summary>
    public class PlayConfig
    {
        public PlayConfig(PlayMode<PlayTypeInfoApiResult> info, IBonusService service, ILotteryUserData lotteryUserData)
        {
            PlayModeId = info.PlayModeId;
            PlayTypeList = info.PlayTypeInfos.Select(x => new PlayTypeList(x, service, lotteryUserData)).ToArray();
        }

        /// <summary>
        /// 型態(專家，經典)
        /// </summary>
        public int PlayModeId { get; set; }

        /// <summary>
        /// 玩法大類
        /// </summary>
        public PlayTypeList[] PlayTypeList { get; set; }
    }

    /// <summary>
    /// 玩法大類
    /// </summary>
    public class PlayTypeList
    {
        public PlayTypeList(PlayTypeInfoApiResult info, IBonusService service, ILotteryUserData lotteryUserData)
        {
            PlayType = info.Info.PlayTypeID;
            PlayTypeName = info.Info.PlayTypeName;
            var rawDatas = info.PlayTypeRadioInfos.SelectMany(x => x.Value);
            var lists = rawDatas.Where(x => !LotteryPlayTypeInfoConst.SplitPlayTypeRadioId.Contains(x.Info.PlayTypeRadioID))
                .Select(x => new PlayTypeRadioList(x, service, lotteryUserData)).ToList();
            lists.AddRange(rawDatas.Where(x => LotteryPlayTypeInfoConst.SplitPlayTypeRadioId.Contains(x.Info.PlayTypeRadioID))
                .SelectMany(x => x.Fields, (radio, field) =>
                {
                    return new Tuple<PlayTypeRadioInfo, Field>(radio, field);
                })
                .GroupBy(x => x.Item2.Prompt, (key, tuples) =>
                {
                    return new PlayTypeRadioList(tuples.FirstOrDefault().Item1, tuples.Select(y => y.Item2).ToArray(), service, lotteryUserData);
                }));
            PlayTypeRadioList = lists.ToArray();
        }

        /// <summary>
        /// 玩法大類編號
        /// </summary>
        public int PlayType { get; set; }

        /// <summary>
        /// 玩法大類名稱
        /// </summary>
        public string PlayTypeName { get; set; }

        /// <summary>
        /// 玩法子類
        /// </summary>
        public PlayTypeRadioList[] PlayTypeRadioList { get; set; }
    }

    /// <summary>
    /// 玩法子類
    /// </summary>
    public class PlayTypeRadioList
    {
        public PlayTypeRadioList(PlayTypeRadioInfo x, IBonusService service, ILotteryUserData lotteryUserData)
        {
            PlayTypeRadio = x.Info.PlayTypeRadioID;
            PlayTypeRadioName = x.Info.PlayTypeRadioName;
            var playInfo = new JxLottery.Models.Lottery.Bet.PlayInfo()
            {
                LotteryId = service.LotteryId,
                PlayTypeId = x.Info.PlayTypeID.Value,
                PlayTypeRadioId = x.Info.PlayTypeRadioID,
                UserId = lotteryUserData.UserId,
                Rebate = 0.0M,
                UserRebate = (lotteryUserData?.RebatePro ?? 0.0M) + (lotteryUserData?.AddedRebatePro) ?? 0.0M
            };
            var numberOdds = service.GetNumberOdds(playInfo);
            SelectionList = x.Fields.SelectMany(number => number.Numbers).Select(item =>
            {
                var selection = item as string;
                var odds = numberOdds[selection];
                return new SelectionList()
                {
                    Selection = selection,
                    Odds = odds.ToString()
                };
            }).ToArray();
        }

        public PlayTypeRadioList(PlayTypeRadioInfo radio, Field[] fields, IBonusService service, ILotteryUserData lotteryUserData)
        {
            PlayTypeRadio = radio.Info.PlayTypeRadioID;
            PlayTypeRadioName = fields.FirstOrDefault().Prompt;
            PlayTypeRadioViewType = radio.ViewType[PlayTypeRadioName];
            var playInfo = new JxLottery.Models.Lottery.Bet.PlayInfo()
            {
                LotteryId = service.LotteryId,
                PlayTypeId = radio.Info.PlayTypeID.Value,
                PlayTypeRadioId = radio.Info.PlayTypeRadioID,
                UserId = lotteryUserData.UserId,
                Rebate = 0.0M,
                UserRebate = (lotteryUserData?.RebatePro ?? 0.0M) + (lotteryUserData?.AddedRebatePro) ?? 0.0M
            };
            var numberOdds = service.GetNumberOdds(playInfo);
            SelectionList = fields.SelectMany(number => number.Numbers, (field, num) =>
            {
                return new Tuple<Field, object>(field, num);
            }).Select(item =>
            {
                var selection = $"{item.Item1.Prompt} {item.Item2 as string}";
                var odds = numberOdds[selection];
                return new SelectionList()
                {
                    Selection = item.Item2 as string,
                    Odds = odds.ToString()
                };
            }).ToArray();
        }

        /// <summary>
        /// 玩法子類編號
        /// </summary>
        public int PlayTypeRadio { get; set; }

        /// <summary>
        /// 玩法子類名稱
        /// </summary>
        public string PlayTypeRadioName { get; set; }

        /// <summary>
        /// 玩法畫面顯示
        /// </summary>
        public string PlayTypeRadioViewType { get; set; }

        /// <summary>
        /// 頭注項列表
        /// </summary>
        public SelectionList[] SelectionList { get; set; }
    }

    /// <summary>
    /// 投注項內容
    /// </summary>
    public class SelectionList
    {
        /// <summary>
        /// 投注項
        /// </summary>
        public string Selection { get; set; }

        /// <summary>
        /// 賠率
        /// </summary>
        public string Odds { get; set; }
    }
}