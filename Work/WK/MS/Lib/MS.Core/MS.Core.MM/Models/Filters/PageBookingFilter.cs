using MS.Core.Models.Models;

namespace MS.Core.MM.Models.Filters
{
    public class PageBookingFilter : BookingFilter
    {
        public PaginationModel Pagination { get; set; }
    }
}
