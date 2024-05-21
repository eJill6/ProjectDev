using AutoMapper;
using BackSideWeb.Models.ViewModel.OperatingData;
using MS.Core.MMModel.Models.OperationOverview;

namespace BackSideWeb.MapConfig
{
    public class AutoMapperConfigs:Profile
    {
        public AutoMapperConfigs() {

            CreateMap<List<DailyRevenue>, List<DailyRevenueViewModel>>().ReverseMap();
            
        }
    }
}
