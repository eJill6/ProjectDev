namespace JxMsgEntities
{
    using System;
    using System.Runtime.CompilerServices;

    public class KJEntity : BaseEntity
    {
        public decimal AvailableScore { get; set; }

        public decimal FrozenScore { get; set; }

        public decimal ProfitAndLossScore { get; set; }

        public string Summary { get; set; }
    }
}

