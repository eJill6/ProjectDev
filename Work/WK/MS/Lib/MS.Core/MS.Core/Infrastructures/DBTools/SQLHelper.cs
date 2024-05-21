using MS.Core.Attributes;
using MS.Core.Models;
using System.Data;
using System.Reflection;
using System.Text;

namespace MMService.DBTools
{
    public class SQLHelper
    {
        public static string ConcatReturnIdentityCommand(string sql)
        {
            return sql + ";select @@IDENTITY";
        }

        public static Type GetTableType<T>() where T : BaseDBModel
        {
            Type currentType = typeof(T);
            while (currentType.BaseType != typeof(BaseDBModel) && currentType.BaseType != null)
            {
                currentType = currentType.BaseType;
            }
            return currentType;
        }

        public static string GetTableName(Type type)
        {
            return type.Name.Replace("Model", string.Empty).Replace("Entity", string.Empty);
        }

        public static string GetInsertSql<T>() where T : BaseDBModel
        {
            Type type = GetTableType<T>();
            var ignoreFields = GetAutoKeyName(type);
            return GetInsertSql(type, ignoreFields);
        }

        public static string GetUpdateCommand<T>() where T : BaseDBModel
        {
            Type type = GetTableType<T>();
            string tableName = GetTableName(type);
            var pkFields = GetPrimaryKeyFields(type);
            return GetUpdateCommand(type, tableName, null, pkFields);
        }

        public static string GetInsertSql(Type type, List<string> ignoreFields)
        {
            string tableName = GetTableName(type);
            var query = string.Format("INSERT INTO {0} ({1}) VALUES({2})",
                tableName,
                GetFieldList(type, ignoreFields),
                GetFieldPatternList(type, ignoreFields));

            return query;
        }

        private static string GetFieldPatternList(Type type , List<string> ignoreFields)
        {
            var sb = new StringBuilder();
            var filterProperties = GetPropertiesAndFilterIgnore(type, ignoreFields);

            foreach (var item in filterProperties.OrderBy(x => x.Name))
            {
                if (IsICollection(item.PropertyType))
                {
                    continue;
                }

                if (IsVirtual(item))
                {
                    continue;
                }

                sb.Append("@" + item.Name + ",");
            }
            var query = sb.ToString();
            return query.Remove(query.Length - 1);
        }
        private static string GetFieldList(Type type,List<string> ignoreFields)
        {
            var sb = new StringBuilder();
            var filterProperties = GetPropertiesAndFilterIgnore(type, ignoreFields);

            foreach (var item in filterProperties.OrderBy(x => x.Name))
            {
                if (IsICollection(item.PropertyType))
                {
                    continue;
                }

                if (IsVirtual(item))
                {
                    continue;
                }

                sb.Append($"[{item.Name}],");
            }
            var query = sb.ToString();
            return query.Remove(query.Length - 1);
        }

        private static string GetUpdateCommand(Type type, string tableName, List<string> ignoreFields, List<string> pkFields)
        {
            var updateFieldString = GetUpdateFieldString(type, ignoreFields, pkFields);
            var primaryKeyString = GetPrimaryKeyString(pkFields);
            var command = $"UPDATE {tableName} SET {updateFieldString} WHERE {primaryKeyString}";
            return command;
        }

        private static string GetUpdateFieldString(Type type, List<string> ignoreFields, List<string> pkFields)
        {
            var filterProperties = GetPropertiesAndFilterIgnore(type, ignoreFields).OrderBy(a => a.Name);
            var array = (from item in filterProperties
                         where !IsICollection(item.PropertyType) &&
                               !IsVirtual(item) &&
                               !pkFields.Contains(item.Name)
                         select item.Name + "=@" + item.Name).ToArray();
            return string.Join(",", array);
        }

        private static string GetPrimaryKeyString(IEnumerable<string> pkFields)
        {
            var array = pkFields.Select(a => a + "=@" + a).ToArray();
            return string.Join(" AND ", array);
        }

        private static List<PropertyInfo> GetPropertiesAndFilterIgnore(Type type, List<string> ignoreFields)
        {
            var properties = type.GetProperties();
            List<PropertyInfo> filterProperties = properties.ToList();
            if (ignoreFields != null && ignoreFields.Count > 0)
            {
                filterProperties = properties
                    .Where(a => !ignoreFields.Contains(a.Name)).ToList();
            }

            return filterProperties;
        }

        private static bool IsVirtual(PropertyInfo info)
        {
            bool isExplicitlyVirtual = false;

            MethodInfo getMethod = info.GetGetMethod();
            if (getMethod != null)
            {
                if (getMethod.IsVirtual && !getMethod.IsFinal && !getMethod.IsHideBySig)
                {
                    isExplicitlyVirtual = true;
                }
            }
            return isExplicitlyVirtual;
        }

        private static bool IsICollection(Type type)
        {
            if (type == null || !type.IsGenericType)
            {
                return false;
            }

            return typeof(ICollection<>).IsAssignableFrom(type
                .GetGenericTypeDefinition());

        }

        private static List<string> GetPrimaryKeyFields(Type type)
        {
            var result = new List<string>();

            foreach (var prop in type.GetProperties())
            {
                PrimaryKeyAttribute? attr = prop.GetCustomAttribute(typeof(PrimaryKeyAttribute), false) as PrimaryKeyAttribute;

                if (attr != null)
                {
                    result.Add(prop.Name);
                }
            }

            return result;
        }

        public static string GetDeleteCommand<T>() where T : BaseDBModel
        {
            Type type = GetTableType<T>();
            string tableName = GetTableName(type);
            string sqlScript = $@"
                            DELETE FROM [{tableName}]
                            WHERE {GetKeyConditionScript(type)}
                            ";
            return sqlScript;
        }

        public static string GetDeletesCommand<T>() where T : BaseDBModel
        {
            Type type = GetTableType<T>();
            string tableName = GetTableName(type);

            string sqlScript = $@"
                            DELETE FROM [{tableName}]
                            WHERE {string.Join(" and ", GetKeyName(type).Select(x => "[" + x + "] IN " + "@Ids").ToArray())}
                            ";
            return sqlScript;
        }

        public static string GetKeyConditionScript(Type type)
        {
            return string.Join(" and ", GetKeyName(type).Select(x => "[" + x + "] = " + "@" + x).ToArray());
        }

        public static List<string> GetKeyName(Type type)
        {
            List<string> keyName = new List<string>();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(PrimaryKeyAttribute)) as PrimaryKeyAttribute;

                if (attribute != null)
                {
                    keyName.Add(property.Name);
                    continue;
                }
            }
            return keyName;
        }

        public static List<string> GetAutoKeyName(Type type)
        {
            List<string> keyName = new List<string>();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(AutoKeyAttribute)) as AutoKeyAttribute;

                if (attribute != null)
                {
                    keyName.Add(property.Name);
                    continue;
                }
            }
            return keyName;
        }
    }
}
