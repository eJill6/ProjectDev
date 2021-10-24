using System;
using System.Collections.Generic;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service.VIP.Activity;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Entity.VIP.Rule;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.VIP;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.Param.VIP;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.VIP.Activity;

namespace JxBackendService.Service.User.Activity
{
    public class VIPMonthlyDepositService : BaseVIPUserEventDetailService, IVIPUserEventDetailService
    {
        private readonly IUserInfoRep _userInfoRep;
        private readonly IVIPLevelSettingRep _userLevelSettingRep;
        private readonly ICMoneyInInfoRep _cmoneyInInfoRep;

        private readonly static int _depositHoursBefore = -12;
        private readonly static int _flowMultiple = 12;

        public VIPMonthlyDepositService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _userLevelSettingRep = ResolveJxBackendService<IVIPLevelSettingRep>();
            _cmoneyInInfoRep = ResolveJxBackendService<ICMoneyInInfoRep>();
        }

        public BaseReturnDataModel<string> PreCheckApplyForMonthlyDesposit()
        {
            int userId = EnvLoginUser.LoginUser.UserId;

            // 活動日期區間
            DateTime activityStartDate = DateTimeUtil.GetThisMonthFirstDay();
            DateTime activityEndDate = activityStartDate.ToQuerySmallThanTime(DatePeriods.Month);
            // 充值日期區間
            DateTime nowDate = DateTime.Now;
            DateTime depositStartDate = nowDate.AddHours(_depositHoursBefore);
            DateTime depositEndDate = nowDate;

            var checkParam = new CheckMonthlyDepositParam
            {
                UserID = userId,
                EventTypeID = VIPEventType.MonthlyDepositPrize.Value,
                ActivityStartDate = activityStartDate,
                ActivityEndDate = activityEndDate,
                OrderStartDate = depositStartDate,
                OrderEndDate = depositEndDate
            };

            BaseReturnDataModel<DespositInfo> returnDataModel = CheckApplyForMonthlyActivity(checkParam);

            if (!returnDataModel.IsSuccess)
            {
                return new BaseReturnDataModel<string>(ReturnCode.GetSingle(returnDataModel.Code));
            }

            string strDepositAmount = returnDataModel.DataModel.Amount.ToString().ToDecimal().ToCurrency();
            string returnContent = string.Format(VIPContentElement.AreYouSureApplyForMonthlyDesposit, strDepositAmount);

            return new BaseReturnDataModel<string>(ReturnCode.GetSingle(returnDataModel.Code), returnContent);
        }

        /// <summary>
        /// 檢查月存款活動的資格
        /// </summary>
        /// <param name="checkParam"></param>
        /// <returns></returns>
        private BaseReturnDataModel<DespositInfo> CheckApplyForMonthlyActivity(CheckMonthlyDepositParam checkParam)
        {
            int userId = checkParam.UserID;

            UserInfo userInfo = _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo { UserID = userId });
            VIPUserInfo vipUserInfo = VIPUserInfoRep.GetSingleByKey(InlodbType.Inlodb, new VIPUserInfo { UserID = userId });

            if (vipUserInfo == null || userInfo == null)
            {
                return new BaseReturnDataModel<DespositInfo>(ReturnCode.NotVIPUser);
            }

            // 用户已冻结
            if (!userInfo.IsActive)
            {
                return new BaseReturnDataModel<DespositInfo>(ReturnCode.UserIsFrozen);
            }

            VIPLevelSetting vipLevelSetting = _userLevelSettingRep.GetSingleByKey(InlodbType.Inlodb,
                new VIPLevelSetting { VIPLevel = vipUserInfo.CurrentLevel });

            if (vipLevelSetting == null)
            {
                return new BaseReturnDataModel<DespositInfo>(ReturnCode.NotExistVIPLevel);
            }

            // VIP等級對應月存款規則
            if (vipLevelSetting.MonthlyDepositRuleJson == null ||
                vipLevelSetting.MonthlyDepositRuleJson.JoinTimes == 0 ||
                vipLevelSetting.MonthlyDepositRuleJson.MaxGiftMoney == 0)
            {
                return new BaseReturnDataModel<DespositInfo>(ReturnCode.NoQualifyForActivity);
            }

            // 檢查过去12小时内是否有最後一筆充值成功的訂單號
            DespositInfo depositInfo = _cmoneyInInfoRep.GetDepositDoneOrderInfo(userId, checkParam.OrderStartDate, checkParam.OrderEndDate);

            if (depositInfo == null)
            {
                return new BaseReturnDataModel<DespositInfo>(ReturnCode.NoQualifyForActivity);
            }

            // 檢查充值單是否已參加過當月活動
            var refIDParam = new CheckUserEventRefIDParam
            {
                UserID = userId,
                EventTypeID = checkParam.EventTypeID,
                RefID = depositInfo.MoneyInID.ToNonNullString()
            };

            bool hasAuditRefIDOrder = VIPUserEventDetailRep.HasAuditRefIDActivity(refIDParam);

            if (hasAuditRefIDOrder)
            {
                return new BaseReturnDataModel<DespositInfo>(ReturnCode.NoQualifyForActivity);
            }

            // 檢查有待審核活動單
            bool hasAuditUnprocessed = VIPUserEventDetailRep.HasAuditUnprocessedActivity(userId, checkParam.EventTypeID);

            if (hasAuditUnprocessed)
            {
                return new BaseReturnDataModel<DespositInfo>(ReturnCode.HasUnprocessedAuditActivity);
            }

            // 檢查活動區間內已經審核通過的相關總筆數與金額
            var queryParam = checkParam.CastByJson<VIPUserEventQueryParam>();

            // 對應不同等級有參加月存款活動的頻率
            if (vipLevelSetting.MonthlyDepositRuleJson.JoinFrequencyType == JoinFrequencyType.Once.Value)
            {
                queryParam.ActivityStartDate = null;
                queryParam.ActivityEndDate = null;
            }

            // 檢查活動區間內已經審核通過的相關總筆數
            queryParam.AuditStatus = AuditStatusType.Pass.Value;

            VIPUserEventAuditStat vipAuditStat = VIPUserEventDetailRep.GetVIPUserEventAuditStat(queryParam);

            if (vipAuditStat.AuditTotalCount >= vipLevelSetting.MonthlyDepositRuleJson.JoinTimes)
            {
                return new BaseReturnDataModel<DespositInfo>(ReturnCode.EnoughMonthlyActivity);
            }

            return new BaseReturnDataModel<DespositInfo>(ReturnCode.Success, depositInfo);
        }

        public override BaseReturnModel VIPUserApplyForActivity()
        {
            int userId = EnvLoginUser.LoginUser.UserId;

            // 活動日期區間
            DateTime activityStartDate = DateTimeUtil.GetThisMonthFirstDay();
            DateTime activityEndDate = activityStartDate.ToQuerySmallThanTime(DatePeriods.Month);
            // 充值日期區間
            DateTime nowDate = DateTime.Now;
            DateTime depositStartDate = nowDate.AddHours(_depositHoursBefore);
            DateTime depositEndDate = nowDate;

            var checkParam = new CheckMonthlyDepositParam
            {
                UserID = userId,
                EventTypeID = VIPEventType.MonthlyDepositPrize.Value,
                ActivityStartDate = activityStartDate,
                ActivityEndDate = activityEndDate,
                OrderStartDate = depositStartDate,
                OrderEndDate = depositEndDate
            };

            // 檢查是否有月存款活動的資格            
            BaseReturnDataModel<DespositInfo> checkQualify = CheckApplyForMonthlyActivity(checkParam);

            if (!checkQualify.IsSuccess)
            {
                return checkQualify;
            }

            DespositInfo depositInfo = checkQualify.DataModel;

            // 用戶VIP資訊
            int userCurrentLevel = VIPUserInfoRep.GetUserCurrentLevel(userId).Value;

            // VIP用戶等級設定
            VIPLevelSetting vipLevelSetting = _userLevelSettingRep.GetSingleByKey(InlodbType.Inlodb,
                new VIPLevelSetting { VIPLevel = userCurrentLevel });

            // 對應VIP等級月存款規則
            MonthlyDepositRule monthlyDepositRule = vipLevelSetting.MonthlyDepositRuleJson;

            // 活動區間內已經審核通過的相關總筆數與金額
            var queryParam = checkParam.CastByJson<VIPUserEventQueryParam>();
            queryParam.AuditStatus = AuditStatusType.Pass.Value;

            // 月存款福利單次礼金上限
            decimal maxGiftMoney = monthlyDepositRule.MaxGiftMoney;

            // 月存款福利存送比率
            decimal bonusRate = monthlyDepositRule.BonusRate;

            // 此次申請金額最大值
            decimal maxApplyAmount = maxGiftMoney / bonusRate;

            // 此次申请金额(充值單金額)
            decimal applyAmount = depositInfo.Amount.ToString().ToDecimal();

            // 此次可參與活動金額，若申请金额已超過最大值，則以申請金額最大值為主
            decimal eventAmount = applyAmount;

            if (applyAmount > maxApplyAmount)
            {
                eventAmount = maxApplyAmount;
            }

            // 此次可領的紅利獎金
            decimal bonusAmount = eventAmount * bonusRate;
            // 流水倍數
            decimal flowMultiple = _flowMultiple;
            // 流水
            decimal financialFlowAmount = (eventAmount + bonusAmount) * flowMultiple;

            string moneyInId = depositInfo.MoneyInID.ToNonNullString();

            var localizationParam = new LocalizationParam
            {
                LocalizationSentences = new List<LocalizationSentence>
                {
                    new LocalizationSentence
                    {
                        ResourcePropertyName = nameof(DBContentElement.VIPMonthlyDepositActivityMemo),
                        Args = new List<string> { eventAmount.ToCurrency(), depositInfo.OrderID }
                    },
                }
            };

            // 建立活動審核單 
            var createVIPUserEventAuditParam = new CreateVIPUserEventAuditParam
            {
                EventTypeID = VIPEventType.MonthlyDepositPrize.Value,
                AuditStatus = AuditStatusType.Unprocessed.Value,
                CurrentLevel = userCurrentLevel,
                ApplyAmount = applyAmount,
                EventAmount = eventAmount,
                BonusAmount = bonusAmount,
                FlowMultiple = flowMultiple,
                FinancialFlowAmount = financialFlowAmount,
                RefID = moneyInId,
                MemoJson = localizationParam.ToLocalizationJsonString()
            };

            return CreateVIPUserEventInfo(createVIPUserEventAuditParam);
        }
    }
}
