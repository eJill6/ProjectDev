using JxBackendService.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JxBackendService.Common.Util
{
    public static class ReflectUtil
    {
        public static void SetPropertyValue(this object model, string propertyName, object value)
        {
            PropertyInfo prop = model.GetType().GetProperty(propertyName);

            if (prop == null || !prop.CanWrite)
            {
                return;
            }

            prop.SetValue(model, value, null);
        }

        public static void SetPropertyValueWhenNull(this object model, string propertyName, Func<object> getValue)
        {
            PropertyInfo prop = model.GetType().GetProperty(propertyName);

            if (prop == null)
            {
                return;
            }

            object originValue = prop.GetValue(model, null);

            if (originValue == null)
            {
                prop.SetValue(model, getValue.Invoke(), null);
            }
        }

        public static string GetFieldValue(Type type, string fieldName)
        {
            string fieldValue = null;
            FieldInfo info = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);

            if (info != null)
            {
                object value = info.GetValue(null);
                if (value != null)
                {
                    fieldValue = value.ToString();
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            return fieldValue;
        }

        public static FieldInfo[] GetAllFieldInfos<T>()
        {
            Type type = typeof(T);
            return type.GetFields();
        }

        public static List<T> GetAllFields<T>()
        {
            return GetAllFields<T>(GetAllFieldInfos<T>());
        }

        public static List<T> GetAllFields<T>(FieldInfo[] fieldInfos)
        {
            var results = new List<T>();
            Type type = typeof(T);

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                var value = (T)fieldInfo.GetValue(type);
                results.Add(value);
            }

            return results;
        }

        public static object GetValue(this PropertyInfo propertyInfo, object param)
        {
            return propertyInfo.GetValue(param, null);
        }

        public static string GenerateInsertSQL<T>() where T : class
        {
            return GenerateInsertSQL<T>(null, null, null);
        }

        public static string GenerateInsertSQL<T>(string dbName, string owner, string tableName) where T : class
        {
            if (tableName.IsNullOrEmpty())
            {
                tableName = ModelUtil.GetTableName(typeof(T));
            }

            string fullTableName = string.Join(".", new List<string>() { $"[{dbName}]", $"[{owner}]", $"[{tableName}]" }
             .Where(w => !w.IsNullOrEmpty()));

            return GenerateInsertSQL<T>(fullTableName);
        }

        public static string GenerateInsertSQL<T>(string fullTableName) where T : class
        {
            Type type = typeof(T);
            List<PropertyInfo> typeProperties = ModelUtil.TypePropertiesCache(type);
            List<PropertyInfo> keyProperties = ModelUtil.KeyPropertiesCache(type);

            List<PropertyInfo> explicitKeyProperties = ModelUtil.ExplicitKeyPropertiesCache(type);

            //擴充套件無法使用DynamicParameters, 若直接轉DynamicParameter,又會出現無法取得key,value的情況,只好先轉成自訂model再轉回來
            StringBuilder sql = new StringBuilder();
            IEnumerable<string> query = typeProperties.Select(s => s.Name);

            //排除自動增號欄位
            if (keyProperties.AnyAndNotNull())
            {
                query = query.Where(w => !keyProperties.Select(s => s.Name).Contains(w));
            }

            List<string> columnNameList = query.ToList();

            sql.Append($"INSERT INTO {fullTableName} ({string.Join(",", columnNameList)}) " +
                $"VALUES({string.Join(",", columnNameList.Select(s => "@" + s))});");

            return sql.ToString();
        }

        public static void CopyValue(object source, object target)
        {
            PropertyInfo[] propertyInfos = target.GetType().GetProperties();
            Type sourceType = source.GetType();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (!propertyInfo.CanWrite)
                {
                    continue;
                }

                string propertyName = propertyInfo.Name;
                PropertyInfo sourcePropertyInfo = sourceType.GetProperty(propertyName);

                if (sourcePropertyInfo == null)
                {
                    continue;
                }

                propertyInfo.SetValue(target, sourcePropertyInfo.GetValue(source), null);
            }
        }
    }
}