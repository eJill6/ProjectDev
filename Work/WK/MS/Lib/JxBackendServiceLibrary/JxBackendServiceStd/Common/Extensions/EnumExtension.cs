using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace JxBackendService.Common.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}