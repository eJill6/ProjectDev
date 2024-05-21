using System.Diagnostics;

namespace JxBackendService.Model.Param.Filter
{
    public class ApiLogRequestHttpContextItem
    {
        public string CorrelationId { get; set; }

        public string Url { get; set; }

        public string RawRequest { get; set; }

        public Stopwatch Stopwatch { get; set; }
    }
}