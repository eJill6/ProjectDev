using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.VIP
{
    public interface IVIPUserService
    {
        int GetUserCurrentLevel(int userId);

        List<VIPUserInfo> GetVIPUserInfos(List<int> userIds);
        
        BaseReturnModel CheckQualifiedForUser(int userId, WalletType walletType);
    }
}