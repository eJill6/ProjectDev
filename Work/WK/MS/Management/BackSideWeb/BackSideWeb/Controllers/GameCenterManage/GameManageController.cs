using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Controllers.GameCenterManage
{
    public class GameManageController : BaseGameCenterManageController<QueryGameManageParam>
    {
        protected override string[] PageJavaScripts => new string[]
        {
            "business/gameCenterManage/gameManageService.js",
        };

        protected override string ClientServiceName => "gameManageService";

        public override ActionResult GetGridViewResult(QueryGameManageParam searchParam)
        {
            List<GameCenterManageModel> model = FrontsideMenuService.GetModelsByType(FrontsideMenuTypeSetting.GetSingle(searchParam.TypeValue));

            return PartialView(model);
        }

        public override ActionResult Index()
        {
            base.Index();

            List<JxBackendSelectListItem> typeSelectListItems = FrontsideMenuTypeService.GetGameTypesSelectListItems();

            return View(typeSelectListItems.CastByJson<List<SelectListItem>>());
        }

        public override BaseReturnModel Update([FromBody] List<GameCenterUpdateParam> param)
        {
            var gameManageService = DependencyUtil.ResolveJxBackendService<IGameManageService>(EnvLoginUser, DbConnectionTypes.Master).Value;

            return gameManageService.Update(param);
        }
    }
}