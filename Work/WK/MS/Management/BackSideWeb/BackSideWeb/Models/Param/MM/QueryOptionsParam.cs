using JxBackendService.Model.Paging;

namespace BackSideWeb.Model.Param.MM
{
    public class QueryOptionsParam : BasePagingRequestParam
    {
        public int OptionType { get; set; }
        public int PostType { get; set; }
    }
}
