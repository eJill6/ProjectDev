using JxBackendService.Model.MessageQueue;

namespace JxBackendService.Model.Param.Finance
{
    public class TransferToMiseLiveParam
    {
        public int UserID { get; set; }

        public decimal Amount { get; set; }

        public string ProductCode { get; set; }

        public RoutingSetting RoutingSetting { get; set; }
    }
}