using AutoMapper;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MMModel.Models.My;
using MS.Core.Models.Models;

namespace MMService.Infrastructure.AutoMapper
{
    public class MyPostListProfile : Profile
    {
        public MyPostListProfile()
        {
            CreateMap<MMPost, MyPostList>()
                .ForMember(dest => dest.CreateTime,
                    opt => opt.MapFrom<string>(src =>
                        src.UpdateTime.HasValue ?
                            ((DateTime)src.UpdateTime).ToString(GlobalSettings.DateTimeFormat) :
                            string.Empty));

            CreateMap<MMOfficialPost, MyPostList>()
                .ForMember(dest => dest.CreateTime,
                    opt => opt.MapFrom<string>(src =>
                        src.UpdateTime.HasValue ?
                            ((DateTime)src.UpdateTime).ToString(GlobalSettings.DateTimeFormat) :
                            string.Empty))
                .ForMember(dest => dest.UnlockCount, opt => opt.MapFrom<int>(src => src.AppointmentCount));
        }
    }
}