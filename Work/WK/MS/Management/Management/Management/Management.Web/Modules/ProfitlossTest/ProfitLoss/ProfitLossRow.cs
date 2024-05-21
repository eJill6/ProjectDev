using Serenity.ComponentModel;
using Serenity.Data;
using Serenity.Data.Mapping;
using System;
using System.ComponentModel;

namespace Management.ProfitlossTest
{
    [ConnectionKey("Inlodb"), Module("ProfitlossTest"), TableName("[dbo].[ProfitLoss]")]
    [DisplayName("Profit Loss"), InstanceName("Profit Loss")]
    [ReadPermission("Administration:General")]
    [ModifyPermission("Administration:General")]
    public sealed class ProfitLossRow : Row<ProfitLossRow.RowFields>, IIdRow, INameRow
    {
        [DisplayName("Profit Loss Id"), Column("ProfitLossID"), Size(32), PrimaryKey, NotNull, IdProperty, QuickSearch, NameProperty]
        public string ProfitLossId
        {
            get => fields.ProfitLossId[this];
            set => fields.ProfitLossId[this] = value;
        }

        [DisplayName("User Id"), Column("UserID"), NotNull]
        public int? UserId
        {
            get => fields.UserId[this];
            set => fields.UserId[this] = value;
        }

        [DisplayName("Profit Loss Time"), NotNull]
        public DateTime? ProfitLossTime
        {
            get => fields.ProfitLossTime[this];
            set => fields.ProfitLossTime[this] = value;
        }

        [DisplayName("Profit Loss Type"), Size(50), NotNull]
        public string ProfitLossType
        {
            get => fields.ProfitLossType[this];
            set => fields.ProfitLossType[this] = value;
        }

        [DisplayName("Profit Loss Money"), Size(18), Scale(4), NotNull]
        public decimal? ProfitLossMoney
        {
            get => fields.ProfitLossMoney[this];
            set => fields.ProfitLossMoney[this] = value;
        }

        [DisplayName("Win Money"), Size(18), Scale(4), NotNull]
        public decimal? WinMoney
        {
            get => fields.WinMoney[this];
            set => fields.WinMoney[this] = value;
        }

        [DisplayName("Prize Money"), Size(18), Scale(4), NotNull]
        public decimal? PrizeMoney
        {
            get => fields.PrizeMoney[this];
            set => fields.PrizeMoney[this] = value;
        }

        [DisplayName("All Bet Money"), Size(18), Scale(4), NotNull]
        public decimal? AllBetMoney
        {
            get => fields.AllBetMoney[this];
            set => fields.AllBetMoney[this] = value;
        }

        [DisplayName("Game Type"), Size(50), NotNull]
        public string GameType
        {
            get => fields.GameType[this];
            set => fields.GameType[this] = value;
        }

        [DisplayName("Play Id"), Column("PlayID"), Size(50), NotNull]
        public string PlayId
        {
            get => fields.PlayId[this];
            set => fields.PlayId[this] = value;
        }

        [DisplayName("Memo"), Size(500)]
        public string Memo
        {
            get => fields.Memo[this];
            set => fields.Memo[this] = value;
        }

        public ProfitLossRow()
            : base()
        {
        }

        public ProfitLossRow(RowFields fields)
            : base(fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public StringField ProfitLossId;
            public Int32Field UserId;
            public DateTimeField ProfitLossTime;
            public StringField ProfitLossType;
            public DecimalField ProfitLossMoney;
            public DecimalField WinMoney;
            public DecimalField PrizeMoney;
            public DecimalField AllBetMoney;
            public StringField GameType;
            public StringField PlayId;
            public StringField Memo;
        }
    }
}