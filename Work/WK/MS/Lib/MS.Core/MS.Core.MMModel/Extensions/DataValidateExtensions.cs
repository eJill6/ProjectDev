using MS.Core.MMModel.Attributes.Validate;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MS.Core.MMModel.Extensions
{
    public static class DataValidateExtensions
    {
        public static string Validate<T>(this T t)
        {
            Type type = t.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            List<string> errors = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                if (property.IsDefined(typeof(BaseAttribute)))
                {
                    foreach (BaseAttribute attribute in property.GetCustomAttributes(typeof(BaseAttribute)))
                    {
                        if (!attribute.Validate(property.GetValue(t, null)))
                        {
                            errors.Add($"{property.Name}"+attribute.ValidateError);
                        }
                    }
                }
            }
            return string.Join(",", errors); 
        }
    }
}
