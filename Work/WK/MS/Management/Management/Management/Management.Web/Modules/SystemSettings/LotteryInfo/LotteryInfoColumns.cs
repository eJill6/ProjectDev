using Serenity.ComponentModel;
using System.ComponentModel;

namespace Management.SystemSettings.Columns
{
    [ColumnsScript("SystemSettings.LotteryInfo")]
    [BasedOnRow(typeof(LotteryInfoRow), CheckNames = false)]
    public class LotteryInfoColumns
    {
        [DisplayName("Db.Shared.RecordId"), AlignRight,Hidden]
        public int LotteryID { get; set; }
        [ DisplayName("彩种名称"), Width(100)]
        public string LotteryType { get; set; }
        [DisplayName("彩种状态"), Width(100)]
        public int Status { get; set; }
        [DisplayName("最大自订金额"), Width(200)]
        public int CustomMoney { get; set; }
        [DisplayName("最大赔付金额"), Width(200)]
        public int MaxBonusMoney { get; set; }
        [DisplayName("WEB排序"), Width(200)]
        public int WebSeq { get; set; }
        [DisplayName("APP排序"), Width(200)]
        public int AppSeq { get; set; }

        //public string TypeUrl { get; set; }
        //public int GameTypeId { get; set; }
        
        //public string OfficialLotteryUrl { get; set; }
        //public string NumberTrendUrl { get; set; }
        //public int DefaultSec { get; set; }
        
        //public int HotNew { get; set; }
        
        //public string Notice { get; set; }
        //public int RecommendSort { get; set; }
    }
}