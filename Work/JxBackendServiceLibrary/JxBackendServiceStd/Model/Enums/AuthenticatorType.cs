using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using JxBackendService.Service.BackSideWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class AuthenticatorType : BaseStringValueModel<AuthenticatorType>
    {
        public RegularExpressionType ValidRegularExpressionType { get; private set; }

        public Type AuthenticatorServiceType { get; private set; }

        public FormInputType InputType { get; private set; }

        private AuthenticatorType()
        { }

        public static AuthenticatorType Google = new AuthenticatorType()
        {
            Value = "Google",
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.InputType_Google),
            AuthenticatorServiceType = typeof(BWAuthenticatorService),
            InputType = FormInputType.Google,
            ValidRegularExpressionType = RegularExpressionType.PositiveInteger,
            Sort = 1
        };

        public static AuthenticatorType GetSingleByFormInputType(string formInputTypeValue) => GetSingle(FormInputType.GetSingle(formInputTypeValue));

        public static AuthenticatorType GetSingle(FormInputType formInputType) => GetAll().SingleOrDefault(s => s.InputType == formInputType);
    }
}
