using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Param.User
{
    public class UserRegisterParam
    {
        public string UserName { get; set; } 
        public string UserPwdHash { get; set; }
        public int ParentID { get; set; }
        public decimal RebatePro { get; set; }
        public decimal MaxRebatePro { get; set; }
        public decimal? UpgradRebatePro { get; set; }
        public int CustomerType { get; set; }
        public string RC_Success => ReturnCode.Success.Value;
        public string RC_RegisterFail => ReturnCode.RegisterFailedPleaseTryLater.Value;
    }
}
