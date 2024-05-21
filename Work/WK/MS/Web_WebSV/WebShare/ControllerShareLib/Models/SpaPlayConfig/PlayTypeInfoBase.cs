namespace ControllerShareLib.Models.SpaPlayConfig
{
    /// <summary>
    /// 玩法類別
    /// </summary>
    public class PlayTypeInfoBase<T>
    {
        /// <summary>
        /// PlayTypeInfo 資料庫資料
        /// </summary>
        public SLPolyGame.Web.Model.PlayTypeInfo Info { get; set; }

        /// <summary>
        /// 玩法Enum名稱
        /// </summary>
        public string PlayTypeEnum { get; set; }

        /// <summary>
        /// 玩法的基礎Id
        /// </summary>
        public int BasePlayTypeId { get; set; }

        /// <summary>
        /// 玩法子類的陣列
        /// </summary>
        public T PlayTypeRadioInfos { get; set; }
    }
}