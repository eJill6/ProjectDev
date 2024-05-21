namespace JxBackendService.Model.db
{
    public class JxBasicCrossDbQueryParam<T>
    {
        public string FullTableName { get; set; }
        public string Filters { get; set; }

        public T DataModel { get; set; }
    }

    public class JxBasicCrossDbQueryParam : JxBasicCrossDbQueryParam<object>
    {

    }
}