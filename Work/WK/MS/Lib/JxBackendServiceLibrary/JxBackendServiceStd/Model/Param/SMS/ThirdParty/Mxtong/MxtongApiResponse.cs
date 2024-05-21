using System.Collections.Generic;

namespace JxBackendService.Model.Param.SMS.ThirdParty.Mxtong
{
    public class BaseMxtongApiResult
    {
        public int? Result { get; set; }

        public string Ts { get; set; }

        public bool IsSuccess => Result == 0;
    }

    public class MxtongChinaSendSMSResult : BaseMxtongApiResult
    {
        public string Msgid { get; set; }
    }

    public class MxtongInternationalSendSMSResult
    {
        public int? Code { get; set; }

        public string Msg { get; set; }

        public int? Msg_Id { get; set; }

        public string Failed_data { get; set; }

        public bool IsSuccess => Code == 0;
    }
}