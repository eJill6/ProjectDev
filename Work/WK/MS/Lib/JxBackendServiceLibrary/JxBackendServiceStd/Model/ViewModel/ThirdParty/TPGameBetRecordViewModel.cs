using JxBackendService.Common.Util;
using System;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class TPGameBetRecordViewModel : TPGamePlayInfoRowModel
    {
        public string TPGameAccountName { get; set; }
    }

    public class AllGamePlayInfoRowModel : TPGamePlayInfoRowModel
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }        
    }

    public class MobileApiBetRecordViewModel
    {
        private string _gameType;

        private string _memo;

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string PlayInfoID { get; set; }

        public DateTime BetTime { get; set; }

        /// <summary> 有效投注額 </summary>
        public decimal? BetMoney { get; set; }

        public string BetMoneyText
        {
            get => BetMoney.ToMobileApiAmountString();
            set { }
        }

        /// <summary> 總投注額 </summary>
        public decimal? AllBetMoney { get; set; }

        public string AllBetMoneyText
        {
            get => AllBetMoney.ToMobileApiAmountString();
            set { }
        }

        public decimal? WinMoney { get; set; }

        public string WinMoneyText
        {
            get => WinMoney.ToMobileApiAmountString();
            set { }
        }

        public string GameType { get => _gameType.ToNonNullString(); set => _gameType = value; }

        public string Memo { get => _memo.ToNonNullString(); set => _memo = value; }

        /// <summary>第三方注单号</summary>
        public string PlayID { get; set; }

        public int AwardType { get; set; }

        public string BetResultText { get; set; }

        public LotteryPlayInfoDetail LotteryPlayInfoDetail { get; set; }

        public int? BetResultType { get; set; }
    }

    public class MobileApiPlayInfoStatModel : TPGamePlayInfoStatModel
    {
        public override string TotalAllBetMoneyText
        {
            get => TotalAllBetMoney.ToMobileApiAmountString();
            set { }
        }

        public override string TotalBetMoneyText
        {
            get => TotalBetMoney.ToMobileApiAmountString();
            set { }
        }

        public override string TotalWinMoneyText
        {
            get => TotalWinMoney.ToMobileApiAmountString();
            set { }
        }
    }
}