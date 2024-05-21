namespace JxBackendService.Attributes
{
    public class DataBaseSyncResponseTimeAttribute : ResponseTimeLimitAttribute
    {
        public DataBaseSyncResponseTimeAttribute() : base(waitMilliSeconds: 300) { }
    }
}
