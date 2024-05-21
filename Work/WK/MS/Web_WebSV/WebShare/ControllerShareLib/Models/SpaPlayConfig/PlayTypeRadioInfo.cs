using SLPolyGame.Web.Model;

namespace ControllerShareLib.Models.SpaPlayConfig
{
    /// <summary>
    /// 玩法子類
    /// </summary>
    public class PlayTypeRadioInfo
    {
        /// <summary>
        /// App用畫面呈現
        /// </summary>
        public IDictionary<string, string> ViewType { get; set; } = new Dictionary<string, string>();

        /// <summary>
        ///  所有的按鈕
        /// </summary>
        public IList<Field> Fields { get; set; }

        /// <summary>
        /// PlayTypeInfo 資料庫資料
        /// </summary>
        public PlayTypeRadio Info { get; set; }

        /// <summary>
        /// 玩法Enum名稱
        /// </summary>
        public string PlayTypeRadioEnum { get; set; }

        /// <summary>
        /// 子玩法基礎Id
        /// </summary>
        public int BasePlayTypeRadioId { get; set; }

        /// <summary>
        /// 是否可以下追號單
        /// </summary>
        public bool CanPlayAfter { get; set; } = true;
    }
}