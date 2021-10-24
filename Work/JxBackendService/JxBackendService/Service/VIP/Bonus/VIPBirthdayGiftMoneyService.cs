using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Model.Entity.User;
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
    public class VIPBirthdayGiftMoneyService : BaseVIPBonusTypeService
    {
        private readonly IUserInfoAdditionalRep _userInfoAdditionalRep;

        protected override ReturnCode ReceivedCode => ReturnCode.BirthdayGiftMoneyReceived;

        protected override int BonusTypeToken => DateTime.Now.Year;

        protected override VIPBonusType VIPBonusType => VIPBonusType.BirthdayGiftMoney;

        protected override decimal GetBonusMoney(VIPLevelSetting vipLevelSetting) => vipLevelSetting.BirthdayGiftMoney;

        public VIPBirthdayGiftMoneyService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userInfoAdditionalRep = ResolveJxBackendService<IUserInfoAdditionalRep>();
        }

        protected override bool IsReceiveDateValid(VIPUserInfo vipUserInfo)
        {
            UserInfoAdditional userInfoAdditional = _userInfoAdditionalRep
                .GetSingleByKey(InlodbType.Inlodb, new UserInfoAdditional() { UserID = vipUserInfo.UserID });

            if (userInfoAdditional != null && userInfoAdditional.Birthday.HasValue &&
                userInfoAdditional.Birthday.Value.ToFormatMonthDayValue() == DateTime.Now.ToFormatMonthDayValue()) //當日領取
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
                        ResourcePropertyName = nameof(DBContentElement.VIPReceivedBirthdayGiftMoney),
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
