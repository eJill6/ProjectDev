namespace BackSideWeb.Helpers
{
    public class ApiSingleResult<T>
    {
        public T? Datas { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
