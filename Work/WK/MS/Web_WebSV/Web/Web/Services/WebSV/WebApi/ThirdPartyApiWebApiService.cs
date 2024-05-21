using JxBackendService.Model.ViewModel;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Web;
using Web.Services.WebSV.Base;
using JxBackendService.Common.Util;
using JxBackendService.Model.Entity;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.Param.ThirdParty;
using JxLottery.Adapters.Models.Lottery;

namespace Web.Services.WebSV.WebApi
{
    public class ThirdPartyApiWebApiService : BaseWebSVService, IThirdPartyApiWebSVService
    {
        protected override string RemoteControllerName => "ThirdPartyApiService";

        public FrontsideMenu GetActiveFrontsideMenu(string productCode, string gameCode)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "productCode", productCode },
                { "gameCode", gameCode },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponse<FrontsideMenu>(queryStringParts);
        }

        public BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(string productCode, string loginInfoJson, bool isMobile, string correlationId)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "productCode", productCode },
                { "loginInfoJson", loginInfoJson },
                { "isMobile", isMobile.ToString().ToLower() },
                { "correlationId", correlationId },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponse<BaseReturnDataModel<TPGameOpenParam>>(queryStringParts);
        }
    }
}