using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMBGDataBase.Model
{
    public class IMBGMoneyOutInfo
    {
        public string MoneyOutID { get; set; }
        public decimal Amount { get; set; }
        public string OrderID { get; set; }
        public DateTime OrderTime { get; set; }
        public string Handle { get; set; }
        public DateTime HandTime { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
        public string Memo { get; set; }
    }
}
