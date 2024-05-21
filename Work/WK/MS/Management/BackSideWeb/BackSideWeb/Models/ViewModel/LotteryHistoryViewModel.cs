using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models.PlayBetRecord;
using MS.Core.MMModel.Models.PlayBetRecord.Enum;
using MS.Core.MMModel.Extensions;
using MS.Core.Models.Models;
using JxBackendService.Model.BackSideWeb;
using MS.Core.MMModel.Models.Lottery.Enum;
using JxBackendService.Model.Enums.MiseOrder;
using BackSideWeb.Helpers;
using static BackSideWeb.Helpers.NuiNuiGame;
using System.Text;

namespace BackSideWeb.Models.ViewModel.PlayBetRecord
{
    public class LotteryHistoryViewModel : CurrentLotteryInfo
    {
        public List<SelectListItem>? LotteryIdSelectList { get; set; }
        public List<SelectListItem>? IsLotterySelectList { get; set; }
        public string LotteryProcessingTime
        {
            get
            {
                if (CurrentLotteryTime == null || UpdateTime == null)
                {
                    return string.Empty;
                }
                else
                {
                    TimeSpan timeDifference = (TimeSpan)(UpdateTime - CurrentLotteryTime);
                    int sec = (int)Math.Floor(timeDifference.TotalSeconds);
                    return sec + "秒";
                }
            }
        }
        public string CurrentLotteryTimeText => CurrentLotteryTime.HasValue ? CurrentLotteryTime.Value.ToString(GlobalSettings.DateTimeFormat) : string.Empty;
        public string UpdateTimeText => UpdateTime.HasValue ? UpdateTime.Value.ToString(GlobalSettings.DateTimeFormat) : string.Empty;
        public string IsLotteryText
        {
            get
            {
                if (IsLottery.HasValue)
                {
                    if (Enum.IsDefined(typeof(IsLotteryEnum), IsLottery))
                    {
                        var enumValue = (IsLotteryEnum)IsLottery;
                        return enumValue.GetDescription();
                    }
                    else
                    {
                        return IsLottery.Value.ToString();
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CurrentLotteryNumText
        {
            get
            {
                string result = string.Empty;
                //處理一分六合格式
                if (LotteryID.HasValue && LotteryID.Value.ToString() == MiseOrderGameId.OMLHC.Value && !string.IsNullOrEmpty(CurrentLotteryNum))
                {
                    string[] numbers = CurrentLotteryNum.Split(',');
                    numbers[numbers.Length - 1] = "+" + numbers[numbers.Length - 1];
                    result = string.Join(",", numbers);
                }
                //牛牛格式
                else if (LotteryID.HasValue && LotteryID.Value.ToString() == MiseOrderGameId.NUINUI.Value && !string.IsNullOrEmpty(CurrentLotteryNum))
                {
                    List<string> numbers = CurrentLotteryNum.Split(',').ToList();
                    NuiNui nui = new NuiNui();
                    var confirmResult = nui.ConfirmResult(numbers, false);
                    string blueResult = confirmResult[0].VictoryConditions.Weight.GetDescription();
                    string redResult = confirmResult[1].VictoryConditions.Weight.GetDescription();
                    var resultBuilder = new StringBuilder();
                    resultBuilder.Append("蓝方：");
                    PokerBase.AppendCards(confirmResult[0].Cards, resultBuilder);
                    resultBuilder.Append(blueResult);
                    resultBuilder.Append("<br>红方：");
                    PokerBase.AppendCards(confirmResult[1].Cards, resultBuilder);
                    resultBuilder.Append(redResult);
                    result = resultBuilder.ToString();
                }
                else if (LotteryID.HasValue && LotteryID.Value.ToString() == MiseOrderGameId.JSBaccarat.Value && !string.IsNullOrEmpty(CurrentLotteryNum))
                {
                    result = JSBaccaratGame.GetPokerString(CurrentLotteryNum);
                }
                else if (LotteryID.HasValue && LotteryID.Value.ToString() == MiseOrderGameId.JSYXX.Value && !string.IsNullOrEmpty(CurrentLotteryNum))
                {
                    result = CurrentLotteryNum.Replace("1", "鱼1").Replace("2", "虾2").Replace("3", "葫芦3").Replace("4", "铜钱4").Replace("5", "蟹5").Replace("6", "鸡6"); ;
                }
                else if (LotteryID.HasValue && LotteryID.Value.ToString() == MiseOrderGameId.JSSG.Value && !string.IsNullOrEmpty(CurrentLotteryNum))
                {
                    result = JssgGame.GetPokerString(CurrentLotteryNum);
                }
                else if (LotteryID.HasValue && LotteryID.Value.ToString() == MiseOrderGameId.JSLH.Value && !string.IsNullOrEmpty(CurrentLotteryNum))
                {
                    result = JslhGame.GetPokerString(CurrentLotteryNum);
                }
                else
                {
                    return CurrentLotteryNum;
                }
                return result;
            }
        }
    }
}