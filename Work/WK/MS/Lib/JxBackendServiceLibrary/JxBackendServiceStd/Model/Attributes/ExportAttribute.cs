using JxBackendService.Common.Util;
using System;
using System.Reflection;

namespace JxBackendService.Model.Attributes
{
    public class ExportAttribute : Attribute
    {
        public string ResourcePropertyName { get; set; }

        public Type ResourceType { get; set; }

        public int Sort { get; set; } = int.MaxValue;

        public string GetNameByResourceInfo()
        {
            if (ResourceType == null || ResourcePropertyName.IsNullOrEmpty())
            {
                return string.Empty;
            }

            PropertyInfo propertyInfo = ResourceType.GetProperty(ResourcePropertyName, BindingFlags.Public | BindingFlags.Static);

            if (propertyInfo == null)
            {
                return string.Empty;
            }

            return propertyInfo.GetValue(null, null).ToNonNullString();
        }
    }
}