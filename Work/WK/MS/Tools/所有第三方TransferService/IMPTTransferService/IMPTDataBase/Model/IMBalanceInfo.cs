using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPTDataBase.Model
{
    public class IMBalanceInfo : ApiResult
    {
        public string Balance { get; set; }
        public string Currency { get; set; }
    }
}
