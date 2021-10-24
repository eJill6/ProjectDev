using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.Mail
{
    public class InternalMailApiResponse
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }

        public bool IsSuccess => Code == 0;
    }
}
