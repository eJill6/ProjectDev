using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class TPGameTransferOutService : BaseService, ITPGameTransferOutService
    {
        private static readonly int s_transferOutAllCacheSeconds = 600;

        private readonly IPlatformProductService _platformProductService;

        private readonly ITPGameAccountReadService _tpGameAccountReadService;

        private readonly IJxCacheService _jxCacheService;

        private readonly IMessageQueueService _messageQueueService;

        public TPGameTransferOutService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(EnvLoginUser.Application, Merchant);
            _tpGameAccountReadService = ResolveJxBackendService<ITPGameAccountReadService>(Merchant, DbConnectionTypes.Slave);
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
            _messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(EnvLoginUser.Application);
        }

        /// <summary> 單一轉出產品 </summary>
        public BaseReturnModel SingleProduct(PlatformProduct product, decimal amount, bool isSynchronizing, string correlationId)
        {
            ITPGameApiService gameApiService = ResolveJxBackendService<ITPGameApiService>(product, DbConnectionType);

            return gameApiService.CreateTransferOutInfo(
                new TPGameTranfserParam()
                {
                    UserID = EnvLoginUser.LoginUser.UserId,
                    Amount = amount.Floor(0), // 單一轉出只能整數
                    IsSynchronizing = isSynchronizing,
                    CorrelationId = correlationId
                },
                isTransferOutAll: false,
                out decimal actuallyAmount,
                out string moneyId);
        }

        /// <summary> 轉回指定第三方所有餘額 </summary>
        public BaseReturnModel AllAmountBySingleProduct(PlatformProduct product, bool isSynchronizing, IInvocationParam invocationParam)
        {
            return AllAmountBySingleProduct(product, isSynchronizing, invocationParam, out decimal actuallyAmount, out string moneyId);
        }

        /// <summary> 轉回指定第三方所有餘額，out moneyId </summary>
        public BaseReturnModel AllAmountBySingleProduct(PlatformProduct product, bool isSynchronizing, IInvocationParam invocationParam, out decimal actuallyAmount, out string moneyId)
        {
            ITPGameApiService gameApiService = ResolveJxBackendService<ITPGameApiService>(product, DbConnectionType);

            var invocationUserParam = new InvocationUserParam()
            {
                UserID = EnvLoginUser.LoginUser.UserId,
                CorrelationId = invocationParam.CorrelationId,
            };

            BaseReturnDataModel<UserScore> userScoreResult = gameApiService.GetRemoteUserScore(invocationUserParam, isRetry: false);

            moneyId = null;
            actuallyAmount = 0;

            if (!userScoreResult.IsSuccess)
            {
                return new BaseReturnModel(userScoreResult.Message);
            }

            decimal amount = userScoreResult.DataModel.AvailableScores;

            if (amount > GlobalVariables.TPTransferAmountBound.MaxTPGameTransferAmount)
            {
                amount = GlobalVariables.TPTransferAmountBound.MaxTPGameTransferAmount;
            }

            return gameApiService.CreateTransferOutInfo(
                new TPGameTranfserParam()
                {
                    UserID = invocationUserParam.UserID,
                    Amount = amount.Floor(0),
                    IsSynchronizing = isSynchronizing,
                    CorrelationId = invocationUserParam.CorrelationId,
                },
                isTransferOutAll: false,
                out actuallyAmount,
                out moneyId);
        }

        public BaseReturnModel AllAmountByAllProducts(IInvocationUserParam invocationUserParam, bool isOperateByBackSide)
        {
            //判斷前一次的一鍵轉回處理完了沒
            CacheKey transferOutAllKey = CacheKey.TransferOutAllAmount(invocationUserParam.UserID);
            var transferResultMap = _jxCacheService.GetCache<Dictionary<string, BaseReturnDataModel<decimal>>>(transferOutAllKey); //{productCode, 執行結果}

            if (transferResultMap != null)
            {
                return new BaseReturnModel(ReturnCode.TryTooOften);
            }

            var searchAllGameUserScoresParam = new SearchAllGameUserScoresParam()
            {
                UserID = invocationUserParam.UserID,
                IsForcedRefresh = false,
                IsIncludeMainScores = false
            };

            List<UserProductScore> userProductScores = _tpGameAccountReadService.GetAllTPGameUserScores(searchAllGameUserScoresParam);

            // 曾轉帳過的帳號
            userProductScores = userProductScores.Where(w => w.TransferIn > 0).ToList();
            transferResultMap = userProductScores.ToDictionary(d => d.ProductCode, d => default(BaseReturnDataModel<decimal>));

            if (!transferResultMap.Any())
            {
                return new BaseReturnModel(MessageElement.NoMoneyTransferToPlatform);
            }

            _jxCacheService.SetCache(
                new SetCacheParam()
                {
                    Key = transferOutAllKey,
                    CacheSeconds = s_transferOutAllCacheSeconds
                }
                , transferResultMap);

            foreach (UserProductScore userProductScore in userProductScores)
            {
                PlatformProduct product = _platformProductService.GetSingle(userProductScore.ProductCode);

                var transferOutUserDetail = new TransferOutUserDetail()
                {
                    AffectedUser = new BaseBasicUserInfo() { UserId = invocationUserParam.UserID },
                    IsOperateByBackSide = isOperateByBackSide,
                    CorrelationId = invocationUserParam.CorrelationId
                };

                //放入queue做非同步處理
                _messageQueueService.EnqueueTransferAllOutMessage(product, transferOutUserDetail);
            }

            return new BaseReturnModel(ReturnCode.TransferOutSubmit);
        }

        public Dictionary<string, BaseReturnDataModel<decimal>> UpdateTransferAllResult(int userId, PlatformProduct product, BaseReturnDataModel<decimal> transferResult)
        {
            CacheKey transferOutAllKey = CacheKey.TransferOutAllAmount(userId);

            Dictionary<string, BaseReturnDataModel<decimal>> returnResult = null;

            _jxCacheService.DoWorkWithRemoteLock(
                transferOutAllKey,
                () =>
                {
                    var transferResultMap = _jxCacheService.GetCache<Dictionary<string, BaseReturnDataModel<decimal>>>(transferOutAllKey); //{productCode, 執行結果}

                    if (transferResultMap == null)
                    {
                        return;
                    }

                    transferResultMap.TryGetValue(product.Value, out BaseReturnDataModel<decimal> productTransferResult);

                    if (productTransferResult == null)
                    {
                        productTransferResult = transferResult;
                    }
                    else if (transferResult.IsSuccess)
                    {
                        // 分批轉餘額回平台餘額需進行累加
                        productTransferResult.DataModel += transferResult.DataModel;
                    }

                    // 只記錄最後一次成功轉回餘額結果
                    productTransferResult.IsSuccess = transferResult.IsSuccess;

                    transferResultMap[product.Value] = productTransferResult;

                    _jxCacheService.SetCache(
                        new SetCacheParam()
                        {
                            Key = transferOutAllKey,
                            CacheSeconds = s_transferOutAllCacheSeconds,
                        },
                        transferResultMap);

                    returnResult = transferResultMap;
                });

            return returnResult;
        }

        public void RemoveTransferAllResult(int userId)
        {
            CacheKey transferOutAllKey = CacheKey.TransferOutAllAmount(userId);
            _jxCacheService.RemoveCache(transferOutAllKey);
        }

        public bool ProcessTransferAllOutQueue(PlatformProduct product, TransferOutUserDetail transferOutUserDetail)
        {
            if (transferOutUserDetail == null)
            {
                return false;
            }

            ITPGameApiService gameApiService = ResolveJxBackendService<ITPGameApiService>(product, DbConnectionType);

            BaseReturnDataModel<decimal> transferResult = gameApiService.CreateAllAmountTransferOutInfo(
                new TPGameTranfserParam()
                {
                    UserID = transferOutUserDetail.AffectedUser.UserId,
                    IsSynchronizing = true,
                    IsOperateByBackSide = transferOutUserDetail.IsOperateByBackSide,
                    CorrelationId = transferOutUserDetail.CorrelationId
                },
                out bool isAllAmount);

            //往MQ丟統一由BatchService做轉回MiseLive
            if (transferResult.IsSuccess && transferResult.DataModel > 0)
            {
                _messageQueueService.EnqueueTransferToMiseLiveMessage(
                    new TransferToMiseLiveParam()
                    {
                        UserID = transferOutUserDetail.AffectedUser.UserId,
                        Amount = transferResult.DataModel
                    });
            }

            // 若是補償機制的轉帳流程就不往下進行更新redis 內容
            if (transferOutUserDetail.IsCompensation)
            {
                return true;
            }

            //回壓 成功或失敗結果到redis
            Dictionary<string, BaseReturnDataModel<decimal>> transferResultMap = UpdateTransferAllResult(transferOutUserDetail.AffectedUser.UserId, product, transferResult);

            //如果全部完成, 移除redis資料
            if (isAllAmount && transferResultMap != null && transferResultMap.Where(w => w.Value == null).Count() == 0)
            {
                TransferAllJobComplete(transferOutUserDetail.AffectedUser.UserId, transferResultMap);
            }

            return true;
        }

        private void TransferAllJobComplete(int userId, Dictionary<string, BaseReturnDataModel<decimal>> transferResultMap)
        {
            IEnumerable<KeyValuePair<string, BaseReturnDataModel<decimal>>> fails = transferResultMap.Where(w => !w.Value.IsSuccess);

            if (fails.Any())
            {
                LogUtil.ForcedDebug(fails.ToJsonString());
            }

            RemoveTransferAllResult(userId);
        }
    }
}