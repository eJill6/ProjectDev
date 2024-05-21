namespace JxBackendService.Model.MiseLive.Response
{
    public class BaseMiseLiveResponse
    {
        public bool Success { get; set; }

        public string Error { get; set; }
    }

    public class MiseLiveResponse<T> : BaseMiseLiveResponse where T : class
    {
        public T Data { get; set; }
    }
}