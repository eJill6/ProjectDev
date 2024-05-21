using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToFormatDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}