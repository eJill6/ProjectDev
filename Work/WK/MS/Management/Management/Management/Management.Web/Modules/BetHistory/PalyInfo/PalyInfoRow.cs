using Serenity.ComponentModel;
using Serenity.Data;
using Serenity.Data.Mapping;
using System;
using System.ComponentModel;

namespace Management.BetHistory
{
    [ConnectionKey("Inlodb"), Module("BetHistory"), TableName("[dbo].[PalyInfo]")]
    [DisplayName("Paly Info"), InstanceName("Paly Info")]
    [ReadPermission("Bethistory:PalyInfo")]
    [ModifyPermission("Bethistory:PalyInfo")]
    public sealed class PalyInfoRow : Row<PalyInfoRow.RowFields>, IIdRow, INameRow
    {
        [DisplayName("Paly Id"), Column("PalyID"), Size(32), PrimaryKey, NotNull, IdProperty, QuickSearch, NameProperty]
        public string PalyId
        {
            get => fields.PalyId[this];
            set => fields.PalyId[this] = value;
        }

        [DisplayName("Paly Current Num"), Size(100)]
        public string PalyCurrentNum
        {
            get => fields.PalyCurrentNum[this];
            set => fields.PalyCurrentNum[this] = value;
        }

        [DisplayName("Paly Num")]
        public string PalyNum
        {
            get => fields.PalyNum[this];
            set => fields.PalyNum[this] = value;
        }

        [DisplayName("Play Type Id"), Column("PlayTypeID")]
        public int? PlayTypeId
        {
            get => fields.PlayTypeId[this];
            set => fields.PlayTypeId[this] = value;
        }

        [DisplayName("Lottery Id"), Column("LotteryID")]
        public int? LotteryId
        {
            get => fields.LotteryId[this];
            set => fields.LotteryId[this] = value;
        }

        [DisplayName("User Name"), Size(50)]
        public string UserName
        {
            get => fields.UserName[this];
            set => fields.UserName[this] = value;
        }

        [DisplayName("Note Num")]
        public int? NoteNum
        {
            get => fields.NoteNum[this];
            set => fields.NoteNum[this] = value;
        }

        [DisplayName("Single Money"), Size(18), Scale(4)]
        public decimal? SingleMoney
        {
            get => fields.SingleMoney[this];
            set => fields.SingleMoney[this] = value;
        }

        [DisplayName("Note Money"), Size(18), Scale(4)]
        public decimal? NoteMoney
        {
            get => fields.NoteMoney[this];
            set => fields.NoteMoney[this] = value;
        }



        [DisplayName("Is Win")]
        public bool? IsWin
        {
            get => fields.IsWin[this];
            set => fields.IsWin[this] = value;
        }

        [DisplayName("Win Money"), Size(18), Scale(4)]
        public decimal? WinMoney
        {
            get => fields.WinMoney[this];
            set => fields.WinMoney[this] = value;
        }

        [DisplayName("Is Faction Award")]
        public int? IsFactionAward
        {
            get => fields.IsFactionAward[this];
            set => fields.IsFactionAward[this] = value;
        }

        [DisplayName("Play Type Radio Id"), Column("PlayTypeRadioID")]
        public int? PlayTypeRadioId
        {
            get => fields.PlayTypeRadioId[this];
            set => fields.PlayTypeRadioId[this] = value;
        }

        [DisplayName("Rebate Pro"), Size(18), Scale(4)]
        public decimal? RebatePro
        {
            get => fields.RebatePro[this];
            set => fields.RebatePro[this] = value;
        }

        [DisplayName("Rebate Pro Money"), Size(50)]
        public string RebateProMoney
        {
            get => fields.RebateProMoney[this];
            set => fields.RebateProMoney[this] = value;
        }

        [DisplayName("Win Num")]
        public int? WinNum
        {
            get => fields.WinNum[this];
            set => fields.WinNum[this] = value;
        }

        [DisplayName("User Id"), Column("UserID")]
        public int? UserId
        {
            get => fields.UserId[this];
            set => fields.UserId[this] = value;
        }

        [DisplayName("Notice Id"), Column("NoticeID")]
        public int? NoticeId
        {
            get => fields.NoticeId[this];
            set => fields.NoticeId[this] = value;
        }

        [DisplayName("Note Time")]
        public DateTime? NoteTime
        {
            get => fields.NoteTime[this];
            set => fields.NoteTime[this] = value;
        }

        [DisplayName("Lottery Time")]
        public DateTime? LotteryTime
        {
            get => fields.LotteryTime[this];
            set => fields.LotteryTime[this] = value;
        }

        [DisplayName("User Rebate Pro"), Size(18), Scale(4)]
        public decimal? UserRebatePro
        {
            get => fields.UserRebatePro[this];
            set => fields.UserRebatePro[this] = value;
        }

        [DisplayName("Multiple")]
        public int? Multiple
        {
            get => fields.Multiple[this];
            set => fields.Multiple[this] = value;
        }

        [DisplayName("Order Key"), Size(40)]
        public string OrderKey
        {
            get => fields.OrderKey[this];
            set => fields.OrderKey[this] = value;
        }

        [DisplayName("Currency Unit"), Size(18), Scale(4)]
        public decimal? CurrencyUnit
        {
            get => fields.CurrencyUnit[this];
            set => fields.CurrencyUnit[this] = value;
        }

        [DisplayName("Ratio")]
        public int? Ratio
        {
            get => fields.Ratio[this];
            set => fields.Ratio[this] = value;
        }

        [DisplayName("Source Type"), Size(10)]
        public string SourceType
        {
            get => fields.SourceType[this];
            set => fields.SourceType[this] = value;
        }

        [DisplayName("Memo Json"), Size(2000)]
        public string MemoJson
        {
            get => fields.MemoJson[this];
            set => fields.MemoJson[this] = value;
        }

        [DisplayName("Client Ip"), Column("ClientIP"), Size(128), NotNull]
        public string ClientIp
        {
            get => fields.ClientIp[this];
            set => fields.ClientIp[this] = value;
        }

        [DisplayName("Room Id"), Size(50), NotNull]
        public string RoomId
        {
            get => fields.RoomId[this];
            set => fields.RoomId[this] = value;
        }

        [DisplayName("Result Json")]
        public string ResultJson
        {
            get => fields.ResultJson[this];
            set => fields.ResultJson[this] = value;
        }

        public PalyInfoRow()
            : base()
        {
        }

        public PalyInfoRow(RowFields fields)
            : base(fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public StringField PalyId;
            public StringField PalyCurrentNum;
            public StringField PalyNum;
            public Int32Field PlayTypeId;
            public Int32Field LotteryId;
            public StringField UserName;
            public Int32Field NoteNum;
            public DecimalField SingleMoney;
            public DecimalField NoteMoney;
            public DateTimeField NoteTime;
            public BooleanField IsWin;
            public DecimalField WinMoney;
            public Int32Field IsFactionAward;
            public Int32Field PlayTypeRadioId;
            public DecimalField RebatePro;
            public StringField RebateProMoney;
            public Int32Field WinNum;
            public Int32Field UserId;
            public Int32Field NoticeId;
            public DateTimeField LotteryTime;
            public DecimalField UserRebatePro;
            public Int32Field Multiple;
            public StringField OrderKey;
            public DecimalField CurrencyUnit;
            public Int32Field Ratio;
            public StringField SourceType;
            public StringField MemoJson;
            public StringField ClientIp;
            public StringField RoomId;
            public StringField ResultJson;
        }
    }
}