using System.Drawing;
using Newtonsoft.Json;

namespace JxBackendService.Model.Param.SMS.ThirdParty.Mxtong
{
    public class MxtongSMSSetting
    {
        public string ServiceUrl { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

        public string TemplateInfo { get; set; }
    }

    public class MxtongInternationalRequest
    {
        public string Sp_id { get; set; }

        public string Mobile { get; set; }

        public string Content { get; set; }

        public string Password { get; set; }
    }
}