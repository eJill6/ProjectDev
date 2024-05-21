using AutoMapper;
using MS.Core.MM.Model.Media;
using MS.Core.MMModel.Models;

namespace MMService.Infrastructure.AutoMapper
{
    public class SaveMediaToOssParamProfile : Profile
    {
        public SaveMediaToOssParamProfile()
        {
            CreateMap<SaveMediaToOssParamForClient, SaveMediaToOssParam>();
        }
    }
}
