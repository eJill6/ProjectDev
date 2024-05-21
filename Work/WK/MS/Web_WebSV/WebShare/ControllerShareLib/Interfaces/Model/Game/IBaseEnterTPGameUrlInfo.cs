namespace ControllerShareLib.Interfaces.Model.Game
{
    public interface IBaseEnterTPGameUrlInfo
    {
        string Url { get; set; }

        bool IsHideHeaderWithFullScreen { get; set; }

        string GameLobbyTypeValue { get; set; }

        string Title { get; set; }
    }
}