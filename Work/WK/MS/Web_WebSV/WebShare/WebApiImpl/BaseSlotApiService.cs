using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using SLPolyGame.Web.Interface;
using System.Collections.Generic;

namespace WebApiImpl
{
    public abstract class BaseSlotApiService : BaseWebApiService, ISlotApiWebSVService
    {
        public BaseSlotApiService()
        {
        }

        public List<GameLobbyInfo> GetGameList(string gameLobbyType)
        {
            var subGameService = DependencyUtil.ResolveJxBackendService<ISubGameService>(
                GameLobbyType.GetSingle(gameLobbyType),
                EnvLoginUser,
                DbConnectionTypes.Slave).Value;

            return subGameService.GetVisibleGameList();
        }

        public string GetJackpotAmount(string gameLobbyType)
        {
            var imoneSubGameService = DependencyUtil.ResolveJxBackendService<IIMOneSubGameService>(
                GameLobbyType.GetSingle(gameLobbyType),
                EnvLoginUser,
                DbConnectionTypes.Slave).Value;

            return imoneSubGameService.GetJackpotAmount();
        }
    }
}