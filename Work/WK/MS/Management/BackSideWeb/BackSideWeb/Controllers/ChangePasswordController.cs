using BackSideWeb.Controllers.Base;
using BackSideWeb.Filters;
using BackSideWeb.Models.Enums;
using JxBackendService.Attributes;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers
{
    public class ChangePasswordController : BaseAuthController
    {
        private readonly Lazy<IBWChangePasswordService> _bwChangePasswordService;

        public ChangePasswordController()
        {
            _bwChangePasswordService = DependencyUtil.ResolveJxBackendService<IBWChangePasswordService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        [HttpGet]
        public IActionResult Index()
        {
            InitChangePasswordView();
            ViewBag.ClientPopupWindowServiceName = "changePasswordEditSingleRowService";

            return View();
        }

        [HttpGet]
        public IActionResult GetEditModalView()
        {
            InitChangePasswordView();
            ViewBag.ClientPopupWindowServiceName = "editSingleRowService";

            return View("Edit");
        }

        [HttpPost]
        [AjaxValidModelState]
        public virtual IActionResult Update(ChangePasswordParam param)
        {
            BaseReturnModel baseReturnModel = _bwChangePasswordService.Value.ChangePassword(param);

            return Json(baseReturnModel);
        }

        private void InitChangePasswordView()
        {
            SetPageTitle("修改密码");
            SetLayout(LayoutType.EditSingleRow);
            ViewBag.IsShowChangePasswordButton = false;
            ViewBag.SubmitUrl = Url.Action(nameof(Update));
        }
    }
}