using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers.GameCenterManage
{
    public class GameTypeManageController : BaseGameCenterManageController<object>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/gameCenterManage/gameTypeManageService.js"
        };

        protected override string ClientServiceName => "gameTypeManageService";

        public override ActionResult GetGridViewResult(object _)
        {
            List<GameCenterManageModel> model = FrontsideMenuTypeService.GetModels();
            return PartialView(model);
        }

        public override BaseReturnModel Update([FromBody] List<GameCenterUpdateParam> param)
        {
            var gameTypeManageService = DependencyUtil.ResolveJxBackendService<IGameTypeManageService>(EnvLoginUser, DbConnectionTypes.Master).Value;

            return gameTypeManageService.Update(param);
        }
    }
}