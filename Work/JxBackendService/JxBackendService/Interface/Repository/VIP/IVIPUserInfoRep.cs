using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.StoredProcedureParam.VIP;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.VIP
{
    public interface IVIPUserInfoRep : IBaseDbRepository<VIPUserInfo>
    {
        BaseReturnModel ReceivedPrize(ProVIPPrizesParam proVipPrizesParam);

        int? GetUserCurrentLevel(int userId);

        List<VIPUserInfo> GetVIPUserInfos(List<int> userIds);

        BaseReturnModel RegisterVIPUser(RegisterVIPUserParam registerVIPUserParam);
        BaseReturnModel CheckQualifiedForUser(int userId, WalletType walletType);
    }
}