namespace JxBackendService.Model.Entity
{
    public class BWLoginDetail
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string LoginIp { get; set; }

        public System.DateTime LoginTime { get; set; }

        public string MachineName { get; set; }

        public string WinLoginName { get; set; }

        public string LocalIP { get; set; }

        public System.DateTime LocalUTCTime { get; set; }

        public bool LoginFail { get; set; }

        public string LoginToolVersion { get; set; }

        public int? LoginStatus { get; set; }
    }
}
