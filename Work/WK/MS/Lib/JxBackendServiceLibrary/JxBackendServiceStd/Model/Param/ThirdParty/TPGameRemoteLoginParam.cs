using JxBackendService.Interface.Model.Common;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class TPGameRemoteLoginParam : IInvocationUserParam
    {
        public CreateRemoteAccountParam CreateRemoteAccountParam { get; set; }

        public string IpAddress { get; set; }

        public bool IsMobile { get; set; }

        public LoginInfo LoginInfo { get; set; }
        
        public int UserID { get; set; }

        public string CorrelationId { get; set; }
    }
}