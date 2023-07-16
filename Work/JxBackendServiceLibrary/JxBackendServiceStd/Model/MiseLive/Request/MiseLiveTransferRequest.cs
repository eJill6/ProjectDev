using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Model.Attributes.Security;

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
    }

    public class MiseLiveTransferRequestParam : IMiseLiveTransferRequestParam
    {
        public int UserId { get; set; }

        public string OrderNo { get; set; }

        public decimal Amount { get; set; }
    }
}