using JxBackendService.Interface.Model.Common;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class ProcessSuccessOrderParam
    {
        public bool IsMoneyIn { get; set; }

        public BaseTPGameMoneyInfo TPGameMoneyInfo { get; set; }

        public UserScore RemoteUserScore { get; set; }
    }

    public class BaseTPGameTranfserParam
    {
        public int UserID { get; set; }

        public string CorrelationId { get; set; }
    }

    public class TPGameTranfserOutParam : BaseTPGameTranfserParam, IInvocationUserParam
    {
    }

    public class TPGameTranfserParam : BaseTPGameTranfserParam, IInvocationUserParam
    {
        public decimal Amount { get; set; }
    }
}