using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.VIP;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.VIP;
using JxBackendService.Model.Util;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.VIP.Bonus
{
    public class VIPBonusMonthlyRedEnvelopeService : BaseVIPBonusTypeService
    {
        protected override ReturnCode ReceivedCode => throw new NotImplementedException();

        protected override int BonusTypeToken => DateTime.Now.ToFormatYearMonthValue().ToInt32(); //暫定用系統日期

        protected override VIPBonusType VIPBonusType => VIPBonusType.MonthlyRedEnvelope;

        public VIPBonusMonthlyRedEnvelopeService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        protected override decimal GetBonusMoney(VIPLevelSetting vipLevelSetting) => vipLevelSetting.MonthlyRedEnvelopeMoney;

        protected override bool IsReceiveDateValid(VIPUserInfo vipUserInfo)
        {
            string yyyyMMdd = BonusTypeToken.ToString() + "01";

            DateTime startDate = yyyyMMdd.ToDateTime();
            DateTime endDate = startDate.ToQuerySmallThanTime(DatePeriods.Month);

            if (DateTime.Now >= startDate && DateTime.Now < endDate)
            {
                return true;
            }

            return false;
        }

        protected override BaseReturnModel SaveReceiveBonus(VIPBonusType vipBonusType, string currentLevelName, decimal bonusMoney)
        {
            var localizationParam = new LocalizationParam
            {
                LocalizationSentences = new List<LocalizationSentence>
                {
                    new LocalizationSentence
                    {
                        ResourcePropertyName = nameof(DBContentElement.VIPReceivedMonthlyRedEnvelopeMoney),
                        Args = new List<string> { currentLevelName }
                    },
                }
            };

            var proVipPrizesParam = new ProVIPPrizesParam
            {
                UserID = EnvLoginUser.LoginUser.UserId,
                Handle = EnvLoginUser.LoginUser.UserName,
                Money = bonusMoney,
                BonusType = VIPBonusType.Value,
                ProcessToken = BonusTypeToken,
                ReceivedDate = DateTime.Now,
                ReceivedStatus = ReceivedStatus.Received.Value,
                MemoJson = localizationParam.ToLocalizationJsonString(),
                CreateUser = EnvLoginUser.LoginUser.UserName,
                UserBonusActType = (int)ActTypes.Insert
            };

            BaseReturnModel returnModel = VIPUserInfoRep.ReceivedPrize(proVipPrizesParam);

            return returnModel;
        }
    }
}
