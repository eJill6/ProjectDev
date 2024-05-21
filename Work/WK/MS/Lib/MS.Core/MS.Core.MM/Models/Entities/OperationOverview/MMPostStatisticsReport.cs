using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Entities.OperationOverview
{
    public class MMPostStatisticsReport
    {
        public string Id { get; set; }
        public byte AggregateType { get; set; }
        public decimal Amount { get; set; }
        public byte DateType { get; set; }
        public DateTime StatisticsTime { get; set; }
    }
}
