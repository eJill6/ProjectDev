using JxBackendService.Interface.Model.Common;

namespace SLPolyGame.Web.Model
{
    public class ForwardGameUrlSVApiParam : IInvocationParam
    {
        public string ProductCode { get; set; }

        public string LoginInfoJson { get; set; }

        public bool IsMobile { get; set; }

        public string CorrelationId { get; set; }
    }
}