using ProductTransferService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class ProductTransferScheduleMockService : ProductTransferScheduleService
    {
        protected override bool IsClearExpiredProfitLossJobEnabled => false;

        protected override bool IsRecheckProcessingStatusOrderJobEnabled => false;

        protected override bool IsSaveBetLogToPlatformJobEnabled => false;

        protected override bool IsSaveBetLogToSQLiteJobEnabled => false;

        protected override bool IsTransferAllOutJobEnabled => true;
    }
}
