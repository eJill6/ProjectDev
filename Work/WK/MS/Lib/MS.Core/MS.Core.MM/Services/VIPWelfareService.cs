using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.Bases;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.Vip.Enums;

namespace MS.Core.MM.Service
{
    public class VipWelfareService : BaseHttpRequestService, IVipWelfareService
    {
        public VipWelfareService(IRequestIdentifierProvider provider, ILogger logger, IVIPWelfareRepo vipWelfareRepo, IDateTimeProvider dateTimeProvider) 
            : base(provider, logger, dateTimeProvider)
        {
            VipWelfareRepo = vipWelfareRepo;
        }

        IVIPWelfareRepo VipWelfareRepo { get; }

        public Task<IEnumerable<MMVipWelfare>> GetVipWelfares(IEnumerable<VipType> vipTypes, VIPWelfareTypeEnum type)
        {
            return VipWelfareRepo.GetVipWelfares(vipTypes, type);
        }

        public Task<IEnumerable<MMVipWelfare>> GetVipWelfares(IEnumerable<VipType> vipTypes)
        {
            return VipWelfareRepo.GetVipWelfaresByFilter(new VipWelfareFilter 
            {
                VipTypes = vipTypes,
            });
        }

        public Task<IEnumerable<MMVipWelfare>> GetVipWelfares(VipType vipType)
        {
            return VipWelfareRepo.GetVipWelfaresByFilter(new VipWelfareFilter
            {
                VipTypes = vipType.ToEnumerable(),
            });
        }
    }
}