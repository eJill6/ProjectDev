using Azure.Messaging;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.User;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class TPGameTransferOutService : BaseService, ITPGameTransferOutService
    {
        private static readonly int s_transferOutAllCacheSeconds = 600;

        private static readonly int s_transferOutBySingleProductCacheSeconds = 12;

        private readonly IPlatformProductService _platformProductService;

        private readonly ITPGameAccountReadService _tpGameAccountReadService;

        private readonly IJxCacheService _jxCacheService;

        private readonly IMessageQueueService _messageQueueService;

        private readonly ILogUtilService _logUtilService;

        private readonly IUserInfoRelatedService _userInfoRelatedService;

        private readonly IUserInfoRelatedReadService _userInfoRelatedReadService;

        public TPGameTransferOutService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(EnvLoginUser.Application, Merchant);
            _tpGameAccountReadService = ResolveJxBackendService<ITPGameAccountReadService>(Merchant, DbConnectionTypes.Slave);
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
            _messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(EnvLoginUser.Application);
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            _userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(EnvLoginUser, DbConnectionTypes.Master);
            _userInfoRelatedReadService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedReadService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        /// <summary> 單一轉出產品 </summary>
        public BaseReturnModel SingleProduct(PlatformProduct product, string correlationId)
        {
            ITPGameApiService gameApiService = ResolveJxBackendService<ITPGameApiService>(product, DbConnectionType);

            return gameApiService.CreateTransferOutInfo(
                new TPGameTranfserOutParam()
                {
                    UserID = EnvLoginUser.LoginUser.UserId,
                    CorrelationId = correlationId
                },
                isTransferOutAll: false,
                out decimal actuallyAmount,
                out string moneyId);
        }

        /// <summary> 轉回指定第三方所有餘額 </summary>
        public BaseReturnModel AllAmountBySingleProduct(PlatformProduct product, IInvocationParam invocationParam)
        {
            return AllAmountBySingleProduct(product, invocationParam, out decimal actuallyAmount, out string moneyId);
        }

        /// <summary> 轉回指定第三方所有餘額，out moneyId </summary>
        public BaseReturnModel AllAmountBySingleProduct(PlatformProduct product, IInvocationParam invocationParam, out decimal actuallyAmount, out string moneyId)
        {
            ITPGameApiService gameApiService = ResolveJxBackendService<ITPGameApiService>(product, DbConnectionType);

            var invocationUserParam = new InvocationUserParam()
            {
                UserID = EnvLoginUser.LoginUser.UserId,
                CorrelationId = invocationParam.CorrelationId,
            };

            moneyId = null;
            actuallyAmount = 0;

            return gameApiService.CreateTransferOutInfo(
                new TPGameTranfserOutParam()
                {
                    UserID = invocationUserParam.UserID,
                    CorrelationId = invocationUserParam.CorrelationId,
                },
                isTransferOutAll: false,
                out actuallyAmount,
                out moneyId);
        }

        public BaseReturnModel AllAmountByAllProducts(IInvocationUserParam invocationUserParam)
        {
            return DoAllAmountByAllProducts(
                invocationUserParam,
                routingSetting: null,
                (userHasTransferInScores, userHasNoTransferInScores) =>
                {
                    if (!userHasTransferInScores.Any())
                    {
                        return new BaseReturnModel(MessageElement.NoMoneyTransferToPlatform);
                    }

                    return null;
                });
        }

        public BaseReturnModel AllAmountByAllProductsWithMQToClient(IInvocationUserParam invocationUserParam, RoutingSetting routingSetting)
        {
            return DoAllAmountByAllProducts(
                invocationUserParam,
                routingSetting,
                (userHasTransferInScores, userHasNoTransferInScores) =>
                {
                    //沒有轉帳過的也要推播結果
                    if (userHasNoTransferInScores.Any())
                    {
                        foreach (PlatformProduct product in userHasNoTransferInScores.Select(s => _platformProductService.GetSingle(s.ProductCode)).Where(w => w != null))
                        {
                            SendProductHasNoTransferInMessage(product, routingSetting);
                        }
                    }

                    if (!userHasTransferInScores.Any())
                    {
                        //回傳成功讓前端不顯示錯誤提示
                        return new BaseReturnModel(ReturnCode.Success);
                    }

                    return null;
                });
        }

        private BaseReturnModel DoAllAmountByAllProducts(
            IInvocationUserParam invocationUserParam,
            RoutingSetting routingSetting,
            Func<List<UserProductScore>, List<UserProductScore>, BaseReturnModel> doErrorHandle)
        {
            BaseReturnModel checkResult = IsRateLimitValid(invocationUserParam.UserID, product: null);

            if (!checkResult.IsSuccess)
            {
                return checkResult;
            }

            var searchAllGameUserScoresParam = new SearchAllGameUserScoresParam()
            {
                UserID = invocationUserParam.UserID,
                IsForcedRefresh = false,
                IsIncludeMainScores = false
            };

            List<UserProductScore> userProductAllScores = _tpGameAccountReadService.GetAllTPGameUserScores(searchAllGameUserScoresParam);
            var userHasTransferInScores = new List<UserProductScore>(); // 曾轉帳過的帳號
            var userHasNoTransferInScores = new List<UserProductScore>(); // 未曾轉帳過的帳號

            foreach (UserProductScore userProductScore in userProductAllScores)
            {
                if (userProductScore.TransferIn > 0)
                {
                    userHasTransferInScores.Add(userProductScore);
                }
                else
                {
                    userHasNoTransferInScores.Add(userProductScore);
                }
            }

            BaseReturnModel returnModel = doErrorHandle.Invoke(userHasTransferInScores, userHasNoTransferInScores);

            if (returnModel != null)
            {
                return returnModel;
            }

            Dictionary<string, BaseReturnDataModel<decimal>> transferResultMap = userHasTransferInScores
               .ToDictionary(d => d.ProductCode, d => default(BaseReturnDataModel<decimal>));

            _jxCacheService.SetCache(
                new SetCacheParam()
                {
                    Key = CacheKey.TransferOutAllAmount(invocationUserParam.UserID),
                    CacheSeconds = s_transferOutAllCacheSeconds
                }, transferResultMap);

            EnqueueTransferAllOutMessage(
                invocationUserParam,
                userHasTransferInScores.Select(s => _platformProductService.GetSingle(s.ProductCode)).ToList(),
                routingSetting);

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel AllAmountBySingleProductQueue(IInvocationUserParam invocationUserParam, PlatformProduct product, RoutingSetting routingSetting)
        {
            BaseReturnModel checkResult = IsRateLimitValid(invocationUserParam.UserID, product);

            if (!checkResult.IsSuccess)
            {
                return checkResult;
            }

            CacheKey cacheKey = CacheKey.TransferOutBySingleProduct(invocationUserParam.UserID, product);

            _jxCacheService.SetCache(
                new SetCacheParam()
                {
                    Key = cacheKey,
                    CacheSeconds = s_transferOutBySingleProductCacheSeconds
                }, product.Value);

            var tpGameUserInfoService = ResolveJxBackendService<ITPGameUserInfoService>(product);
            BaseTPGameUserInfo tpGameUserInfo = tpGameUserInfoService.GetTPGameUserInfo(invocationUserParam.UserID);

            if (tpGameUserInfo == null || tpGameUserInfo.TransferIn == 0)
            {
                SendProductHasNoTransferInMessage(product, routingSetting);

                return new BaseReturnModel(ReturnCode.Success);
            }

            EnqueueTransferAllOutMessage(
                invocationUserParam,
                new List<PlatformProduct>() { product },
                routingSetting);

            return new BaseReturnModel(ReturnCode.Success);
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
                        }, transferResultMap);

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
                new TPGameTranfserOutParam()
                {
                    UserID = transferOutUserDetail.AffectedUser.UserId,
                    CorrelationId = transferOutUserDetail.CorrelationId
                }, out bool isAllAmount);

            if (transferResult.IsSuccess && isAllAmount)
            {
                ClearLastAutoTransInfo(transferOutUserDetail.AffectedUser.UserId, product);
            }

            //往MQ丟統一由BatchService做轉回MiseLive
            if (transferResult.IsSuccess && transferResult.DataModel > 0)
            {
                _messageQueueService.EnqueueTransferToMiseLiveMessage(
                    new TransferToMiseLiveParam()
                    {
                        UserID = transferOutUserDetail.AffectedUser.UserId,
                        Amount = transferResult.DataModel,
                        ProductCode = product.Value,
                        RoutingSetting = transferOutUserDetail.RoutingSetting
                    });
            }

            SendTransferMsgFromThirdPartyToRelayWallet(product, transferOutUserDetail.RoutingSetting, transferResult.Message);

            // 若是補償機制的轉帳流程就不往下進行更新redis 內容
            if (transferOutUserDetail.IsCompensation)
            {
                return true;
            }

            //回壓 成功或失敗結果到redis
            Dictionary<string, BaseReturnDataModel<decimal>> transferResultMap = UpdateTransferAllResult(
                transferOutUserDetail.AffectedUser.UserId,
                product,
                transferResult);

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
                _logUtilService.ForcedDebug(fails.ToJsonString());
            }

            RemoveTransferAllResult(userId);
        }

        private BaseReturnModel IsRateLimitValid(int userId, PlatformProduct product)
        {
            CacheKey cacheKey;

            if (product == null)
            {
                //全部轉回，判斷前一次的一鍵轉回處理完了沒
                cacheKey = CacheKey.TransferOutAllAmount(userId);
            }
            else
            {
                cacheKey = CacheKey.TransferOutBySingleProduct(userId, product);
            }

            object cacheObject = _jxCacheService.GetCache<object>(cacheKey);

            if (cacheObject != null)
            {
                return new BaseReturnModel(ReturnCode.TryTooOften);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        private void EnqueueTransferAllOutMessage(IInvocationUserParam invocationUserParam, List<PlatformProduct> products, RoutingSetting routingSetting)
        {
            foreach (PlatformProduct product in products)
            {
                if (product == null)
                {
                    continue;
                }

                var transferOutUserDetail = new TransferOutUserDetail()
                {
                    AffectedUser = new BaseBasicUserInfo() { UserId = invocationUserParam.UserID },
                    CorrelationId = invocationUserParam.CorrelationId,
                    RoutingSetting = routingSetting
                };

                //放入queue做非同步處理
                _messageQueueService.EnqueueTransferAllOutMessage(product, transferOutUserDetail);
            }
        }

        private void ClearLastAutoTransInfo(int userId, PlatformProduct product)
        {
            UserInfoAdditional userInfoAdditional = _userInfoRelatedReadService.GetUserInfoAdditional(userId);
            UserTransferSetting setting = userInfoAdditional.GetUserTransferSetting();

            if (setting == null || setting.LastAutoTransProductCode != product)
            {
                return;
            }

            _userInfoRelatedService.UpdateLastAutoTransInfo(userId, productCode: null);
        }

        private void SendProductHasNoTransferInMessage(PlatformProduct product, RoutingSetting routingSetting)
        {
            SendTransferMsgFromThirdPartyToRelayWallet(product, routingSetting, MessageElement.BalanceIsZero);
        }

        private void SendTransferMsgFromThirdPartyToRelayWallet(PlatformProduct product, RoutingSetting routingSetting, string messageContent)
        {
            //若有RoutingKey才要發mq
            if (routingSetting == null || routingSetting.RoutingKey.IsNullOrEmpty())
            {
                return;
            }

            string message = string.Format(MessageElement.ThirdPartyToRelayWallet, messageContent);

            var transferMessage = new TransferMessage()
            {
                RoutingKey = routingSetting.RoutingKey,
                ProductCode = product.Value,
                RequestId = routingSetting.RequestId,
                Summary = message
            };

            _messageQueueService.SendTransferMessage(transferMessage);
        }
    }
}