using System.Collections.Generic;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ViewModel.ThirdParty;

namespace JxBackendService.Interface.Repository.Game
{
    public interface IGameLobbyListRep : IBaseDbRepository<GameLobbyList>
    {
        List<GameLobbyInfo> GetActiveGameLobbyList(string thirtyPartyCode);

        GameLobbyList GetByCodes(string thirdPartyCode, string gameCode);

        PagedResultModel<GameLobbyList> GetPagedModel(SlotGameManageQueryParam queryParam);
    }
}