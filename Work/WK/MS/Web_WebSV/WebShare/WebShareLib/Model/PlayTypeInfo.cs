namespace SLPolyGame.Web.Model
{
    /// <summary>
    /// PlayTypeInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    //[Serializable]
    public class PlayTypeInfo
    {
        public PlayTypeInfo()
        { }
        #region Model
        private int playtypeid;
        private int? lotteryid;
        private string playtypename;
        /// <summary>
        /// 选号类型ID   类型例如：三星直选，三星组选
        /// </summary>
        public int PlayTypeID
        {
            set { playtypeid = value; }
            get { return playtypeid; }
        }
        /// <summary>
        /// 彩种类型ID
        /// </summary>
        public int? LotteryID
        {
            set { lotteryid = value; }
            get { return lotteryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PlayTypeName
        {
            set { playtypename = value; }
            get { return playtypename; }
        }
        #endregion Model

        public int UserType { get; set; }

    }
}

