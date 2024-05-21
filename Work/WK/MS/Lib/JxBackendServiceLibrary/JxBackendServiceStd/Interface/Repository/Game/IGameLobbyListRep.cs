using JxBackendService.Model.Entity;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Game
{
    public interface IGameLobbyListRep : IBaseDbRepository<GameLobbyList>
    {
        List<GameLobbyList> GetActiveGameLobbyList(string thirtyPartyCode);

        GameLobbyList GetByCodes(string thirdPartyCode, string gameCode);

        PagedResultModel<GameLobbyList> GetPagedModel(SlotGameManageQueryParam queryParam);
    }
}