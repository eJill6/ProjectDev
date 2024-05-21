using JxBackendService.Model.Enums.MM;
using JxBackendService.Model.Paging;
using MS.Core.MMModel.Models.Post.Enums;

namespace BackSideWeb.Model.Param.MM
{
    public class QueryBannerParam : BasePagingRequestParam
    {
        public LocationType LocationType { get; set; }
    }
}