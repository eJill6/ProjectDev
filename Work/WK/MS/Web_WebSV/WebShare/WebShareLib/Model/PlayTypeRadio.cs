namespace SLPolyGame.Web.Model
{
    /// <summary>
    /// PlayTypeRadio:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    //[Serializable]
    public class PlayTypeRadio
    {
        public PlayTypeRadio()
        { }
        #region Model
        private int playtyperadioid;
        private string playtyperadioname;
        private int? playtypeid;
        /// <summary>
        /// 
        /// </summary>
        public int PlayTypeRadioID
        {
            set { playtyperadioid = value; }
            get { return playtyperadioid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PlayTypeRadioName
        {
            set { playtyperadioname = value; }
            get { return playtyperadioname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PlayTypeID
        {
            set { playtypeid = value; }
            get { return playtypeid; }
        }
        #endregion Model

        private string playDescription;
        /// <summary>
        /// 玩法说明
        /// </summary>
        public string PlayDescription
        {
            get { return playDescription; }
            set { playDescription = value; }
        }

        public int Priority { get; set; }

        public string WinExample { get; set; }

        public string TypeModel { get; set; }

        public int UserType { get; set; }
    }
}

