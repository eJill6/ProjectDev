using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class MxtongSMSErrorCode : BaseIntValueModel<MxtongSMSErrorCode>
    {
        private MxtongSMSErrorCode()
        {
            ResourceType = typeof(SMSErrorCodeElement);
        }

        public static MxtongSMSErrorCode NoSuchUser = new MxtongSMSErrorCode()
        {
            Value = 101,
            ResourcePropertyName = nameof(SMSErrorCodeElement.NoSuchUser),
        };

        public static MxtongSMSErrorCode WrongPassword = new MxtongSMSErrorCode()
        {
            Value = 102,
            ResourcePropertyName = nameof(SMSErrorCodeElement.WrongPassword),
        };

        public static MxtongSMSErrorCode SubmittingTooFast = new MxtongSMSErrorCode()
        {
            Value = 103,
            ResourcePropertyName = nameof(SMSErrorCodeElement.SubmittingTooFast),
        };

        public static MxtongSMSErrorCode SystemBusy = new MxtongSMSErrorCode()
        {
            Value = 104,
            ResourcePropertyName = nameof(SMSErrorCodeElement.SystemBusy),
        };

        public static MxtongSMSErrorCode SensitiveSMS = new MxtongSMSErrorCode()
        {
            Value = 105,
            ResourcePropertyName = nameof(SMSErrorCodeElement.SensitiveSMS),
        };

        public static MxtongSMSErrorCode WrongMessageLength = new MxtongSMSErrorCode()
        {
            Value = 106,
            ResourcePropertyName = nameof(SMSErrorCodeElement.WrongMessageLength),
        };

        public static MxtongSMSErrorCode ContainsWrongPhoneNumber = new MxtongSMSErrorCode()
        {
            Value = 107,
            ResourcePropertyName = nameof(SMSErrorCodeElement.ContainsWrongPhoneNumber),
        };

        public static MxtongSMSErrorCode TheNumberOfMobilePhoneNumbersWrong = new MxtongSMSErrorCode()
        {
            Value = 108,
            ResourcePropertyName = nameof(SMSErrorCodeElement.TheNumberOfMobilePhoneNumbersWrong),
        };

        public static MxtongSMSErrorCode NoSendingQuota = new MxtongSMSErrorCode()
        {
            Value = 109,
            ResourcePropertyName = nameof(SMSErrorCodeElement.NoSendingQuota),
        };

        public static MxtongSMSErrorCode NotWithinSendingTime = new MxtongSMSErrorCode()
        {
            Value = 110,
            ResourcePropertyName = nameof(SMSErrorCodeElement.NotWithinSendingTime),
        };

        public static MxtongSMSErrorCode OverAccountSendingLimitForTheMonth = new MxtongSMSErrorCode()
        {
            Value = 111,
            ResourcePropertyName = nameof(SMSErrorCodeElement.OverAccountSendingLimitForTheMonth),
        };

        public static MxtongSMSErrorCode NoSuchProduct = new MxtongSMSErrorCode()
        {
            Value = 112,
            ResourcePropertyName = nameof(SMSErrorCodeElement.NoSuchProduct),
        };

        public static MxtongSMSErrorCode ExtnoFormatError = new MxtongSMSErrorCode()
        {
            Value = 113,
            ResourcePropertyName = nameof(SMSErrorCodeElement.ExtnoFormatError),
        };

        public static MxtongSMSErrorCode TheNumberOfAvailableParameterGroupsIsWrong = new MxtongSMSErrorCode()
        {
            Value = 114,
            ResourcePropertyName = nameof(SMSErrorCodeElement.TheNumberOfAvailableParameterGroupsIsWrong),
        };

        public static MxtongSMSErrorCode AutomaticReviewRejection = new MxtongSMSErrorCode()
        {
            Value = 115,
            ResourcePropertyName = nameof(SMSErrorCodeElement.AutomaticReviewRejection),
        };

        public static MxtongSMSErrorCode SignatureInvalid = new MxtongSMSErrorCode()
        {
            Value = 116,
            ResourcePropertyName = nameof(SMSErrorCodeElement.SignatureInvalid),
        };

        public static MxtongSMSErrorCode IPAddressAuthenticationError = new MxtongSMSErrorCode()
        {
            Value = 117,
            ResourcePropertyName = nameof(SMSErrorCodeElement.IPAddressAuthenticationError),
        };

        public static MxtongSMSErrorCode UserHasNotPermissionToSend = new MxtongSMSErrorCode()
        {
            Value = 118,
            ResourcePropertyName = nameof(SMSErrorCodeElement.UserHasNotPermissionToSend),
        };

        public static MxtongSMSErrorCode UserExpired = new MxtongSMSErrorCode()
        {
            Value = 119,
            ResourcePropertyName = nameof(SMSErrorCodeElement.UserExpired),
        };

        public static MxtongSMSErrorCode ContentNotInWhitelistTemplate = new MxtongSMSErrorCode()
        {
            Value = 120,
            ResourcePropertyName = nameof(SMSErrorCodeElement.ContentNotInWhitelistTemplate),
        };
    }
}