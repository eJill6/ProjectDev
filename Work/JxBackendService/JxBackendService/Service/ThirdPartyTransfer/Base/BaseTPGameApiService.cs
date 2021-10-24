using FluentFTP;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Ftp;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseTPGameApiService : BaseService, ITPGameApiService, ITPGameApiReadService
    {
        private static readonly int _delaySendSecond = 1;
        private static readonly int _defaultMaxTryCount = 5;
        private static readonly int _defaultRetryIntervalSeconds = 5;
        private static readonly string _playInfoIsExistsText = "单号已存在";
        private static readonly double _fetchFileIntervalMinutes = 10;

        private readonly ITPGameStoredProcedureRep _tpGameStoredProcedureRep;
        private readonly ITPGameAccountService _tpGameAccountService;
        private readonly ITPGameAccountReadService _tpGameAccountReadService;
        private readonly IMessageQueueService _tpGameMqService;
        private readonly ITPGameUserInfoService _tpGameUserInfoService;
        private readonly IFrontsideMenuService _frontsideMenuService;

        public abstract PlatformProduct Product { get; }

        public BaseTPGameApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _tpGameStoredProcedureRep = ResolveJxBackendService<ITPGameStoredProcedureRep>(Product);
            _tpGameAccountService = ResolveJxBackendService<ITPGameAccountService>(SharedAppSettings.PlatformMerchant);
            _tpGameAccountReadService = ResolveJxBackendService<ITPGameAccountReadService>(SharedAppSettings.PlatformMerchant);
            _tpGameMqService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(envLoginUser.Application);
            _tpGameUserInfoService = ResolveJxBackendService<ITPGameUserInfoService>(Product);
            _frontsideMenuService = ResolveJxBackendService<IFrontsideMenuService>();
        }

        #region abstract methods

        /// <summary>
        /// 去遠端的第三方實際打轉帳API取得結果
        /// </summary>
        protected abstract string GetRemoteTransferApiResult(bool isMoneyIn, string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo);

        /// <summary>
        /// 去遠端的第三方實際打查詢訂單API取得結果
        /// </summary>
        protected abstract string GetRemoteOrderApiResult(string tpGameAccount, BaseTPGameMoneyInfo tpGameMoneyInfo);

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

        /// <summary>
        /// 取得遊戲網址
        /// </summary>
        protected abstract BaseReturnDataModel<string> GetRemoteForwardGameUrl(string tpGameAccount, string ip, bool isMobile);

        /// <summary>
        /// 取得帳號是否存在的遠端結果(用於mock覆寫用)
        /// </summary>
        protected abstract BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(string tpGameAccount);

        /// <summary>
        /// 取得建立帳號的遠端結果(用於mock覆寫用)
        /// </summary>
        protected abstract BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param);
        #endregion

        protected virtual bool IsBackupBetLog { get; } = true;

        public BaseReturnDataModel<RequestAndResponse> GetRemoteBetLog(string lastSearchToken)
        {
            BaseReturnDataModel<RequestAndResponse> returnDataModel = GetRemoteBetLogApiResult(lastSearchToken);

            if (returnDataModel.IsSuccess && IsBackupBetLog)
            {
                // 利用時間戳記當作檔名
                DateTime nowDateTime = DateTime.Now;
                string dateFolderName = nowDateTime.ToFormatYearMonthDateValue();
                string fileName = nowDateTime.ToUnixOfTime().ToString();

                // 讓下載FTP平台可從RequestBody得知交換TOKEN 
                var returnFtpDataModel = new BaseReturnDataModel<RequestAndResponse>
                {
                    IsSuccess = returnDataModel.IsSuccess,
                    DataModel = new RequestAndResponse
                    {
                        RequestBody = fileName,
                        ResponseContent = returnDataModel.DataModel.ResponseContent
                    }
                };

                // 儲放本地端第三方檔案路徑
                string saveLocalFilePath = $"{FtpSharedSettings.SaveLocalFilePath}/{Product.Value}/{dateFolderName}";
                CreateLocalFileFolder(saveLocalFilePath);

                string fileContent = returnFtpDataModel.ToJsonString();

                File.WriteAllText($"{saveLocalFilePath}/{fileName}.json", fileContent, Encoding.UTF8);
            }

            return returnDataModel;
        }

        private void CreateLocalFileFolder(string saveLocalFilePath)
        {
            if (Directory.Exists(saveLocalFilePath))
            {
                return;
            }

            Directory.CreateDirectory(saveLocalFilePath);
        }

        public virtual BaseReturnModel GetAllowCreateTransferOrderResult()
        {
            if (_frontsideMenuService.GetActiveFrontsideMenu().Any(W => W.ProductCode == Product.Value))
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
        public BaseReturnModel CreateTransferBeforeCheck(int userID, string userName, decimal amount)
        {
            if (amount <= 0)
            {
                return new BaseReturnModel(ThirdPartyGameElement.TransferMoneyMustThanZero);
            }
            else if (amount > 40000)
            {
                return new BaseReturnModel(ThirdPartyGameElement.TransferMoneyMustUnderFourW);
            }

            // 檢查 FrontsideMenu Active 啟用狀態 判斷能否建立轉帳單
            BaseReturnModel allowCreateOrderResult = GetAllowCreateTransferOrderResult();

            if (allowCreateOrderResult.IsSuccess == false)
            {
                return allowCreateOrderResult;
            }

            BaseReturnModel checkOrCreateResult = CheckOrCreateAccount(userID, userName);
            return checkOrCreateResult;
        }

        /// <summary>
        /// 建立轉入單
        /// </summary>
        public BaseReturnModel CreateTransferInInfo(int userID, string userName, decimal amount)
        {
            BaseReturnModel checkModel = CreateTransferBeforeCheck(userID, userName, amount);

            if (!checkModel.IsSuccess)
            {
                return checkModel;
            }

            amount = decimal.Round(amount, 2);
            string result = _tpGameStoredProcedureRep.CreateMoneyInOrder(userID, amount, GetTPGameAccount(userID));

            if (result.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.TransferSubmit);
            }
            else
            {
                LogUtil.Error(result);
                return new BaseReturnModel(result);
            }
        }

        /// <summary>
        /// 建立轉出單
        /// </summary>
        public BaseReturnModel CreateTransferOutInfo(int userID, string userName, decimal amount)
        {
            BaseReturnModel checkModel = CreateTransferBeforeCheck(userID, userName, amount);

            if (!checkModel.IsSuccess)
            {
                return checkModel;
            }

            BaseReturnDataModel<UserScore> checkScore = GetRemoteUserScore(userID, false);

            if (!checkScore.IsSuccess)
            {
                return checkScore;
            }

            amount = decimal.Round(amount, 2);

            if (checkScore.DataModel.AvailableScores < amount)
            {
                return new BaseReturnModel(string.Format(ThirdPartyGameElement.TransferErrorNotEnoughMoney, checkScore.DataModel.AvailableScores.ToString("N4")));
            }

            var result = _tpGameStoredProcedureRep.CreateMoneyOutOrder(userID, amount, GetTPGameAccount(userID));

            if (string.IsNullOrWhiteSpace(result))
            {
                return new BaseReturnModel(ReturnCode.TransferOutSubmit);
            }
            else
            {
                LogUtil.Error(result);
                return new BaseReturnModel(ThirdPartyGameElement.TransferFail);
            }
        }

        public void TransferIn(object state)
        {
            ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
            {
                if (state is TPGameMoneyInInfo == false)
                {
                    throw new ArgumentException("傳入型態不符");
                }

                var tpGameMoneyInfo = state as TPGameMoneyInInfo;
                DoTransfer(tpGameMoneyInfo);
            });
        }

        public void TransferOut(object state)
        {
            ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
            {
                if (state is TPGameMoneyOutInfo == false)
                {
                    throw new ArgumentException("傳入型態不符");
                }

                var tpGameMoneyInfo = state as TPGameMoneyOutInfo;
                DoTransfer(tpGameMoneyInfo);
            });
        }

        public void RecheckProcessingOrders(object state)
        {
            ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
            {
                if (state is BaseTPGameMoneyInfo == false)
                {
                    throw new ArgumentException("傳入型態不符");
                }

                RecheckAfterTransfer(state as BaseTPGameMoneyInfo, null);
            });
        }

        public BaseReturnDataModel<UserScore> GetRemoteUserScore(int userId, bool isRetry)
        {
            int maxTryCount = _defaultMaxTryCount;

            if (!isRetry)
            {
                maxTryCount = 1;
            }

            return GetJobResultWithRetry(() =>
            {
                string tpGameAccount = GetTPGameAccount(userId);
                return GetUserScoreReturnModel(GetRemoteUserScoreApiResult(tpGameAccount));
            }, maxTryCount);
        }

        public BaseReturnModel CheckOrCreateAccount(int userId, string userName)
        {
            if (_tpGameAccountService.CheckTPGAccountExist(userId, Product))
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            BaseReturnModel createAccountModel = CheckOrCreateRemoteAccount(new CreateRemoteAccountParam()
            {
                TPGameAccount = GetTPGameAccount(userId),
                UserName = userName
            });

            if (createAccountModel.IsSuccess)
            {
                return CheckOrCreateLocalAccount(userId, userName);
            }

            return createAccountModel;
        }

        /// <summary>
        /// 創建本地帳號
        /// </summary>
        private BaseReturnModel CheckOrCreateLocalAccount(int userId, string userName)
        {
            //創建user

            if (!_tpGameUserInfoService.IsUserExists(userId))
            {
                if (!_tpGameUserInfoService.CreateUser(userId, userName))
                {
                    return new BaseReturnModel(ThirdPartyGameElement.CreateAccountFail);
                }
            }

            //創建thirdPartyUserAccount
            if (!_tpGameAccountService.CheckTPGAccountExist(userId, Product))
            {
                _tpGameAccountService.Create(userId, userName, Product, GetTPGameAccount(userId));
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnDataModel<string> GetForwardGameUrl(int userId, string userName, string ip, bool isMobile)
        {
            //判斷遊戲開關
            BaseReturnModel returnModel = GetAllowCreateTransferOrderResult();

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(returnModel.Message, null);
            }

            if (!CheckOrCreateAccount(userId, userName).IsSuccess)
            {
                return new BaseReturnDataModel<string>(ThirdPartyGameElement.CreateAccountFail, string.Empty);
            }

            return GetRemoteForwardGameUrl(GetTPGameAccount(userId), ip, isMobile);
        }

        public Dictionary<string, int> GetUserIdsFromTPGameAccounts(List<string> tpGameAccounts)
        {
            return _tpGameAccountReadService.GetUserIdsByTPGameAccounts(Product, tpGameAccounts.ToHashSet());
        }

        public void SaveProfitlossToPlatform(List<InsertTPGameProfitlossParam> tpGameProfitlosses, Func<string, SaveBetLogFlags, bool> updateSQLiteToSavedStatus)
        {
            Dictionary<int, UserScore> userScoreMap = new Dictionary<int, UserScore>();

            //先取得用戶資訊去遠端取得積分,再統一送入SP更新
            foreach (int userId in tpGameProfitlosses.Where(w => !w.IsIgnore).Select(s => s.UserID).Distinct())
            {
                try
                {
                    var returnModel = GetRemoteUserScore(userId, false);

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

            foreach (InsertTPGameProfitlossParam tpGameProfitloss in tpGameProfitlosses)
            {
                if (tpGameProfitloss.IsIgnore)
                {
                    updateSQLiteToSavedStatus.Invoke(tpGameProfitloss.KeyId, SaveBetLogFlags.Ignore);
                    continue;
                }

                if (userScoreMap.ContainsKey(tpGameProfitloss.UserID))
                {
                    tpGameProfitloss.AvailableScores = userScoreMap[tpGameProfitloss.UserID].AvailableScores;
                    tpGameProfitloss.FreezeScores = userScoreMap[tpGameProfitloss.UserID].FreezeScores;
                }

                //寫入盈虧跟注單
                BaseReturnModel saveResult = _tpGameStoredProcedureRep.AddProductProfitLossAndPlayInfo(tpGameProfitloss);

                if (!saveResult.IsSuccess)
                {
                    if (saveResult.Message == _playInfoIsExistsText)
                    {
                        updateSQLiteToSavedStatus.Invoke(tpGameProfitloss.KeyId, SaveBetLogFlags.Success);
                    }
                    else
                    {
                        LogUtil.ForcedDebug($"保存{Product.Value}虧盈與注單數據 {tpGameProfitloss.KeyId} 到資料庫失敗: {saveResult.Message}");
                        updateSQLiteToSavedStatus.Invoke(tpGameProfitloss.KeyId, SaveBetLogFlags.Fail);
                    }

                    continue;
                }

                LogUtil.ForcedDebug($"保存{Product.Value}虧盈與注單數據 {tpGameProfitloss.KeyId} 到資料庫成功");

                //回寫sqlite狀態
                if (!updateSQLiteToSavedStatus.Invoke(tpGameProfitloss.KeyId, SaveBetLogFlags.Success))
                {
                    throw new InvalidOperationException("UpdateSQLiteToSavedStatus Fail");
                }
            }

            SendRefreshUserInfoMqMessage(userScoreMap.Keys.ToList());
        }

        private string GetActionName(bool isMoneyIn)
        {
            return TPGameMoneyTransferActionType.GetName(isMoneyIn);
        }

        private void DoTransfer(BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            bool isMoneyIn = tpGameMoneyInfo is TPGameMoneyInInfo;
            string actionName = GetActionName(isMoneyIn);
            string tpGameAccount = GetTPGameAccount(tpGameMoneyInfo.UserID);
            string apiResult = GetRemoteTransferApiResult(isMoneyIn, tpGameAccount, tpGameMoneyInfo);
            BaseReturnDataModel<UserScore> returnModel = GetTransferReturnModel(apiResult);

            string requestJson = new { MoneyID = tpGameMoneyInfo.GetMoneyID(), tpGameMoneyInfo.OrderID }.ToJsonString(true);

            if (returnModel.IsSuccess)
            {
                //等候, 避免第三方資料還沒同步完成
                Thread.Sleep(5000);
                LogUtil.ForcedDebug($"遠端{actionName}成功等待確認. " +
                    $"Request:{requestJson} " +
                    $"Response:{apiResult}");

                //如果第三方有回傳積分,要另做處理
                RecheckAfterTransfer(tpGameMoneyInfo, returnModel.DataModel);
            }
            else
            {
                //記錄失敗結果
                var failLog = new StringBuilder($"遠端{actionName}失敗. ");
                failLog.Append($"Request:{requestJson} ");
                failLog.Append($"Response:{apiResult}");
                failLog.Append($"ErrorMsg:{returnModel.Message}");
                LogUtil.ForcedDebug(failLog.ToString());
            }
        }

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
        private void RecheckAfterTransfer(BaseTPGameMoneyInfo tpGameMoneyInfo, UserScore remoteUserScore)
        {
            bool isMoneyIn = tpGameMoneyInfo is TPGameMoneyInInfo;
            //確認訂單

            BaseReturnModel orderReturnModel = GetRemoteOrderStatus(tpGameMoneyInfo);

            if (orderReturnModel == null)
            {
                return;
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
                //有取得第三方回應的資料要回寫DB
                //若訂單狀態為人工洗資料的狀態9,先更新為1, 否則後續EXEC SP會鎖狀態造成更新失敗                
                if (isMoneyIn &&
                    tpGameMoneyInfo.Status == TPGameMoneyInOrderStatus.Manual &&
                    !_tpGameStoredProcedureRep.UpdateMoneyInOrderStatusFromManualToProcessing(tpGameMoneyInfo.GetMoneyID()))
                {
                    return;
                }
                else if (!isMoneyIn &&
                    tpGameMoneyInfo.Status == TPGameMoneyOutOrderStatus.Manual &&
                    !_tpGameStoredProcedureRep.UpdateMoneyOutOrderStatusFromManualToProcessing(tpGameMoneyInfo.GetMoneyID()))
                {
                    return;
                }

                ProcessSuccessOrder(isMoneyIn, tpGameMoneyInfo, remoteUserScore);
            }
            else
            {
                //確定第三方有回訂單狀態為失敗,所以要退款
                ProcessFailOrder(isMoneyIn, tpGameMoneyInfo, orderReturnModel.Message);
            }
        }

        private void ProcessSuccessOrder(bool isMoneyIn, BaseTPGameMoneyInfo tpGameMoneyInfo, UserScore remoteUserScore)
        {
            UserScore userScoreParam = remoteUserScore;

            //如果建單後有回傳餘額,以建單後的資料為主,若沒有則反查
            if (userScoreParam == null)
            {
                //取得第三方餘額                     
                BaseReturnDataModel<UserScore> userScoreReturnModel = GetRemoteUserScore(tpGameMoneyInfo.UserID, true);

                if (userScoreReturnModel.IsSuccess &&
                userScoreReturnModel.DataModel != null)
                {
                    userScoreParam = userScoreReturnModel.DataModel;
                }
            }

            if (userScoreParam != null)
            {
                //call 本地轉帳完成sp
                if (_tpGameStoredProcedureRep.DoTransferSuccess(isMoneyIn, tpGameMoneyInfo.GetMoneyID(), userScoreParam))
                {
                    string summary;

                    if (isMoneyIn)
                    {
                        summary = string.Format(MessageElement.YouTransferInSuccessfully,
                            tpGameMoneyInfo.Amount, Product.Name);
                    }
                    else
                    {
                        summary = string.Format(MessageElement.YouTransferOutSuccessfully,
                           Product.Name, tpGameMoneyInfo.Amount);
                    }

                    ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
                    {
                        _tpGameMqService.SendTransferMessage(tpGameMoneyInfo.UserID, tpGameMoneyInfo.Amount, summary, _delaySendSecond);
                    });
                }
            }
        }

        private void ProcessFailOrder(bool isMoneyIn, BaseTPGameMoneyInfo tpGameMoneyInfo, string errorMsg)
        {
            //call 本地轉帳失敗sp
            if (_tpGameStoredProcedureRep.DoTransferRollback(isMoneyIn, tpGameMoneyInfo.GetMoneyID(), errorMsg))
            {
                string summary;

                if (isMoneyIn)
                {
                    summary = string.Format(MessageElement.YouTransferInFailly,
                        tpGameMoneyInfo.Amount, Product.Name);
                }
                else
                {
                    summary = string.Format(MessageElement.YouTransferOutFailly,
                       Product.Name, tpGameMoneyInfo.Amount);
                }

                ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
                {
                    _tpGameMqService.SendTransferMessage(tpGameMoneyInfo.UserID, tpGameMoneyInfo.Amount, summary);
                });
            }
        }

        private BaseReturnModel GetRemoteOrderStatus(BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            return GetJobResultWithRetry(() =>
            {
                string apiResult = GetRemoteOrderApiResult(GetTPGameAccount(tpGameMoneyInfo.UserID), tpGameMoneyInfo);
                BaseReturnModel orderReturnModel = GetQueryOrderReturnModel(apiResult);

                if (orderReturnModel == null || !orderReturnModel.IsSuccess)
                {
                    LogUtil.ForcedDebug($"遠端確認{GetActionName(tpGameMoneyInfo is TPGameMoneyInInfo)}完成, 訂單結果為非成功狀態. apiResult={apiResult}");
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

        private void SendRefreshUserInfoMqMessage(List<int> userIds)
        {
            foreach (int userId in userIds)
            {
                ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
                {
                    _tpGameMqService.SendRefreshUserInfoMessage(userId);
                });

                Thread.Sleep(500);
            }
        }

        protected string GetFullUrl(string rootUrl, string relativeUrl)
        {
            return new Uri(new Uri(rootUrl), relativeUrl).ToString();
        }

        protected string GetCombineUrl(params string[] urls)
        {
            return string.Join("/", urls.Select(s => s.TrimStart("/").TrimEnd("/")));
        }

        protected BaseReturnDataModel<RequestAndResponse> GetRemoteFileBetLogResult(string lastSearchToken)
        {
            long currentFileUnixOfTime = long.Parse(lastSearchToken);
            FtpLoginParam ftpLoginParam = FtpSharedSettings.FtpLoginParam;

            // 先取remote檔案路徑
            string dateFolderName = GetRemoteFolderName();
            string remotePath = $"{ftpLoginParam.FtpRemoteFilePath}/{Product.Value}/{dateFolderName}";

            // 取remote檔案list
            List<FtpFileInfo> ftpFileInfos = FtpUtil.GetFileList(ftpLoginParam, remotePath)
                .Where(w => w.FileType == FtpFileSystemObjectType.File)
                .OrderBy(o => long.Parse(o.FileNameWithoutExtension))
                .ToList();

            string downloadFileName = string.Empty;

            // 取未處理最舊一筆檔案回來
            foreach (FtpFileInfo ftpFileInfo in ftpFileInfos)
            {
                long downloadFileUnixOfTime = long.Parse(ftpFileInfo.FileNameWithoutExtension);

                if (downloadFileUnixOfTime > currentFileUnixOfTime)
                {
                    downloadFileName = ftpFileInfo.FileNameWithoutExtension;
                    break;
                }
            }

            if (downloadFileName.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<RequestAndResponse>(ReturnCode.NoDataChanged);
            }

            string remoteFilePath = $"{remotePath}/{downloadFileName}.json";
            string saveLocalFilePath = $"{FtpSharedSettings.SaveLocalFilePath}/{Product.Value}/{dateFolderName}/{downloadFileName}.json";

            bool isDownloadSuccess = FtpUtil.DownloadFile(ftpLoginParam, saveLocalFilePath, remoteFilePath);

            if (isDownloadSuccess)
            {
                // 讀檔回來處理
                string betLogContent = File.ReadAllText(saveLocalFilePath, Encoding.UTF8);
                // 做完將資料進行刪除
                File.Delete(saveLocalFilePath);
                return betLogContent.Deserialize<BaseReturnDataModel<RequestAndResponse>>();
            }

            return new BaseReturnDataModel<RequestAndResponse>($"{MessageElement.FtpDownloadFileNotSuccessfully}，info={remotePath}");
        }

        private string GetRemoteFolderName()
        {
            DateTime nowDateTime = DateTime.Now;
            DateTime earlyDateTime = nowDateTime.AddMinutes(_fetchFileIntervalMinutes * (-1));

            if (earlyDateTime.ToFormatYearMonthDateValue() == nowDateTime.ToFormatYearMonthDateValue())
            {
                return nowDateTime.ToFormatYearMonthDateValue();
            }

            return earlyDateTime.ToFormatYearMonthDateValue();
        }
    }
}
