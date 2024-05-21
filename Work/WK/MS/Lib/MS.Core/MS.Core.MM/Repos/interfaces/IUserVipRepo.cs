using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IUserVipRepo
    {
        Task<MMUserVip[]> GetUserVips(int userId);
        Task<MMUserVip[]> GetUserVips(IEnumerable<int> userIds);
        Task<MMUserVip[]> GetUserEfficientVipsByType(VipType vipId);
    }
}
