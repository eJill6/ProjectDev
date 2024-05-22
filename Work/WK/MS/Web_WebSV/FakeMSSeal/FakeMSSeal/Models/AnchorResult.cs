namespace FakeMSSeal.Models
{
    public class AnchorResult : ResultModel<Anchor[]>
    {
    }

    public class Anchor
    {
        public int AnchorId { get; set; }
        public int AnchorType { get; set; }
        public int RoomNum { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
    }
}
