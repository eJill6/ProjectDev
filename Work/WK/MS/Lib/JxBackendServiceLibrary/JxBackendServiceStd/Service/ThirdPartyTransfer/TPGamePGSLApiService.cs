using JxBackendService.Common;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Net;
using System.Web;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGamePGSLApiService : BaseTPGameApiService
    {
        private static readonly int _maxSearchCount = 1500;

        private PGSLSharedAppSetting AppSettings => PGSLSharedAppSetting.Instance;

        public override PlatformProduct Product => PlatformProduct.PGSL;

        protected override int? TransferAmountFloorDigit => 2;

        private static readonly int s_thirdPartyGameHtmlTokenSeconds = 30;

        public TPGamePGSLApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount) => null;

        /// <summary>
        /// 轉帳
        /// </summary>
        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new PGTransferRequestModel
            {
                player_name = createRemoteAccountParam.TPGameAccount,
                amount = tpGameMoneyInfo.Amount.ToString("0.####"),
                currency = AppSettings.Currncy,
                transfer_reference = tpGameMoneyInfo.OrderID
            };

            DoPostRequest(
                isMoneyIn ? AppSettings.TransferInUrl : AppSettings.TransferOutUrl,
                request.ToKeyValueURL(),
                out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢訂單
        /// </summary>
        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            var request = new PGCheckTransferRequestModel
            {
                player_name = tpGameAccount,
                transfer_reference = tpGameMoneyInfo.OrderID
            };

            DoPostRequest(AppSettings.CheckTransferUrl, request.ToKeyValueURL(), out DetailRequestAndResponse detail);

            return detail;
        }

        /// <summary>
        /// 查詢餘額
        /// </summary>
        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            var request = new PGUserBaseRequestModel
            {
                player_name = createRemoteAccountParam.TPGameAccount
            };

            var returnModel = DoPostRequest(AppSettings.GetBalanceUrl, request.ToKeyValueURL(), out DetailRequestAndResponse detail);

            return returnModel.DataModel;
        }

        /// <summary>
        /// 取得盈虧資料
        /// </summary>
        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            var request = new PGBetLogRequestModel
            {
                bet_type = 1, //1:真實遊戲
                count = _maxSearchCount,
                row_version = long.Parse(lastSearchToken)
            };

            string requestBody = request.ToKeyValueURL();

            //這邊是用另外一個網址
            BaseReturnDataModel<string> returnModel = DoPostRequest(
                AppSettings.GrabServiceUrl,
                AppSettings.GrabUrlPathPrefix + AppSettings.GetBetLogUrl, requestBody,
                out DetailRequestAndResponse detail);

            var returnDataModel = returnModel.CastByCodeAndMessage<RequestAndResponse>();

            returnDataModel.DataModel = new RequestAndResponse()
            {
                RequestBody = requestBody,
                ResponseContent = returnModel.DataModel
            };

            return returnDataModel;
        }

        /// <summary>
        /// 處理轉帳回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            PGTransferResponseModel transferModel = apiResult.Deserialize<PGTransferResponseModel>();

            if (transferModel.error == null && transferModel.data != null)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success);
            }

            string errorMsg = PGResponseCode.CompleteMessage(transferModel.error.message, transferModel.error.code);

            return new BaseReturnDataModel<UserScore>(errorMsg);
        }

        /// <summary>
        /// 處理查詢訂單回傳結果
        /// </summary>
        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            PGQueryOrderResponseModel transferModel = apiResult.Deserialize<PGQueryOrderResponseModel>();

            if (transferModel.data != null && transferModel.error == null)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            string errorMsg = PGResponseCode.CompleteMessage(transferModel.error.message, transferModel.error.code);

            return new BaseReturnModel($"error:{errorMsg}");
        }

        /// <summary>
        /// 處理取得餘額回傳結果
        /// </summary>
        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            PGBalanceResponseModel userScoreModel = apiResult.Deserialize<PGBalanceResponseModel>();

            if (userScoreModel.error == null)
            {
                return new BaseReturnDataModel<UserScore>(ReturnCode.Success, new UserScore() { AvailableScores = userScoreModel.data.cashBalance });
            }

            return new BaseReturnDataModel<UserScore>(userScoreModel.error.message, null);
        }

        /// <summary>
        /// 檢查以及創建帳號
        /// </summary>
        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            BaseReturnDataModel<string> returnModel = GetRemoteCreateAccountApiResult(createRemoteAccountParam);

            if (returnModel.IsSuccess)
            {
                PGRegisterResponseModel registerModel = returnModel.DataModel.Deserialize<PGRegisterResponseModel>();
                //檢查帳號重複同一隻API
                if ((registerModel.data != null && registerModel.data.action_result == 1) || registerModel.error.code == PGResponseCode.AccountExist)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                return new BaseReturnModel(registerModel.error.message);
            }

            return new BaseReturnModel(returnModel.Message);
        }

        /// <summary>
        /// 取得遊戲入口網址
        /// </summary>
        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            string host = AppSettings.LaunchGameUrl;
            string path = AppSettings.LaunchGameUrlPath;

            string url = GetFullUrl(host, path);

            string bodyExtraArgs, bodyPath, urlType;

            if (tpGameRemoteLoginParam.LoginInfo.RemoteCode.IsNullOrEmpty())
            {
                bodyExtraArgs = HttpUtility.UrlEncode($"ops={CreatePlayerSession(tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount)}&l=zh");
                bodyPath = HttpUtility.UrlEncode(AppSettings.WebLobbyPath);
                urlType = AppSettings.WebLobbyUrlType;
            }
            else
            {
                bodyPath = HttpUtility.UrlEncode(AppSettings.FormLaunchGameUrlPath(tpGameRemoteLoginParam.LoginInfo.RemoteCode));
                bodyExtraArgs = HttpUtility.UrlEncode($"btt=1&ops={CreatePlayerSession(tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount)}&l=zh");
                urlType = AppSettings.WebGameUrlType;
            }

            string body = $"operator_token={AppSettings.OperatorToken}&" +
                $"path={bodyPath}&" +
                $"extra_args={bodyExtraArgs}&" +
                $"url_type={urlType}&" +
                $"client_ip={tpGameRemoteLoginParam.IpAddress}";

            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>().Value;

            var webRequestParam = new WebRequestParam()
            {
                Method = HttpMethod.Post,
                Url = url,
                Body = body,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
            };

            string response = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);

            if (httpStatusCode != HttpStatusCode.OK)
            {
                LogUtilService.ForcedDebug($"{nameof(GetRemoteForwardGameUrl)}, httpStatusCode={(int)httpStatusCode}, response={response}");

                string errMessage = $"{ReturnCode.OperationFailed.Name}({httpStatusCode})";

                return new BaseReturnDataModel<string>(errMessage);
            }

            string token = HashExtension.MD5($"{Product.Value}:{tpGameRemoteLoginParam.CreateRemoteAccountParam.TPGameAccount}") + DateTime.Now.ToUnixOfTime().ToString();

            JxCacheService.SetCache(new SearchCacheParam()
            {
                Key = CacheKey.TPGameLaunchURLHTML(token),
                CacheSeconds = s_thirdPartyGameHtmlTokenSeconds
            }, response);

            string returnUrl = httpWebRequestUtilService.CombineUrl(SharedAppSettings.FrontSideWebUrl,
                 $"GameCenter/LaunchURLHTML?token={token}");

            return new BaseReturnDataModel<string>(ReturnCode.Success, returnUrl);
        }

        private string CreatePlayerSession(string tpGameAccount)
        {
            PGLaunchGameRequestModel launchGameRequestModel = new PGLaunchGameRequestModel
            {
                Acc = tpGameAccount,
                Key = EnvLoginUser.LoginUser.UserKey
            };

            //正式環境不用傳，減少長度
            if (EnvCode != EnvironmentCode.Production)
            {
                launchGameRequestModel.EnvCode = EnvCode.Value;
            }

            //加密參數傳遞
            DESTool tool = new DESTool(AppSettings.EncryptKey);
            string encryptedModel = tool.DESEnCode(launchGameRequestModel.ToJsonString());

            return HttpUtility.UrlEncode(encryptedModel);
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            throw new NotSupportedException();
        }

        protected override void DoKickUser(Model.Entity.ThirdPartyUserAccount thirdPartyUserAccount) => new NotImplementedException();

        /// <summary>
        /// 打第三方API
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            return DoPostRequest(AppSettings.ServiceUrl, AppSettings.UrlPathPrefix + relativeUrl, requestBody, out detail);
        }

        /// <summary>
        /// 打第三方API，加密簽名方法
        /// </summary>
        private BaseReturnDataModel<string> DoPostRequest(string url, string relativeUrl, string requestBody, out DetailRequestAndResponse detail)
        {
            url = GetFullUrl(url, relativeUrl);
            string queryString = requestBody + "&operator_token=" + AppSettings.OperatorToken + "&secret_key=" + AppSettings.SecretKey;
            var httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>().Value;

            var webRequestParam = new WebRequestParam()
            {
                Purpose = $"TPGService.{Product.Value}請求",
                Method = HttpMethod.Post,
                Url = url,
                Body = queryString,
                ContentType = HttpWebRequestContentType.WwwFormUrlencoded,
                IsResponseValidJson = true
            };

            string apiResult = httpWebRequestUtilService.GetResponse(webRequestParam, out HttpStatusCode httpStatusCode);
            detail = new DetailRequestAndResponse(webRequestParam, apiResult);

            return ConvertToApiReturnDataModel(httpStatusCode, apiResult);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            //無此API
            throw new NotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            var request = new PGRegisterRequestModel
            {
                player_name = createRemoteAccountParam.TPGameAccount,
                currency = AppSettings.Currncy
            };

            return DoPostRequest(AppSettings.CreateAccountUrl, request.ToKeyValueURL(), out DetailRequestAndResponse detail);
        }
    }
}