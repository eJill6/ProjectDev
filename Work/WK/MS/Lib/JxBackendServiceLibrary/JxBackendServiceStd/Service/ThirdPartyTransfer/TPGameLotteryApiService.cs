using JxBackendService.Common;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.Web;
using JxBackendService.Service.ThirdPartyTransfer.Base;
using System;
using System.Web;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameLotteryApiService : BaseTPGameApiService
    {
        private readonly Lazy<IHttpWebRequestUtilService> _httpWebRequestUtilService;

        public override PlatformProduct Product => PlatformProduct.Lottery;

        public TPGameLotteryApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();
        }

        public override BaseReturnModel GetQueryOrderReturnModel(string apiResult)
        {
            throw new PlatformNotSupportedException();
        }

        protected override DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            throw new PlatformNotSupportedException();
        }

        public override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            throw new PlatformNotSupportedException();
        }

        public override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam, IInvocationUserParam invocationUserParam)
        {
            throw new PlatformNotSupportedException();
        }

        public override BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult)
        {
            throw new PlatformNotSupportedException();
        }

        public override BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult)
        {
            throw new PlatformNotSupportedException();
        }

        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam)
        {
            throw new PlatformNotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            return GetRemoteLoginApiResult(tpGameRemoteLoginParam);
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            throw new PlatformNotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            throw new PlatformNotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            throw new PlatformNotSupportedException();
        }

        protected override string CreateTPGamePasswordByRule(int userId, string tpGameAccount)
        {
            throw new PlatformNotSupportedException();
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            CacheObj cacheObj = null;

            try
            {
                CacheKey cacheKey = CacheKey.GetFrontSideUserInfoKey(EnvLoginUser.LoginUser.UserKey);
                cacheObj = JxCacheService.GetCache<CacheObj>(cacheKey, getCacheData: null);
                var cachedUserInfoToken = cacheObj.Value.Deserialize<CachedUserInfoToken>();
                string subGameCode = GetThirdPartyRemoteCode(tpGameRemoteLoginParam.LoginInfo);

                var queryString = $"?userId={cachedUserInfoToken.UserId}" +
                        $"&userName={HttpUtility.UrlEncode(cachedUserInfoToken.UserName)}" +
                        $"&roomNo=0" + //固定為0
                        $"&gameID={subGameCode}" +
                        $"&depositUrl={HttpUtility.UrlEncode(cachedUserInfoToken.DepositUrl)}" +
                        $"&pageParamInfo=fullscreen";

                var xorQueryString = HttpUtility.UrlEncode(XorEncryptTool.XorEncryptToString(queryString));

                string path = _httpWebRequestUtilService.Value.CombineUrl(
                    SharedAppSettings.FrontSideWebUrl, $"?ep={xorQueryString}");

                return new BaseReturnDataModel<string>(ReturnCode.Success, path);
            }
            catch (Exception ex)
            {
                string errorMsg = $"tpGameRemoteLoginParam={tpGameRemoteLoginParam.ToJsonString().ToJsonString()}," +
                    $"JxCacheService is null={JxCacheService == null}," +
                    $"cacheObj={cacheObj.ToJsonString()}";

                ErrorMsgUtil.ErrorHandle(errorMsg, EnvLoginUser, isSendMessageToTelegram: true);

                throw ex;
            }
        }

        protected override void DoKickUser(Model.Entity.ThirdPartyUserAccount thirdPartyUserAccount) => new NotImplementedException();
    }
}