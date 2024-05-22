using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMeBetDataBase.Model
{
    public class IMeBetBalanceInfo : ApiResult
    {
        public string Balance { get; set; }
        public string Currency { get; set; }
    }
}
