namespace SLPolyGame.Web.Model
{
    /// <summary>
    /// 秘色資料庫的投注資訊
    /// </summary>
    public class PlayInfoDto : JxLottery.Models.Lottery.Bet.PlayInfo
    {
        public string RoomId { get; set; }
    }
}