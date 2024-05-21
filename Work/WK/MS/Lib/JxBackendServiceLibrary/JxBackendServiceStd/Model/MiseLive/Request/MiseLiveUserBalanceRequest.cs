using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Model.MiseLive.Request;
using Newtonsoft.Json;

namespace JxBackendService.Model.MiseLive.Request
{
    public class MiseLiveUserBalanceRequest : BaseMiseUserSaltRequest, IMiseLiveUserBalanceRequest
    {
        public MiseLiveUserBalanceRequest()
        {
        }

        [JsonIgnore]
        public string CorrelationId { get; set; }
    }

    public class MiseLiveUserBalanceRequestParam : IMiseLiveUserBalanceRequestParam
    {
        public MiseLiveUserBalanceRequestParam()
        {
        }

        /// <summary>第三方使用的UserId</summary>
        public int UserId { get; set; }

        /// <summary cref="IInvocationUserParam.UserID"></summary>
        public int UserID { get => UserId; set => UserId = value; }

        public string CorrelationId { get; set; }
    }
}