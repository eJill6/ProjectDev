using JxBackendService.Model.Enums;

namespace JxBackendService.Model.StoredProcedureParam.VIP
{
    public class RegisterVIPUserParam
    {
        public int UserID { get; set; }

        public string CreateUser { get; set; }

        public string RC_Success => ReturnCode.Success.Value;

        public string RC_SystemError => ReturnCode.SystemError.Value;
    }
}