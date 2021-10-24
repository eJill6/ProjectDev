using System;
using System.Collections.Generic;
using System.Linq;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using JxBackendService.Service.User;

namespace JxBackendService.Service.Game
{
    public class GameCommissionRuleInfoService : BaseService, IGameCommissionRuleInfoService
    {
        private static readonly decimal _commissionRangeMin = 0;
        private static readonly decimal _commissionRangeMax = 9999999999;
        private static readonly int _maxCommissionRuleCount = 7;
        private static readonly int _platformUserId = 1;
        private readonly IOperationLogService _operationLogService;
        private readonly IUserInfoRelatedReadService _userinfoRelatedReadService;
        private readonly IPlatformProductService _platformProductService;

        public GameCommissionRuleInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _operationLogService = ResolveJxBackendService<IOperationLogService>();
            _userinfoRelatedReadService = ResolveJxBackendService<IUserInfoRelatedReadService>();
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
        }

        public List<GameCommissionRuleInfo> GetAllGameCommissionRuleInfos(int userId)
        {
            List<GameCommissionRuleInfo> result = new List<GameCommissionRuleInfo>();

            foreach (var commissionGroupType in CommissionGroupType.GetAll())
            {
                var subResult = GetGameCommissionRuleInfos(commissionGroupType, userId);
                subResult.ForEach(x => x.CommissionGroupType = commissionGroupType.Value);
                result.AddRange(subResult);
            }

            return result;
        }

        public List<GameCommissionRuleInfo> GetGameCommissionRuleInfos(CommissionGroupType commissionGroupType, int userId)
        {
            if (commissionGroupType == null)
            {
                return GetAllGameCommissionRuleInfos(userId);
            }
            else
            {
                return CreateGameCommissionRuleRep(commissionGroupType).GetGameCommissionRuleInfos(userId);
            }
        }

        /// <summary>
        /// 分紅設置區間儲存
        /// </summary>
        /// <param name="commissionGroupType"></param>
        /// <param name="saveCommissionRuleInfos"></param>
        /// <returns></returns>
        public BaseReturnModel SaveRuleInfoForPeriodCommission(CommissionGroupType commissionGroupType, List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            return SaveRuleInfo(commissionGroupType, saveCommissionRuleInfos, false);
        }

        /// <summary>
        /// 固定比例分紅設置儲存
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="commissionGroupType"></param>
        /// <param name="commissionPercent"></param>
        /// <returns></returns>
        public BaseReturnModel SaveRuleInfoFixedContractForFrontUser(int userId, CommissionGroupType commissionGroupType, double commissionPercent)
        {
            // 後台不允許走固定比例儲存分紅契約
            if (EnvLoginUser.Application == JxApplication.BackSideWeb)
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            // 前台 & API 固定比例
            // 查詢出原用戶分紅設置
            List<SaveCommissionRuleInfo> saveCommissionRuleInfos = GetFixedContractForFrontUser(userId, commissionGroupType, commissionPercent);

            return SaveRuleInfo(commissionGroupType, saveCommissionRuleInfos, true);
        }

        /// <summary>
        /// 固定比例分紅設置規格檢查
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="commissionGroupType"></param>
        /// <param name="commissionPercent"></param>
        /// <returns></returns>
        public BaseReturnModel CheckRuleInfoFixedContractForFrontUser(int userId, CommissionGroupType commissionGroupType, double commissionPercent)
        {
            List<SaveCommissionRuleInfo> saveCommissionRuleInfos = GetFixedContractForFrontUser(userId, commissionGroupType, commissionPercent);

            return CheckRuleInfo(commissionGroupType, saveCommissionRuleInfos, true);
        }

        /// <summary>
        /// 區間分紅設置規格檢查
        /// </summary>
        /// <param name="commissionGroupType"></param>
        /// <param name="saveCommissionRuleInfos"></param>
        /// <returns></returns>
        public BaseReturnModel CheckRuleInfoForPeriodCommission(CommissionGroupType commissionGroupType, List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            return CheckRuleInfo(commissionGroupType, saveCommissionRuleInfos, false);
        }

        public List<string> GetAvailableRuleInfoTableName()
        {
            List<CommissionGroupType> commissionGroupTypes = CommissionTypes.GetAll()
                .Where(w => w.Product != null && _platformProductService.GetAll().Any(p => p.Value == w.Product.Value))
                .Select(s => s.CommissionGroupType)
                .ToList();

            return commissionGroupTypes.Select(s => GetRuleInfoTableName(s)).Distinct().ToList();
        }

        private string GetRuleInfoTableName(CommissionGroupType commissionGroupType)
        {
            return CreateGameCommissionRuleRep(commissionGroupType).GetRuleInfoTableName();
        }

        private List<SaveCommissionRuleInfo> GetFixedContractForFrontUser(int userId, CommissionGroupType commissionGroupType, double commissionPercent)
        {
            // 前台 & API 固定比例
            // 查詢出原用戶分紅設置
            List<GameCommissionRuleInfo> originalData = GetGameCommissionRuleInfos(commissionGroupType, userId);
            var saveCommissionRuleInfos = new List<SaveCommissionRuleInfo>();

            if (originalData.AnyAndNotNull())
            {
                // 將目前所有區間分紅比例都更新
                originalData.ForEach(f =>
                {
                    f.CommissionPercent = commissionPercent;
                });
                saveCommissionRuleInfos = originalData.CastByJson<List<SaveCommissionRuleInfo>>();
            }
            else
            {
                // 都沒有設置過分紅契約，預設一筆 0~9999999999
                string userName = _userinfoRelatedReadService.GetUserName(userId);
                saveCommissionRuleInfos.Add(new SaveCommissionRuleInfo
                {
                    UserID = userId,
                    UserName = userName,
                    MinProfitLossRange = _commissionRangeMin,
                    MaxProfitLossRange = _commissionRangeMax,
                    CommissionPercent = commissionPercent,
                    Visible = true,
                });
            }

            return saveCommissionRuleInfos;
        }

        /// <summary>
        /// 儲存分紅資料
        /// </summary>
        /// <param name="commissionGroupType">分紅契約分類</param>
        /// <param name="saveCommissionRuleInfos">儲存的分紅條件資料</param>
        /// <param name="isFixedCommissionPercent">是否強制為固定比例</param>
        /// <returns></returns>        
        private BaseReturnModel SaveRuleInfo(CommissionGroupType commissionGroupType, List<SaveCommissionRuleInfo> saveCommissionRuleInfos,
            bool isFixedCommissionPercent)
        {
            BaseReturnModel returnModel;

            if (saveCommissionRuleInfos == null)
            {
                return new BaseReturnModel(CommissionElement.CommissionRuleInfoIsNull);
            }

            #region 強制最後一筆設定為最大值為 9999999999
            if (saveCommissionRuleInfos.Any() && saveCommissionRuleInfos.Last().MaxProfitLossRange != _commissionRangeMax)
            {
                saveCommissionRuleInfos.Last().MaxProfitLossRange = _commissionRangeMax;
            }
            #endregion

            #region 檢查所有規則

            returnModel = CheckRuleInfo(commissionGroupType, saveCommissionRuleInfos, isFixedCommissionPercent);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }

            #endregion


            List<GameCommissionRuleInfo> originalData = GetGameCommissionRuleInfos(commissionGroupType, saveCommissionRuleInfos.First().UserID);

            BaseReturnModel result = CreateGameCommissionRuleRep(commissionGroupType).SaveRuleInfo(saveCommissionRuleInfos);

            if (result.IsSuccess)
            {
                //log
                SaveRuleInfoLog(originalData, saveCommissionRuleInfos);
                return new BaseReturnModel(ReturnCode.Success);
            }

            return result;
        }

        /// <summary>
        /// 所有分紅設置規格檢查
        /// </summary>
        /// <param name="commissionGroupType"></param>
        /// <param name="saveCommissionRuleInfos"></param>
        /// <param name="isFixedCommissionPercent"></param>
        /// <returns></returns>
        private BaseReturnModel CheckRuleInfo(CommissionGroupType commissionGroupType, List<SaveCommissionRuleInfo> saveCommissionRuleInfos,
            bool isFixedCommissionPercent)
        {
            var returnModel = new BaseReturnModel(ReturnCode.Success);

            #region 比例至少一條規則
            returnModel = IsAtLeastOneRule(saveCommissionRuleInfos);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            #endregion

            #region 不可以有比例為0的資料
            returnModel = IsAnyCommissionRateZero(saveCommissionRuleInfos);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            #endregion

            #region 第一筆區間要從0開始
            returnModel = IsFirstRuleMinLimitEqualsZero(saveCommissionRuleInfos);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            #endregion

            #region 不可為負值
            returnModel = IsGreaterEqualThanZero(saveCommissionRuleInfos);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            #endregion

            #region 檢查前台異動分紅契約，是否為登入者用戶下級
            if (EnvLoginUser.Application != JxApplication.BackSideWeb)
            {
                returnModel = IsChildOfLoginUser(saveCommissionRuleInfos);

                if (!returnModel.IsSuccess)
                {
                    return returnModel;
                }
            }
            #endregion

            #region 檢查前台若有設定過分紅區間不能再異動，且分紅比例只能往上遞增
            if (EnvLoginUser.Application != JxApplication.BackSideWeb)
            {
                returnModel = IsCannotModifyProfitLossRange(commissionGroupType, saveCommissionRuleInfos);

                if (!returnModel.IsSuccess)
                {
                    return returnModel;
                }
            }
            #endregion

            #region 只能設定萬位整數
            returnModel = IsTenThousandRangeFormat(saveCommissionRuleInfos);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            #endregion

            #region 上限數值要大於下限數值
            returnModel = IsMaxLimitGreaterThanMinLimit(saveCommissionRuleInfos);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            #endregion

            #region 分紅區間和比例只能遞增
            returnModel = IsRangePercentageIncreaseProgressively(saveCommissionRuleInfos);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            #endregion

            #region 區間數量是否超過上限
            returnModel = IsRuleCountOverflow(saveCommissionRuleInfos);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            #endregion

            #region 比例不可高於上級
            returnModel = IsPercentageSmallThanParent(commissionGroupType, saveCommissionRuleInfos);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            #endregion

            #region 前台勾選固定比例時，檢查分紅比例是否在合法區間內
            if (EnvLoginUser.Application != JxApplication.BackSideWeb && isFixedCommissionPercent)
            {
                returnModel = IsVaildFixedCommissionPercentage(commissionGroupType, saveCommissionRuleInfos, isFixedCommissionPercent);

                if (!returnModel.IsSuccess)
                {
                    return returnModel;
                }
            }
            #endregion

            #region 比例不可低於第一層下級
            returnModel = IsPercentageGreaterThanChild(commissionGroupType, saveCommissionRuleInfos);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            #endregion

            return returnModel;
        }

        private BaseReturnModel IsAnyCommissionRateZero(List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            if (saveCommissionRuleInfos.Any(a => a.CommissionValue == 0))
            {
                return new BaseReturnModel(MessageElement.PlzInputCommissionPercent);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>比例至少一條規則</summary>        
        private BaseReturnModel IsAtLeastOneRule(List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            if (!saveCommissionRuleInfos.AnyAndNotNull())
            {
                return new BaseReturnModel(MessageElement.AtLeastOneRule);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>檢查前台異動分紅契約，是否為登入者用戶下級</summary>       
        private BaseReturnModel IsChildOfLoginUser(List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            if (EnvLoginUser.Application != JxApplication.BackSideWeb)
            {
                IEnumerable<int> query = saveCommissionRuleInfos.Select(s => s.UserID).Distinct();

                if (query.Count() > 1)
                {
                    return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                }

                int? parentId = _userinfoRelatedReadService.GetParentUserId(query.First());

                if (!parentId.HasValue || parentId != EnvLoginUser.LoginUser.UserId)
                {
                    return new BaseReturnModel(CommissionElement.CommissionRuleNotChild);
                }
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>檢查前台若有設定過分紅區間不能再異動，且分紅比例只能往上遞增</summary>        
        private BaseReturnModel IsCannotModifyProfitLossRange(CommissionGroupType commissionGroupType, List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            if (EnvLoginUser.Application != JxApplication.BackSideWeb)
            {
                List<GameCommissionRuleInfo> originalData = GetGameCommissionRuleInfos(commissionGroupType, saveCommissionRuleInfos.First().UserID);

                // 當前台用戶有設定過分紅區間不能再異動，且分紅比例只能往上遞增
                if (originalData.AnyAndNotNull())
                {
                    for (int i = 0; i < originalData.Count; i++)
                    {
                        if (originalData[i].CommissionPercent > saveCommissionRuleInfos[i].CommissionPercent)
                        {
                            return new BaseReturnModel(CommissionElement.NewPercentageMustBeIncreaseProgressively);
                        }

                        if (originalData[i].MinProfitLossRange != saveCommissionRuleInfos[i].MinProfitLossRange)
                        {
                            return new BaseReturnModel(CommissionElement.CannotModifyProfitLossRange);
                        }

                        // 最後一筆原資料為_commissionRangeMax 時跳過判斷最大值，用於修改新增列、固定切換到分紅比例
                        if (originalData[i].MaxProfitLossRange != _commissionRangeMax &&
                            originalData[i].MaxProfitLossRange != saveCommissionRuleInfos[i].MaxProfitLossRange)
                        {
                            return new BaseReturnModel(CommissionElement.CannotModifyProfitLossRange);
                        }
                    }
                }
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>只能設定萬位整數</summary>        
        private BaseReturnModel IsTenThousandRangeFormat(List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            if (saveCommissionRuleInfos.Count == 1)
            {
                if (saveCommissionRuleInfos.First().MaxProfitLossRange == _commissionRangeMax)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }
            }

            foreach (SaveCommissionRuleInfo saveCommissionRuleInfo in saveCommissionRuleInfos)
            {
                var validNumbers = new List<decimal>();

                if (saveCommissionRuleInfo.MinProfitLossRange != _commissionRangeMin)
                {
                    validNumbers.Add(saveCommissionRuleInfo.MinProfitLossRange);
                }

                if (saveCommissionRuleInfo.MaxProfitLossRange != _commissionRangeMax)
                {
                    validNumbers.Add(saveCommissionRuleInfo.MaxProfitLossRange);
                }

                foreach (decimal validNumber in validNumbers)
                {
                    if (!IsTenThousandNumber(validNumber))
                    {
                        return new BaseReturnModel(MessageElement.IsNotTenThousandFormat);
                    }
                }
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>不可為負值</summary>        
        private BaseReturnModel IsGreaterEqualThanZero(List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            if (saveCommissionRuleInfos.Any(a => a.MinProfitLossRange < 0 || a.MaxProfitLossRange < 0 || a.CommissionPercent < 0))
            {
                return new BaseReturnModel(MessageElement.IsNotAllowNegative);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>分紅區間和比例只能遞增</summary>
        private BaseReturnModel IsRangePercentageIncreaseProgressively(List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            for (int i = 1; i < saveCommissionRuleInfos.Count; i++)
            {
                if (saveCommissionRuleInfos[i - 1].MinProfitLossRange >= saveCommissionRuleInfos[i].MinProfitLossRange ||
                    saveCommissionRuleInfos[i - 1].MaxProfitLossRange != saveCommissionRuleInfos[i].MinProfitLossRange)
                {
                    return new BaseReturnModel(CommissionElement.RangeMustBeIncreaseProgressively);
                }

                if (saveCommissionRuleInfos[i - 1].CommissionPercent > saveCommissionRuleInfos[i].CommissionPercent)
                {
                    return new BaseReturnModel(CommissionElement.PercentageMustBeIncreaseProgressively);
                }
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>上限數值要大於下限數值</summary>  
        private BaseReturnModel IsMaxLimitGreaterThanMinLimit(List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            for (int i = 0; i < saveCommissionRuleInfos.Count; i++)
            {
                if (saveCommissionRuleInfos[i].MaxProfitLossRange <= saveCommissionRuleInfos[i].MinProfitLossRange)
                {
                    return new BaseReturnModel(CommissionElement.MaxLimitGreaterThanMinLimit);
                }
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>區間數量是否超過上限(最多只能七個區間)</summary>
        private BaseReturnModel IsRuleCountOverflow(List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            if (saveCommissionRuleInfos.Count > _maxCommissionRuleCount)
            {
                return new BaseReturnModel(string.Format(CommissionElement.RuleCountOverflow, _maxCommissionRuleCount));
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>第一筆區間要從0開始</summary>
        private BaseReturnModel IsFirstRuleMinLimitEqualsZero(List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            if (saveCommissionRuleInfos.OrderBy(o => o.MinProfitLossRange).First().MinProfitLossRange != 0)
            {
                return new BaseReturnModel(CommissionElement.FirstRuleMinLimitEqualsZero);
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>比例不可高於上級</summary>
        private BaseReturnModel IsPercentageSmallThanParent(CommissionGroupType commissionGroupType, List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            int userId = saveCommissionRuleInfos.First().UserID;

            // 查出用戶上級
            int? parentUserId = _userinfoRelatedReadService.GetParentUserId(userId);

            if (parentUserId.HasValue)
            {
                // 若上級是平台，則忽略判斷上級分紅比例
                if (parentUserId.Value == _platformUserId)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                // 查出用戶上級分紅設置
                List<SaveCommissionRuleInfo> parentCommissionRules = GetGameCommissionRuleInfos(commissionGroupType, parentUserId.Value).CastByJson<List<SaveCommissionRuleInfo>>();

                // 上級必須設置分紅比例才能設置下級
                if (parentCommissionRules.Count == 0)
                {
                    string userName = saveCommissionRuleInfos.First().UserName;
                    if (EnvLoginUser.Application != JxApplication.BackSideWeb)
                    {
                        return new BaseReturnModel(string.Format(CommissionElement.ParentMustBeSettingCommissionContract, CommissionElement.SelfUser, userName));
                    }

                    return new BaseReturnModel(string.Format(CommissionElement.ParentMustBeSettingCommissionContract, CommissionElement.ParentUser, userName));
                }

                bool isSuccess = IsParentRulesGreaterEqualThanChild(parentCommissionRules, saveCommissionRuleInfos);

                if (!isSuccess)
                {
                    string parentUserName = _userinfoRelatedReadService.GetUserName(parentUserId.Value);

                    return new BaseReturnModel(string.Format(CommissionElement.PercentageMustBeSmallThanParent, parentUserName));
                }

                return new BaseReturnModel(ReturnCode.Success);
            }

            return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
        }

        /// <summary>前台勾選固定比例時，檢查分紅比例是否在合法區間內</summary>        
        private BaseReturnModel IsVaildFixedCommissionPercentage(CommissionGroupType commissionGroupType,
            List<SaveCommissionRuleInfo> saveCommissionRuleInfos, bool isFixedCommissionPercent)
        {
            if (EnvLoginUser.Application != JxApplication.BackSideWeb && isFixedCommissionPercent)
            {
                // 選擇固定比例時，不能低於下級目前設定，且不能高於上級分紅比例
                // 確認固定比例是否都一致
                IEnumerable<double> commissionPercentList = saveCommissionRuleInfos.Select(s => s.CommissionPercent).Distinct();

                if (commissionPercentList.Count() > 1)
                {
                    return new BaseReturnModel(CommissionElement.FixedCommissionPercentIsNotVaild);
                }

                double saveCommissionPercent = commissionPercentList.First();
                // 不能低於用戶目前設定
                int childId = saveCommissionRuleInfos.First().UserID;
                List<GameCommissionRuleInfo> childGameCommissionRuleInfos = GetGameCommissionRuleInfos(commissionGroupType, childId);
                double childMaxProfitLossRange = 0;

                if (childGameCommissionRuleInfos.Count != 0)
                {
                    childMaxProfitLossRange = childGameCommissionRuleInfos.Select(s => s.CommissionPercent).Max();
                }

                if (saveCommissionPercent < childMaxProfitLossRange)
                {
                    return new BaseReturnModel(string.Format(CommissionElement.CommissionPercentCannotSmallerThanSetting, CommissionElement.ChildUser));
                }

                // 不能高於上級分紅比例
                int parentId = EnvLoginUser.LoginUser.UserId;
                List<GameCommissionRuleInfo> parentGameCommissionRuleInfos = GetGameCommissionRuleInfos(commissionGroupType, parentId);
                double parentMaxProfitLossRange = parentGameCommissionRuleInfos.Select(s => s.CommissionPercent).Max();

                if (saveCommissionPercent > parentMaxProfitLossRange)
                {
                    return new BaseReturnModel(string.Format(CommissionElement.CommissionPercentCannotBiggerThanParent, CommissionElement.ChildUser));
                }
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>比例不可低於第一層下級</summary>
        private BaseReturnModel IsPercentageGreaterThanChild(CommissionGroupType commissionGroupType, List<SaveCommissionRuleInfo> saveCommissionRuleInfos)
        {
            int parentId = parentId = saveCommissionRuleInfos.First().UserID;

            // 查出第一層下級所有分紅設置
            List<GameCommissionRuleInfo> allChildRules = GetGameCommissionRuleInfosByParentId(commissionGroupType, parentId);
            var childRuleMap = new Dictionary<int, List<GameCommissionRuleInfo>>();

            foreach (GameCommissionRuleInfo rule in allChildRules)
            {
                childRuleMap.TryGetValue(rule.UserID, out List<GameCommissionRuleInfo> childRuleList);

                if (childRuleList == null)
                {
                    childRuleList = new List<GameCommissionRuleInfo>();
                    childRuleMap.Add(rule.UserID, childRuleList);
                }

                childRuleList.Add(rule);
            }

            List<int> userIds = allChildRules.Select(s => s.UserID).Distinct().ToList();

            for (int i = 0; i < userIds.Count; i++)
            {
                // 取出下級分紅設置進行比較
                int userId = userIds[i];
                List<SaveCommissionRuleInfo> childCommissionRules = childRuleMap[userId].ToList<SaveCommissionRuleInfo>();

                if (!childCommissionRules.Any())
                {
                    continue;
                }

                bool isSuccess = IsParentRulesGreaterEqualThanChild(saveCommissionRuleInfos, childCommissionRules);

                if (!isSuccess)
                {
                    string childUserName = childCommissionRules.First().UserName;

                    return new BaseReturnModel(string.Format(CommissionElement.PercentageMustBeGreaterThanChild, childUserName));
                }
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        /// <summary>檢查上級所有規則的比例必須大於等於下級</summary>        
        private bool IsParentRulesGreaterEqualThanChild(List<SaveCommissionRuleInfo> parentRules, List<SaveCommissionRuleInfo> childRules)
        {
            // 沒有下級時忽略檢查
            if (childRules.Count() == 0)
            {
                return true;
            }

            foreach (SaveCommissionRuleInfo parentRule in parentRules)
            {
                var matchPercents = new List<double>();

                SaveCommissionRuleInfo matchMinRule = childRules
                    .Where(w => parentRule.MinProfitLossRange >= w.MinProfitLossRange && parentRule.MinProfitLossRange < w.MaxProfitLossRange)
                    .SingleOrDefault();

                if (matchMinRule != null)
                {
                    matchPercents.Add(matchMinRule.CommissionPercent);
                }

                decimal compareMaxProfitLossRange = parentRule.MaxProfitLossRange - 0.00001m;
                SaveCommissionRuleInfo matchMaxRule = childRules
                    .Where(w => compareMaxProfitLossRange >= w.MinProfitLossRange && compareMaxProfitLossRange < w.MaxProfitLossRange)
                    .SingleOrDefault();

                if (matchMaxRule != null)
                {
                    matchPercents.Add(matchMaxRule.CommissionPercent);
                }

                if (matchPercents.Any() && parentRule.CommissionPercent < matchPercents.Max())
                {
                    return false;
                }
            }

            return true;
        }

        private void SaveRuleInfoLog(IEnumerable<SaveCommissionRuleInfo> source, IEnumerable<SaveCommissionRuleInfo> list)
        {
            Func<IEnumerable<SaveCommissionRuleInfo>, string> ratesToString = (sourceList) =>
            {
                if (sourceList.Count() == 0)
                {
                    return CommissionElement.NoneRule;
                }
                string visibleDesc = string.Format(CommissionElement.OperationActive, sourceList.First().Visible.ToString());
                string ratesDesc = string.Join("", sourceList.Select(x => $"({x.MinProfitLossRange}-{x.MaxProfitLossRange},{x.CommissionPercent * 100})"));
                return $"{visibleDesc},{ratesDesc}";
            };

            var user = list.First();
            string content = string.Format(CommissionElement.OperationLog, user.UserName, ratesToString(source), ratesToString(list));

            //前後台類別不同
            if (EnvLoginUser.Application == JxApplication.BackSideWeb)
            {
                _operationLogService.InsertModifyMemberOperationLog(new InsertModifyMemberOperationLogParam()
                {
                    Category = JxOperationLogCategory.Member,
                    AffectedUserId = user.UserID,
                    AffectedUserName = user.UserName,
                    Content= content,
                });
            }
            else
            {
                _operationLogService.InsertFrontSideOperationLog(new InsertFrontSideOperationLogParam()
                {
                    AffectedUserId = user.UserID,
                    AffectedUserName = user.UserName,
                    Content = content,
                });
            }
        }

        private IGameCommissionRuleInfoRep CreateGameCommissionRuleRep(CommissionGroupType commissionGroupType)
        {
            return ResolveJxBackendService<IGameCommissionRuleInfoRep>(commissionGroupType);
        }

        private bool IsTenThousandNumber(decimal input)
        {
            decimal remainder = input % 10000;

            return remainder == 0;
        }

        private List<GameCommissionRuleInfo> GetGameCommissionRuleInfosByParentId(CommissionGroupType commissionGroupType, int parentId)
        {
            return CreateGameCommissionRuleRep(commissionGroupType).GetGameCommissionRuleInfosByParentId(parentId);
        }
    }
}
