using Serenity.Services;

namespace Management.Web.Modules.Common.Helpers
{
    public class ShareModel: ServiceRequest
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
