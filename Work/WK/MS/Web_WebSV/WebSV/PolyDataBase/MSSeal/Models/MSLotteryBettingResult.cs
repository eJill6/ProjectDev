using JxBackendService.Model.Enums.MiseOrder;
using JxLottery.Services.Extensions;
using SLPolyGame.Web.Enums;
using SLPolyGame.Web.Model;
using SLPolyGame.Web.MSSeal.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLPolyGame.Web.MSSeal.Models
{
    public class MSLotteryBettingResult : IGameIdInfo
    {
        private static readonly IDictionary<int, IDictionary<string, string[][]>> TurnOvers = new Dictionary<int, IDictionary<string, string[][]>>()
        {
            {
                (int)JxLottery.Adapters.Models.Lottery.LotteryId.OMKS,
                new Dictionary<string, string[][]>()
                {
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.K3.PlayTypeRadio.LiangMian.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"大", "小"},
                            new string[]{"单", "双"},
                        }
                    }
                }
            },
            {
                (int)JxLottery.Adapters.Models.Lottery.LotteryId.OMPK10,
                new Dictionary<string, string[][]>()
                {
                    {
                        "冠亚和两面",
                        new string[][]
                        {
                            new string[]{"和大", "和小"},
                            new string[]{"和单", "和双"},
                        }
                    },
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.PK10.PlayTypeRadio.KuaiXuanGuanJunTeShu.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"大", "小"},
                            new string[]{"单", "双"},
                            new string[]{"大单", "大双"},
                            new string[]{"小单", "小双"},
                        }
                    },
                    {
                        //冠亚和
                        JxLottery.Models.Lottery.PlayTypeRadio.PK10.PlayTypeRadio.KuaiXuanGuanYaHe.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19"},
                        }
                    },
                    {
                        //冠軍
                        JxLottery.Models.Lottery.PlayTypeRadio.PK10.PlayTypeRadio.KuaiXuanGuanJun.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{ "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"},
                        }
                    }
                }
            },
            {
                (int)JxLottery.Adapters.Models.Lottery.LotteryId.OMSSC,
                new Dictionary<string, string[][]>()
                {
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.SSC.PlayTypeRadio.KuaiXuanZongHe.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"大", "小"},
                            new string[]{"单", "双"},
                        }
                    },
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.SSC.PlayTypeRadio.KuaiXuanLongHuQiu1VsQiu5.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"龙", "虎"},
                        }
                    }
                }
            },
            {
                (int)JxLottery.Adapters.Models.Lottery.LotteryId.OMLHC,
                new Dictionary<string, string[][]>()
                {
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.LHC.PlayTypeRadio.TeMaLiangMian.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"大", "小"},
                            new string[]{"单", "双"},
                        }
                    },
                    {
                        //特碼
                        JxLottery.Models.Lottery.PlayTypeRadio.LHC.PlayTypeRadio.KuaiXuanTaMa.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"01", "02", "03", "04", "05", "06", "07", "08", "09", "10"
                            , "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"
                            , "21", "22", "23", "24", "25", "26", "27", "28", "29", "30"
                            , "31", "32", "33", "34", "35", "36", "37", "38", "39", "40"
                            , "41", "42", "43", "44", "45", "46", "47", "48", "49"},
                        }
                    },
                    {
                        //特碼生肖
                        JxLottery.Models.Lottery.PlayTypeRadio.LHC.PlayTypeRadio.KuaiXuanTaMaShengXiao.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"鼠","牛","虎","兔","龙","蛇","马","羊","猴","鸡","狗","猪"},
                        }
                    },
                    {
                        //特碼色波
                        JxLottery.Models.Lottery.PlayTypeRadio.LHC.PlayTypeRadio.KuaiXuanTaMaSeBo.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"红波","蓝波","绿波"},
                        }
                    }
                }
            },
            {
                (int)JxLottery.Adapters.Models.Lottery.LotteryId.NUINUI,
                new Dictionary<string, string[][]>()
                {
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.NuiNui.PlayTypeRadio.KuaiXuanShengFu.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"蓝方胜", "红方胜"},
                        }
                    },
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.NuiNui.PlayTypeRadio.KuaiXuanLongHuPai1VSPai5.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"龙", "虎"},
                        }
                    },
                }
            },
            {
                (int)JxLottery.Adapters.Models.Lottery.LotteryId.JSBaccarat,
                new Dictionary<string, string[][]>()
                {
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.Baccarat.PlayTypeRadio.KuaiXuanZhuangXian.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{ "庄", "闲"},
                        }
                    },
                }
            },
            {
                (int)JxLottery.Adapters.Models.Lottery.LotteryId.JSLP,
                new Dictionary<string, string[][]>()
                {
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.LP.PlayTypeRadio.KuaiXuanHongHei.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"红", "黑"},
                            new string[]{"大", "小"},
                            new string[]{"单", "双"},
                        }
                    },
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.LP.PlayTypeRadio.KuaiXuanZhiZhu.ExtGetDescription(),
                        new string[][]
                        {
                            Enumerable.Range(0, 37).Select(x => x.ToString()).ToArray()
                        }
                    },
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.LP.PlayTypeRadio.KuaiXuanZuHe.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"1-12", "13-24", "25-36"},
                        }
                    },

                }
            },
            {
                (int)JxLottery.Adapters.Models.Lottery.LotteryId.JSYXX,
                new Dictionary<string, string[][]>()
                {
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.YXX.PlayTypeRadio.KuaiXuanDaXiao.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"大", "小"},
                            new string[]{"单", "双"},
                        }
                    },
                    {
                        // JxLottery.Services.BonusService.Calculators.YXX.Const.YinXiangZongHeCalculator.ManPeis
                        
                        JxLottery.Models.Lottery.PlayTypeRadio.YXX.PlayTypeRadio.KuaiXuanDanTou.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"鱼","虾","葫芦","铜钱","蟹","鸡"},
                        }
                    },
                }
            },
            {
                (int)JxLottery.Adapters.Models.Lottery.LotteryId.JSSG,
                new Dictionary<string, string[][]>()
                {
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.SG.PlayTypeRadio.KuaiXuanZhuangXian.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"闲", "和", "庄"},
                        }
                    },
                    {
                        // JxLottery.Services.BonusService.Calculators.SG.Const.YinXiangZongHeCalculator.ManPeis
                        JxLottery.Models.Lottery.PlayTypeRadio.SG.PlayTypeRadio.KuaiXuanSanGong.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"闲三公","庄三公"},
                        }
                    },
                }
            },
            {
                (int)JxLottery.Adapters.Models.Lottery.LotteryId.JSLH,
                new Dictionary<string, string[][]>()
                {
                    {
                        JxLottery.Models.Lottery.PlayTypeRadio.LH.PlayTypeRadio.KuaiXuanLongHu.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"龙", "和", "虎"},
                        }
                    },
                    {
                        // JxLottery.Services.BonusService.Calculators.LH.Const.YinXiangZongHeCalculator.ManPeis
                        JxLottery.Models.Lottery.PlayTypeRadio.LH.PlayTypeRadio.KuaiXuanHongHei.ExtGetDescription(),
                        new string[][]
                        {
                            new string[]{"红龙","黑龙", "红虎","黑虎"},
                        }
                    },
                }
            }
        };

        public MSLotteryBettingResult()
        { }

        public MSLotteryBettingResult(PalyInfo playInfo)
        {
            var format = "yyyy-MM-dd HH:mm:ss";
            PlayId = playInfo.PalyCurrentNum;
            SerialNumber = playInfo.PalyID;
            UserId = playInfo.UserID.Value.ToString();
            Nickname = playInfo.UserName;
            RoomNumber = playInfo.RoomId;
            Amount = playInfo.NoteMoney.ToString();
            CreateTime = playInfo.NoteTime?.ToString(format);
            SettleTime = playInfo.CurrentLotteryTime?.ToString(format) ?? string.Empty;

            switch ((AwardStatus)playInfo.IsFactionAward)
            {
                case AwardStatus.IsDone:
                    if (playInfo.IsWin)
                    {
                        Status = "1";
                    }
                    else
                    {
                        Status = "2";
                    }
                    ProfitLoss = playInfo.WinMoney.ToString();
                    break;

                case AwardStatus.SystemCancel:
                    Status = "3";
                    break;

                case AwardStatus.Unawarded:
                    Status = playInfo.IsFactionAward.ToString();
                    break;

                default:
                    //(比如 - 1表示注单撤销，0表示未结算，1表示赢，2表示亏，3表示平局）
                    Status = "-1";
                    ProfitLoss = "0";
                    break;
            }

            GameId = playInfo.LotteryID.ToString();
            GameName = playInfo.LotteryType;
            GameDetail = playInfo.PalyNum;
            PeriodNumber = playInfo.PalyCurrentNum;
            GameResult = playInfo.CurrentLotteryNum ?? string.Empty;
            Turnover = playInfo.WinMoney == 0 ? 0 : CalcTurnover(playInfo);

            MiseOrderGameId miseOrderGameId = MiseOrderGameId.GetSingle(playInfo.LotteryID.ToString());

            if (miseOrderGameId != null)
            {
                this.SetByGameId(miseOrderGameId);
            }
        }

        /// <summary>
        /// 計算有效流水
        /// </summary>
        /// <param name="playInfo">注單資訊</param>
        /// <returns>有效流水</returns>
        private decimal CalcTurnover(PalyInfo playInfo)
        {
            var items = playInfo.PalyNum.Split('|').GroupBy(x => x.Split(' ')[0]).ToDictionary(x => x.Key, x => x.Select(item => item.Split(' ')[1]).ToArray());
            var result = 0.0M;

            if(!TurnOvers.ContainsKey(playInfo.LotteryID.Value))
            {
                return result;
            }

            var turnOver = TurnOvers[playInfo.LotteryID.Value];

            if (turnOver.Equals(default(IDictionary<string, string[][]>)))
            {
                return result;
            }

            foreach (var item in items)
            {
                if (turnOver.ContainsKey(item.Key))
                {
                    var checkResult = turnOver[item.Key].Where(betItems => betItems.All(b => item.Value.Contains(b))).ToArray();
                    if (!checkResult.Any())
                    {
                        // 都沒找到對打的投注項
                        result += item.Value.Length * playInfo.SingleMoney.Value;
                    }
                    else
                    {
                        // 目前單注金額一樣 所以不做事
                        var count = item.Value.Where(x => !checkResult.Any(c => c.Contains(x))).Count();
                        result += count * playInfo.SingleMoney.Value;
                    }
                }
                else
                {
                    // 沒有限制有效流水的玩法
                    result += item.Value.Length * playInfo.SingleMoney.Value;
                }
            }
            return result;
        }

        /// <summary> 局ID </summary>
        public string PlayId { get; set; }

        /// <summary> 订单号 </summary>
        public string SerialNumber { get; set; }

        /// <summary> 用户ID </summary>
        public string UserId { get; set; }

        /// <summary> 用户昵称 </summary>
        public string Nickname { get; set; }

        /// <summary> 房间号 </summary>
        public string RoomNumber { get; set; }

        /// <summary> 本金 </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 创建时间，格式为 年月日 时分秒 2022-10-01 08:10:10
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 结算时间，格式为 年月日 时分秒 2022-10-01 08:10:10
        /// </summary>
        public string SettleTime { get; set; }

        /// <summary>
        /// 状态 (比如-1表示注单撤销，0表示未结算，1表示赢，2表示亏，3表示平局）
        /// </summary>
        public string Status { get; set; }

        /// <summary> 亏盈 </summary>
        public string ProfitLoss { get; set; }

        /// <summary>遊戲第1級分類</summary>
        public string Type { get; set; }

        /// <summary>遊戲第2級分類</summary>
        public string SubType { get; set; }

        /// <summary>
        /// 游戏ID
        /// </summary>
        public string GameId { get; set; }

        /// <summary>
        /// 游戏名
        /// </summary>
        public string GameName { get; set; }

        /// <summary> 游戏内容 </summary>
        public string GameDetail { get; set; }

        /// <summary> 期號 </summary>
        public string PeriodNumber { get; set; }

        /// <summary> 遊戲結果(當期號碼) </summary>
        public string GameResult { get; set; }

        /// <summary>
        /// 有效流水
        /// </summary>
        public decimal Turnover { get; set; }
    }
}