using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Param.SMS
{
    public class SendUserSMSParam
    {
        /// <summary>國碼</summary>
        public string CountryCode { get; set; }

        /// <summary>簡訊用途</summary>
        public SMSUsages Usage { get; set; }

        /// <summary>要寄送到的手機號碼</summary>
        public string PhoneNo { get; set; }

        /// <summary>簡訊參數</summary>
        public string ContentParamInfo { get; set; }
    }

    public enum SMSUsages
    {
        ValidateCode = 1,
    }

    public class CountryCode : BaseStringValueModel<CountryCode>
    {
        private CountryCode()
        { }

        /// <summary>中國</summary>
        public static CountryCode China = new CountryCode()
        {
            Value = "86"
        };

        /// <summary>臺灣(台灣)</summary>
        public static CountryCode Taiwan = new CountryCode()
        {
            Value = "886"
        };
    }
}