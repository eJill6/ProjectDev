namespace JxBackendService.Model.Common
{
    public class JDBFISharedAppSetting : SharedAppSettings
    {
        private JDBFISharedAppSetting()
        { }

        /* API  */

        public static string ActionUrl => "apiRequest.do";

        public static string ServiceUrl => Get("TPGame.JDBFI.ServiceBaseUrl");

        public static string DC => Get("TPGame.JDBFI.DC");

        public static string IV => Get("TPGame.JDBFI.IV");

        public static string KEY => Get("TPGame.JDBFI.KEY");

        public static string Parent => Get("TPGame.JDBFI.Parent");
    }
}