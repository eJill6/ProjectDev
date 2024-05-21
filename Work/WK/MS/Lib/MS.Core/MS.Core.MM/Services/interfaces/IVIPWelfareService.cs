using MS.Core.MM.Models.Entities.User;
using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Services.interfaces
{
    public interface IVipWelfareService
    {
        Task<IEnumerable<MMVipWelfare>> GetVipWelfares(IEnumerable<VipType> vipTypes, VIPWelfareTypeEnum discount);
        Task<IEnumerable<MMVipWelfare>> GetVipWelfares(IEnumerable<VipType> vipTypes);
        Task<IEnumerable<MMVipWelfare>> GetVipWelfares(VipType vipType);
    }
}