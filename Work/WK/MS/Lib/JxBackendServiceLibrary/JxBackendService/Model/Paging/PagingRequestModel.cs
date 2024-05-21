namespace JxBackendService.Model.Paging
{
    public class PagingRequestModel<T> where T: class
    {
        public T Parameter { get; set; }

        public BasePagingRequestParam PagingParameter { get; set; }
    }
}
