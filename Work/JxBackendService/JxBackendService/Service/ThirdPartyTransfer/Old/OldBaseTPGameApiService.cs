using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Model.ViewModel.ThirdParty.Old;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Text;
using System.Threading;

namespace JxBackendService.Service.ThirdPartyTransfer.Old
{
    public abstract class OldBaseTPGameApiService<ApiParamType> : BaseService
    {
        private readonly int _delaySendSecond = 1;
        private readonly int _maxTryCount = 5;
        private readonly int _retryIntervalSeconds = 5;
        private readonly IMessageQueueService _messageQueueService;
        private readonly ITPGameStoredProcedureRep _tpGameStoredProcedureRep;
        protected ITPGameApiReadService TPGameApiReadService { get; private set; }

        public OldBaseTPGameApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(envLoginUser.Application);
            _tpGameStoredProcedureRep = ResolveJxBackendService<ITPGameStoredProcedureRep>(Product, DbConnectionTypes.Master);
            TPGameApiReadService = DependencyUtil.ResolveJxBackendService<ITPGameApiReadService>(Product, Merchant, EnvLoginUser, DbConnectionTypes.Master); //舊版只有master
        }

        protected JxApplication Application => EnvLoginUser.Application;

        protected abstract PlatformProduct Product { get; }

        public BaseReturnModel GetAllowCreateTransferOrderResult => TPGameApiReadService.GetAllowCreateTransferOrderResult();

        protected abstract BaseReturnModel GetRemoteOrderStatus(ApiParamType apiParam);

        protected abstract BaseReturnDataModel<UserScore> GetRemoteUserScore(ApiParamType apiParam);

        protected abstract BaseReturnModel SaveTransferOrderSuccess(OldTPGameOrderParam<ApiParamType> oldTPGameOrderParam, UserScore userScore);

        protected abstract BaseReturnModel SaveTransferOrderFail(OldTPGameOrderParam<ApiParamType> oldTPGameOrderParam, string errorMsg);

        protected abstract BaseReturnDataModel<TPGameTransferReturnResult> GetTransferReturnModel(OldTPGameOrderParam<ApiParamType> oldTPGameOrderParam);

        public void TransferHandle(object obj)
        {
            try
            {
                var oldTPGameOrderParam = obj as OldTPGameOrderParam<ApiParamType>;
                bool isMoneyIn = oldTPGameOrderParam.TPGameMoneyInfo is TPGameMoneyInInfo;
                string transferActionName = TPGameMoneyTransferActionType.GetName(isMoneyIn);
                BaseReturnDataModel<TPGameTransferReturnResult> returnModel = GetTransferReturnModel(oldTPGameOrderParam);

                string requestJson = new
                {
                    MoneyID = oldTPGameOrderParam.TPGameMoneyInfo.GetMoneyID(),
                    oldTPGameOrderParam.TPGameMoneyInfo.OrderID
                }.ToJsonString(true);

                if (returnModel.IsSuccess)
                {
                    //等候, 避免第三方資料還沒同步完成
                    Thread.Sleep(5000);

                    LogUtil.ForcedDebug($"遠端{transferActionName}成功等待確認. " +
                        $"Request:{requestJson} " +
                        $"Response:{returnModel.DataModel.ToJsonString(true)}");

                    //如果第三方有回傳積分,要另做處理
                    RecheckAfterTransfer(oldTPGameOrderParam, returnModel.DataModel.RemoteUserScore);
                }
                else
                {
                    //記錄失敗結果
                    var failLog = new StringBuilder($"遠端{transferActionName}失敗. ");
                    failLog.Append($"Request:{requestJson} ");
                    failLog.Append($"Response:{returnModel.DataModel.ToJsonString(true)}");
                    failLog.Append($"ErrorMsg:{returnModel.Message}");
                    LogUtil.ForcedDebug(failLog.ToString());
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
            }
        }


        /// <summary>
        /// 確認上下分狀態
        /// </summary>
        public void CheckProcessingOrderAndSaveToRemoteDB(object state)
        {
            if (state == null)
            {
                return;
            }

            var oldTPGameOrderParam = state as OldTPGameOrderParam<ApiParamType>;

            if (oldTPGameOrderParam == null)
            {
                LogUtil.Error(new NotSupportedException("param is not OldTPGameOrderParam<ApiParamType> "));
                return;
            }

            int tryCount = 0;

            while (tryCount <= _maxTryCount)
            {
                tryCount++;

                if (RecheckAfterTransfer(oldTPGameOrderParam, null))
                {
                    break;
                }

                Thread.Sleep(_retryIntervalSeconds * 1000);
            }
        }

        /// <summary>
        /// 設定本地端餘額到變數
        /// </summary>        
        public void SetLocalUserScores(int userId, ref decimal availableScores, ref decimal freezeScores)
        {
            var tpGameUserInfoService = ResolveJxBackendService<ITPGameUserInfoService>(
               Product,               
               DbConnectionTypes.Master);//舊config沒有db02連線

            BaseTPGameUserInfo tpGameUserInfo = tpGameUserInfoService.GetTPGameUserInfo(userId);

            if (tpGameUserInfo != null)
            {
                availableScores = tpGameUserInfo.AvailableScores.GetValueOrDefault();
                freezeScores = tpGameUserInfo.FreezeScores.GetValueOrDefault();
            }
        }

        /// <summary>
        /// 確認上下分狀態
        /// </summary>
        private bool RecheckAfterTransfer(OldTPGameOrderParam<ApiParamType> oldTPGameOrderParam, UserScore remoteUserScore)
        {
            bool isMoneyIn = oldTPGameOrderParam.TPGameMoneyInfo is TPGameMoneyInInfo;
            string transferActionName = TPGameMoneyTransferActionType.GetName(isMoneyIn);

            //確認訂單
            BaseReturnModel remoteOrderResult = GetRemoteOrderStatus(oldTPGameOrderParam.ApiParam.CloneByJson());

            if (remoteOrderResult == null)
            {
                //其餘異常狀況,不做任何處理
                LogUtil.Error($"资料回传异常，无法取得数据或解析, info={oldTPGameOrderParam.TPGameMoneyInfo.ToJsonString()}");
                return false;
            }

            //有取得第三方回應的資料要回寫DB
            //若訂單狀態為人工洗資料的狀態9,先更新為1, 否則後續EXEC SP會鎖狀態造成更新失敗
            if (oldTPGameOrderParam.TPGameMoneyInfo.Status == TPGameMoneyOutOrderStatus.Manual &&
                !UpdateOrderStatusFromManualToProcessing(isMoneyIn, oldTPGameOrderParam.TPGameMoneyInfo.GetMoneyID()))
            {
                return false;
            }

            if (remoteOrderResult.IsSuccess)
            {
                LogUtil.ForcedDebug($"远程确认{transferActionName}成功，info={oldTPGameOrderParam.TPGameMoneyInfo.ToJsonString()}");

                //有些轉帳完會有餘額資訊, 若沒有需要重新取得
                if (remoteUserScore == null)
                {
                    BaseReturnDataModel<UserScore> remoteUserScoreResult = GetRemoteUserScore(oldTPGameOrderParam.ApiParam);

                    if (remoteUserScoreResult.IsSuccess && remoteUserScoreResult.DataModel != null)
                    {
                        remoteUserScore = remoteUserScoreResult.DataModel;
                    }
                }

                if (remoteUserScore != null)
                {
                    //本地转账
                    BaseReturnModel saveResult = SaveTransferOrderSuccess(oldTPGameOrderParam, remoteUserScore);

                    if (saveResult.IsSuccess)
                    {
                        string summary = string.Empty;

                        if (isMoneyIn)
                        {
                            summary = string.Format(MessageElement.YouTransferInSuccessfully,
                                oldTPGameOrderParam.TPGameMoneyInfo.Amount, Product.Name);
                        }
                        else
                        {
                            summary = string.Format(MessageElement.YouTransferOutSuccessfully,
                               Product.Name, oldTPGameOrderParam.TPGameMoneyInfo.Amount);
                        }


                        _messageQueueService.SendTransferMessage(
                            oldTPGameOrderParam.TPGameMoneyInfo.UserID,
                            oldTPGameOrderParam.TPGameMoneyInfo.Amount,
                            summary, _delaySendSecond);

                        LogUtil.ForcedDebug($"本地{transferActionName}成功，MoneyID：{oldTPGameOrderParam.TPGameMoneyInfo.GetMoneyID()}");
                        return true;
                    }
                    else
                    {
                        LogUtil.ForcedDebug($"本地{transferActionName}失败，info={new { saveResult.Message, oldTPGameOrderParam.TPGameMoneyInfo }.ToJsonString()}");
                        return false;
                    }
                }
            }
            else
            {
                //確定第三方有回訂單狀態為失敗,所以要退款
                BaseReturnModel saveOrderFailResult = SaveTransferOrderFail(oldTPGameOrderParam, remoteOrderResult.Message);

                if (saveOrderFailResult.IsSuccess)
                {
                    string summary = string.Empty;

                    if (isMoneyIn)
                    {
                        summary = string.Format(MessageElement.YouTransferInFailly,
                            oldTPGameOrderParam.TPGameMoneyInfo.Amount, Product.Name);
                    }
                    else
                    {
                        summary = string.Format(MessageElement.YouTransferOutFailly,
                           Product.Name, oldTPGameOrderParam.TPGameMoneyInfo.Amount);
                    }

                    _messageQueueService.SendTransferMessage(oldTPGameOrderParam.TPGameMoneyInfo.UserID,
                        oldTPGameOrderParam.TPGameMoneyInfo.Amount, summary, _delaySendSecond);
                    return true;
                }
                else
                {
                    LogUtil.Error(new { oldTPGameOrderParam.TPGameMoneyInfo, saveOrderFailResult.Message }.ToJsonString());
                    return false;
                }
            }

            return false;
        }

        private bool UpdateOrderStatusFromManualToProcessing(bool isMoneyIn, string moneyId)
        {
            bool isSuccess;

            if (isMoneyIn)
            {
                isSuccess = _tpGameStoredProcedureRep.UpdateMoneyInOrderStatusFromManualToProcessing(moneyId);
            }
            else
            {
                isSuccess = _tpGameStoredProcedureRep.UpdateMoneyOutOrderStatusFromManualToProcessing(moneyId);
            }

            return isSuccess;
        }
    }

    /// <summary>
    /// 把舊版錢包的服務獨立出來
    /// </summary>
    public abstract class BaseOldTPGameWalletApiService : BaseService, IOldTPGameApiService, IOldTPGameApiReadService
    {
        private readonly ITPGameApiReadService _tpGameApiReadService;

        protected abstract PlatformProduct Product { get; }

        public BaseReturnModel GetAllowCreateTransferOrderResult() => _tpGameApiReadService.GetAllowCreateTransferOrderResult();

        protected abstract BaseReturnDataModel<UserScore> GetRemoteUserScore(BasicUserInfo loginUser);

        public BaseOldTPGameWalletApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _tpGameApiReadService = ResolveJxBackendService<ITPGameApiReadService>(Product);
        }

        public BaseReturnModel UpdateUserScoreFromRemote()
        {
            BaseReturnDataModel<UserScore> userScoreReturnDataModel = GetRemoteUserScore(EnvLoginUser.LoginUser);

            if (userScoreReturnDataModel.IsSuccess)
            {
                //todo 更新餘額
                throw new NotImplementedException();

                //return new BaseReturnModel(ReturnCode.Success);
            }

            return userScoreReturnDataModel.CastByJson<BaseReturnModel>();
        }
    }
}
