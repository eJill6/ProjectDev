using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service.VIP;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.VIP
{
    public class VIPUserService : BaseService, IVIPUserService
    {
        private readonly IVIPUserInfoRep _vipUserInfoRep;        

        public VIPUserService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser,
            dbConnectionType)
        {
            _vipUserInfoRep = ResolveJxBackendService<IVIPUserInfoRep>();
        }

        public int GetUserCurrentLevel(int userId)
        {
            int? currentLevel = _vipUserInfoRep.GetUserCurrentLevel(userId);

            if (!currentLevel.HasValue)
            {
                //直客制商戶一定要有資料
                throw new ArgumentException();
            }

            return currentLevel.Value;
        }


        public BaseReturnModel CheckQualifiedForUser(int userId, WalletType walletType)
        {
            return _vipUserInfoRep.CheckQualifiedForUser(userId, walletType);
		}

        public List<VIPUserInfo> GetVIPUserInfos(List<int> userIds)
        {
            return _vipUserInfoRep.GetVIPUserInfos(userIds);
        }
    }
}