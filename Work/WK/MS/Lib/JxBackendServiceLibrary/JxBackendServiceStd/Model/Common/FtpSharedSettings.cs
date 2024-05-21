using JxBackendService.Common.Util;
using JxBackendService.Model.Ftp;

namespace JxBackendService.Model.Common
{
    public class FtpSharedSettings : SharedAppSettings
    {
        protected FtpSharedSettings() { }

        public static readonly FtpLoginParam FtpLoginParam = new FtpLoginParam()
        {
            FtpAddress = Get("FtpServerAddress"),
            FtpUser = Get("FtpServerUser"),
            FtpPassword = Get("FtpServerPassword"),
            FtpPort = GetFtpPort(),
        };

        public static string FtpRemoteFilePath => Get("FtpServerRemoteFilePath");

        public static string SaveLocalFilePath => Get("SaveLocalFilePath");

        private static int GetFtpPort()
        {
            string ftpPort = Get("FtpServerPort");

            if (ftpPort.IsNullOrEmpty())
            {
                return 21;
            }

            return int.Parse(ftpPort);
        }
    }
}
