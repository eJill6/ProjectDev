namespace JxBackendService.Interface.Model.Paging
{
    public interface IPagerInfo
    {
        int PageNo { get; set; }

        int PageSize { get; set; }

        int TotalPageCount { get; set; }

        int TotalCount { get; set; }
    }
}