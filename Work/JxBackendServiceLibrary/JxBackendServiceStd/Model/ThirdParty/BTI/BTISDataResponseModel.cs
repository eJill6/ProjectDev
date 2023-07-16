using JxBackendService.Model.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public string Code { get; private set; }

        public static BTISDataErrorCode Success = new BTISDataErrorCode() { Value = 0, Code = nameof(Success) };

        public static BTISDataErrorCode CustomerDoesNotExist = new BTISDataErrorCode() { Value = -2, Code = nameof(CustomerDoesNotExist) };

        public static BTISDataErrorCode GeneralError = new BTISDataErrorCode() { Value = -3, Code = nameof(GeneralError) };

        public static BTISDataErrorCode InvalidOrMissingParameters = new BTISDataErrorCode() { Value = -4, Code = nameof(InvalidOrMissingParameters) };

        public static BTISDataErrorCode WrongAgentUsernameOrPassword = new BTISDataErrorCode() { Value = -5, Code = nameof(WrongAgentUsernameOrPassword) };

        public static BTISDataErrorCode ExceedQueryPeriod = new BTISDataErrorCode() { Value = -8, Code = nameof(ExceedQueryPeriod) };

        public static BTISDataErrorCode ExceededApiCalls = new BTISDataErrorCode() { Value = -9, Code = nameof(ExceededApiCalls) };

        public static BTISDataErrorCode ApiMethodIsNotAllowed = new BTISDataErrorCode() { Value = -10, Code = nameof(ApiMethodIsNotAllowed) };

        public static BTISDataErrorCode WrongOrExpiredToken = new BTISDataErrorCode() { Value = -1000, Code = nameof(WrongOrExpiredToken) };
    }
}