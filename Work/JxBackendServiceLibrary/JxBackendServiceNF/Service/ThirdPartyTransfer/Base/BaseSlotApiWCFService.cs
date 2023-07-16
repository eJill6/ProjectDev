using System.Collections.Generic;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;

namespace JxBackendServiceNF.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseSlotApiWCFService : BaseApplicationService, ISlotApiService
    {
        public BaseSlotApiWCFService()
        {
        }

        public List<GameLobbyInfo> GetGameList(string gameLobbyType)
        {
            var subGameService = DependencyUtil.ResolveJxBackendService<ISubGameService>(GameLobbyType.GetSingle(gameLobbyType), EnvLoginUser, DbConnectionTypes.Slave);

            return subGameService.GetVisibleGameList();
        }

        public string GetJackpotAmount(string gameLobbyType)
        {
            var subGameService = DependencyUtil.ResolveJxBackendService<IIMOneSubGameService>(GameLobbyType.GetSingle(gameLobbyType), EnvLoginUser, DbConnectionTypes.Slave);

            return subGameService.GetJackpotAmount();
        }
    }
}