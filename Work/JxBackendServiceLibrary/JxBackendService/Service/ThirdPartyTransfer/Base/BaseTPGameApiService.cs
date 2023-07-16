using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseTPGameApiService : BaseService, ITPGameApiService, ITPGameApiReadService
    {
        private static readonly int _defaultMaxTryCount = 5;

        private static readonly int _defaultRetryIntervalSeconds = 5;

        private static readonly string _playInfoIsExistsText = "单号已存在";

        //private static readonly double _fetchFileIntervalMinutes = 10;
        private static readonly int _maxMemoLength = 1020;

        protected readonly ITPGameStoredProcedureRep _tpGameStoredProcedureRep;

        private readonly ITPGameAccountService _tpGameAccountService;

        private readonly IMessageQueueService _messageQueueService;

        private readonly ITPGameUserInfoService _tpGameUserInfoService;

        private readonly IFrontsideMenuService _frontsideMenuService;

        private readonly IPlatformProductSettingService _platformProductSettingService;

        private readonly IPlatformProductService _platformProductService;

        protected readonly ITPGameAccountReadService _tpGameAccountReadService;

        private readonly IJxCacheService _jxCacheService;

        private readonly IHttpWebRequestUtilService _httpWebRequestUtilService;

        private readonly ITransferCompensationService _transferCompensationService;

        protected IJxCacheService JxCacheService => _jxCacheService;

        protected IHttpWebRequestUtilService HttpWebRequestUtilService => _httpWebRequestUtilService;

        protected string ProductName => _platformProductService.GetName(Product.Value);

        public BaseTPGameApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _tpGameStoredProcedureRep = ResolveJxBackendService<ITPGameStoredProcedureRep>(Product);
            _tpGameAccountService = ResolveJxBackendService<ITPGameAccountService>(SharedAppSettings.PlatformMerchant);
            _tpGameAccountReadService = ResolveJxBackendService<ITPGameAccountReadService>(SharedAppSettings.PlatformMerchant);
            _messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(envLoginUser.Application);
            _tpGameUserInfoService = ResolveJxBackendService<ITPGameUserInfoService>(Product);
            _frontsideMenuService = ResolveJxBackendService<IFrontsideMenuService>();
            _platformProductSettingService = ResolveKeyed<IPlatformProductSettingService>(Product);
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
            _httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();
            _transferCompensationService = ResolveJxBackendService<ITransferCompensationService>();
        }

        #region abstract methods,properties

        public abstract PlatformProduct Product { get; }

        /// <summary>
        /// 去遠端的第三方實際打轉帳API取得結果
        /// </summary>
        protected abstract DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo);

        /// <summary>
        /// 去遠端的第三方實際打查詢訂單API取得結果
        /// </summary>
        protected abstract DetailRequestAndResponse GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo);

        /// <summary>
        /// 去遠端的第三方實際打查詢餘額API取得結果
        /// </summary>
        protected abstract string GetRemoteUserScoreApiResult(string tpGameAccount);

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
        protected abstract BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param);

        protected abstract BaseReturnDataModel<string> GetRemoteLoginApiResult(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo);

        /// <summary>
        /// 取得遊戲網址
        /// </summary>
        protected abstract BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ipAddress, bool isMobile, LoginInfo loginInfo);

        /// <summary>
        /// 取得帳號是否存在的遠端結果(目前在base無引用，因為每家的流程不太一樣，目前用於子類別中會呼叫)
        /// </summary>
        protected abstract BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount);

        /// <summary>
        /// 取得建立帳號的遠端結果(目前在base無引用，因為每家的流程不太一樣，目前用於子類別中會呼叫)
        /// </summary>
        protected abstract BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param);

        /// <summary>是否把注單資料複製到特定資料夾</summary>
        public abstract bool IsBackupBetLog { get; }

        protected virtual int? TransferAmountFloorDigit { get; }

        #endregion abstract methods,properties

        /// <summary>是否計算有效投注額,未來新商戶一律計算故預設為true</summary>
        public virtual bool IsComputeAdmissionBetMoney { get; } = true;

        public BaseReturnDataModel<RequestAndResponse> GetRemoteBetLog(string lastSearchToken)
        {
            BaseReturnDataModel<RequestAndResponse> returnDataModel = GetRemoteBetLogApiResult(lastSearchToken);

            return returnDataModel;
        }

        public void BackupBetLog(BaseReturnDataModel<RequestAndResponse> requestAndResponseResult)
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
            var service = DependencyUtil.ResolveService<ITransferServiceAppSettingService>();

            //不是每個站點都有oss設定, 所以改為區域變數, 沒有設定的時候會報錯
            var betLogFileService = ResolveJxBackendService<IBetLogFileService>(DbConnectionTypes.Slave);
            betLogFileService.WriteRemoteContentToOtherMerchant(Product, fileSeq, fileContent, service.CopyBetLogToMerchantCodes);
        }

        public virtual BaseReturnModel GetAllowCreateTransferOrderResult()
        {
            if (_frontsideMenuService.GetActiveFrontsideMenus().Any(W => W.ProductCode == Product.Value))
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
            return _tpGameStoredProcedureRep.GetTPGameUnprocessedMoneyInInfo();
        }

        /// <summary>
        /// 取得第三方轉入單正在處理中資訊
        /// </summary>
        /// <returns></returns>
        public List<TPGameMoneyInInfo> GetTPGameProcessingMoneyInInfo()
        {
            return _tpGameStoredProcedureRep.GetTPGameProcessingMoneyInInfo();
        }

        /// <summary>
        /// 取得第三方轉出單未處理資訊
        /// </summary>
        /// <returns></returns>
        public List<TPGameMoneyOutInfo> GetTPGameUnprocessedMoneyOutInfo()
        {
            return _tpGameStoredProcedureRep.GetTPGameUnprocessedMoneyOutInfo();
        }

        /// <summary>
        /// 取得第三方轉出單正在處理中資訊
        /// </summary>
        /// <returns></returns>
        public List<TPGameMoneyOutInfo> GetTPGameProcessingMoneyOutInfo()
        {
            return _tpGameStoredProcedureRep.GetTPGameProcessingMoneyOutInfo();
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

            TPGameMoneyInOrderStatus transferInStatus = TPGameMoneyInOrderStatus.Unprocessed;

            if (param.IsSynchronizing)
            {
                transferInStatus = TPGameMoneyInOrderStatus.Processing;
            }

            TPGameTransferMoneyResult result = _tpGameStoredProcedureRep
               .CreateMoneyInOrder(param.UserID, param.Amount, GetTPGameAccount(param.UserID), transferInStatus);

            if (!result.ErrorMsg.IsNullOrEmpty())
            {
                LogUtil.Error(result.ErrorMsg);

                return new BaseReturnModel(result.ErrorMsg);
            }

            if (!param.IsSynchronizing)
            {
                return new BaseReturnModel(ReturnCode.TransferOutSubmit);
            }

            return DoTransfer(result.TPGameMoneyInfo, param.IsSynchronizing);
        }

        /// <summary> 建立全轉回的轉出單 </summary>
        public BaseReturnDataModel<decimal> CreateAllAmountTransferOutInfo(TPGameTranfserParam param, out bool isAllAmount)
        {
            BaseReturnModel transferResult = BaseCreateTransferOutInfo(param, isTransferOutAll: true, out decimal actuallyAmount, out isAllAmount, out string moneyId);
            BaseReturnDataModel<decimal> returnDataModel = transferResult.CastByJson<BaseReturnDataModel<decimal>>();
            // 回傳真正轉出金額數目
            returnDataModel.DataModel = actuallyAmount;

            return returnDataModel;
        }

        /// <summary> 建立轉出單 </summary>
        public BaseReturnModel CreateTransferOutInfo(TPGameTranfserParam param, bool isTransferOutAll, out string moneyId)
        {
            return BaseCreateTransferOutInfo(param, isTransferOutAll, out decimal actuallyAmount, out bool isAllAmount, out moneyId);
        }

        /// <summary> 建立轉出單 </summary>
        public BaseReturnModel CreateTransferOutInfo(TPGameTranfserParam param, bool isTransferOutAll, out decimal actuallyAmount, out string moneyId)
        {
            return BaseCreateTransferOutInfo(param, isTransferOutAll, out actuallyAmount, out bool isAllAmount, out moneyId);
        }

        private BaseReturnModel BaseCreateTransferOutInfo(TPGameTranfserParam param, bool isTransferOutAll, out decimal actuallyAmount, out bool isAllAmount, out string moneyId)
        {
            // 設定最終轉帳金額
            actuallyAmount = param.Amount;
            isAllAmount = true;
            moneyId = null;

            // 原流程進行轉出判斷金額及開關
            if (!isTransferOutAll)
            {
                BaseReturnModel checkModel = CreateTransferBeforeCheck(param.UserID, param.Amount);

                if (!checkModel.IsSuccess)
                {
                    return checkModel;
                }
            }

            BaseReturnDataModel<UserScore> checkScore = GetRemoteUserScore(param, isRetry: false);

            if (!checkScore.IsSuccess)
            {
                return new BaseReturnModel(checkScore.Message);
            }

            // 全部轉回的情況下不判斷金額及開關
            if (isTransferOutAll)
            {
                decimal minTPGameTransferAmount = GlobalVariables.TPTransferAmountBound.MinTPGameTransferAmount;
                decimal maxTPGameTransferAmount = GlobalVariables.TPTransferAmountBound.MaxTPGameTransferAmount;

                // 小於最小轉帳值不進行轉回
                if (checkScore.DataModel.AvailableScores < minTPGameTransferAmount)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                // 查回第三方錢包最後金額，為實際轉回主錢包的金額
                actuallyAmount = checkScore.DataModel.AvailableScores;

                if (TransferAmountFloorDigit.HasValue)
                {
                    actuallyAmount = actuallyAmount.Floor(TransferAmountFloorDigit.Value);
                }

                //怕一次轉不完,需要判斷第三方餘額
                if (actuallyAmount > maxTPGameTransferAmount)
                {
                    actuallyAmount = maxTPGameTransferAmount;
                    isAllAmount = false;

                    var transferOutUserDetail = new TransferOutUserDetail()
                    {
                        AffectedUser = new BaseBasicUserInfo() { UserId = param.UserID },
                        IsOperateByBackSide = param.IsOperateByBackSide,
                    };

                    //放入queue做非同步處理
                    _messageQueueService.EnqueueTransferAllOutMessage(Product, transferOutUserDetail);
                }
            }

            if (checkScore.DataModel.AvailableScores < actuallyAmount)
            {
                return new BaseReturnModel(string.Format(ThirdPartyGameElement.TransferErrorNotEnoughMoney, checkScore.DataModel.AvailableScores.ToString("N4")));
            }

            TPGameMoneyOutOrderStatus transferOutStatus = TPGameMoneyOutOrderStatus.Unprocessed;

            if (param.IsSynchronizing)
            {
                transferOutStatus = TPGameMoneyOutOrderStatus.Processing;
            }

            string tpGameAccount = GetTPGameAccount(param.UserID);

            TPGameTransferMoneyResult result = _tpGameStoredProcedureRep
                .CreateMoneyOutOrder(new CreateTransferOutOrderParam()
                {
                    UserId = param.UserID,
                    Amount = actuallyAmount,
                    TPGameAccount = tpGameAccount,
                    TransferOutStatus = transferOutStatus,
                    IsOperateByBackSide = param.IsOperateByBackSide
                });

            if (!result.ErrorMsg.IsNullOrEmpty())
            {
                LogUtil.Error(string.Format($"CreateMoneyOutOrder failed {ProductName} {result.ErrorMsg}"));

                return new BaseReturnModel(result.ErrorMsg);
            }

            moneyId = result.TPGameMoneyInfo.GetMoneyID();

            if (!param.IsSynchronizing)
            {
                return new BaseReturnModel(ReturnCode.TransferOutSubmit);
            }

            return DoTransfer(result.TPGameMoneyInfo, param.IsSynchronizing);
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
            int maxTryCount = _defaultMaxTryCount;

            if (!isRetry)
            {
                maxTryCount = 1;
            }

            return GetJobResultWithRetry(() =>
            {
                string tpGameAccount = GetTPGameAccount(invocationUserParam.UserID);

                return GetUserScoreReturnModel(GetRemoteUserScoreApiResult(tpGameAccount));
            },
            maxTryCount);
        }

        public BaseReturnModel CheckOrCreateAccount(int userId)
        {
            if (_tpGameAccountService.CheckTPGAccountExist(userId, Product))
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            BaseReturnModel createAccountModel = CheckOrCreateRemoteAccount(new CreateRemoteAccountParam()
            {
                TPGameAccount = GetTPGameAccount(userId),
            });

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
            if (!_tpGameUserInfoService.IsUserExists(userId))
            {
                if (!_tpGameUserInfoService.CreateUser(userId))
                {
                    return new BaseReturnDataModel<string>(ThirdPartyGameElement.CreateAccountFail);
                }
            }

            //創建thirdPartyUserAccount
            ThirdPartyUserAccount thirdPartyUserAccount = _tpGameAccountReadService.GetThirdPartyUserAccount(userId, Product);
            string tpGameAccount;

            if (thirdPartyUserAccount == null)
            {
                tpGameAccount = _tpGameAccountReadService.GetTPGameAccountByRule(Product, userId);
                _tpGameAccountService.Create(userId, Product, tpGameAccount);
            }
            else
            {
                tpGameAccount = thirdPartyUserAccount.Account;
            }

            return new BaseReturnDataModel<string>(ReturnCode.Success, tpGameAccount);
        }

        public BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(ForwardGameUrlParam param)
        {
            BaseReturnModel preCheckResult = GetLoginPreCheckResult(param.LoginUser.UserId);

            if (!preCheckResult.IsSuccess)
            {
                return new BaseReturnDataModel<TPGameOpenParam>(preCheckResult.Message);
            }

            BaseReturnDataModel<string> gameUrlResult = GetRemoteForwardGameUrl(GetTPGameAccount(param.LoginUser.UserId), param.IpAddress, param.IsMobile, param.LoginInfo);

            if (!gameUrlResult.IsSuccess)
            {
                return new BaseReturnDataModel<TPGameOpenParam>(gameUrlResult.Message);
            }

            return new BaseReturnDataModel<TPGameOpenParam>(ReturnCode.Success, new TPGameOpenParam()
            {
                Url = gameUrlResult.DataModel,
                OpenGameModeValue = _platformProductSettingService.OpenMode.Value
            });
        }

        public BaseReturnDataModel<string> GetLoginApiResult(ForwardGameUrlParam param)
        {
            BaseReturnModel preCheckResult = GetLoginPreCheckResult(param.LoginUser.UserId);

            if (!preCheckResult.IsSuccess)
            {
                return new BaseReturnDataModel<string>(preCheckResult.Message);
            }

            BaseReturnDataModel<string> loginApiResult = GetRemoteLoginApiResult(GetTPGameAccount(param.LoginUser.UserId), param.IpAddress, param.IsMobile, param.LoginInfo);

            return loginApiResult;
        }

        public Dictionary<string, int> GetUserIdsFromTPGameAccounts(List<string> tpGameAccounts)
        {
            return _tpGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts.ToHashSet());
        }

        public void SaveProfitlossToPlatform(List<InsertTPGameProfitlossParam> tpGameProfitlosses, Func<string, SaveBetLogFlags, bool> updateSQLiteToSavedStatus)
        {
            SaveProfitlossToPlatform(new SaveProfitlossToPlatformParam()
            {
                TPGameProfitlosses = tpGameProfitlosses,
                UpdateSQLiteToSavedStatus = updateSQLiteToSavedStatus
            });
        }

        public void SaveProfitlossToPlatform(SaveProfitlossToPlatformParam saveProfitlossToPlatformParam)
        {
            List<int> userIds = saveProfitlossToPlatformParam.TPGameProfitlosses.Where(w => !w.IsIgnore).Select(s => s.UserID).Distinct().ToList();

            if (saveProfitlossToPlatformParam.UserScoreMap == null)
            {
                saveProfitlossToPlatformParam.UserScoreMap = GetUserScoreMap(userIds);
            }

            //List<InsertTPGameProfitlossSpParam> insertTPGameProfitlossSpParams = saveProfitlossToPlatformParam.TPGameProfitlosses.CastByJson<List<InsertTPGameProfitlossSpParam>>();

            foreach (InsertTPGameProfitlossParam insertTPGameProfitlossParam in saveProfitlossToPlatformParam.TPGameProfitlosses)
            {
                InsertTPGameProfitlossSpParam tpGameProfitloss = insertTPGameProfitlossParam.CastByJson<InsertTPGameProfitlossSpParam>();
                tpGameProfitloss.SetBetMoneys(IsComputeAdmissionBetMoney,
                    insertTPGameProfitlossParam.ProfitLossMoney, insertTPGameProfitlossParam.AllBetMoney);

                if (tpGameProfitloss.IsIgnore)
                {
                    saveProfitlossToPlatformParam.UpdateSQLiteToSavedStatus.Invoke(tpGameProfitloss.KeyId, SaveBetLogFlags.Ignore);
                    continue;
                }

                if (StringUtil.IsValidJson(tpGameProfitloss.Memo))
                {
                    string memoJson = tpGameProfitloss.Memo;
                    tpGameProfitloss.Memo = memoJson.ToLocalizationContent(tpGameProfitloss.Memo, EnvLoginUser.Application);
                }

                tpGameProfitloss.Memo = tpGameProfitloss.Memo.ToShortString(_maxMemoLength);

                if (saveProfitlossToPlatformParam.UserScoreMap.ContainsKey(tpGameProfitloss.UserID))
                {
                    UserScore userScore = saveProfitlossToPlatformParam.UserScoreMap[tpGameProfitloss.UserID];
                    tpGameProfitloss.AvailableScores = userScore.AvailableScores;
                    tpGameProfitloss.FreezeScores = userScore.FreezeScores;
                }

                //寫入盈虧跟注單
                BaseReturnModel saveResult = _tpGameStoredProcedureRep.AddProductProfitLossAndPlayInfo(tpGameProfitloss);

                if (!saveResult.IsSuccess)
                {
                    if (saveResult.Message == _playInfoIsExistsText)
                    {
                        saveProfitlossToPlatformParam.UpdateSQLiteToSavedStatus.Invoke(tpGameProfitloss.KeyId, SaveBetLogFlags.Success);
                    }
                    else
                    {
                        LogUtil.ForcedDebug($"保存{Product.Value}虧盈與注單數據 {tpGameProfitloss.KeyId} 到資料庫失敗: {saveResult.Message}");
                        saveProfitlossToPlatformParam.UpdateSQLiteToSavedStatus.Invoke(tpGameProfitloss.KeyId, SaveBetLogFlags.Fail);
                    }

                    continue;
                }

                LogUtil.ForcedDebug($"保存{Product.Value}虧盈與注單數據 {tpGameProfitloss.KeyId} 到資料庫成功");

                //回寫sqlite狀態
                if (!saveProfitlossToPlatformParam.UpdateSQLiteToSavedStatus.Invoke(tpGameProfitloss.KeyId, SaveBetLogFlags.Success))
                {
                    throw new InvalidOperationException("UpdateSQLiteToSavedStatus Fail");
                }
            }
        }

        /// <summary>計算有效投注額</summary>
        public decimal ComputeAdmissionBetMoney(ComputeAdmissionBetMoneyParam param)
        {
            //目前只有需要盤口的產品需要做特殊計算, 未來如果有沒盤口資料的需要做額外計算,則需要把IsComputeAdmissionBetAmountByHandicap拆分
            if (!IsComputeAdmissionBetMoney || !Product.ProductType.IsComputeAdmissionBetMoneyByHandicap)
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
            bool isTestingEnvironment = SharedAppSettings.GetEnvironmentCode(EnvLoginUser.Application).IsTestingEnvironment;
            TelegramChatGroup telegramChatGroup = TelegramChatGroup.CustomerServiceProduction;

            if (isTestingEnvironment)
            {
                telegramChatGroup = TelegramChatGroup.CustomerServiceTesting;
            }

            TelegramUtil.SendMessageWithEnvInfoAsync(
                new SendTelegramParam()
                {
                    ApiUrl = SharedAppSettings.TelegramApiUrl,
                    EnvironmentUser = new EnvironmentUser()
                    {
                        Application = EnvLoginUser.Application,
                        LoginUser = user
                    },
                    Message = message
                },
                telegramChatGroup);
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

            if (platformHandicap == null)
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

                if (platformHandicap == null)
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

        private Dictionary<int, UserScore> GetUserScoreMap(List<int> userIds)
        {
            Dictionary<int, UserScore> userScoreMap = new Dictionary<int, UserScore>();

            foreach (int userId in userIds)
            {
                if (userScoreMap.ContainsKey(userId))
                {
                    continue;
                }

                try
                {
                    var returnModel = GetRemoteUserScore(new InvocationUserParam() { UserID = userId }, isRetry: false);

                    if (returnModel.IsSuccess)
                    {
                        userScoreMap.Add(userId, returnModel.DataModel);
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"GetRemoteUserScore Error. UserId={userId}. ex={ex}");
                }

                Thread.Sleep(500);
            }

            return userScoreMap;
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

        private BaseReturnModel DoTransfer(BaseTPGameMoneyInfo tpGameMoneyInfo, bool isSynchronizing)
        {
            bool isMoneyIn = tpGameMoneyInfo is TPGameMoneyInInfo;
            string actionName = GetActionName(isMoneyIn);
            string tpGameAccount = GetTPGameAccount(tpGameMoneyInfo.UserID);
            DetailRequestAndResponse detail = GetRemoteTransferApiResult(isMoneyIn, tpGameAccount, tpGameMoneyInfo);
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

                            _transferCompensationService.SaveMoneyOutCompensation(saveCompensationParam);

                            LogUtil.ForcedDebug("補償機制：" + saveCompensationParam.ToJsonString());
                        }
                    }

                    if (!isDoTransferCompensation)
                    {
                        _transferCompensationService.ProcessedMoneyOutCompensation(new ProcessedCompensationParam
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
                    IsSynchronizing = isSynchronizing,
                });
            }
            finally
            {
                //記錄結果
                var failLog = new StringBuilder($"遠端{actionName}結果:{returnModel.IsSuccess}. ");
                failLog.Append($"RequestAndResponse:{detail.ToJsonString(ignoreNull: true)}");
                failLog.Append($"ErrorMsg:{returnModel.Message}");
                LogUtil.ForcedDebug(failLog.ToString());
            }

            /*
            //等候, 避免第三方資料還沒同步完成
            Thread.Sleep(5000);
            LogUtil.ForcedDebug($"遠端{actionName}成功等待確認. " +
                $"Request:{requestJson} " +
                $"Response:{apiResult}");
            //如果第三方有回傳積分,要另做處理

            return RecheckAfterTransfer(tpGameMoneyInfo, returnModel.DataModel, isStopWhenOrderFail: true);
            */
        }

        protected virtual bool IsAllowTransferCompensation => false;

        protected virtual bool IsDoTransferCompensation(string apiResult) => false;

        private string GetTPGameAccount(int userId)
        {
            BaseReturnDataModel<string> result = _tpGameAccountReadService.GetTPGameAccountByLocalAccount(userId, Product);

            if (result.IsSuccess)
            {
                return result.DataModel;
            }

            throw new Exception("GetTPGameAccount:" + userId);
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
                LogUtil.Error(orderReturnModel.ToJsonString());

                return orderReturnModel;
            }

            string paramJson = new
            {
                orderReturnModel.IsSuccess,
                MoneyID = tpGameMoneyInfo.GetMoneyID(),
                tpGameMoneyInfo.OrderID
            }.ToJsonString();

            LogUtil.ForcedDebug($"遠端確認{GetActionName(isMoneyIn)}完成, {paramJson}");

            if (orderReturnModel.IsSuccess)
            {
                return ProcessSuccessOrder(new ProcessSuccessOrderParam()
                {
                    IsMoneyIn = isMoneyIn,
                    TPGameMoneyInfo = tpGameMoneyInfo,
                    RemoteUserScore = remoteUserScore,
                    IsSynchronizing = false
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
                LogUtil.Error("userScoreParam == null");

                return new BaseReturnModel(ReturnCode.SystemError);
            }

            //call 本地轉帳完成sp
            if (!_tpGameStoredProcedureRep.DoTransferSuccess(param.IsMoneyIn, param.TPGameMoneyInfo, userScoreParam))
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
            if (!_tpGameStoredProcedureRep.DoTransferRollback(isMoneyIn, tpGameMoneyInfo, errorMsg))
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
                DetailRequestAndResponse detail = GetRemoteOrderApiResult(GetTPGameAccount(tpGameMoneyInfo.UserID), tpGameMoneyInfo);
                BaseReturnModel orderReturnModel = GetQueryOrderReturnModel(detail.ResponseContent);

                if (orderReturnModel == null || !orderReturnModel.IsSuccess)
                {
                    string debugMessage = "重查第三方订单" + string.Format(MessageElement.ProductTransferFailInfo,
                        Product.Name,
                        GetActionName(tpGameMoneyInfo is TPGameMoneyInInfo),
                        tpGameMoneyInfo.OrderID,
                        tpGameMoneyInfo.UserID,
                        orderReturnModel == null ? "orderReturnModel is null" : orderReturnModel.Message,
                        detail.ToJsonString(ignoreNull: true));

                    LogUtil.ForcedDebug(debugMessage);

                    SendTelegramMessageToCustomerService(
                        new BasicUserInfo() { UserId = tpGameMoneyInfo.UserID },
                        debugMessage);
                }

                return orderReturnModel;
            });
        }

        private T GetJobResultWithRetry<T>(Func<T> job)
        {
            return GetJobResultWithRetry(job, _defaultMaxTryCount);
        }

        private T GetJobResultWithRetry<T>(Func<T> job, int maxTryCount)
        {
            return RetryJobUtil.GetJobResultWithRetry(job, maxTryCount, _defaultRetryIntervalSeconds);
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
            var betLogFileService = ResolveJxBackendService<IBetLogFileService>(DbConnectionTypes.Slave);
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

        protected BaseReturnDataModel<string> ConverToApiReturnDataModel(HttpStatusCode httpStatusCode, string apiResult)
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