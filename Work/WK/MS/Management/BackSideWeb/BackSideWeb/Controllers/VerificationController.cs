using BackSideWeb.Controllers.Base;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BackSideWeb.Controllers
{
    public class VerificationController : BaseAuthController
    {
        private readonly Lazy<IBWAuthenticatorService> _bwAuthenticatorService;

        public VerificationController()
        {
            _bwAuthenticatorService = DependencyUtil.ResolveJxBackendService<IBWAuthenticatorService>(EnvLoginUser, DbConnectionTypes.Master);
        }

        [HttpGet]
        public JsonResult CheckVerificationExpiring()
        {
            int userid = EnvLoginUser.LoginUser.UserId;

            return Json(_bwAuthenticatorService.Value.CheckVerificationExpiring(userid));
        }
    }
}