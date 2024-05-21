using AutoMapper;
using System.Collections.Generic;

namespace Management.Web.Modules.Common.Helpers
{
    public class MapTool
    {
        public static TAfterModel MapToModel<TBeforeModel, TAfterModel>(TBeforeModel source)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<TBeforeModel, TAfterModel>());

            var mapper = config.CreateMapper();
            var result = mapper.Map<TAfterModel>(source);

            return result;
        }
        public static List<TAfterModel> MapToModelList<TBeforeModel, TAfterModel>(TBeforeModel source)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<TBeforeModel, TAfterModel>());

            var mapper = config.CreateMapper();
            var result = mapper.Map<List<TAfterModel>>(source);

            return result;
        }
    }
}
