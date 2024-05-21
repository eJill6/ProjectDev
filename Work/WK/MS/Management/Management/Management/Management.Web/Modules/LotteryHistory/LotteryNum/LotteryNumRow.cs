using Serenity.ComponentModel;
using Serenity.Data;
using Serenity.Data.Mapping;
using System;
using System.ComponentModel;

namespace Management.LotteryHistory
{
    [ConnectionKey("Inlodb"), Module("LotteryHistory"), TableName("[dbo].[CurrentLotteryInfo]")]
    [DisplayName("Lottery Num"), InstanceName("Lottery Num")]
    [ReadPermission("LotteryHistory:LotteryNum")]
    [ModifyPermission("LotteryHistory:LotteryNum")]
    public sealed class LotteryNumRow : Row<LotteryNumRow.RowFields>, IIdRow, INameRow
    {
        [DisplayName("Current Lottery Id"), Column("CurrentLotteryID"), Identity, IdProperty, Visible(false)]
        public int? CurrentLotteryId
        {
            get => fields.CurrentLotteryId[this];
            set => fields.CurrentLotteryId[this] = value;
        }

        [DisplayName("Current Lottery Time"), DisplayFormat("yyyy/MM/dd HH:mm:ss"),Width(170), QuickFilter]
        public DateTime? CurrentLotteryTime
        {
            get => fields.CurrentLotteryTime[this];
            set => fields.CurrentLotteryTime[this] = value;
        }

        [DisplayName("Lottery Type"), Size(50), QuickSearch, NameProperty, QuickFilter]
        public string LotteryType
        {
            get => fields.LotteryType[this];
            set => fields.LotteryType[this] = value;
        }

        [DisplayName("Current Lottery Num"), Size(50), Width(100)]
        public string CurrentLotteryNum
        {
            get => fields.CurrentLotteryNum[this];
            set => fields.CurrentLotteryNum[this] = value;
        }

        [DisplayName("Lottery Id"), Column("LotteryID"),Visible(false)]
        public int? LotteryId
        {
            get => fields.LotteryId[this];
            set => fields.LotteryId[this] = value;
        }

        [DisplayName("Issue No"), Size(50), QuickFilter]
        public string IssueNo
        {
            get => fields.IssueNo[this];
            set => fields.IssueNo[this] = value;
        }

        [DisplayName("Add Time"), DisplayFormat("yyyy/MM/dd HH:mm:ss"), Width(170), Visible(false)]
        public DateTime? AddTime
        {
            get => fields.AddTime[this];
            set => fields.AddTime[this] = value;
        }

        [DisplayName("Update Time"), DisplayFormat("yyyy/MM/dd HH:mm:ss"), Width(170)]
        public DateTime? UpdateTime
        {
            get => fields.UpdateTime[this];
            set => fields.UpdateTime[this] = value;
        }

        [DisplayName("Is Lottery"),Width(100)]
        public bool? IsLottery
        {
            get => fields.IsLottery[this];
            set => fields.IsLottery[this] = value;
        }

        [DisplayName("Draw Time Consuming"), Expression("(datediff(ss,CurrentLotteryTime,UpdateTime))"), Width(200)]
        public int? DrawTimeConsuming
        {
            get { return Fields.DrawTimeConsuming[this]; }
            set { Fields.DrawTimeConsuming[this] = value; }
        }


        [DisplayName("Msg"), Size(200)]
        public string Msg
        {
            get => fields.Msg[this];
            set => fields.Msg[this] = value;
        }

        public LotteryNumRow()
            : base()
        {
        }

        public LotteryNumRow(RowFields fields)
            : base(fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public Int32Field CurrentLotteryId;
            public DateTimeField CurrentLotteryTime;
            public StringField LotteryType;
            public StringField CurrentLotteryNum;
            public Int32Field LotteryId;
            public StringField IssueNo;
            public DateTimeField AddTime;
            public DateTimeField UpdateTime;
            public BooleanField IsLottery;
            public StringField Msg;
            public Int32Field DrawTimeConsuming;
        }
    }
}