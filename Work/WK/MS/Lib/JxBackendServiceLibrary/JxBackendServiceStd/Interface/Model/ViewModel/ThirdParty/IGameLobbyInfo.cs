namespace JxBackendService.Interface.Model.ViewModel.ThirdParty
{
    public interface IGameLobbyInfo
    {
        int No { get; set; }

        string ChineseName { get; set; }

        string MobileGameCode { get; set; }

        bool IsHot { get; set; }

        int Sort { get; set; }

        string FullImageUrl { get; set; }
    }
}