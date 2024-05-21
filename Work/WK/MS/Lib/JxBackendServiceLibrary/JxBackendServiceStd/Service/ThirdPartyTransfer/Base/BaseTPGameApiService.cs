using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Interface.Service.GlobalSystem;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.MessageQueue;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseTPGameApiService : BaseTPGameRemoteApiService, ITPGameApiService, ITPGameApiReadService
    {
        private static readonly int s_defaultMaxTryCount = 5;

        private static readonly int s_defaultRetryIntervalSeconds = 5;

        private static readonly string s_playInfoIsExistsText = "单号已存在";

        private static readonly int s_maxMemoLength = 1020;

        private static readonly int s_saveBetLogToPlatformWaitMilliseconds = 300;

        private static readonly int s_batchSaveProfitlossCount = 300;

        private static readonly int s_delayUpdateTPGameUserScoreSeconds = 30;

        private readonly Lazy<ITPGameStoredProcedureRep> _tpGameStoredProcedureRep;

        private readonly Lazy<ITPGameAccountService> _tpGameAccountService;

        private readonly Lazy<ITPGameAccountReadService> _tpGameAccountReadService;

        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;

        private readonly Lazy<ITPGameUserInfoService> _tpGameUserInfoService;

        private readonly Lazy<ITPGameUserInfoService> _tpGameUserInfoReadService;

        private readonly Lazy<IFrontsideMenuService> _frontsideMenuReadService;

        private readonly Lazy<IPlatformProductSettingService> _platformProductSettingService;

        private readonly Lazy<IPlatformProductService> _platformProductService;

        protected ITPGameStoredProcedureRep TPGameStoredProcedureRep => _tpGameStoredProcedureRep.Value;

        protected ITPGameAccountReadService TPGameAccountReadService => _tpGameAccountReadService.Value;

        private readonly Lazy<IJxCacheService> _jxCacheService;

        private readonly Lazy<IHttpWebRequestUtilService> _httpWebRequestUtilService;

        private readonly Lazy<ITransferCompensationService> _transferCompensationService;

        private readonly Lazy<ISendTelegramMessageService> _sendTelegramMessageService;

        private readonly Lazy<IMerchantSettingService> _merchantSettingService;

        private readonly Lazy<IMessageQueueDelayJobService> _messageQueueDelayJobService;

        private readonly Lazy<IDebugUserService> _debugUserService;

        private readonly Lazy<ITPGameRemoteApiService> _tpGameRemoteApiService;

        protected IJxCacheService JxCacheService => _jxCacheService.Value;

        protected IHttpWebRequestUtilService HttpWebRequestUtilService => _httpWebRequestUtilService.Value;

        protected IDebugUserService DebugUserService => _debugUserService.Value;

        protected string ProductName => _platformProductService.Value.GetName(Product.Value);

        /// <summary> 某些第三方需要返回網址 </summary>
        protected string DefaultReturnUrl => GetCombineUrl(SharedAppSettings.FrontSideWebUrl, "ReconnectTips");

        public BaseTPGameApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _tpGameStoredProcedureRep = ResolveJxBackendService<ITPGameStoredProcedureRep>(Product);
            _tpGameAccountService = ResolveJxBackendService<ITPGameAccountService>(SharedAppSettings.PlatformMerchant);
            _tpGameAccountReadService = ResolveJxBackendService<ITPGameAccountReadService>(SharedAppSettings.PlatformMerchant);
            _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
            _frontsideMenuReadService = ResolveJxBackendService<IFrontsideMenuService>();
            _platformProductSettingService = ResolveKeyed<IPlatformProductSettingService>(Product);
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
            _jxCacheService = DependencyUtil.ResolveService<IJxCacheService>();
            _httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();
            _transferCompensationService = ResolveJxBackendService<ITransferCompensationService>();
            _sendTelegramMessageService = ResolveJxBackendService<ISendTelegramMessageService>();
            _merchantSettingService = ResolveJxBackendService<IMerchantSettingService>(DbConnectionTypes.Slave);
            _messageQueueDelayJobService = ResolveJxBackendService<IMessageQueueDelayJobService>(DbConnectionTypes.Slave);
            _debugUserService = DependencyUtil.ResolveService<IDebugUserService>();

            //為了使用LogToDbInterceptor，所以獨立把原本protected abstract的方法提到 ITPGameRemoteApiService
            _tpGameRemoteApiService = ResolveJxBackendService<ITPGameRemoteApiService>(Product);

            if (!Product.IsSelfProduct)
            {
                _tpGameUserInfoService = ResolveJxBackendService<ITPGameUserInfoService>(Product);
                _tpGameUserInfoReadService = ResolveJxBackendService<ITPGameUserInfoService>(Product, DbConnectionTypes.Slave);
            }
        }

        #region abstract methods,properties

        public abstract PlatformProduct Product { get; }

        /// <summary>
        /// 去遠端的第三方實際打查詢訂單API取得結果
        /// </summary>
        protected abstract DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo);

        /// <summary>
        /// 去遠端的第三方實際打取得投注資料結果
        /// </summary>
        protected abstract BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken);

        /// <summary>
        /// 轉換遠端轉帳結果為平台模型, 若第三方有回傳積分則需要建立UserScore物件,否則維持DataModel = null
        /// </summary>
        public abstract BaseReturnDataModel<UserScore> GetTransferReturnModel(string apiResult);

        /// <summary>
        /// 轉換遠端訂單資料為平台模型
        /// </summary>
        public abstract BaseReturnModel GetQueryOrderReturnModel(string apiResult);

        /// <summary>
        /// 轉換遠端使用者積分為平台模型
        /// </summary>
        public abstract BaseReturnDataModel<UserScore> GetUserScoreReturnModel(string apiResult);

        /// <summary>
        /// 創立第三方帳號
        /// </summary>
        protected abstract BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam createRemoteAccountParam);

        protected abstract BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam);

        /// <summary>取得遊戲網址</summary>
        protected abstract BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam);

        /// <summary>
        /// 取得帳號是否存在的遠端結果(目前在base無引用，因為每家的流程不太一樣，目前用於子類別中會呼叫)
        /// </summary>
        protected abstract BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam);

        /// <summary>
        /// 取得建立帳號的遠端結果(目前在base無引用，因為每家的流程不太一樣，目前用於子類別中會呼叫)
        /// </summary>
        protected abstract BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam createRemoteAccountParam);

        /// <summary>是否把注單資料複製到特定資料夾</summary>
        public abstract bool IsWriteRemoteContentToOtherMerchant { get; }

        protected virtual int? TransferAmountFloorDigit { get; }

        /// <summary>提供各家產生密碼的方法</summary>
        protected abstract string CreateTPGamePasswordByRule(int userId, string tpGameAccount);

        protected abstract void DoKickUser(ThirdPartyUserAccount thirdPartyUserAccount);

        #endregion abstract methods,properties

        protected virtual bool IsAllowTransferCompensation => false;

        protected virtual bool IsDoTransferCompensation(string apiResult) => false;

        public BaseReturnDataModel<RequestAndResponse> GetRemoteBetLog(string lastSearchToken)
        {
            BaseReturnDataModel<RequestAndResponse> returnDataModel = GetRemoteBetLogApiResult(lastSearchToken);

            return returnDataModel;
        }

        public void WriteRemoteContentToOtherMerchant(BaseReturnDataModel<RequestAndResponse> requestAndResponseResult)
        {
            // 利用時間戳記當作檔名
            long fileSeq = DateTime.Now.ToUnixOfTime();

            // 讓下載FTP平台可從RequestBody得知交換TOKEN
            var returnFtpDataModel = new BaseReturnDataModel<RequestAndResponse>
            {
                IsSuccess = requestAndResponseResult.IsSuccess,
                DataModel = new RequestAndResponse
                {
                    RequestBody = fileSeq.ToString(),
                    ResponseContent = requestAndResponseResult.DataModel.ResponseContent
                }
            };

            string fileContent = returnFtpDataModel.ToJsonString();
            var service = DependencyUtil.ResolveService<ITransferServiceAppSettingService>().Value;

            //不是每個站點都有oss設定, 所以改為區域變數, 沒有設定的時候會報錯
            var betLogFileService = ResolveJxBackendService<IBetLogFileService>(DbConnectionTypes.Slave).Value;
            betLogFileService.WriteRemoteContentToOtherMerchant(Product, fileSeq, fileContent, service.CopyBetLogToMerchantCodes);
        }

        public virtual BaseReturnModel GetAllowCreateTransferOrderResult()
        {
            if (_frontsideMenuReadService.Value.GetActiveFrontsideMenus().Any(W => W.ProductCode == Product.Value))
            {
                return new BaseReturnModel(ReturnCode.Success);
            }
            else
            {
                return new BaseReturnModel(ThirdPartyGameElement.GameMaintain);
            }
        }

        public List<TPGameMoneyInInfo> GetTPGameUnprocessedMoneyInInfo()
        {
            return TPGameStoredProcedureRep.GetTPGameUnprocessedMoneyInInfo();
        }

        /// <summary>
        /// 取得第三方轉入單正在處理中資訊
        /// </summary>
        /// <returns></returns>
        public List<TPGameMoneyInInfo> GetTPGameProcessingMoneyInInfo()
        {
            return TPGameStoredProcedureRep.GetTPGameProcessingMoneyInInfo();
        }

        /// <summary>
        /// 取得第三方轉出單未處理資訊
        /// </summary>
        /// <returns></returns>
        public List<TPGameMoneyOutInfo> GetTPGameUnprocessedMoneyOutInfo()
        {
            return TPGameStoredProcedureRep.GetTPGameUnprocessedMoneyOutInfo();
        }

        /// <summary>
        /// 取得第三方轉出單正在處理中資訊
        /// </summary>
        /// <returns></returns>
        public List<TPGameMoneyOutInfo> GetTPGameProcessingMoneyOutInfo()
        {
            return TPGameStoredProcedureRep.GetTPGameProcessingMoneyOutInfo();
        }

        /// <summary>
        /// 建立轉帳單前的檢查
        /// </summary>
        private BaseReturnModel CreateTransferBeforeCheck(int userId, decimal amount)
        {
            if (amount < GlobalVariables.TPTransferAmountBound.MinTPGameTransferAmount)
            {
                return new BaseReturnModel(string.Format(ThirdPartyGameElement.TransferMoneyMustThanAmount,
                    GlobalVariables.TPTransferAmountBound.MinTPGameTransferAmount));
            }
            else if (amount > GlobalVariables.TPTransferAmountBound.MaxTPGameTransferAmount)
            {
                return new BaseReturnModel(ThirdPartyGameElement.TransferMoneyMustUnderLimitation);
            }

            // 檢查 FrontsideMenu Active 啟用狀態 判斷能否建立轉帳單
            BaseReturnModel allowCreateOrderResult = GetAllowCreateTransferOrderResult();

            if (allowCreateOrderResult.IsSuccess == false)
            {
                return allowCreateOrderResult;
            }

            BaseReturnModel checkOrCreateResult = CheckOrCreateAccount(userId);

            return checkOrCreateResult;
        }

        /// <summary>
        /// 建立轉入單
        /// </summary>
        public BaseReturnModel CreateTransferInInfo(TPGameTranfserParam param)
        {
            BaseReturnModel checkModel = CreateTransferBeforeCheck(param.UserID, param.Amount);

            if (!checkModel.IsSuccess)
            {
                return checkModel;
            }

            TPGameMoneyInOrderStatus transferInStatus = TPGameMoneyInOrderStatus.Processing;

            TPGameTransferMoneyResult result = TPGameStoredProcedureRep.CreateMoneyInOrder(
                param.UserID,
                param.Amount,
                GetTPGameAccountAndUpdatePassword(param.UserID).TPGameAccount,
                transferInStatus);

            if (!result.ErrorMsg.IsNullOrEmpty())
            {
                LogUtilService.Error(result.ErrorMsg);

                return new BaseReturnModel(result.ErrorMsg);
            }

            result.TPGameMoneyInfo.CorrelationId = param.CorrelationId;

            return DoTransfer(result.TPGameMoneyInfo);
        }

        /// <summary> 建立全轉回的轉出單 </summary>
        public BaseReturnDataModel<decimal> CreateAllAmountTransferOutInfo(TPGameTranfserOutParam param, out bool isAllAmount)
        {
            BaseReturnModel transferResult = BaseCreateTransferOutInfo(param, isTransferOutAll: true, out decimal actuallyAmount, out isAllAmount, out string moneyId);
            BaseReturnDataModel<decimal> returnDataModel = transferResult.CastByJson<BaseReturnDataModel<decimal>>();
            // 回傳真正轉出金額數目
            returnDataModel.DataModel = actuallyAmount;

            return returnDataModel;
        }

        /// <summary> 建立轉出單 </summary>
        public BaseReturnModel CreateTransferOutInfo(TPGameTranfserOutParam param, bool isTransferOutAll, out string moneyId)
        {
            return BaseCreateTransferOutInfo(param, isTransferOutAll, out decimal actuallyAmount, out bool isAllAmount, out moneyId);
        }

        /// <summary> 建立轉出單 </summary>
        public BaseReturnModel CreateTransferOutInfo(TPGameTranfserOutParam param, bool isTransferOutAll, out decimal actuallyAmount, out string moneyId)
        {
            return BaseCreateTransferOutInfo(param, isTransferOutAll, out actuallyAmount, out bool isAllAmount, out moneyId);
        }

        private BaseReturnModel BaseCreateTransferOutInfo(TPGameTranfserOutParam param, bool isTransferOutAll, out decimal actuallyAmount, out bool isAllAmount, out string moneyId)
        {
            // 設定最終轉帳金額
            actuallyAmount = 0;
            isAllAmount = true;
            moneyId = null;

            BaseReturnDataModel<UserScore> checkScore = GetRemoteUserScore(param, isRetry: false);

            if (!checkScore.IsSuccess)
            {
                return new BaseReturnModel(checkScore.Message);
            }

            //同步餘額回table
            CheckAndUpdateTPGameUserInfoScores(param.UserID, checkScore.DataModel);

            decimal minTPGameTransferAmount = GlobalVariables.TPTransferAmountBound.MinTPGameTransferAmount;
            decimal maxTPGameTransferAmount = GlobalVariables.TPTransferAmountBound.MaxTPGameTransferAmount;

            // 小於最小轉帳值不進行轉回
            if (checkScore.DataModel.AvailableScores < minTPGameTransferAmount)
            {
                string message = string.Format(MessageElement.InsufficientBalance, minTPGameTransferAmount);

                if (checkScore.DataModel.AvailableScores == 0)
                {
                    message = MessageElement.BalanceIsZero;
                }

                return new BaseReturnModel(new SuccessMessage(message));
            }

            // 查回第三方錢包最後金額，為實際轉回主錢包的金額
            actuallyAmount = checkScore.DataModel.AvailableScores;

            if (TransferAmountFloorDigit.HasValue)
            {
                actuallyAmount = actuallyAmount.Floor(TransferAmountFloorDigit.Value);
            }

            // 全部轉回的情況下不判斷金額及開關
            if (isTransferOutAll)
            {
                //怕一次轉不完,需要判斷第三方餘額
                if (actuallyAmount > maxTPGameTransferAmount)
                {
                    actuallyAmount = maxTPGameTransferAmount;
                    isAllAmount = false;
                }
            }

            TPGameMoneyOutOrderStatus transferOutStatus = TPGameMoneyOutOrderStatus.Processing;
            string tpGameAccount = GetTPGameAccountAndUpdatePassword(param.UserID).TPGameAccount;

            TPGameTransferMoneyResult result = TPGameStoredProcedureRep
                .CreateMoneyOutOrder(new CreateTransferOutOrderParam()
                {
                    UserId = param.UserID,
                    Amount = actuallyAmount,
                    TPGameAccount = tpGameAccount,
                    TransferOutStatus = transferOutStatus,
                });

            if (!result.ErrorMsg.IsNullOrEmpty())
            {
                LogUtilService.Error(string.Format($"CreateMoneyOutOrder failed {ProductName} {result.ErrorMsg}"));

                return new BaseReturnModel(result.ErrorMsg);
            }

            moneyId = result.TPGameMoneyInfo.GetMoneyID();
            result.TPGameMoneyInfo.CorrelationId = param.CorrelationId;

            BaseReturnModel transferResult = DoTransfer(result.TPGameMoneyInfo);

            if (transferResult.IsSuccess && isTransferOutAll && !isAllAmount)
            {
                var transferOutUserDetail = new TransferOutUserDetail()
                {
                    AffectedUser = new BaseBasicUserInfo() { UserId = param.UserID },
                };

                //放入queue做非同步處理
                _internalMessageQueueService.Value.EnqueueTransferAllOutMessage(Product, transferOutUserDetail);
            }

            return transferResult;
        }

        private void CheckAndUpdateTPGameUserInfoScores(int userID, UserScore userScore)
        {
            ITPGameUserInfoService tpGameUserInfoReadService = DependencyUtil
                .ResolveJxBackendService<ITPGameUserInfoService>(Product, SharedAppSettings.PlatformMerchant, EnvLoginUser, DbConnectionTypes.Slave).Value;

            BaseTPGameUserInfo tpGameUserInfo = tpGameUserInfoReadService.GetTPGameUserInfo(userID);

            if (tpGameUserInfo == null ||
                userScore == null ||
                (tpGameUserInfo.AvailableScores == userScore.AvailableScores && tpGameUserInfo.FreezeScores == userScore.FreezeScores))
            {
                return;
            }

            ITPGameUserInfoService tpGameUserInfoService = DependencyUtil
                .ResolveJxBackendService<ITPGameUserInfoService>(Product, SharedAppSettings.PlatformMerchant, EnvLoginUser, DbConnectionTypes.Master).Value;

            tpGameUserInfoService.UpdateUserScores(userID, userScore);
        }

        public void RecheckProcessingOrders(object state)
        {
            ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
            {
                if (state is BaseTPGameMoneyInfo == false)
                {
                    throw new ArgumentException("傳入型態不符");
                }

                RecheckAfterTransfer(state as BaseTPGameMoneyInfo, null, isStopWhenOrderFail: false);
            });
        }

        public BaseReturnDataModel<UserScore> GetRemoteUserScore(IInvocationUserParam invocationUserParam, bool isRetry)
        {
            int maxTryCount = s_defaultMaxTryCount;

            if (!isRetry)
            {
                maxTryCount = 1;
            }

            return GetJobResultWithRetry(() =>
            {
                CreateRemoteAccountParam createRemoteAccountParam = GetTPGameAccountAndUpdatePassword(invocationUserParam.UserID);

                return GetUserScoreReturnModel(_tpGameRemoteApiService.Value.GetRemoteUserScoreApiResult(createRemoteAccountParam, invocationUserParam));
            },
            maxTryCount);
        }

        public BaseReturnModel CheckOrCreateAccount(int userId)
        {
            CreateRemoteAccountParam createRemoteAccountParam = GetTPGameAccountAndUpdatePassword(userId, out bool isDbExists);

            if (isDbExists)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            BaseReturnModel createAccountModel = CheckOrCreateRemoteAccount(createRemoteAccountParam);

            if (createAccountModel.IsSuccess)
            {
                return CheckOrCreateLocalAccount(userId);
            }

            return createAccountModel;
        }

        /// <summary>
        /// 創建本地帳號
        /// </summary>
        private BaseReturnDataModel<string> CheckOrCreateLocalAccount(int userId)
        {
            //創建user
            if (!_tpGameUserInfoReadService.Value.IsUserExists(userId))
            {
                BaseReturnDataModel<string> createResult = _jxCacheService.Value.DoWorkWithRemoteLock(
                    CacheKey.TPGameUserInfoLock(userId),
                    () =>
                    {
                        if (_tpGameUserInfoService.Value.IsUserExists(userId))
                        {
                            return new BaseReturnDataModel<string>(ReturnCode.Success);
                        }

                        if (!_tpGameUserInfoService.Value.CreateUser(userId))
                        {
                            return new BaseReturnDataModel<string>(ThirdPartyGameElement.CreateAccountFail);
                        }

                        return new BaseReturnDataModel<string>(ReturnCode.Success);
                    });

                if (!createResult.IsSuccess)
                {
                    return createResult;
                }
            }

            //創建thirdPartyUserAccount
            ThirdPartyUserAccount thirdPartyUserAccount = TPGameAccountReadService.GetThirdPartyUserAccount(userId, Product);
            string tpGameAccount;

            if (thirdPartyUserAccount == null)
            {
                tpGameAccount = TPGameAccountReadService.GetTPGameAccountByRule(Product, userId);
                string tpGamePassword = CreateTPGamePasswordByRule(userId, tpGameAccount);
                BaseReturnDataModel<long> createResult = _tpGameAccountService.Value.Create(userId, Product, tpGameAccount, tpGamePassword);

                if (!createResult.IsSuccess)
                {
                    return new BaseReturnDataModel<string>(createResult.Message);
                }
            }
            else
            {
                tpGameAccount = thirdPartyUserAccount.Account;
            }

            return new BaseReturnDataModel<string>(ReturnCode.Success, tpGameAccount);
        }

        public BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(ForwardGameUrlParam param)
        {
            CreateRemoteAccountParam createRemoteAccountParam = null;

            if (!Product.IsSelfProduct)
            {
                BaseReturnModel preCheckResult = GetLoginPreCheckResult(param.LoginUser.UserId);

                if (!preCheckResult.IsSuccess)
                {
                    return new BaseReturnDataModel<TPGameOpenParam>(preCheckResult.Message);
                }

                createRemoteAccountParam = GetTPGameAccountAndUpdatePassword(param.LoginUser.UserId);
            }

            var tpGameRemoteLoginParam = new TPGameRemoteLoginParam()
            {
                CreateRemoteAccountParam = createRemoteAccountParam,
                IpAddress = param.IpAddress,
                IsMobile = param.IsMobile,
                LoginInfo = param.LoginInfo,
                UserID = param.LoginUser.UserId,
                CorrelationId = param.CorrelationId
            };

            BaseReturnDataModel<string> gameUrlResult = GetRemoteForwardGameUrl(tpGameRemoteLoginParam);

            if (!gameUrlResult.IsSuccess)
            {
                return new BaseReturnDataModel<TPGameOpenParam>(gameUrlResult.Message);
            }

            return new BaseReturnDataModel<TPGameOpenParam>(ReturnCode.Success, new TPGameOpenParam()
            {
                Url = gameUrlResult.DataModel,
                OpenGameModeValue = _platformProductSettingService.Value.OpenMode.Value
            });
        }

        public BaseReturnDataModel<string> GetLoginApiResult(ForwardGameUrlParam param)
        {
            BaseReturnModel preCheckResult = GetLoginPreCheckResult(param.LoginUser.UserId);

            if (!preCheckResult.IsSuccess)
            {
                return new BaseReturnDataModel<string>(preCheckResult.Message);
            }

            TPGameRemoteLoginParam tpGameRemoteLoginParam = new TPGameRemoteLoginParam()
            {
                CreateRemoteAccountParam = GetTPGameAccountAndUpdatePassword(param.LoginUser.UserId),
                IpAddress = param.IpAddress,
                IsMobile = param.IsMobile,
                LoginInfo = param.LoginInfo,
                UserID = param.LoginUser.UserId,
                CorrelationId = param.CorrelationId
            };

            BaseReturnDataModel<string> loginApiResult = GetRemoteLoginApiResult(tpGameRemoteLoginParam);

            return loginApiResult;
        }

        public void KickUser(int userId)
        {
            ThirdPartyUserAccount thirdPartyUserAccount = TPGameAccountReadService.GetThirdPartyUserAccount(userId, Product);

            if (thirdPartyUserAccount == null)
            {
                return;
            }

            DoKickUser(thirdPartyUserAccount);
        }

        public void StartDequeueUpdateTPGameUserScoreJob()
        {
            IInternalMessageQueueService internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>().Value;
            ITPGameTransferOutService tpGameTransferOutService = ResolveJxBackendService<ITPGameTransferOutService>(DbConnectionTypes.Master).Value;

            internalMessageQueueService.StartNewDequeueJob(
                TaskQueueName.UpdateTPGameUserScore(Product),
                (DoDequeueJobAfterReceivedParam doDequeueJobAfterReceivedParam) =>
                {
                    int userId;

                    try
                    {
                        userId = doDequeueJobAfterReceivedParam.Message.Deserialize<int>();
                    }
                    catch (Exception ex)
                    {
                        LogUtilService.ForcedDebug($"UpdateTPGameUserScore DoJobAfterReceived:{doDequeueJobAfterReceivedParam.Message}");
                        ErrorMsgUtil.ErrorHandle(ex, EnvLoginUser);

                        return true;
                    }

                    BaseReturnDataModel<UserScore> baseReturnDataModel = GetRemoteUserScore(
                        new InvocationUserParam() { UserID = userId },
                        isRetry: false);

                    if (baseReturnDataModel.IsSuccess)
                    {
                        BaseTPGameUserInfo tpGameUserInfo = _tpGameUserInfoReadService.Value.GetTPGameUserInfo(userId);

                        var prevScore = new UserScore()
                        {
                            AvailableScores = tpGameUserInfo.AvailableScores,
                            FreezeScores = tpGameUserInfo.FreezeScores
                        };

                        UserScore newScore = baseReturnDataModel.DataModel;

                        if (prevScore.AvailableScores != newScore.AvailableScores ||
                            prevScore.FreezeScores != newScore.FreezeScores)
                        {
                            _tpGameUserInfoService.Value.UpdateUserScores(userId, baseReturnDataModel.DataModel);
                        }
                    }

                    //不管成功或失敗要讓queue繼續處理下一筆
                    return true;
                });
        }

        public Dictionary<string, int> GetUserIdsFromTPGameAccounts(List<string> tpGameAccounts)
        {
            return TPGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts.ConvertToHashSet());
        }

        public void SaveMultipleProfitlossToPlatform(SaveProfitlossToPlatformParam saveProfitlossToPlatformParam)
        {
            List<int> userIds = saveProfitlossToPlatformParam.TPGameProfitlosses.Where(w => !w.IsIgnore).Select(s => s.UserID).Distinct().ToList();
            AddUpdateTPUserScoreDelayJob(userIds);

            var allTPGameProfitlosses = new List<InsertTPGameProfitlossParam>();
            var saveLocalBetLogParams = new List<SaveLocalBetLogParam>();

            foreach (InsertTPGameProfitlossParam tpGameProfitloss in saveProfitlossToPlatformParam.TPGameProfitlosses)
            {
                tpGameProfitloss.SetBetMoneys(
                    _merchantSettingService.Value.IsComputeAdmissionBetMoney,
                    tpGameProfitloss.ProfitLossMoney,
                    tpGameProfitloss.AllBetMoney);

                if (tpGameProfitloss.IsIgnore)
                {
                    saveLocalBetLogParams.Add(
                        new SaveLocalBetLogParam()
                        {
                            KeyId = tpGameProfitloss.KeyId,
                            SaveBetLogFlag = SaveBetLogFlags.Ignore
                        });

                    continue;
                }

                if (StringUtil.IsValidJson(tpGameProfitloss.Memo))
                {
                    string memoJson = tpGameProfitloss.Memo;
                    tpGameProfitloss.Memo = memoJson.ToLocalizationContent(tpGameProfitloss.Memo, EnvLoginUser.Application);
                }

                tpGameProfitloss.Memo = tpGameProfitloss.Memo.ToShortString(s_maxMemoLength);
                allTPGameProfitlosses.Add(tpGameProfitloss);
            }

            saveLocalBetLogParams.Clear();

            while (allTPGameProfitlosses.Any())
            {
                List<InsertTPGameProfitlossParam> batchTPGameProfitlosses = allTPGameProfitlosses.Take(s_batchSaveProfitlossCount).ToList();
                //寫入盈虧跟注單
                List<BaseReturnDataModel<string>> saveResults = TPGameStoredProcedureRep.AddMultipleProductProfitLossAndPlayInfo(batchTPGameProfitlosses);
                allTPGameProfitlosses.RemoveRangeByFit(0, s_batchSaveProfitlossCount);
                var successKeyIds = new List<string>();

                foreach (BaseReturnDataModel<string> saveResult in saveResults)
                {
                    string keyId = saveResult.DataModel;

                    if (!saveResult.IsSuccess)
                    {
                        SaveBetLogFlags saveBetLogFlag;

                        if (saveResult.Message == s_playInfoIsExistsText)
                        {
                            saveBetLogFlag = SaveBetLogFlags.Success;
                        }
                        else
                        {
                            LogUtilService.ForcedDebug($"保存{Product.Value}虧盈與注單數據 {keyId} 到資料庫失敗: {saveResult.Message}");
                            saveBetLogFlag = SaveBetLogFlags.Fail;
                        }

                        saveLocalBetLogParams.Add(
                            new SaveLocalBetLogParam()
                            {
                                KeyId = keyId,
                                SaveBetLogFlag = saveBetLogFlag
                            });

                        continue;
                    }

                    successKeyIds.Add(keyId);

                    saveLocalBetLogParams.Add(
                        new SaveLocalBetLogParam()
                        {
                            KeyId = keyId,
                            SaveBetLogFlag = SaveBetLogFlags.Success
                        });
                }

                saveLocalBetLogParams.Clear();

                LogUtilService.ForcedDebug($"保存{Product.Value}虧盈與注單數據 {successKeyIds.Count}筆 {successKeyIds.ToJsonString()} 到資料庫成功");
                TaskUtil.DelayAndWait(s_saveBetLogToPlatformWaitMilliseconds);
            }
        }

        /// <summary>計算有效投注額</summary>
        public decimal ComputeAdmissionBetMoney(ComputeAdmissionBetMoneyParam param)
        {
            //目前只有需要盤口的產品需要做特殊計算, 未來如果有沒盤口資料的需要做額外計算,則需要把IsComputeAdmissionBetAmountByHandicap拆分
            if (!_merchantSettingService.Value.IsComputeAdmissionBetMoney || !Product.ProductType.IsComputeAdmissionBetMoneyByHandicap)
            {
                return param.GetDefaultAdmissionBetMoney();
            }

            //若沒有盤口資料則不計算(對應不到盤口賠率，有效投注一律等於總投注額)
            if (!param.HandicapInfos.AnyAndNotNull())
            {
                return param.AllBetMoney;
            }

            if (param.WagerType == null || param.BetResultType == null)
            {
                throw new ArgumentNullException(); //各家有盤口的第三方須提供
            }

            if (param.BetResultType == BetResultType.Lose)
            {
                //结算结果为「输」的注单，有效投注额 = 总投注额
                return param.AllBetMoney;
            }
            else if (param.BetResultType == BetResultType.HalfWin || param.BetResultType == BetResultType.HalfLose)
            {
                //结算结果为「半输/半赢」的注单，有效投注额 = 总投注额 * 0.5
                return param.AllBetMoney * 0.5m;
            }
            else if (param.BetResultType == BetResultType.Draw)
            {
                //结算结果为「和局」的注单，有效投注额 = 0
                return 0m;
            }
            else if (param.BetResultType == BetResultType.Cashout)
            {
                return GetAdmissionBetMoneyByCashout(param.AllBetMoney, param.ProfitLossMoney);
            }
            else if (param.BetResultType == BetResultType.Win)
            {
                if (param.WagerType == WagerType.Single)
                {
                    return GetAdmissionBetMoneyBySingle(param.HandicapInfos.First(), param.AllBetMoney, param.ProfitLossMoney);
                }
                else if (param.WagerType == WagerType.Combo)
                {
                    return GetAdmissionBetMoneyByCombo(param.HandicapInfos, param.AllBetMoney, param.ProfitLossMoney);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            throw new NotSupportedException();
        }

        /// <summary>發送訊息給客服群</summary>
        protected void SendTelegramMessageToCustomerService(BasicUserInfo user, string message)
        {
            _sendTelegramMessageService.Value.SendToCustomerService(user, message);
        }

        protected string GetThirdPartyRemoteCode(LoginInfo loginInfo)
        {
            if (loginInfo == null)
            {
                return null;
            }

            if (!loginInfo.RemoteCode.IsNullOrEmpty())
            {
                return loginInfo.RemoteCode;
            }

            if (loginInfo.GameCode.IsNullOrEmpty())
            {
                return null;
            }

            var thirdPartySubGame = ThirdPartySubGameCodes.GetSingle(loginInfo.GameCode);

            if (thirdPartySubGame == null)
            {
                return null;
            }

            return thirdPartySubGame.RemoteGameCode;
        }

        private decimal GetAdmissionBetMoneyBySingle(HandicapInfo handicapInfo, decimal allBetMoney, decimal profitLossMoney)
        {
            /* 赔率 ≤ [盤口标准值]：有效投注额 = 0
            [盤口标准值] < 赔率 < [盤口上限值]：有效投注额 = 盈亏金额（会小于投注本金）
            [盤口上限值] ≤ 赔率：有效投注额 = 总投注额 */
            //取得盤口
            PlatformHandicap platformHandicap = ConvertToPlatformHandicapWithErrorHandle(handicapInfo.Handicap);

            if (platformHandicap == null || platformHandicap == PlatformHandicap.NotBelongAnyMarket)
            {
                return allBetMoney;
            }
            else if (handicapInfo.Odds <= platformHandicap.StandardOdds)
            {
                return 0m;
            }
            else if (handicapInfo.Odds >= platformHandicap.MaxOdds)
            {
                return allBetMoney;
            }
            else
            {
                return Math.Abs(profitLossMoney);
            }
        }

        private decimal GetAdmissionBetMoneyByCombo(List<HandicapInfo> handicapInfos, decimal allBetMoney, decimal profitLossMoney)
        {
            foreach (HandicapInfo handicapInfo in handicapInfos)
            {
                //取得盤口
                PlatformHandicap platformHandicap = ConvertToPlatformHandicapWithErrorHandle(handicapInfo.Handicap);

                if (platformHandicap == null || platformHandicap == PlatformHandicap.NotBelongAnyMarket)
                {
                    continue;
                }

                //其中某一投注賽事賠率小於等於(≤)盤口標準值時，有效投注額寫入為0
                if (!handicapInfo.Odds.HasValue ||  //沒傳賠率
                    handicapInfo.Odds.GetValueOrDefault() <= platformHandicap.StandardOdds)//小於等於(≤)標準值
                {
                    //盤口賠率小於等於(≤)標準值時，有效投注額寫入為0
                    return 0m;
                }
            }

            //若该串关中没有賽事賠率小於等於(≤)盤口標準值時，有效投注额 = 总投注额
            return allBetMoney;
        }

        private decimal GetAdmissionBetMoneyByCashout(decimal allBetMoney, decimal profitLossMoney)
        {
            /*
              结算结果为「兌現」的注单，
              若盈亏金额绝对值 < 总投注金额，有效投注额 = 盈亏金额绝对值
              若盈亏金额绝对值 ≧ 总投注金额，有效投注额 = 总投注金额
             */
            decimal absProfitLossMoney = Math.Abs(profitLossMoney);

            if (absProfitLossMoney < allBetMoney)
            {
                return absProfitLossMoney;
            }
            else
            {
                return allBetMoney;
            }
        }

        private PlatformHandicap ConvertToPlatformHandicapWithErrorHandle(string handicapValue)
        {
            PlatformHandicap platformHandicap = ConvertToPlatformHandicap(handicapValue);

            if (platformHandicap == null)
            {
                //對應不到要通知RD來修復
                ErrorMsgUtil.ErrorHandle(
                    new ArgumentOutOfRangeException($"Handicap not found. {new { productCode = Product.Value, handicapValue }.ToJsonString()}"),
                    EnvLoginUser);
            }

            return platformHandicap;
        }

        private void AddUpdateTPUserScoreDelayJob(List<int> userIds)
        {
            foreach (int userId in userIds)
            {
                //設定N秒後排入queue做更新,所以N秒內快取有資料就不再重複排入延遲任務
                var searchCacheParam = new SearchCacheParam()
                {
                    Key = CacheKey.UpdateTPGameUserScore(Product, userId),
                    CacheSeconds = s_delayUpdateTPGameUserScoreSeconds,
                    IsCloneInstance = false,
                };

                bool isAddDelayJob = false;

                _jxCacheService.Value.GetCache(
                   searchCacheParam,
                   () =>
                   {
                       isAddDelayJob = true;

                       return new object();
                   });

                if (isAddDelayJob)
                {
                    var updateTPGameUserScoreParam = new UpdateTPGameUserScoreParam()
                    {
                        Product = Product,
                        UserID = userId,
                    };

                    _messageQueueDelayJobService.Value.AddDelayJobParam(updateTPGameUserScoreParam, s_delayUpdateTPGameUserScoreSeconds);
                }
            }
        }

        private BaseReturnModel GetLoginPreCheckResult(int userId)
        {
            //判斷遊戲開關
            BaseReturnModel returnModel = GetAllowCreateTransferOrderResult();

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnModel(returnModel.Message);
            }

            if (!CheckOrCreateAccount(userId).IsSuccess)
            {
                return new BaseReturnModel(ThirdPartyGameElement.CreateAccountFail);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private string GetActionName(bool isMoneyIn)
        {
            return TPGameMoneyTransferActionType.GetName(isMoneyIn);
        }

        private BaseReturnModel DoTransfer(BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            bool isMoneyIn = tpGameMoneyInfo is TPGameMoneyInInfo;
            string actionName = GetActionName(isMoneyIn);
            CreateRemoteAccountParam createRemoteAccountParam = GetTPGameAccountAndUpdatePassword(tpGameMoneyInfo.UserID);
            DetailRequestAndResponse detail = _tpGameRemoteApiService.Value.GetRemoteTransferApiResult(isMoneyIn, createRemoteAccountParam, tpGameMoneyInfo);
            string apiResult = detail.ResponseContent;
            BaseReturnDataModel<UserScore> returnModel = GetTransferReturnModel(apiResult);
            string requestDetailJson = detail.ToJsonString(ignoreNull: true);

            try
            {
                if (IsAllowTransferCompensation)
                {
                    bool isDoTransferCompensation = false;

                    if (!returnModel.IsSuccess)
                    {
                        isDoTransferCompensation = IsDoTransferCompensation(apiResult);

                        if (isDoTransferCompensation)
                        {
                            var saveCompensationParam = new SaveCompensationParam
                            {
                                TransferID = tpGameMoneyInfo.GetMoneyID(),
                                UserID = tpGameMoneyInfo.UserID,
                                ProductCode = Product.Value,
                            };

                            _transferCompensationService.Value.SaveMoneyOutCompensation(saveCompensationParam);

                            LogUtilService.ForcedDebug("補償機制：" + saveCompensationParam.ToJsonString());
                        }
                    }

                    if (!isDoTransferCompensation)
                    {
                        _transferCompensationService.Value.ProcessedMoneyOutCompensation(new ProcessedCompensationParam
                        {
                            UserID = tpGameMoneyInfo.UserID,
                            ProductCode = Product.Value,
                        });
                    }
                }

                if (!returnModel.IsSuccess)
                {
                    //發送訊息給客服群
                    SendTelegramMessageToCustomerService(
                        new BasicUserInfo()
                        {
                            UserId = tpGameMoneyInfo.UserID,
                        },
                        string.Format(MessageElement.ProductTransferFailInfo,
                            Product.Name,
                            GetActionName(tpGameMoneyInfo is TPGameMoneyInInfo),
                            tpGameMoneyInfo.OrderID,
                            tpGameMoneyInfo.UserID,
                            returnModel.Message,
                            requestDetailJson));

                    return new BaseReturnModel(returnModel.Message);
                }

                //只要api回傳成功就繼續走流程, 不再做recheck訂單
                return ProcessSuccessOrder(new ProcessSuccessOrderParam()
                {
                    IsMoneyIn = isMoneyIn,
                    TPGameMoneyInfo = tpGameMoneyInfo,
                    RemoteUserScore = returnModel.DataModel,
                });
            }
            finally
            {
                //記錄結果
                var failLog = new StringBuilder($"遠端{actionName}結果:{returnModel.IsSuccess}. ");
                failLog.Append($"RequestAndResponse:{detail.ToJsonString(ignoreNull: true)}");
                failLog.Append($"ErrorMsg:{returnModel.Message}");
                LogUtilService.ForcedDebug(failLog.ToString());
            }
        }

        private CreateRemoteAccountParam GetTPGameAccountAndUpdatePassword(int userId)
        {
            return GetTPGameAccountAndUpdatePassword(userId, out bool isDbExists);
        }

        private CreateRemoteAccountParam GetTPGameAccountAndUpdatePassword(int userId, out bool isDbExists)
        {
            CreateRemoteAccountParam createRemoteAccountParam = TPGameAccountReadService.GetTPGameAccountByLocalAccount(userId, Product, out isDbExists);

            if (createRemoteAccountParam.TPGamePassword.IsNullOrEmpty())
            {
                string newPassword = CreateTPGamePasswordByRule(userId, createRemoteAccountParam.TPGameAccount);

                if (!newPassword.IsNullOrEmpty())
                {
                    //update password
                    if (isDbExists)
                    {
                        ITPGameAccountService tpGameAccountService = ResolveJxBackendService<ITPGameAccountService>(SharedAppSettings.PlatformMerchant, DbConnectionTypes.Master).Value;
                        tpGameAccountService.UpdatePassword(userId, Product, newPassword);
                    }

                    createRemoteAccountParam.TPGamePassword = newPassword;
                }
            }

            return createRemoteAccountParam;
        }

        /// <summary>
        /// 確認上下分狀態
        /// </summary>
        private BaseReturnModel RecheckAfterTransfer(BaseTPGameMoneyInfo tpGameMoneyInfo, UserScore remoteUserScore, bool isStopWhenOrderFail)
        {
            bool isMoneyIn = tpGameMoneyInfo is TPGameMoneyInInfo;
            //確認訂單

            BaseReturnModel orderReturnModel = GetRemoteOrderStatus(tpGameMoneyInfo);

            if (orderReturnModel == null)
            {
                return new BaseReturnModel(ReturnCode.SystemError);
            }
            else if (isStopWhenOrderFail && !orderReturnModel.IsSuccess)
            {
                LogUtilService.Error(orderReturnModel.ToJsonString());

                return orderReturnModel;
            }

            string paramJson = new
            {
                orderReturnModel.IsSuccess,
                MoneyID = tpGameMoneyInfo.GetMoneyID(),
                tpGameMoneyInfo.OrderID
            }.ToJsonString();

            LogUtilService.ForcedDebug($"遠端確認{GetActionName(isMoneyIn)}完成, {paramJson}");

            if (orderReturnModel.IsSuccess)
            {
                return ProcessSuccessOrder(new ProcessSuccessOrderParam()
                {
                    IsMoneyIn = isMoneyIn,
                    TPGameMoneyInfo = tpGameMoneyInfo,
                    RemoteUserScore = remoteUserScore,
                });
            }
            else
            {
                //確定第三方有回訂單狀態為失敗,所以要退款
                return ProcessFailOrder(isMoneyIn, tpGameMoneyInfo, orderReturnModel.Message);
            }
        }

        private BaseReturnModel ProcessSuccessOrder(ProcessSuccessOrderParam param)
        {
            UserScore userScoreParam = param.RemoteUserScore;

            //如果建單後有回傳餘額,以建單後的資料為主,若沒有則反查
            if (userScoreParam == null)
            {
                //取得第三方餘額
                BaseReturnDataModel<UserScore> userScoreReturnModel = GetRemoteUserScore(param.TPGameMoneyInfo, true);

                if (userScoreReturnModel.IsSuccess && userScoreReturnModel.DataModel != null)
                {
                    userScoreParam = userScoreReturnModel.DataModel;
                }
            }

            if (userScoreParam == null)
            {
                LogUtilService.Error("userScoreParam == null");

                return new BaseReturnModel(ReturnCode.SystemError);
            }

            //call 本地轉帳完成sp
            if (!TPGameStoredProcedureRep.DoTransferSuccess(param.IsMoneyIn, param.TPGameMoneyInfo, userScoreParam))
            {
                ErrorMsgUtil.ErrorHandle(
                    new InvalidProgramException($"DoTransferSuccess Error, Param={new { param, userScoreParam }.ToJsonString()}"),
                    EnvLoginUser);

                return new BaseReturnModel(ReturnCode.UpdateFailed);
            }

            return new BaseReturnModel(ReturnCode.TransferMoneySuccess);
        }

        private BaseReturnModel ProcessFailOrder(bool isMoneyIn, BaseTPGameMoneyInfo tpGameMoneyInfo, string errorMsg)
        {
            //call 本地轉帳失敗sp
            if (!TPGameStoredProcedureRep.DoTransferRollback(isMoneyIn, tpGameMoneyInfo, errorMsg))
            {
                ErrorMsgUtil.ErrorHandle(
                    new InvalidProgramException($"DoTransferRollback Error, Param={new { isMoneyIn, tpGameMoneyInfo, errorMsg }.ToJsonString()}"),
                    EnvLoginUser);

                return new BaseReturnModel(ReturnCode.UpdateFailed);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private BaseReturnModel GetRemoteOrderStatus(BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            return GetJobResultWithRetry(() =>
            {
                DetailRequestAndResponse detail = GetRemoteOrderApiResult(GetTPGameAccountAndUpdatePassword(tpGameMoneyInfo.UserID).TPGameAccount, tpGameMoneyInfo);
                BaseReturnModel orderReturnModel = GetQueryOrderReturnModel(detail.ResponseContent);
                string errorMsg = null;

                if (orderReturnModel == null)
                {
                    errorMsg = "orderReturnModel is null";
                }
                else if (!orderReturnModel.IsSuccess)
                {
                    errorMsg = orderReturnModel.Message;
                }

                string debugMessage = null;

                if (!errorMsg.IsNullOrEmpty())
                {
                    debugMessage = "重查第三方订单" + string.Format(MessageElement.ProductTransferFailInfo,
                        Product.Name,
                        GetActionName(tpGameMoneyInfo is TPGameMoneyInInfo),
                        tpGameMoneyInfo.OrderID,
                        tpGameMoneyInfo.UserID,
                        errorMsg,
                        detail.ToJsonString(ignoreNull: true));
                }
                else
                {
                    debugMessage = "重查第三方订单" + string.Format(MessageElement.ProductTransferSuccessInfo,
                        Product.Name,
                        GetActionName(tpGameMoneyInfo is TPGameMoneyInInfo),
                        tpGameMoneyInfo.OrderID,
                        tpGameMoneyInfo.UserID,
                        detail.ToJsonString(ignoreNull: true));
                }

                LogUtilService.ForcedDebug(debugMessage);

                if (!errorMsg.IsNullOrEmpty())
                {
                    SendTelegramMessageToCustomerService(
                        new BasicUserInfo() { UserId = tpGameMoneyInfo.UserID },
                        debugMessage);
                }

                return orderReturnModel;
            });
        }

        private T GetJobResultWithRetry<T>(Func<T> job)
        {
            return GetJobResultWithRetry(job, s_defaultMaxTryCount);
        }

        private T GetJobResultWithRetry<T>(Func<T> job, int maxTryCount)
        {
            return RetryJobUtil.GetJobResultWithRetry(job, maxTryCount, s_defaultRetryIntervalSeconds);
        }

        protected string GetFullUrl(string rootUrl, string relativeUrl)
        {
            return new Uri(new Uri(rootUrl), relativeUrl).ToString();
        }

        protected string GetCombineUrl(params string[] urls)
        {
            return string.Join("/", urls.Select(s => s.TrimStart("/").TrimEnd("/")));
        }

        /// <summary>
        /// 新版Transfer的反序列方法
        /// </summary>
        protected BaseReturnDataModel<RequestAndResponse> GetRemoteFileBetLogResult(string lastSearchToken)
        {
            BaseReturnDataModel<RequestAndResponse> returnDataModel = GetRemoteFileBetLogWrapResult(lastSearchToken);

            if (!returnDataModel.IsSuccess)
            {
                return returnDataModel;
            }

            string betLogContent = returnDataModel.DataModel.ResponseContent;

            return betLogContent.Deserialize<BaseReturnDataModel<RequestAndResponse>>();
        }

        /// <summary>
        /// 舊版Transfer的反序列方法(少封裝一層)
        /// </summary>
        protected BaseReturnDataModel<RequestAndResponse> GetRemoteFileBetLogWrapResult(string lastSearchToken)
        {
            long.TryParse(lastSearchToken, out long lastFileSeq); //parse失敗就當作0, 有可能會有舊的時間token
            var betLogFileService = ResolveJxBackendService<IBetLogFileService>(DbConnectionTypes.Slave).Value;
            RequestAndResponse fileRequestAndContent = betLogFileService.GetBetLogContent(Product, lastFileSeq);
            RemoteFileSetting.HasNewRemoteFile = false;

            if (fileRequestAndContent == null)
            {
                return new BaseReturnDataModel<RequestAndResponse>(MessageElement.FtpDownloadFileNotSuccessfully);
            }
            else if (fileRequestAndContent.RequestBody.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.NoDataChanged);
            }

            // 讀檔回來處理
            RemoteFileSetting.HasNewRemoteFile = true;
            string betLogContent = fileRequestAndContent.ResponseContent;

            var requestAndResponse = new RequestAndResponse()
            {
                RequestBody = fileRequestAndContent.RequestBody,
                ResponseContent = betLogContent
            };

            return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.Success, requestAndResponse);
        }

        protected virtual PlatformHandicap ConvertToPlatformHandicap(string handicap)
        {
            //由於並非所有類型都要實作,故這邊採用virtual的方式, 讓呼叫到此函數的時候回傳null, 並且讓後續維持舊有流程
            return null;
        }

        protected BaseReturnDataModel<string> ConvertToApiReturnDataModel(HttpStatusCode httpStatusCode, string apiResult)
        {
            ReturnCode returnCode = ReturnCode.Success;

            if (httpStatusCode == HttpStatusCode.OK)
            {
                return new BaseReturnDataModel<string>(returnCode, apiResult);
            }

            returnCode = ReturnCode.HttpStatusCodeNotOK;
            var messageArgs = new string[] { ((int)httpStatusCode).ToString() };

            var returnDataModel = new BaseReturnDataModel<string>(returnCode, messageArgs)
            {
                DataModel = apiResult
            };

            return returnDataModel;
        }
    }
}