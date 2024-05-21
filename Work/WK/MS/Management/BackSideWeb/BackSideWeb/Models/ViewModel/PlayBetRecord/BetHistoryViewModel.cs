using JxBackendService.Model.BackSideWeb;
using Microsoft.AspNetCore.Mvc.Rendering;
using MS.Core.MMModel.Models.PlayBetRecord;
using MS.Core.MMModel.Models.PlayBetRecord.Enum;
using MS.Core.MMModel.Extensions;
using MS.Core.Models.Models;
using BackSideWeb.Helpers;

namespace BackSideWeb.Models.ViewModel.PlayBetRecord
{
    public class BetHistoryViewModel : PalyInfo
    {
        /// <summary>
        /// 彩种下拉選單
        /// </summary>
        public List<SelectListItem>? LotteryIdSelectList { get; set; }
        /// <summary>
        /// 状态下拉選單
        /// </summary>
        public List<SelectListItem>? IsFactionAwardSelectList { get; set; }
        /// <summary>
        /// 输赢下拉選單
        /// </summary>
        public List<SelectListItem>? IsWinSelectList { get; set; }
        public string LotteryText => MsHelpers.GetLotteryName(LotteryID);
        public string PlayTypeText
        {
            get
            {
                if (PlayTypeID == null)
                {
                    return string.Empty;
                }
                else
                {
                    return "綜合";
                }
            }
        }
        public string NoteTimeText => NoteTime.HasValue ? NoteTime.Value.ToString(GlobalSettings.DateTimeFormat) : string.Empty;
        public string LotteryTimeText => LotteryTime.HasValue ? LotteryTime.Value.ToString(GlobalSettings.DateTimeFormat) : string.Empty;
        public string IsFactionAwardText
        {
            get
            {
                if (IsFactionAward.HasValue)
                {
                    if (Enum.IsDefined(typeof(IsFactionAwardEnum), IsFactionAward))
                    {
                        var enumValue = (IsFactionAwardEnum)IsFactionAward;
                        return enumValue.GetDescription();
                    }
                    else
                    {
                        return IsFactionAward.Value.ToString();
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string IsWinText
        {
            get
            {
                if (IsWin.HasValue)
                {
                    if (IsFactionAward != null && IsFactionAward.Value == (int)IsFactionAwardEnum.SystemRefund)
                    {
                        return string.Empty;
                    }
                    if (Enum.IsDefined(typeof(IsWinEnum), IsWin))
                    {
                        var enumValue = (IsWinEnum)IsWin;
                        return enumValue.GetDescription();
                    }
                    else
                    {
                        return IsWin.Value.ToString();
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string PrizeMoneyText
        {
            get
            {
                if (!NoteMoney.HasValue || !WinMoney.HasValue || (IsFactionAward != null && IsFactionAward.Value == (int)IsFactionAwardEnum.SystemRefund))
                {
                    return string.Empty;
                }
                else
                {
                    return (NoteMoney.Value + WinMoney.Value).ToString("N2");
                }
            }
        }
        public string SingleMoneyText => SingleMoney.HasValue ? SingleMoney.Value.ToString("N2") : string.Empty;
        public string NoteMoneyText => NoteMoney.HasValue ? NoteMoney.Value.ToString("N2") : string.Empty;
        public string WinMoneyText
        {
            get 
            {
                if (!WinMoney.HasValue || (IsFactionAward != null && IsFactionAward.Value == (int)IsFactionAwardEnum.SystemRefund))
                {
                    return string.Empty;
                }
                else
                {
                    return WinMoney.Value.ToString("N2");
                }
            }
        }
        public string WinNumText
        {
            get
            {
                if (!WinNum.HasValue || (IsFactionAward != null && IsFactionAward.Value == (int)IsFactionAwardEnum.SystemRefund))
                {
                    return string.Empty;
                }
                else
                {
                    return WinNum.Value.ToString();
                }
            }
        }
    }
}
