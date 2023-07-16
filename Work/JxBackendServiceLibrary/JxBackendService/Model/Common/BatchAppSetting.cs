namespace JxBackendService.Model.Common
{
    public class BatchAppSetting : SharedAppSettings
    {
        private BatchAppSetting()
        { }

        public static BatchAppSetting Instance = new BatchAppSetting();

        public string CacheExpiredDays => Get("CacheExpiredDays");

        public string ProcessPreCount => Get("ProcessPreCount");

        public string ProcessTimes => Get("ProcessTimes");
    }
}