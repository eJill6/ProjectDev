using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

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
    }
}
