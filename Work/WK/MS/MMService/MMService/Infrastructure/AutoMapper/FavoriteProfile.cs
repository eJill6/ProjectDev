using AutoMapper;
using MS.Core.MM.Model.Banner;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.Favorite;
using MS.Core.MMModel.Models.My;
using MS.Core.Models.Models;

namespace MMService.Infrastructure.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class FavoriteProfile: Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public FavoriteProfile()
        {
            CreateMap<MyFavoriteQueryParamForClient, FavoriteListParam>();

            CreateMap<MMBoss, MyFavoriteOfficialShop>().ForMember(a => a.OrderQuantity, b => b.MapFrom(c => c.DealOrder));
            //CreateMap<List<MMBoss>, List<MyFavoriteOfficialShop>>(); 

            CreateMap<MMPostFavorite, MyFavorite>();
            //CreateMap<PageResultModel<MMPostFavorite>, PageResultModel<MyFavorite>>();

        }
    }
}
