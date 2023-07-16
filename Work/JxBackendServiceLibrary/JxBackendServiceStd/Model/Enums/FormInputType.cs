using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class FormInputType : BaseStringValueModel<FormInputType>
    {
        public bool HasSendVerifyCodeButton { get; private set; }

        private FormInputType()
        {
            ResourceType = typeof(SelectItemElement);
        }

        public static readonly FormInputType Google = new FormInputType()
        {
            Value = "Google",
            ResourcePropertyName = nameof(SelectItemElement.InputType_Google),
            HasSendVerifyCodeButton = false,
        };
    }
}
