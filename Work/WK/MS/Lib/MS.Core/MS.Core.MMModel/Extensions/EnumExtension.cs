using MS.Core.MM.Attributes;
using MS.Core.MMModel.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MS.Core.MMModel.Extensions
{
    public static class EnumExtension
    {
        public static List<T> ToList<T>(this Enum value)
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static IEnumerable<T> ToEnumerable<T>(this Enum value)
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static IEnumerable<T> GetAll<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

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

        public static string GetDescription<T>(this Enum value) where T : DescriptionAttributeBase
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            T[] attributes = fieldInfo.GetCustomAttributes(typeof(T), false) as T[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        public static Dictionary<string, object> GetSelectListItemDic<T1, T2>() where T1 : DescriptionAttribute
        {
            var dic = new Dictionary<string, object>();
            Type enumType = typeof(T2);
            if (enumType.IsEnum)
            {
                foreach (var item in Enum.GetNames(enumType))
                {
                    var description = string.Empty;
                    var field = enumType.GetField(item);
                    T1[] attributes = field.GetCustomAttributes(typeof(T1), false) as T1[];
                    if (attributes != null && attributes.Any())
                        description = attributes.First().Description;
                    else
                        description = item;
                    dic.Add(description, Enum.Parse(enumType, item));
                }
            }
            return dic;
        }
    }
}