using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class FrontSideMainMenu
    {
        /// <summary>產品代碼</summary>
        [NVarcharColumnInfo(50)]
        public string ProductCode { get; set; }

        /// <summary>本地遊戲代碼</summary>
        [NVarcharColumnInfo(50)]
        public string GameCode { get; set; }
    }

    public class BaseGameCenterLogin : FrontSideMainMenu, ILoginInfo
    {
        /// <summary>遠端遊戲代碼</summary>
        public string RemoteCode { get; set; }
    }

    public class WebGameCenterLogin : BaseGameCenterLogin
    {
        public string Title { get; set; }
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