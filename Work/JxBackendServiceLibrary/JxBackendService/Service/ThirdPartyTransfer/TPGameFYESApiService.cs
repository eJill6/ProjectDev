using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.FYES;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameFYESApiService : BaseTPGameApiService
    {
        public static readonly int MaxSearchRangeMinutes = 30;

        public static readonly int BackSqliteSearchDateMinutes = 20;

        public override PlatformProduct Product => PlatformProduct.FYES;

        public TPGameFYESApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var requestModel = new FYESTransferRequestModel
            {
                UserName = tpGameAccount,
                Type = isMoneyIn ? FYESTransferType.In.Value : FYESTransferType.Out.Value,
                Money = tpGameMoneyInfo.Amount,
                ID = tpGameMoneyInfo.OrderID,
            };

            DoPostRequest(FYESSharedAppSetting.TransferUrl, requestModel, out DetailRequestAndResponse detail);

            return detail;
        }

        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var requestModel = new FYESGetTransferInfoRequestModel
            {
                ID = tpGameMoneyInfo.OrderID
            };

            DoPostRequest(FYESSharedAppSetting.GetTransferInfoUrl, requestModel, out DetailRequestAndResponse detail);

            return detail;
        }

        protected override string GetRemoteUserScoreApiResult(string tpGameAccount)
        {
            var requestModel = new FYESGetBalanceRequestModel
            {
                UserName = tpGameAccount,
            };

            BaseReturnDataModel<string> returnModel = DoPostRequest(FYESSharedAppSetting.GetBalanceUrl, requestModel, out DetailRequestAndResponse detail);

            return returnModel.DataModel;
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            DateTime maxSearchEndDate = DateTime.Now;
            DateTime searchStartDate = DateTime.Now.AddMinutes(-MaxSearchRangeMinutes);

            if (!lastSearchToken.IsNullOrEmpty())
            {
                searchStartDate = DateTime.Parse(lastSearchToken);
            }

            DateTime searchEndDate = searchStartDate.AddMinutes(MaxSearchRangeMinutes);

            if (searchEndDate > maxSearchEndDate)
            {
                searchEndDate = maxSearchEndDate;
            }

            if (searchEndDate < searchStartDate)
            {
                searchEndDate = searchStartDate;
            }

            int totalPageCount = 1; //不知道有多少頁 先預設1
            var requestBodys = new List<string>();
            var responseContents = new List<string>();

            var requestData = new FYESGetBetLogRequestModel
            {
                StartAt = searchStartDate.ToFormatDateTimeString(),
                EndAt = searchEndDate.ToFormatDateTimeString(),
                PageSize = FYESSharedAppSetting.BetLogPageSize,
            };

            for (int pageIndex = 1; pageIndex <= totalPageCount; pageIndex++)
            {
                requestData.PageIndex = pageIndex;

                // 10秒/4次
                if (pageIndex > 1)
                {
                    Thread.Sleep(400);
                }

                BaseReturnDataModel<string> returnModel = DoPostRequest(FYESSharedAppSetting.GetBetLogUrl, requestData, out DetailRequestAndResponse detail);
                var betLogModel = returnModel.DataModel.Deserialize<FYESGetBetLogResponseModel>();

                //確保全部分頁的request都要成功
                if (!betLogModel.IsSuccess)
                {
                    return new BaseReturnDataModel<RequestAndResponse>(betLogModel.msg, new RequestAndResponse()
                    {
                        RequestBody = requestData.ToJsonString(),
                        ResponseContent = returnModel.DataModel
                    });
                }

                requestBodys.Add(requestData.ToJsonString());
                responseContents.Add(returnModel.DataModel);

                if (pageIndex == 1)
                {
                    //校正頁數
                    totalPageCount = (int)Math.Ceiling((decimal)betLogModel.info.RecordCount / requestData.PageSize);
                }
            }

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success,
                new RequestAndResponse()
                {
                    RequestBody = requestBodys.ToJsonString(),
                    ResponseContent = responseContents.ToJsonString()
                });
        }

        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            FYESTransferResponseModel transferModel = apiResult.Deserialize<FYESTransferResponseModel>();

            if (transferModel.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            return new BaseReturnDataModel<UserScore>(transferModel.msg, null);
        }

        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            FYESGetTransferInfoResponseModel transferModel = apiResult.Deserialize<FYESGetTransferInfoResponseModel>();

            if (transferModel.IsSuccess)
            {
                if (transferModel.info.Status == FYESTransferStatus.Finish)
                {
                    return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
                }

                string statusName = FYESTransferStatus.GetName(transferModel.info.Status);
                statusName = statusName.IsNullOrEmpty() ? transferModel.info.Status : statusName; // 防呆，以免第三方调整后对不上列举显示空白

                return new BaseReturnDataModel<UserScore>($"状态:{statusName}", null);
            }

            return new BaseReturnDataModel<UserScore>($"error:{transferModel.msg}", null);
        }

        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            FYESGetBalanceResponseModel userScoreModel = apiResult.Deserialize<FYESGetBalanceResponseModel>();

            if (userScoreModel.IsSuccess)
            {
                return new BaseReturnDataModel<UserScore>(
                    ReturnCode.Success,
                    new UserScore() { AvailableScores = userScoreModel.info.Money.ToDecimal() });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.msg, null);
        }

        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(param);

            if (returnModel.IsSuccess)
            {
                FYESRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<FYESRegisterResponseModel>();

                //檢查帳號重複同一隻API
                if (registerModel.IsSuccess ||
                    returnModel.DataModel.Deserialize<FYESErrorResponseModel>().info.Error == FYESErrorResponseCode.UserExists)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.msg);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo)
        {
            var request = new FYESLoginRequestModel
            {
                UserName = tpGameAccount,
            };

            return DoPostRequest(FYESSharedAppSetting.LoginUrl, request, out DetailRequestAndResponse detail);
        }

        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteLoginApiResult(tpGameAccount, ipAddress, isMobile, loginInfo);

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(returnModel.Message, string.Empty);
            }

            FYESLoginResponseModel loginModel = returnModel.DataModel.Deserialize<FYESLoginResponseModel>();

            if (loginModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(ReturnCode.Success, loginModel.info.Url);
            }

            return new BaseReturnDataModel<string>(loginModel.msg);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount)
        {
            throw new NotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            var requestModel = new FYESRegisterRequestModel
            {
                UserName = param.TPGameAccount,
                Password = param.TPGameAccount,
            };

            return DoPostRequest(FYESSharedAppSetting.RegisterUrl, requestModel, out DetailRequestAndResponse detail);
        }

        protected override PlatformHandicap ConvertToPlatformHandicap(string handicap)
        {
            FYESHandicap fyesHandicap = FYESHandicap.GetSingle(handicap);

            if (fyesHandicap == null)
            {
                return null;
            }

            return fyesHandicap.PlatformHandicap;
        }

        /// <summary>
        /// 打第三方API
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest<T>(string relativeUrl, T requestBody, out DetailRequestAndResponse detail)
        {
            return DoPostRequest(FYESSharedAppSetting.ServiceUrl, relativeUrl, requestBody.ToKeyValueURL(), out detail);
        }

        /// <summary>
        /// 打第三方API
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string url, string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            string fullUrl = GetFullUrl(url, relativeUrl);

            var headers = new Dictionary<string, string>
            {
                { "Authorization", FYESSharedAppSetting.AuthorizationKey },
                { "Content-Language", FYESSharedAppSetting.Language }
            };

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();
            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = fullUrl,
                Body = requestBody,
                Headers = headers,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                IsResponseValidJson = true
            };

            string apiResult = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConverToApiReturnDataModel(httpStatusCode, apiResult);
        }
    }
}