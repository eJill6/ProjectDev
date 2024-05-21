using JxBackendService.Model.Paging;
using MS.Core.MMModel.Models.Post.Enums;

namespace BackSideWeb.Model.Param.MM
{
    public class QueryPostWeightParam : BasePagingRequestParam
    {
        public PostType PostType { get; set; }
    }
}
