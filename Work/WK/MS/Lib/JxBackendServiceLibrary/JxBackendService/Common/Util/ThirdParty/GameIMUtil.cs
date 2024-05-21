using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Common.Util.ThirdParty
{
    public static class GameIMUtil
    {
        public static string ToBetRecordTimeFormatString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH.mm.ss");
        }

        public static DateTime ToBetRecordDateTime(string betRecordTimeString)
        {
            return DateTime.Parse(betRecordTimeString.Replace(".", ":"));
        }
    }
}
