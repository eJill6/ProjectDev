namespace MS.Core.Infrastructures.ZeroOne.Models.Responses
{
    public class ZenoOneResDataBase<T> : ZenoOneResDataBase
    {
        public T Data { get; set; }
    }
    public class ZenoOneResDataBase
    {
        public string Error { get; set; }
        public bool Success { get; set; }
    }
}