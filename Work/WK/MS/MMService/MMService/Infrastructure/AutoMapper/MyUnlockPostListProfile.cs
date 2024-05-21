using AutoMapper;
using MMService.Models.My;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MMModel.Extensions;
using MS.Core.MMModel.Models.Post.Enums;

namespace MMService.Infrastructure.AutoMapper
{
    /// <summary>
    /// 個人中心-解鎖清單
    /// </summary>
    public class MyUnlockPostListProfile : Profile
    {
        /// <summary>
        /// 個人中心-解鎖清單
        /// </summary>
        public MyUnlockPostListProfile()
        {
            CreateMap<MMPost, MyUnlockPostList>()
                .ForMember(dest => dest.Height, opt =>
                    opt.MapFrom<string>(src => Enum.IsDefined(typeof(BodyHeightDefined), src.Height) ?
                        ((BodyHeightDefined)src.Height).GetDescription() :
                        BodyHeightDefined.H_Plus.GetDescription()))
                .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom<string>(src => Enum.IsDefined(typeof(AgeDefined), (int)src.Age) ?
                        ((AgeDefined)(int)src.Age).GetDescription() :
                        AgeDefined.Y_Plus.GetDescription()))
                .ForMember(dest => dest.Cup, opt =>
                    opt.MapFrom<string>(src => Enum.IsDefined(typeof(CupDefined), (int)src.Cup) ?
                        ((CupDefined)(int)src.Cup).GetDescription() :
                        CupDefined.Plus.GetDescription()))
                .ForMember(dest=>dest.Unlocks,opt=>opt.MapFrom(c=>c.UnlockCount));
        }
    }
}