using JxBackendService.Model.ViewModel.ThirdParty;

namespace Web.Models
{
    public class ThirdPartyGamesViewModel : Pagination<GameLobbyInfo>
    {
        public int GameTabType { get; set; }

        public string SearchGameName { get; set; }

        public bool IsSquareGameImage { get; set; }

        public bool IsSelfOpenPage { get; set; }
    }
}