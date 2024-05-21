using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums.Net;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SLPolyGame.Web.Core.Controllers.Base;
using SLPolyGame.Web.Core.Filters;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace SLPolyGame.Web.Core.Controllers.Web
{
    public class ThirdPartyApiServiceController : BaseAuthApiController, IThirdPartyApiWebSVService
    {
        private readonly Lazy<IThirdPartyApiWebSVService> _thirdPartyApiWebSVService;

        public ThirdPartyApiServiceController()
        {
            _thirdPartyApiWebSVService = DependencyUtil.ResolveService<IThirdPartyApiWebSVService>();
        }

        [HttpGet]
        public FrontsideMenu GetActiveFrontsideMenu(string productCode, string? gameCode)
            => _thirdPartyApiWebSVService.Value.GetActiveFrontsideMenu(productCode, gameCode);

        [HttpGet]
        [ApiLogRequest(isLogToDB: true)]
        public BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl([FromQuery] ForwardGameUrlSVApiParam param)
        {
            return _thirdPartyApiWebSVService.Value.GetForwardGameUrl(param);
        }

        [AllowAnonymous]
        [HttpGet]
        public string GetTPGameLaunchURLHTML(string token)
            => _thirdPartyApiWebSVService.Value.GetTPGameLaunchURLHTML(token);
    }
}