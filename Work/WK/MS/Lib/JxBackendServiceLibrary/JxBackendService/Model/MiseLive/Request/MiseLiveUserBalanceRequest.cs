using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Model.Attributes.Security;

namespace JxBackendService.Model.MiseLive.Request
{
    public class MiseLiveUserBalanceRequest : BaseMiseUserSaltRequest, IMiseLiveUserBalanceRequest
    {
        public MiseLiveUserBalanceRequest()
        {
        }
    }

    public class MiseLiveUserBalanceRequestParam : IMiseLiveUserBalanceRequestParam
    {
        public MiseLiveUserBalanceRequestParam()
        {
        }

        public int UserId { get; set; }
    }
}