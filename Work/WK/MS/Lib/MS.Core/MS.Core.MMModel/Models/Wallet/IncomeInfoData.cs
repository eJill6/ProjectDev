using System;

namespace MS.Core.MMModel.Models.Wallet
{
    public class IncomeInfoData
    {
        public byte? PostType { get; set; }

        public DateTime Date { get; set; }

        public int? Page { get; set; }

        public int? PageNo { get; set; }
    }
}