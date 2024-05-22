using System.Runtime.Serialization;
using JxBackendService.Model.ViewModel.ThirdParty.Old;

namespace ProductTransferService.SportDataBase.Model
{
    [DataContract]
    public class ApiResult : IOldBetLogModel
    {
        [DataMember(Order = 1)]
        public int error_code { get; set; }

        [DataMember(Order = 2)]
        public string message { get; set; }

        public string RemoteFileSeq { get; set; }

        public Action WriteRemoteContentToOtherMerchant { get; set; }
    }

    [DataContract]
    public class ApiResult<T> : ApiResult
    {
        [DataMember(Order = 3)]
        public T Data { get; set; }
    }
}