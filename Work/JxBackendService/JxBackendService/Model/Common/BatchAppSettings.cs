namespace JxBackendService.Model.Common
{
    public class BatchAppSettings : SharedAppSettings
    {
        private BatchAppSettings() { }

        public static BatchAppSettings Instance = new BatchAppSettings();

        public string CacheExpiredDays => Get("CacheExpiredDays");

        public string ProcessPreCount => Get("ProcessPreCount");

        public string ProcessTimes => Get("ProcessTimes");

    }

}
