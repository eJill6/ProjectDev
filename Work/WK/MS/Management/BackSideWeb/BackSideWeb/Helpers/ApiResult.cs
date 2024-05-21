namespace BackSideWeb.Helpers
{
    public class ApiResult<T>
    {
        public List<T>? Datas { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Code { get; set; }

    }
    public class ApiResult
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Code { get; set; }
    }
}
