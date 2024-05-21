using JxBackendService.Model.Enums;
using System;

namespace JxBackendService.Model.ThirdParty.BTI
{
    public class BTISBaseDataResponse
    {
        public int ErrorCode { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public bool IsSuccess => ErrorCode == BTISDataErrorCode.Success.Value;

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
    }

    public class BTISGetTokenResponse : BTISBaseDataResponse
    {
        public string Token { get; set; }
    }

    public class BTISDataErrorCode : BaseIntValueModel<BTISDataErrorCode>
    {
        private BTISDataErrorCode()
        { }

        public string Message { get; private set; }

        public static BTISDataErrorCode Success = new BTISDataErrorCode() { Value = 0, Message = nameof(Success) };

        public static BTISDataErrorCode CustomerDoesNotExist = new BTISDataErrorCode() { Value = -2, Message = nameof(CustomerDoesNotExist) };

        public static BTISDataErrorCode GeneralError = new BTISDataErrorCode() { Value = -3, Message = nameof(GeneralError) };

        public static BTISDataErrorCode InvalidOrMissingParameters = new BTISDataErrorCode() { Value = -4, Message = nameof(InvalidOrMissingParameters) };

        public static BTISDataErrorCode WrongAgentUsernameOrPassword = new BTISDataErrorCode() { Value = -5, Message = nameof(WrongAgentUsernameOrPassword) };

        public static BTISDataErrorCode TooManyRequests = new BTISDataErrorCode() { Value = -7, Message = nameof(TooManyRequests) };

        public static BTISDataErrorCode ExceedQueryPeriod = new BTISDataErrorCode() { Value = -8, Message = nameof(ExceedQueryPeriod) };

        public static BTISDataErrorCode ExceededApiCalls = new BTISDataErrorCode() { Value = -9, Message = nameof(ExceededApiCalls) };

        public static BTISDataErrorCode ApiMethodIsNotAllowed = new BTISDataErrorCode() { Value = -10, Message = nameof(ApiMethodIsNotAllowed) };

        public static BTISDataErrorCode WrongOrExpiredToken = new BTISDataErrorCode() { Value = -1000, Message = nameof(WrongOrExpiredToken) };
    }
}