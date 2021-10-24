using System;
using System.ComponentModel.DataAnnotations;
using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.Entity
{
    public class BlockChainInfo
    {
        [Key]
        public int BlockChainID { get; set; }

        public int UserID { get; set; }

        [VarcharColumnInfo(160)]
        public string WalletAddr { get; set; }

        public DateTime? ApplyTime { get; set; }

        public bool IsActive { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
