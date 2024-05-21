using AutoMapper;
using MS.Core.MM.Model.Banner;
using MS.Core.MMModel.Models;

namespace MMService.Infrastructure.AutoMapper
{
    /// <summary>
    /// Mapping BannerInfo to BannerViewModel 
    /// </summary>
    public class BannerInfoProfile : Profile
    {
        public BannerInfoProfile()
        {
            CreateMap<BannerInfo, BannerViewModel>()
                .ForMember(dest => dest.FullMediaUrl, opt => opt.MapFrom(src => src.Media.FullMediaUrl));
        }
    }
}
