using MS.Core.Models.Models;

namespace MS.Core.MM.Models.Filters
{
    public class PageOfficialPostFilter : OfficialPostFilter
    {
        public PaginationModel Pagination { get; set; }
    }
}
