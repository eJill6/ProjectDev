namespace ControllerShareLib.Models.Game
{
    /// <summary>直播間直接取得第三方遊戲網址參數</summary>
    public class EnterThirdPartyGameParam
    {
        /// <summary>祕色定義的GameID</summary>
        public string GameId { get; set; }

        /// <summary>實際第三方需要的RemoteCode</summary>
        public string? RemoteCode { get; set; }
    }
}