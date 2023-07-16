using System;
using System.Collections.Generic;
using System.Linq;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;

namespace JxBackendService.Service
{
    public class TransferCompensationService : BaseService, ITransferCompensationService
    {
        private readonly ITransferCompensationRep _transferCompensationRep;

        private readonly IUserInfoAdditionalService _userInfoAdditionalService;

        private readonly IJxCacheService _jxCacheService;

        private readonly double _createTransferMinsBefore = 30;

        private readonly double _scheduleSearchTransferMinsBefore = 5;

        public TransferCompensationService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _transferCompensationRep = ResolveJxBackendService<ITransferCompensationRep>();
            _userInfoAdditionalService = ResolveJxBackendService<IUserInfoAdditionalService>();
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
        }

        public BaseReturnModel SaveMoneyOutCompensation(SaveCompensationParam param)
        {
            UserInfoAdditional userInfoAdditional = _userInfoAdditionalService.GetSingle(param.UserID);

            if (userInfoAdditional == null)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            // 最後自動轉帳ProductCode
            string lastAutoTransferProductCode = userInfoAdditional.GetUserTransferSetting().LastAutoTransProductCode.ToTrimString();

            if (param.ProductCode != lastAutoTransferProductCode)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            // 是否已經建立在30分鐘內的補償單，有就不再建立
            bool hasCompensation = _transferCompensationRep.HasUnProcessedCompensation(
                new SearchUserCompensationParam
                {
                    UserID = param.UserID,
                    ProductCode = param.ProductCode,
                    Type = CompensationType.TransferMoneyOut.Value,
                    StartDate = DateTime.Now.AddMinutes(-_createTransferMinsBefore),
                    EndDate = DateTime.Now
                });

            if (hasCompensation)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            var createCompensation = param.CastByJson<TransferCompensation>();
            createCompensation.Type = CompensationType.TransferMoneyOut.Value;

            return _transferCompensationRep.CreateByProcedure(createCompensation).CastByJson<BaseReturnModel>();
        }

        public BaseReturnModel ProcessedMoneyOutCompensation(ProcessedCompensationParam param)
        {
            List<TransferCompensation> moneyOutCompensations = _transferCompensationRep.GetUserUnProcessedCompensations(
                new SearchUserCompensationParam
                {
                    UserID = param.UserID,
                    ProductCode = param.ProductCode,
                    Type = CompensationType.TransferMoneyOut.Value,
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now,
                });

            foreach (TransferCompensation compensation in moneyOutCompensations)
            {
                compensation.IsProcessed = true;
                _transferCompensationRep.UpdateByProcedure(compensation);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        public List<int> GetTransferCompensationUserIds(PlatformProduct product)
        {
            DateTime startDate = DateTime.Now.AddMinutes(-_createTransferMinsBefore);
            DateTime dateTime = DateTime.Now.AddMinutes(-_scheduleSearchTransferMinsBefore);

            // 先取得目前未處理的補償單
            List<TransferCompensation> moneyOutCompensations = _transferCompensationRep.GetUnProcessedCompensations(
                new SearchProductCompensationParam
                {
                    ProductCode = product.Value,
                    Type = CompensationType.TransferMoneyOut.Value,
                    StartDate = DateTime.Now.AddMinutes(-_createTransferMinsBefore),
                    EndDate = DateTime.Now.AddMinutes(-_scheduleSearchTransferMinsBefore)
                });

            if (!moneyOutCompensations.Any())
            {
                return new List<int>();
            }

            List<int> userIds = moneyOutCompensations.Select(s => s.UserID).Distinct().ToList();
            HashSet<int> userIdMap = userIds.ConvertToHashSet();

            DateTime minCreateDate = moneyOutCompensations.Select(s => s.CreateDate).Min();

            // 取得用戶這期間有transferOut成功的訂單
            var tpGameStoredProcedureRep = ResolveJxBackendService<ITPGameStoredProcedureRep>(product);
            List<TPGameMoneyOutInfo> processedMoneyOut = tpGameStoredProcedureRep.GetTPGameProcessedMoneyOutInfo(minCreateDate, DateTime.Now, userIds);

            List<int> moneyOutUserIds = (from moc in moneyOutCompensations
                                         join pmo in processedMoneyOut on moc.UserID equals pmo.UserID
                                         where pmo.HandTime > moc.CreateDate
                                         select pmo.UserID).Distinct().ToList();

            // 若有moneyOut成功的userId，就從補償機制userId移除
            userIdMap.ExceptWith(moneyOutUserIds);

            foreach (int userId in userIds)
            {
                CacheKey transferOutAllKey = CacheKey.TransferOutAllAmount(userId);
                var transferResultMap = _jxCacheService.GetCache<Dictionary<string, BaseReturnDataModel<decimal>>>(transferOutAllKey);

                if (transferResultMap != null)
                {
                    // 若有申請過一鍵轉回的userId，就從補償機制userId移除
                    userIdMap.Remove(userId);
                }
            }

            return userIdMap.Select(s => s).ToList();
        }
    }
}