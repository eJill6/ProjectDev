using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SLPolyGame.Web.Helpers
{
    public static class TableHelper
    {
        #region TABLE轉集合

        public static List<T> TableToList<T>(this DataTable table) where T : new()
        {
            IList<System.Reflection.PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            if (table != null)
            {
                //取得DataTable所有的row data
                foreach (var row in table.Rows)
                {
                    var item = MappingItem<T>((DataRow)row, properties);
                    result.Add(item);
                }
            }

            return result;
        }

        private static T MappingItem<T>(DataRow row, IList<System.Reflection.PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (row.Table.Columns.Contains(property.Name))
                {
                    //針對欄位的型態去轉換
                    if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime dt = new DateTime();
                        if (DateTime.TryParse(row[property.Name].ToString(), out dt))
                        {
                            property.SetValue(item, dt, null);
                        }
                        else
                        {
                            property.SetValue(item, null, null);
                        }
                    }
                    else if (property.PropertyType == typeof(decimal))
                    {
                        decimal val = new decimal();
                        decimal.TryParse(row[property.Name].ToString(), out val);
                        property.SetValue(item, val, null);
                    }
                    else if (property.PropertyType == typeof(double))
                    {
                        double val = new double();
                        double.TryParse(row[property.Name].ToString(), out val);
                        property.SetValue(item, val, null);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        int val = new int();
                        int.TryParse(row[property.Name].ToString(), out val);
                        property.SetValue(item, val, null);
                    }
                    else if (property.PropertyType == typeof(byte)) //tinyint
                    {
                        byte val = new byte();
                        byte.TryParse(row[property.Name].ToString(), out val);
                        property.SetValue(item, val, null);
                    }
                    else if (property.PropertyType == typeof(long)) //BigInt
                    {
                        long val = new long();
                        long.TryParse(row[property.Name].ToString(), out val);
                        property.SetValue(item, val, null);
                    }
                    else if (property.PropertyType == typeof(short)) //smallint
                    {
                        short val = new short();
                        short.TryParse(row[property.Name].ToString(), out val);
                        property.SetValue(item, val, null);
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        bool val = false;
                        bool.TryParse(row[property.Name].ToString(), out val);
                        property.SetValue(item, val, null);
                    }
                    else
                    {
                        if (row[property.Name] != DBNull.Value)
                        {
                            property.SetValue(item, row[property.Name], null);
                        }
                    }
                }
            }

            return item;
        }

        #endregion TABLE轉集合
    }
}