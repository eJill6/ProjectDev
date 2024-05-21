using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PolyDataBase.Extensions
{
    public static class DataTableExtension
    {
        public static List<T> ExtConvertToList<T>(this DataTable table) where T : class, new()
        {
            if (table == null)
            {
                throw new ArgumentNullException("DataTable is null!");
            }

            var result = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                var model = Mapping<T>(table, row);
                result.Add(model);
            }
            return result;
        }

        public static T ExtConvertToModel<T>(this DataTable table) where T : class, new()
        {
            if (table == null)
            {
                throw new ArgumentNullException("DataTable is null!");
            }

            if (table.Rows.Count > 1)
            {
                throw new ArgumentNullException("DataTable's row count is more than 1!");
            }

            if (table.Rows.Count == 1)
            {
                var row = table.Rows[0];
                return Mapping<T>(table, row);
            }

            return default(T);
        }

        private static T Mapping<T>(DataTable table, DataRow row) where T : class, new()
        {
            var model = new T();
            foreach (var prop in model.GetType().GetProperties())
            {
                if (!prop.CanWrite)
                {
                    continue;
                }

                if (table.Columns.Contains(prop.Name))
                {
                    object propValue = null;
                    var value = row[prop.Name];
                    if (value != DBNull.Value)
                    {
                        propValue = value;
                    }

                    prop.SetValue(model, propValue, null);
                }
            }
            return model;
        }
    }
}