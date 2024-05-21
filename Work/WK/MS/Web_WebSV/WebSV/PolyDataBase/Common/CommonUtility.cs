using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace SLPolyGame.Web.Common
{
    public class CommonUtility
    {
        public static List<T> ToList<T>(DataTable dt) where T : new()
        {
            List<T> ts = new List<T>();

            // 获得此模型的类型

            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                var type = new T();
                // 获得此模型的公共属性
                PropertyInfo[] propertys = type.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    // 检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            if ((value is long && pi.PropertyType == typeof(string))
                                || (value is int && pi.PropertyType == typeof(string))
                            )
                                pi.SetValue(type, value.ToString(), null);
                            else
                                pi.SetValue(type, value, null);
                        }
                    }
                }

                ts.Add(type);
            }

            return ts;
        }
    }
}
