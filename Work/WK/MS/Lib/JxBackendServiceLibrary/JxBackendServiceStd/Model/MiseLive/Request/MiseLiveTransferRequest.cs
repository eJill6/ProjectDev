using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Model.Attributes.Security;
using Newtonsoft.Json;

namespace JxBackendService.Model.MiseLive.Request
{
    public class MiseLiveTransferRequest : BaseMiseUserSaltRequest, IMiseLiveTransferRequest
    {
        public MiseLiveTransferRequest() : base()
        {
        }

        [MiseLiveSign]
        public string OrderNo { get; set; }

        [MiseLiveSign]
        public decimal Amount { get; set; }

        [MiseLiveSign]
        public string App => GetDefaultApp();

        [JsonIgnore]
        public string CorrelationId { get; set; }
    }

    public class MiseLiveTransferRequestParam : IMiseLiveTransferRequestParam
    {
        /// <summary>第三方使用的UserId</summary>
        public int UserId { get; set; }

        /// <summary cref="IInvocationUserParam.UserID"></summary>
        public int UserID { get => UserId; set => UserId = value; }

        public string OrderNo { get; set; }

        public decimal Amount { get; set; }

        public string CorrelationId { get; set; }
    }
}