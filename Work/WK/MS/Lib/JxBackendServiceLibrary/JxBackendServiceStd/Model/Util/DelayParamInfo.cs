namespace JxBackendService.Model.Util
{
    public class DelayParamInfo<T> where T : class
    {
        public DelayParamInfo()
        { }

        public int DelayCircleCount { get; set; }

        public T Param { get; set; }
    }
}