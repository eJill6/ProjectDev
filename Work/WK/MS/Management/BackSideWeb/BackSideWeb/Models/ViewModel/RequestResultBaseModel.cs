using JxBackendService.Model.Paging;

namespace BackSideWeb.Models.ViewModel
{
    public class RequestResultBaseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public string Code { get; set; }
        public PagedResultModel<T> Data { get; set; }
    }
}
