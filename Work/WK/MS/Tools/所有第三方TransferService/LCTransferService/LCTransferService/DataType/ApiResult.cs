using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LCTransferService.DataType
{
    [DataContract]
    public class ApiResult
    {
        [DataMember(Order = 1)]
        public int error_code { get; set; }
        [DataMember(Order = 2)]
        public string message { get; set; }
    }

    [DataContract]
    public class ApiResult<T> : ApiResult
    {
        [DataMember(Order = 3)]
        public T Data { get; set; }
    }
}
