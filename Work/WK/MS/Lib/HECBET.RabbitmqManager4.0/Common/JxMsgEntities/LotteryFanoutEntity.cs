namespace JxMsgEntities
{
    using System;
    using System.Runtime.CompilerServices;

    public class LotteryFanoutEntity : BaseEntity
    {
        public string LotteryName { get; set; }

        public string Money { get; set; }

        public string Name { get; set; }

        public string PeriodNum { get; set; }

        public string Time { get; set; }
    }
}

