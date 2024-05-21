using AutoMapper;
using MS.Core.Models.Models;

namespace MMService.Infrastructure.AutoMapper
{
    public class PageResultModelProfile : Profile
    {
        public PageResultModelProfile()
        {
            CreateMap(typeof(PageResultModel<>), typeof(PageResultModel<>));
        }
    }
}
