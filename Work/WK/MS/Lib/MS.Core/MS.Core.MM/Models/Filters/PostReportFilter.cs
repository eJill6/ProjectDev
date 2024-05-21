namespace MS.Core.MM.Models.Filters
{
    public class PostReportFilter
    {
        public IEnumerable<string> PostTranIds { get; set; }

        public IEnumerable<string> PostIds { get; set; }
    }
}
