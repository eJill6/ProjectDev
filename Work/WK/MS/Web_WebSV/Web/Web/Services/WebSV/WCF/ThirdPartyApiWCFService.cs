using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Interface;
using System;

namespace Web.Services.WebSV.WCF
{
    public class ThirdPartyApiWCFService : IThirdPartyApiWebSVService
    {
        private readonly ThirdPartyApiService.IThirdPartyApiWCFService _thirdPartyApiWCFService;

        public ThirdPartyApiWCFService()
        {
            _thirdPartyApiWCFService = DependencyUtil.ResolveService<ThirdPartyApiService.IThirdPartyApiWCFService>();
        }

        public FrontsideMenu GetActiveFrontsideMenu(string productCode, string gameCode)
            => _thirdPartyApiWCFService.GetActiveFrontsideMenu(productCode, gameCode).CastByJson<FrontsideMenu>();

        public BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(string productCode, string loginInfoJson, bool isMobile, string correlationId)
        {
            return _thirdPartyApiWCFService.GetForwardGameUrl(productCode, loginInfoJson, isMobile, correlationId)
                .CastByJson<BaseReturnDataModel<TPGameOpenParam>>();
        }
    }
}