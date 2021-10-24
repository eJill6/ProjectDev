using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class LoginType : BaseIntValueModel<LoginType>
    {
        public RegularExpressionType RegularExpressionType { get; private set; }

        private LoginType() { }

        public static LoginType UserName = new LoginType()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.LoginType_UserName)
        };

        public static LoginType Email = new LoginType()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.LoginType_Email)
        };
    }
}
