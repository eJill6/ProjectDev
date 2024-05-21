using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PolyDataBase.Extensions
{
    public static class EnumExtension
    {
        /// <summary>Get description of enumerations
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}