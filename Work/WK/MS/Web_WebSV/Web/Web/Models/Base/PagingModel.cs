namespace Web.Models.Base
{
    /// <summary>
    /// Paging model.
    /// </summary>
    public class PagingModel
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public string SortBy { get; set; }
        public string Direction { get; set; }
    }
}