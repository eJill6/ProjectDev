using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace JxBackendService.Common.Util.Xml
{
    public static class DataTableUtil
    {
        public static void AppendRows<T>(this DataTable dataTable, IEnumerable<T> list)
        {
            foreach (T model in list)
            {
                DataRow dataRow = dataTable.NewRow();

                foreach (string propertyName in ModelUtil.GetPropertiesNameOfType(model.GetType()))
                {
                    PropertyInfo propertyInfo = model.GetType().GetProperty(propertyName);
                    dataRow[propertyInfo.Name] = ReflectUtil.GetValue(propertyInfo, model);
                }

                dataTable.Rows.Add(dataRow);
            }
        }

        public static Dictionary<string, object> ToDictionary(this DataRow row)
        {
            var resultMap = new Dictionary<string, object>();

            // Iterate through the columns and add them to the dictionary
            foreach (DataColumn column in row.Table.Columns)
            {
                string columnName = column.ColumnName;
                object columnValue = row[column];
                resultMap[columnName] = columnValue;
            }

            return resultMap;
        }
    }
}