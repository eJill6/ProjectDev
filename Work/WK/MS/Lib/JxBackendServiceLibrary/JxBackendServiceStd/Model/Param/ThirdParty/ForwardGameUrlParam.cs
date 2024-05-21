using JxBackendService.Interface.Model.Common;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class ForwardGameUrlParam : IInvocationUserParam
    {
        public BaseBasicUserInfo LoginUser { get; set; }

        public string IpAddress { get; set; }

        public bool IsMobile { get; set; }

        /// <summary>登入時需要的額外資訊,例如GameCode</summary>
        public LoginInfo LoginInfo { get; set; } = new LoginInfo();

        public int UserID { get => LoginUser.UserId; set => LoginUser.UserId = value; }

        public string CorrelationId { get; set; }
    }
}