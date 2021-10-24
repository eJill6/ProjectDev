using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ColourEgg;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service
{
    public class ColourEggService : BaseService, IColourEggService
    {
        private readonly IUserInfoRep _userInfoRep;
        private readonly IGiftBagRep _giftBagRep;

        public ColourEggService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _giftBagRep = ResolveJxBackendService<IGiftBagRep>();
        }

        public BaseReturnDataModel<int> CheckReopened(int userId, string ip, bool isInsertLog = false)
        {
            BaseReturnDataModel<int> returnModel = new BaseReturnDataModel<int>(new SuccessMessage(ReturnCode.OpenedSuccess.Name)
                                                                                  , (int)RedEnvelopeOpenTypes.OpenedSuccess);

            if (!IsOldUser(userId))
            {
                //老用户才能参与红包领取活动
                returnModel = new BaseReturnDataModel<int>(ReturnCode.ColourEggOnlyForOldUser
                                                        , (int)RedEnvelopeOpenTypes.IsNotOldUser);
            }

            if (IsSameIPOpened(ip))
            {
                //重覆领取红包
                returnModel = new BaseReturnDataModel<int>(ReturnCode.ColourEggSameIPOpened
                                                        , (int)RedEnvelopeOpenTypes.SameIpReopen);
            }

            if (!IsBindBankCard(userId))
            {
                //请绑定银行卡以便领取红包
                returnModel = new BaseReturnDataModel<int>(ReturnCode.ColourEggNoBindBankCard
                                                        , (int)RedEnvelopeOpenTypes.NoBindBankCard);
            }

            if (isInsertLog && !returnModel.IsSuccess)
            {
                var activityLog = new ActivityLog()
                {
                    PrizeMoney = 1,
                    ActyDate = DateTime.Now,
                    IP = ip,
                    UserID = userId,
                    NotIssuingType = returnModel.DataModel,
                    Msg = returnModel.Message
                };

                _userInfoRep.AddUserActivityLog(activityLog);
            }

            return returnModel;
        }

        /// <summary>
        /// 是否老用戶
        /// </summary>
        /// <param name="userId">用戶代碼</param>
        /// <returns></returns>
        private bool IsOldUser(int userId)
        {
            var userLevel = _userInfoRep.GetUserLevel(userId);

            return userLevel == (int)UserLevelTypes.OldUser;
        }

        /// <summary>
        /// 是否當天同一個IP重覆領取開運紅利1元紅包
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool IsSameIPOpened(string ip)
        {
            //找出當天同一IP登入的用戶
            var userIds = _userInfoRep.GetUserActivityLogByIpInOneDay(ip)
                .Where(item => item.UserID.HasValue)
                .Select(item => item.UserID)
                .ToList();

            if (userIds == null || !userIds.Any())
            {
                return false;
            }

            //查出這些用戶是否任一人有紅包領取記錄
            var redEnvelopeActType = 1; //紅利類型 - 開運紅利1元紅包
            bool isRedEnvelopeOpened = _giftBagRep.GetGiftBagByUserOpened(userIds, redEnvelopeActType).Any();

            return isRedEnvelopeOpened;
        }

        /// <summary>
        /// 是否綁定銀行卡
        /// </summary>
        /// <param name="userId">用戶代碼</param>
        /// <returns></returns>
        private bool IsBindBankCard(int userId)
        {
            return _userInfoRep.HasUserActiveBankInfo(userId);
        }
    }
}
