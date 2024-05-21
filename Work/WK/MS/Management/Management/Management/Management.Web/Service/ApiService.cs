using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Param.BackSide;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Authenticator;
using JxBackendService.Model.ViewModel.BackSide;
using Management.Administration;
using Management.Web.Modules.SystemSettings.LotteryInfo;
using Microsoft.AspNetCore.Http;
using Serenity;
using Serenity.Abstractions;
using System;
using System.Collections.Generic;
using System.Net;

namespace Management.Web.Service
{
    public class ApiService : IApiService
    {
        private readonly ITwoLevelCache _cache;
        private readonly IUserAccessor _userAccessor;
        private readonly IUserRetrieveService _userRetriever;

        private readonly string _serviceUrl = Startup.AppSetting.ServiceUrl;

        private static readonly string _validateLoginTokenPath = "/Management/ValidateLoginToken";
        private static readonly string _getQrCodePath = "/Management/GetQrCode";

        public ApiService(ITwoLevelCache cache, IUserAccessor userAccessor, IUserRetrieveService userRetrieveService)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
            _userRetriever = userRetrieveService ?? throw new ArgumentNullException(nameof(userRetrieveService));
        }

        public BaseReturnDataModel<ValidateLoginTokenResult> ValidateLoginToken(ValidateLoginTokenRequest request)
        {
            return DoPostRequest<ValidateLoginTokenRequest, BaseReturnDataModel<ValidateLoginTokenResult>>(
                nameof(ValidateLoginToken), _validateLoginTokenPath, request);
        }

        public BaseReturnDataModel<QrCodeViewModel> GetQrCode(GetQrCodeRequest request)
        {
            return DoPostRequest<GetQrCodeRequest, BaseReturnDataModel<QrCodeViewModel>>(
                 nameof(GetQrCode), _getQrCodePath, request);
        }

        private TResult DoPostRequest<TRequest, TResult>(string methodName, string path, TRequest request)
        {
            string requestUrl = _serviceUrl + path;

            var header = new Dictionary<string, string>();

            if (_userAccessor.User?.GetIdentifier() is string userId)
            {
                string token = _cache.GetLocalStoreOnly(
                    $"LoginUser:{userId}",
                    TimeSpan.Zero,
                    UserRow.Fields.GenerationKey,
                    () => string.Empty);

                header.Add(nameof(HttpRequest.Headers.Authorization), token);
            }

            string responseString = TryGetResponse(
                methodName,
                requestUrl,
                header,
                request);

            return responseString.Deserialize<TResult>();
        }

        private string TryGetResponse<TRequest>(string methodName, string requestUrl, Dictionary<string, string> header, TRequest request)
        {
            try
            {
                return HttpWebRequestUtil.GetResponse(
                    methodName,
                    HttpMethod.Post,
                    requestUrl,
                    header,
                    request,
                    isResponseValidJson: true);
            }
            catch (WebException webException)
            {
                if (webException.Message.Equals(nameof(HttpStatusCode.Unauthorized)))
                {
                    throw new Exception("请重新登入");
                    //todo signout
                }

                throw webException;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public BaseReturnDataModel<List<LotteryInfoResult>> GetLotteryInfoDatas(BackSideModel request)
        {
            return DoPostRequest<BackSideModel, BaseReturnDataModel<List<LotteryInfoResult>>>(
                nameof(GetLotteryInfoDatas), "/Management/GetLotteryInfoDatas", request);
        }
        public BaseReturnDataModel<List<PlayTypeInfo>> GetPlayTypeInfo(BackSideModel request)
        {
            return DoPostRequest<BackSideModel, BaseReturnDataModel<List<PlayTypeInfo>>>(
            nameof(GetPlayTypeInfo), "/Management/GetPlayTypeInfo", request);
        }
        public BaseReturnDataModel<string> UpdateLotteryInfo(List<UpdateLotteryInfoRequest> request)
        {
            return DoPostRequest<List<UpdateLotteryInfoRequest>, BaseReturnDataModel<string>>(
           nameof(UpdateLotteryInfo), "/Management/UpdateLotteryInfo", request);
        }
        public BaseReturnDataModel<string> UpdateLotteryStatus(BackSideModel request)
        {
            return DoPostRequest<BackSideModel, BaseReturnDataModel<string>>(
           nameof(UpdateLotteryStatus), "/Management/UpdateLotteryStatus", request);
        }
        public BaseReturnDataModel<string> UpdatePlayTypeStatus(BackSideModel request)
        {
            return DoPostRequest<BackSideModel, BaseReturnDataModel<string>>(
           nameof(UpdatePlayTypeStatus), "/Management/UpdatePlayTypeStatus", request);
        }
    }
}
