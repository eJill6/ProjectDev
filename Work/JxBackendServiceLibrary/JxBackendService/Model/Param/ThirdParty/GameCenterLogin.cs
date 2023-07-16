namespace JxBackendService.Model.Param.ThirdParty
{
    public class GameCenterLogin : ILoginInfo
    {
        public string ProductCode { get; set; }

        public string Title { get; set; }

        public string GameCode { get; set; }

        public string RemoteCode { get; set; }
    }

    public interface ILoginInfo
    {
        string GameCode { get; set; }

        string RemoteCode { get; set; }
    }

    public class LoginInfo : ILoginInfo
    {
        public string GameCode { get; set; }

        public string RemoteCode { get; set; }
    }
}