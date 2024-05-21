using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Model.Attributes.Security;

namespace JxBackendService.Model.MiseLive.Request
{
    public class MiseLiveTransferOrderRequest : BaseMiseLiveSaltRequest, IMiseLiveTransferOrderRequest
    {
        [MiseLiveSign]
        public string OrderNo { get; set; }

        [MiseLiveSign]
        public string App => GetDefaultApp();
    }

    public class MiseLiveTransferOrderRequestParam : IMiseLiveTransferOrderRequestParam
    {
        public string OrderNo { get; set; }
    }
}