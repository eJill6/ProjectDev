namespace JxMsgEntities
{
    using System;
    using System.Runtime.CompilerServices;

    public class TransferNoticeEntity : BaseEntity
    {
        public decimal Amount { get; set; }

        public string Summary { get; set; }
    }
}

