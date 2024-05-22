using ControllerShareLib.Interfaces.Service;
using ControllerShareLib.Interfaces.Service.Controller;
using ControllerShareLib.Models.Game;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using M.Core.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace M.Core.Controllers
{
    public class GameCenterController : BaseAuthApiController
    {
        private readonly Lazy<IGameCenterControllerService> _gameCenterControllerService;

        public GameCenterController()
        {
            _gameCenterControllerService = DependencyUtil.ResolveService<IGameCenterControllerService>();
        }

        /// <summary> 取得第三方登入網址 </summary>
        [HttpPost]
        public AppResponseModel<AppOpenUrlInfo> GetForwardGameUrl(BaseGameCenterLogin baseGameCenterLogin)
        {
            BaseReturnDataModel<AppOpenUrlInfo> returnDataModel = _gameCenterControllerService.Value.GetMobileApiForwardGameUrl(baseGameCenterLogin);

            return new AppResponseModel<AppOpenUrlInfo>(returnDataModel);
        }

        /// <summary> 直播間直接取得第三方遊戲網址 </summary>
        [HttpPost]
        public AppResponseModel<MobileApiEnterTPGameUrlInfo> EnterThirdPartyGame(EnterThirdPartyGameParam param)
        {
            BaseReturnDataModel<MobileApiEnterTPGameUrlInfo> returnDataModel = _gameCenterControllerService.Value.GetMobileApiEnterThirdPartyGame(param);

            return new AppResponseModel<MobileApiEnterTPGameUrlInfo>(returnDataModel);
        }
    }
}