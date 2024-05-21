using JxBackendService.Interface.Model.Common;

namespace JxBackendService.Model.GlobalSystem
{
    public class InvocationUserParam : IInvocationUserParam
    {
        public int UserID { get; set; }

        public string CorrelationId { get; set; }
    }
}