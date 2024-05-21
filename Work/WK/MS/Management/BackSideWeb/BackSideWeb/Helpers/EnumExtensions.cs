using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

public static class EnumExtensions
{
    public static string GetEnumDescription(this Enum value)
    {
        var memberInfo = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var descriptionAttribute = memberInfo?.GetCustomAttribute<DescriptionAttribute>();

        return descriptionAttribute != null ? descriptionAttribute.Description : value.ToString();
    }
}