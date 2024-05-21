using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.ThirdPartyTransfer.Old;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer.Old
{
    public abstract class BaseBetDetailService<ApiParamType, ReturnType> : IBetDetailService<ApiParamType, ReturnType>
        where ApiParamType : IOldBetLogApiParam
        where ReturnType : IOldBetLogModel, new()
    {
        private readonly Lazy<IBetLogFileService> _betLogFileService;

        private readonly Lazy<IConfigUtilService> _configUtilService;

        private readonly Lazy<ITPGameApiService> _tpGameApiService;

        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        protected abstract PlatformProduct Product { get; }

        protected abstract BetLogResponseInfo GetRemoteBetDetailApiResult(ApiParamType apiParam);

        protected abstract bool IsWriteRemoteContentToOtherMerchant { get; }

        protected JxApplication Application => s_environmentService.Value.Application;

        protected abstract ReturnType CreateSuccessEmptyResult();

        private readonly EnvironmentUser _envUser;

        protected IBetLogFileService BetLogFileService => _betLogFileService.Value;

        protected IConfigUtilService ConfigUtilService => _configUtilService.Value;

        public BaseBetDetailService()
        {
            _envUser = new EnvironmentUser()
            {
                Application = Application,
                LoginUser = new BasicUserInfo()
            };

            _betLogFileService = DependencyUtil.ResolveJxBackendService<IBetLogFileService>(_envUser, DbConnectionTypes.Slave);
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();

            _tpGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(
                Product,
                SharedAppSettings.PlatformMerchant,
                _envUser,
                DbConnectionTypes.Slave);
        }

        protected virtual long GetBackupBetLogFileSeq() => DateTime.Now.ToUnixOfTime();

        public ReturnType GetRemoteBetDetail(ApiParamType apiParam)
        {
            BetLogResponseInfo betLogResponseInfo = GetRemoteBetDetailApiResult(apiParam);
            ReturnType result = Deserialize(betLogResponseInfo.ApiResult);

            if (result != null)
            {
                result.RemoteFileSeq = betLogResponseInfo.RemoteFileSeq;

                if (IsWriteRemoteContentToOtherMerchant)
                {
                    //這邊改為Action是為了讓後面的流程判斷到如果有資料的時候再備份,節省流量
                    result.WriteRemoteContentToOtherMerchant = () =>
                    {
                        //改為由主序來做, 如果寫檔失敗就停止這次的執行, 避免其他商戶的資料缺失
                        string content = betLogResponseInfo.ApiResult;
                        long fileSeq = GetBackupBetLogFileSeq();
                        var service = DependencyUtil.ResolveService<ITransferServiceAppSettingService>().Value;
                        _betLogFileService.Value.WriteRemoteContentToOtherMerchant(Product, fileSeq, content, service.CopyBetLogToMerchantCodes);
                    };
                }
            }

            return result;
        }

        protected ReturnType Deserialize(string apiResult)
        {
            return apiResult.Deserialize<ReturnType>();
        }

        protected BetLogResponseInfo GetRemoteFileBetLogResult(IOldBetLogApiParam param)
        {
            BaseReturnDataModel<RequestAndResponse> requestReturnModel = _tpGameApiService.Value.GetRemoteBetLog(param.LastSearchToken);

            if (!requestReturnModel.IsSuccess)
            {
                // 適用於FTP情境，沒資料時候TOKEN不推進
                if (requestReturnModel.Code == ReturnCode.NoDataChanged.Value)
                {
                    return new BetLogResponseInfo();
                }

                throw new InvalidProgramException(requestReturnModel.Message);
            }

            RequestAndResponse requestAndResponse = requestReturnModel.DataModel;

            if (requestAndResponse == null || requestAndResponse.RequestBody.IsNullOrEmpty())
            {
                RemoteFileSetting.HasNewRemoteFile = false;
                ReturnType returnModel = CreateSuccessEmptyResult();

                return new BetLogResponseInfo() { ApiResult = returnModel.ToJsonString() }; //避免後續發生null ref error
            }

            RemoteFileSetting.HasNewRemoteFile = true;

            return new BetLogResponseInfo()
            {
                ApiResult = requestAndResponse.ResponseContent,
                RemoteFileSeq = requestAndResponse.RequestBody
            };
        }
    }
}