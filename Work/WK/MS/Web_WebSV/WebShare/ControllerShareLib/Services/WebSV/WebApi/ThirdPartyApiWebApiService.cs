using ControllerShareLib.Services.WebSV.Base;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;

namespace ControllerShareLib.Services.WebSV.WebApi
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

            return GetHttpGetResponse<FrontsideMenu>(nameof(GetActiveFrontsideMenu), queryStringParts);
        }

        public BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(ForwardGameUrlSVApiParam param)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "productCode", param.ProductCode },
                { "loginInfoJson", param.LoginInfoJson },
                { "isMobile", param.IsMobile.ToString().ToLower() },
                { "correlationId", param.CorrelationId },
            };

            string[] queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponse<BaseReturnDataModel<TPGameOpenParam>>(nameof(GetForwardGameUrl), queryStringParts);
        }

        public string GetTPGameLaunchURLHTML(string token)
        {
            var queryStringMap = new Dictionary<string, string>
            {
                { "token", token},
            };

            var queryStringParts = GetQueryStringParts(queryStringMap);

            return GetHttpGetResponseString(nameof(GetTPGameLaunchURLHTML), queryStringParts);
        }
    }
}