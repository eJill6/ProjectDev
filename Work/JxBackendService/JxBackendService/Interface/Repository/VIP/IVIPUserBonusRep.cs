using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.VIP;

namespace JxBackendService.Interface.Repository.VIP
{
    public interface IVIPUserBonusRep : IBaseDbRepository<VIPUserBonus>
    {
        bool IsReceived(int userID, int processToken, int bonusType);

        PagedResultWithAdditionalData<VIPUserBonus, decimal> GetEntityList(VIPUserBonusQueryParam param, BasePagingRequestParam pageParam);
    }
}