using System;
using System.Data;
using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class TPGamePlayInfoRowModel : BasicUserInfo
    {
        public string PalyInfoID { get; set; }

        public DateTime BetTime { get; set; }

        [IgnoreRead]
        public string BetTimeText { get { return BetTime.ToFormatDateTimeString(); } set { } }

        public DateTime ProfitLossTime { get; set; }

        [IgnoreRead]
        public string ProfitLossTimeText { get { return ProfitLossTime.ToFormatDateTimeString(); } set { } }

        public decimal BetMoney { get; set; }

        public decimal WinMoney { get; set; }

        public int IsWin { get; set; }

        [IgnoreRead]
        public string BetResultName { get { return BetResultType.GetName(IsWin); } set { } }

        [IgnoreRead]
        public string BetMoneyText { get { return BetMoney.ToCurrency(); } set { } }

        [IgnoreRead]
        public string WinMoneyText { get { return WinMoney.ToCurrency(); } set { } }

        public string GameType { get; set; }

        public string Memo { get; set; }

        /// <summary>
        /// 給PT用的欄位
        /// </summary>
        [IgnoreRead]
        public int? IsFactionAward { get; set; }
    }

    public class TPGamePlayInfoStatModel
    {
        public decimal TotalBetMoney { get; set; }

        public decimal TotalWinMoney { get; set; }

        /// <summary>
        /// 給前端用的屬性
        /// </summary>
        public string TotalBetMoneyText { get { return TotalBetMoney.ToCurrency(); } set { } }

        /// <summary>
        /// 給前端用的屬性
        /// </summary>
        public string TotalWinMoneyText { get { return TotalWinMoney.ToCurrency(); } set { } }
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

}
