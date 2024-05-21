using JxBackendService.Model.Entity.BackSideUser;

namespace JxBackendService.Model.ViewModel.Authenticator
{
    public class BWUserAuthenticatorInfo
    {
        public BWUserAuthenticator BWUserAuthenticator { get; set; }

        public BWUserAuthenticatorStatuses BWUserAuthenticatorStatus { get; set; }
    }

    public enum BWUserAuthenticatorStatuses
    {
        /// <summary>
        /// 已綁定
        /// </summary>
        Verified,

        /// <summary>
        /// 未綁定
        /// </summary>
        NoVerified,

        /// <summary>
        /// 綁定已過期
        /// </summary>
        Expired,
    }
}