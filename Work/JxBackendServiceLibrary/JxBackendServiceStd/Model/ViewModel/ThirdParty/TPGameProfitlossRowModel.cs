using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using System;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class TPGameProfitLossRowModel : BasicUserInfo
    {
        private string _memo;

        private JxApplication _memoApplicationFilter;

        private decimal allBetMoney;

        public string ProfitLossID { get; set; }

        public string ProfitLossType { get; set; }

        public DateTime BetTime { get; set; }

        public DateTime ProfitLossTime { get; set; }

        [IgnoreRead]
        public string ProfitLossTimeText
        {
            get { return ProfitLossTime.ToFormatDateTimeString(); }
            set { }
        }

        [IgnoreRead]
        public string CompareTimeText { get; set; }

        public decimal ProfitLossMoney { get; set; }

        [IgnoreRead]
        public string ProfitLossMoneyText
        {
            get { return ProfitLossMoney.ToCurrency(); }
            set { }
        }

        public decimal AllBetMoney
        {
            get
            {
                if (ProfitLossType == ProfitLossTypeName.KY)
                {
                    return allBetMoney;
                }

                return 0;
            }
            set => allBetMoney = value;
        }

        [IgnoreRead]
        public string AllBetMoneyText
        {
            get { return AllBetMoney.ToCurrency(); }
            set { }
        }

        public decimal WinMoney { get; set; }

        [IgnoreRead]
        public string WinMoneyText
        {
            get { return WinMoney.ToCurrency(); }
            set { }
        }

        public decimal PrizeMoney { get; set; }

        [IgnoreRead]
        public string PrizeMoneyText
        {
            get { return PrizeMoney.ToCurrency(); }
            set { }
        }

        public string Memo { get => MemoJson.ToLocalizationContent(_memo, _memoApplicationFilter); set => _memo = value; }

        public string MemoJson { get; set; }

        public string GameType { get; set; }

        public string PlayID { get; set; }

        /// <summary>
        /// 用於呈現投注金額
        /// </summary>
        [IgnoreRead]
        public string DisplayBetAmountText
        {
            get
            {
                if (ProfitLossType == ProfitLossTypeName.KY)
                {
                    return ProfitLossMoney.ToCurrency();
                }
                else
                {
                    return 0m.ToCurrency();
                }
            }
            set { }
        }

        /// <summary>
        /// 用於呈現盈虧金額
        /// </summary>
        [IgnoreRead]
        public string DisplayProfitlossAmountText
        {
            get
            {
                if (ProfitLossType == ProfitLossTypeName.KY)
                {
                    return WinMoney.ToCurrency();
                }
                else if (ProfitLossType == ProfitLossTypeName.FD)
                {
                    return (ProfitLossMoney + WinMoney).ToCurrency();
                }
                else
                {
                    return ProfitLossMoney.ToCurrency();
                }
            }
            set { }
        }

        /// <summary>用於呈現平台彩票盈虧金額（因為資料寫入的方式不同，會造成重複計算）</summary>
        [IgnoreRead]
        public string DisplayLotteryProfitlossAmountText
        {
            get
            {
                if (ProfitLossType == ProfitLossTypeName.KY)
                {
                    return WinMoney.ToCurrency();
                }
                else
                {
                    return ProfitLossMoney.ToCurrency();
                }
            }
            set { }
        }

        public void SetMemoApplicationFilter(JxApplication memoApplicationFilter)
        {
            _memoApplicationFilter = memoApplicationFilter;
        }
    }

    public class TPGameProfitLossRowModelCompareTime : BaseStringValueModel<TPGameProfitLossRowModelCompareTime>
    {
        private TPGameProfitLossRowModelCompareTime()
        { }

        public static TPGameProfitLossRowModelCompareTime BetTime = new TPGameProfitLossRowModelCompareTime() { Value = nameof(TPGameProfitLossRowModel.BetTime) };

        public static TPGameProfitLossRowModelCompareTime ProfitLossTime = new TPGameProfitLossRowModelCompareTime() { Value = nameof(TPGameProfitLossRowModel.ProfitLossTime) };
    }
}