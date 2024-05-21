using Serenity.ComponentModel;
using Serenity.Data;
using Serenity.Data.Mapping;
using System.ComponentModel;

namespace Management.SystemSettings
{
    [ConnectionKey("Inlodb"), Module("SystemSettings"), TableName("[dbo].[LotteryInfo]")]
    [DisplayName("Lottery Info"), InstanceName("Lottery Info")]
    [ReadPermission("SystemSettings:LotteryInfo")]
    [ModifyPermission("SystemSettings:LotteryInfo")]
    public sealed class LotteryInfoRow : Row<LotteryInfoRow.RowFields>, IIdRow, INameRow
    {
        [DisplayName("Lottery Id"), Column("LotteryID"), PrimaryKey, NotNull, IdProperty]
        public int? LotteryID
        {
            get => fields.LotteryID[this];
            set => fields.LotteryID[this] = value;
        }

        [DisplayName("Lottery Type"), Size(50), QuickSearch, NameProperty]
        public string LotteryType
        {
            get => fields.LotteryType[this];
            set => fields.LotteryType[this] = value;
        }

        [DisplayName("Type Url"), Column("TypeURL"), Size(50)]
        public string TypeURL
        {
            get => fields.TypeURL[this];
            set => fields.TypeURL[this] = value;
        }

        [DisplayName("Game Type Id"), Column("GameTypeID")]
        public int? GameTypeID
        {
            get => fields.GameTypeID[this];
            set => fields.GameTypeID[this] = value;
        }

        [DisplayName("Priority")]
        public int? Priority
        {
            get => fields.Priority[this];
            set => fields.Priority[this] = value;
        }

        [DisplayName("Official Lottery Url"), Size(200)]
        public string OfficialLotteryUrl
        {
            get => fields.OfficialLotteryUrl[this];
            set => fields.OfficialLotteryUrl[this] = value;
        }

        [DisplayName("Number Trend Url"), Size(200)]
        public string NumberTrendUrl
        {
            get => fields.NumberTrendUrl[this];
            set => fields.NumberTrendUrl[this] = value;
        }

        [DisplayName("Status")]
        public int? Status
        {
            get => fields.Status[this];
            set => fields.Status[this] = value;
        }

        [DisplayName("Default Sec"), Column("Default_Sec"), NotNull]
        public int? DefaultSec
        {
            get => fields.DefaultSec[this];
            set => fields.DefaultSec[this] = value;
        }

        [DisplayName("App Priority"), Column("APPPriority"), NotNull]
        public int? AppPriority
        {
            get => fields.AppPriority[this];
            set => fields.AppPriority[this] = value;
        }

        [DisplayName("Hot New"), NotNull]
        public int? HotNew
        {
            get => fields.HotNew[this];
            set => fields.HotNew[this] = value;
        }

        [DisplayName("Max Bonus Money"), NotNull]
        public int? MaxBonusMoney
        {
            get => fields.MaxBonusMoney[this];
            set => fields.MaxBonusMoney[this] = value;
        }

        [DisplayName("Notice"), Size(100)]
        public string Notice
        {
            get => fields.Notice[this];
            set => fields.Notice[this] = value;
        }

        [DisplayName("Recommend Sort"), NotNull]
        public int? RecommendSort
        {
            get => fields.RecommendSort[this];
            set => fields.RecommendSort[this] = value;
        }
        public int? CustomMoney
        {
            get => fields.CustomMoney[this];
            set => fields.CustomMoney[this] = value;
        }
        public int? WebSeq
        {
            get => fields.WebSeq[this];
            set => fields.WebSeq[this] = value;
        }
        public int? AppSeq
        {
            get => fields.AppSeq[this];
            set => fields.AppSeq[this] = value;
        }

        public LotteryInfoRow()
            : base()
        {
        }

        public LotteryInfoRow(RowFields fields)
            : base(fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public Int32Field LotteryID;
            public StringField LotteryType;
            public StringField TypeURL;
            public Int32Field GameTypeID;
            public Int32Field Priority;
            public StringField OfficialLotteryUrl;
            public StringField NumberTrendUrl;
            public Int32Field Status;
            public Int32Field DefaultSec;
            public Int32Field AppPriority;
            public Int32Field HotNew;
            public Int32Field MaxBonusMoney;
            public StringField Notice;
            public Int32Field RecommendSort;
            public Int32Field CustomMoney;
            public Int32Field WebSeq;
            public Int32Field AppSeq;
        }
    }
}