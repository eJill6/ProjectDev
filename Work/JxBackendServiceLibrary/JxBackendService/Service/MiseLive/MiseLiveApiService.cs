using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Route;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Interface.Service.MiseLive;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.MiseLive.Request;
using JxBackendService.Model.MiseLive.Response;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace JxBackendService.Service.MiseLive
{
    public class MiseLiveApiService : IMiseLiveApiService
    {
        #region api path

        /// <summary>查询用户余额api path</summary>
        private static readonly string s_balancePath = "/dapi/user/balance";

        /// <summary>资金转入api path</summary>
        private static readonly string s_transferInPath = "/dapi/transfer/in";

        /// <summary>资金转出api path</summary>
        private static readonly string s_transferOutPath = "/dapi/transfer/out";

        /// <summary>查詢轉帳結果api path</summary>
        private static readonly string s_transferResultPath = "/dapi/transfer/result";

        #endregion api path

        private readonly IHttpWebRequestUtilService _httpWebRequestUtilService;

        private readonly IMiseLiveAppSettingService _miseLiveAppSettingService;

        public MiseLiveApiService()
        {
            _httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();
            _miseLiveAppSettingService = DependencyUtil.ResolveService<IMiseLiveAppSettingService>();
        }

        #region 查詢用戶餘額

        /// <summary>查询用户余额</summary>
        public MiseLiveResponse<MiseLiveBalance> GetUserBalance(IMiseLiveUserBalanceRequestParam request)
        {
            return GetUserBalanceApiRemoteResult(request).Deserialize<MiseLiveResponse<MiseLiveBalance>>();
        }

        /// <summary>查询用户余额api response</summary>
        protected virtual string GetUserBalanceApiRemoteResult(IMiseLiveUserBalanceRequestParam requestParam)
        {
            IMiseLiveUserBalanceRequest request = requestParam.CastByJson<MiseLiveUserBalanceRequest>();
            request.Salt = _miseLiveAppSettingService.MSSealSalt;

            string requestUrl = _httpWebRequestUtilService.CombineUrl(
                _miseLiveAppSettingService.MSSealAddress,
                s_balancePath,
                $"?{nameof(request.UserId).ToCamelCase()}={request.UserId}");

            request.Sign = CreateMd5Sign(request);
            string responseString = DoRequestWithLog(MethodBase.GetCurrentMethod().Name, HttpMethod.Get, requestUrl, request);

            return responseString;
        }

        #endregion 查詢用戶餘額

        #region 资金转入

        /// <summary>资金转入</summary>
        public MiseLiveResponse<MiseLiveBalance> TransferIn(IMiseLiveTransferRequestParam requestParam)
        {
            return GetTransferInApiRemoteResult(requestParam).Deserialize<MiseLiveResponse<MiseLiveBalance>>();
        }

        /// <summary>资金转入api response</summary>
        protected virtual string GetTransferInApiRemoteResult(IMiseLiveTransferRequestParam requestParam)
        {
            IMiseLiveTransferRequest request = requestParam.CastByJson<MiseLiveTransferRequest>();
            request.Salt = _miseLiveAppSettingService.MSSealSalt;

            string requestUrl = _httpWebRequestUtilService.CombineUrl(
                _miseLiveAppSettingService.MSSealAddress,
                s_transferInPath);

            request.Sign = CreateMd5Sign(request);
            string responseString = DoRequestWithLog(MethodBase.GetCurrentMethod().Name, HttpMethod.Post, requestUrl, request);

            return responseString;
        }

        #endregion 资金转入

        #region 资金转出

        /// <summary>资金转出</summary>
        public MiseLiveResponse<MiseLiveBalance> TransferOut(IMiseLiveTransferRequestParam requestParam)
        {
            return GetTransferOutApiRemoteResult(requestParam).Deserialize<MiseLiveResponse<MiseLiveBalance>>();
        }

        /// <summary>资金转入api response</summary>
        protected virtual string GetTransferOutApiRemoteResult(IMiseLiveTransferRequestParam requestParam)
        {
            IMiseLiveTransferRequest request = requestParam.CastByJson<MiseLiveTransferRequest>();
            request.Salt = _miseLiveAppSettingService.MSSealSalt;

            string requestUrl = _httpWebRequestUtilService.CombineUrl(
                _miseLiveAppSettingService.MSSealAddress,
                s_transferOutPath);

            request.Sign = CreateMd5Sign(request);
            string responseString = DoRequestWithLog(MethodBase.GetCurrentMethod().Name, HttpMethod.Post, requestUrl, request);

            return responseString;
        }

        #endregion 资金转出

        #region 查詢轉帳結果

        /// <summary>查詢轉帳結果</summary>
        public MiseLiveResponse<MiseLiveTransferOrder> GetTransferOrderResult(IMiseLiveTransferOrderRequestParam requestParam)
        {
            return GetTransferResultApiRemoteResult(requestParam).Deserialize<MiseLiveResponse<MiseLiveTransferOrder>>();
        }

        /// <summary>查詢轉帳結果api response</summary>
        protected virtual string GetTransferResultApiRemoteResult(IMiseLiveTransferOrderRequestParam requestParam)
        {
            IMiseLiveTransferOrderRequest request = requestParam.CastByJson<MiseLiveTransferOrderRequest>();
            request.Salt = _miseLiveAppSettingService.MSSealSalt;

            RouteValueDictionary routeValues = RouteUtil.ConvertToRouteValues(
                request,
                (property) =>
                {
                    return property.GetCustomAttributes(inherit: true).Any(w => w is JsonIgnoreAttribute);
                });

            string queryString = string.Join("&", routeValues.Select(s => $"{s.Key.ToCamelCase()}={HttpUtility.UrlEncode(s.Value.ToNonNullString())}"));

            string requestUrl = _httpWebRequestUtilService.CombineUrl(
               _miseLiveAppSettingService.MSSealAddress,
               s_transferResultPath,
               $"?{queryString}");

            request.Sign = CreateMd5Sign(request);
            string responseString = DoRequestWithLog(MethodBase.GetCurrentMethod().Name, HttpMethod.Get, requestUrl, request);

            return responseString;
        }

        #endregion 查詢轉帳結果

        private string CreateMd5Sign<T>(T values)
        {
            return ValidSignUtil.CreateSign(values);
        }

        private string DoRequestWithLog<T>(string purpose, HttpMethod httpMethod, string requestUrl, T request) where T : IMiseLiveRequest
        {
            var headers = new Dictionary<string, string>
            {
                { "sign", request.Sign},
                { "ts", request.Ts.ToString() }
            };

            LogUtil.ForcedDebug($"url={requestUrl}, request={request.ToJsonString(ignoreNull: true, isCamelCaseNaming: true)}, header={headers.ToJsonString()}");

            var webRequestParam = new WebRequestParam()
            {
                Url = requestUrl,
                Body = request.ToJsonString(isCamelCaseNaming: true),
                ContentType = HttpWebRequestContentType.Json,
                IsResponseValidJson = true,
                Method = httpMethod,
                Headers = headers,
                Purpose = purpose,
            };

            string responseString = HttpWebRequestUtil.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);

            LogUtil.ForcedDebug($"response={responseString}");

            return responseString;
        }
    }
}