using System;
using System.Data;
using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class TPGamePlayInfoRowModel : BasicUserInfo
    {
        public string PlayInfoID { get; set; }

        public DateTime BetTime { get; set; }

        [IgnoreRead]
        public string BetTimeText
        {
            get { return BetTime.ToFormatDateTimeString(); }
            set { }
        }

        public DateTime ProfitLossTime { get; set; }

        [IgnoreRead]
        public string ProfitLossTimeText
        {
            get { return ProfitLossTime.ToFormatDateTimeString(); }
            set { }
        }

        /// <summary> 有效投注額 </summary>
        public decimal BetMoney { get; set; }

        /// <summary> 總投注額 </summary>
        public decimal AllBetMoney { get; set; }

        public decimal WinMoney { get; set; }

        public int BetResultType { get; set; }

        [IgnoreRead]
        public string BetResultName
        {
            get { return Enums.BetResultType.GetName(BetResultType); }
            set { }
        }

        [IgnoreRead]
        public string BetMoneyText
        {
            get { return BetMoney.ToCurrency(); }
            set { }
        }

        [IgnoreRead]
        public string AllBetMoneyText
        {
            get { return AllBetMoney.ToCurrency(); }
            set { }
        }

        [IgnoreRead]
        public string WinMoneyText
        {
            get { return WinMoney.ToCurrency(); }
            set { }
        }

        public string GameType { get; set; }

        public string Memo { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 第三方注单号
        /// </summary>
        public string PlayID { get; set; }

        public DateTime SaveTime { get; set; }
    }

    public class TPGamePlayInfoStatModel
    {
        public decimal TotalBetMoney { get; set; }

        public decimal TotalAllBetMoney { get; set; }

        public decimal TotalWinMoney { get; set; }

        /// <summary> 給前端用的屬性 </summary>
        public virtual string TotalBetMoneyText
        { get { return TotalBetMoney.ToCurrency(); } set { } }

        /// <summary> 給前端用的屬性 </summary>
        public virtual string TotalAllBetMoneyText
        { get { return TotalAllBetMoney.ToCurrency(); } set { } }

        /// <summary> 給前端用的屬性 </summary>
        public virtual string TotalWinMoneyText
        { get { return TotalWinMoney.ToCurrency(); } set { } }
    }

    public class TPGamePlayInfoFooter
    {
        public TPGamePlayInfoStatModel PageStat { get; set; } = new TPGamePlayInfoStatModel();

        public TPGamePlayInfoStatModel TotalStat { get; set; } = new TPGamePlayInfoStatModel();
    }

    public class PlayInfoWinFilter
    {
        public string Filter { get; set; }

        public int? IsFactionAward { get; set; }
    }

    public class SqlSelectColumnInfo
    {
        public SqlSelectColumnInfo()
        {
        }

        public SqlSelectColumnInfo(string columnName)
        {
            ColumnName = columnName;
            AliasName = columnName;
        }

        public string TableName { get; set; }

        public bool IsIdentity { get; set; }

        public string ColumnName { get; set; }

        public string AliasName { get; set; }

        public string FullColumnName
        {
            get
            {
                string fullColumnName = ColumnName;

                if (!TableName.IsNullOrEmpty())
                {
                    fullColumnName = TableName + "." + fullColumnName;
                }

                return fullColumnName;
            }
        }

        public DbType? ColumnDbType { get; set; }
    }

    public class LotteryPlayInfoDetail
    {
        public string PalyCurrentNum { set; get; }

        public string LotteryType { get; set; }

        /// <summary>完整玩法名稱</summary>
        public string FullPlayTypeName { get; set; }

        public int LotteryID { get; set; }

        public string CurrentLotteryNum { get; set; }

        public string PalyNum { get; set; }

        public decimal RebatePro { get; set; }

        public string RebateProMoney { get; set; }

        public decimal SingleMoney { get; set; }

        public string SingleMoneyText
        {
            get => SingleMoney.Floor(2).ToAmountString();
            set { }
        }

        public int NoteNum { get; set; }

        public string NoteNumText
        {
            get => NoteNum.ToIntWithThousandComma();
            set { }
        }

        public int WinNum { get; set; }

        public string WinNumText
        {
            get => WinNum.ToIntWithThousandComma();
            set { }
        }

        public int IsFactionAward { get; set; }
    }
}