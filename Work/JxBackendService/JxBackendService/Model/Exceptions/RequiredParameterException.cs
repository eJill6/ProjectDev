using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Exceptions
{
    public class RequiredParameterException : FlowControlException
    {
        public RequiredParameterException(string fieldName)
            : base(string.Format(MessageElement.OldPasswordIsNotValid, fieldName.ToNonNullString()).ToTrimString())
        {

        }
    }
}
