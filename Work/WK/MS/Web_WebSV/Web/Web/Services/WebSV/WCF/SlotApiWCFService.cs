using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ViewModel.ThirdParty;
using SLPolyGame.Web.Interface;
using System.Collections.Generic;

namespace Web.Services.WebSV.WCF
{
    public class SlotApiWCFService : ISlotApiWebSVService
    {
        private readonly SlotApiService.ISlotApiService _slotApiService;

        public SlotApiWCFService()
        {
            _slotApiService = DependencyUtil.ResolveService<SlotApiService.ISlotApiService>();
        }

        public List<GameLobbyInfo> GetGameList(string gameLobbyType)
            => _slotApiService.GetGameList(gameLobbyType).CastByJson<List<GameLobbyInfo>>();

        public string GetJackpotAmount(string gameLobbyType)
            => _slotApiService.GetJackpotAmount(gameLobbyType);
    }
}