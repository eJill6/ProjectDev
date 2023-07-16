using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.EVO
{
    public class EVBaseResponseModel
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string DateTime { get; set; }
    }

    public class EVBaseResponseWtihDataModel<T> : EVBaseResponseModel
    {
        public T Data { get; set; }
    }
}
