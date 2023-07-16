using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JxBackendService.Service.Attribute
{
    public class RequireIfService : IRequiredIfService
    {
        public bool IsValid(object value, object instance, string otherPropertyName, object[] otherPropertyValidValues)
        {
            Type type = instance.GetType();
            PropertyInfo propertyInfo = type.GetProperty(otherPropertyName);
            object otherPropertyValue = propertyInfo.GetValue(instance);

            if (!otherPropertyValidValues.Any(a => a.Equals(otherPropertyValue)))
            {
                return true; //不檢查必填
            }

            if (value == null || (value is string && value.ToNonNullString().IsNullOrEmpty()))
            {
                return false;
            }

            return true;
        }
    }
}
