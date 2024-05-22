using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LCTransferService.DataType
{
    [DataContract]
    public class FundTransferResult
    {
        [DataMember]
        public int trans_id { get; set; }
        [DataMember]
        public decimal before_amount { get; set; }
        [DataMember]
        public decimal after_amount { get; set; }
        [DataMember]
        public string system_id { get; set; }
        [DataMember]
        public int status { get; set; }

        #region for CheckFundTransfer Result
        [DataMember]
        public string transfer_date { get; set; }
        [DataMember]
        public decimal amount { get; set; }
        [DataMember]
        public int currency { get; set; }

        public DateTime transfer_data1
        {
            get
            {
                return DateTime.Parse(transfer_date);
            }
        }
        #endregion
    }
}
