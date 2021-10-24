using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JxBackendService.Common.Util
{
    public static class DecimalUtil
    {
        
        public static decimal Floor(decimal value, int digits)
        {
            decimal multiplier = 1;

            for (int i = 0; i < digits; i++)
            {
                multiplier *= 10;
            }

            return Math.Truncate(value * multiplier) / multiplier;
        }
    }
}
