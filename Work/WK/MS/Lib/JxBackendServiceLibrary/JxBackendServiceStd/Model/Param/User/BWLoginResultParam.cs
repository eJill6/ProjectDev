using JxBackendService.Model.Enums.BackSideWeb.Login;

namespace JxBackendService.Model.Param.User
{
    public class BWLoginResultParam
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string LoginIp { get; set; }

        public System.DateTime LoginTime { get; set; }

        public string MachineName { get; set; }

        public string WinLoginName { get; set; }

        public string LocalIP { get; set; }

        public System.DateTime LocalUTCTime { get; set; }

        public string LoginToolVersion { get; set; }

        public LoginStatuses LoginStatus { get; set; }

    }
}
