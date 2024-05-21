using System;
namespace SLPolyGame.Web.Model
{
    /// <summary>
    /// CurrentLotteryInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    public class CurrentLotteryInfo
    {
        public CurrentLotteryInfo()
        { }
        #region Model
        private int currentlotteryid;
        private DateTime? currentlotterytime;
        private string lotterytype;
        private string currentlotterynum;
        private int? lotteryid;
        private string issueno;
        private DateTime? addtime;
        private DateTime? updatetime;
        private bool islottery;
        /// <summary>
        /// 每期开奖记录ID
        /// </summary>
        public int CurrentLotteryID
        {
            set { currentlotteryid = value; }
            get { return currentlotteryid; }
        }
        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime? CurrentLotteryTime
        {
            set { currentlotterytime = value; }
            get { return currentlotterytime; }
        }
        /// <summary>
        /// 开奖类型
        /// </summary>
        public string LotteryType
        {
            set { lotterytype = value; }
            get { return lotterytype; }
        }
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string CurrentLotteryNum
        {
            set { currentlotterynum = value; }
            get { return currentlotterynum; }
        }
        /// <summary>
        /// 彩种ID
        /// </summary>
        public int? LotteryID
        {
            set { lotteryid = value; }
            get { return lotteryid; }
        }
        /// <summary>
        /// 开奖序号
        /// </summary>
        public string IssueNo
        {
            set { issueno = value; }
            get { return issueno; }
        }
        /// <summary>
        /// 新的一期被添加，此时还未添加开奖号码
        /// </summary>
        public DateTime? AddTime
        {
            set { addtime = value; }
            get { return addtime; }
        }
        /// <summary>
        /// 开奖后更新开奖号码
        /// </summary>
        public DateTime? UpdateTime
        {
            set { updatetime = value; }
            get { return updatetime; }
        }
        /// <summary>
        /// 本期是否开奖
        /// </summary>
        public bool IsLottery
        {
            set { islottery = value; }
            get { return islottery; }
        }
        #endregion Model


        private string lotteryNo;
        /// <summary>
        /// 当前期号
        /// </summary>
        public string LotteryNo
        {
            get { return lotteryNo; }
            set { lotteryNo = value; }
        }
        private DateTime endTime;
        /// <summary>
        /// 当前封单时间
        /// </summary>
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        private DateTime currentTime;
        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime CurrentTime
        {
            get { return currentTime; }
            set { currentTime = value; }
        }
        private float remainTime;
        /// <summary>
        /// 剩余时间
        /// </summary>
        public float RemainTime
        {
            get { return remainTime; }
            set { remainTime = value; }
        }
        private string lottery_result;
        /// <summary>
        /// 上期结果
        /// </summary>
        public string Lottery_result
        {
            get { return lottery_result; }
            set { lottery_result = value; }
        }
        private string preLotteryNo;
        /// <summary>
        /// 上期期号
        /// </summary>
        public string PreLotteryNo
        {
            get { return preLotteryNo; }
            set { preLotteryNo = value; }
        }

    }
}

