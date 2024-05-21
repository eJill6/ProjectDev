using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IVIPWelfareRepo
    {
        Task<IEnumerable<MMVipWelfare>> GetVipWelfares(IEnumerable<VipType> vipTypes, VIPWelfareTypeEnum type);
        Task<IEnumerable<MMVipWelfare>> GetVipWelfaresByFilter(VipWelfareFilter filter);
    }
}
