using System;

namespace LCDataBase.Model
{
    public class LCMoneyInInfo
    {
        public string MoneyInID { get; set; }

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