using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgDataBase.Model
{
    public class AGQueryOrderApiParamModel
    {
        public string Url { get; set; }
        
        public string Cagent { get; set; }
        
        public string Billno { get; set; }
        
        public string Cur { get; set; }

        public string DesKey { get; set; }

        public string Md5Key { get; set; }

        public string Actype { get; set; }
    }

    public class AGTransferMoneyApiParamModel : AGQueryOrderApiParamModel
    {
        public string LoginName { get; set; }

        public string Password { get; set; }

        public string Type { get; set; }

        public string Credit { get; set; }
    }

    public class AGApiParamModel : AGTransferMoneyApiParamModel
    {
        public int TransferID { get; set; }

        public int UserId { get; set; }

        public int OrderStatus { get; set; }

        public decimal Amount { get; set; }
    }

    public class AGApiParamModelJob
    {
        public JobTypes JobType { get; set; }
        public AGApiParamModel AGApiParam { get; set; }
    }

    public enum JobTypes
    {
        TransferOrder,        
        Recheck,
    }
}

