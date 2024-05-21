using JxBackendService.DependencyInjection;
using JxBackendService.Model.ViewModel.ThirdParty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Core.Controllers.Base;
using SLPolyGame.Web.Interface;

namespace SLPolyGame.Web.Core.Controllers.Web
{
    public class SlotApiServiceController : BaseAuthApiController, ISlotApiWebSVService
    {
        private readonly Lazy<ISlotApiWebSVService> _slotApiWebSVService;

        public SlotApiServiceController()
        {
            _slotApiWebSVService = DependencyUtil.ResolveService<ISlotApiWebSVService>();
        }

        [HttpGet, AllowAnonymous]
        public List<GameLobbyInfo> GetGameList(string gameLobbyType)
            => _slotApiWebSVService.Value.GetGameList(gameLobbyType);

        [HttpGet, AllowAnonymous]
        public string GetJackpotAmount(string gameLobbyType)
            => _slotApiWebSVService.Value.GetJackpotAmount(gameLobbyType);
    }
}