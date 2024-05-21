using AutoMapper;
using MS.Core.MM.Models.Vip;
using MS.Core.MMModel.Models.Vip;

namespace MMService.Infrastructure.AutoMapper
{
    public class VipProfile : Profile
    {
        public VipProfile()
        {
            CreateMap<ResVip, VipViewModel>();
            CreateMap<ResBuyVip, BuyVipViewModel>();
            CreateMap<ResUserVipTransLog, UserVipTransLogViewModel>();
        }
    }
}
