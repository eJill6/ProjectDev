using JxBackendService.Interface.Model.Common;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class TransferOutUserDetail : IInvocationParam
    {
        public BaseBasicUserInfo AffectedUser { get; set; }

        public bool IsOperateByBackSide { get; set; }

        public string CorrelationId { get; set; }

        public bool IsCompensation { get; set; }
    }
}