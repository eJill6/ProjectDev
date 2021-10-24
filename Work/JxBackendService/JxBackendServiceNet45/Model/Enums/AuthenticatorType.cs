using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using JxBackendServiceNet45.Service.Authenticator;
using System;

namespace JxBackendServiceNet45.Model.Enums
{
    public class AuthenticatorType : BaseStringValueModel<AuthenticatorType>
    {
        public Type AuthenticatorServiceType { get; private set; }

        private AuthenticatorType() { }

        public static AuthenticatorType Google = new AuthenticatorType()
        {
            Value = "Google",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.AuthenticatorType_Google),
            AuthenticatorServiceType = typeof(GoogleAuthenticatorService),
            Sort = 1
        };
    }
}
