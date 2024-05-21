using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolyDataBase.Helpers
{
    public class DBTypeHelper
    {
        public static object ConvertNullToDBNull(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }
    }
}
