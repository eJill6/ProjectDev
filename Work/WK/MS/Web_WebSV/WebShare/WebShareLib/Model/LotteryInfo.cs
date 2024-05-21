namespace SLPolyGame.Web.Model
{
    /// <summary>
    /// LotteryInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    //[Serializable]
    public class LotteryInfo
    {
        public LotteryInfo()
        { }
        #region Model
        private int lotteryid;
        private string lotterytype;
        /// <summary>
        /// 彩种ID
        /// </summary>
        public int LotteryID
        {
            set { lotteryid = value; }
            get { return lotteryid; }
        }
        /// <summary>
        /// 彩种类型
        /// </summary>
        public string LotteryType
        {
            set { lotterytype = value; }
            get { return lotterytype; }
        }
        #endregion Model

        private string typeURL;

        public string TypeURL
        {
            get { return typeURL; }
            set { typeURL = value; }
        }
        private int gameTypeID;

        public int GameTypeID
        {
            get { return gameTypeID; }
            set { gameTypeID = value; }
        }

        private string officialLotteryUrl;
        public string OfficialLotteryUrl
        {
            get { return officialLotteryUrl; }
            set { officialLotteryUrl = value; }
        }

        private string numberTrendUrl;
        public string NumberTrendUrl
        {
            get { return numberTrendUrl; }
            set { numberTrendUrl = value; }
        }

        private int priority;
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public int GroupPriority { get; set; }

        public int UserType { get; set; }

        public int MaxBonusMoney { get; set; } = 0;

        public int HotNew { get; set; } = 0;

        public string Notice { get; set; } = "";

        //全頻情境下是否顯示維護中
        public bool IsMaintaining { get; set; }
    }
}

