using Dapper;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace JxBackendService.Common.Util
{
    public static class ModelUtil
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> ExplicitKeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        //private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> GetQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, List<string>> ColumnNames = new ConcurrentDictionary<RuntimeTypeHandle, List<string>>();

        public static List<string> GetPropertiesNameOfType(Type type)
        {
            if (ColumnNames.TryGetValue(type.TypeHandle, out List<string> propertyList))
            {
                return propertyList;
            }

            propertyList = new List<string>();

            foreach (PropertyInfo prop in type.GetProperties().Where(p => !p.GetCustomAttributes(true).Where(w => w is IgnoreReadAttribute).Any()))
            {
                string propertyName = prop.Name;
                propertyList.Add(prop.Name);
            }

            ColumnNames[type.TypeHandle] = propertyList;
            return propertyList;
        }

        public static List<PropertyInfo> GetKeys<T>()
        {
            Type type = typeof(T);

            return GetKeys(type);
        }

        public static List<PropertyInfo> GetKeys(object model)
        {
            Type type = model.GetType();

            return GetKeys(type);
        }

        public static List<PropertyInfo> GetKeys(Type type)
        {
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
            propertyInfos.AddRange(KeyPropertiesCache(type));
            propertyInfos.AddRange(ExplicitKeyPropertiesCache(type));

            return propertyInfos;
        }

        public static string GetTableName(Type type)
        {
            if (TypeTableName.TryGetValue(type.TypeHandle, out string name))
            {
                return name;
            }

            //            if (SqlMapperExtensions.TableNameMapper != null)
            //            {
            //                name = SqlMapperExtensions.TableNameMapper(type);
            //            }
            //            else
            //            {
            //#if NETSTANDARD1_3
            //                var info = type.GetTypeInfo();
            //#else
            //                var info = type;
            //#endif
            //                //NOTE: This as dynamic trick falls back to handle both our own Table-attribute as well as the one in EntityFramework
            //                var tableAttrName =
            //                    info.GetCustomAttribute<TableAttribute>(false)?.Name
            //                    ?? (info.GetCustomAttributes(false).FirstOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic)?.Name;

            //                if (tableAttrName != null)
            //                {
            //                    name = tableAttrName;
            //                }
            //                else
            //                {
            //                    name = type.Name;
            //                }
            //            }
            //先直接抓typeName
            name = type.Name;

            TypeTableName[type.TypeHandle] = name;
            return name;
        }

        public static List<PropertyInfo> KeyPropertiesCache(Type type)
        {
            if (KeyProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
            {
                return pi.ToList();
            }

            var allProperties = TypePropertiesCache(type);
            var keyProperties = allProperties.Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

            if (keyProperties.Count() == 0)
            {
                var idProp = allProperties.Find(p => string.Equals(p.Name, "id", StringComparison.CurrentCultureIgnoreCase));
                if (idProp != null && !idProp.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute))
                {
                    keyProperties.Add(idProp);
                }
            }

            KeyProperties[type.TypeHandle] = keyProperties;
            return keyProperties;
        }

        public static List<PropertyInfo> ExplicitKeyPropertiesCache(Type type)
        {
            if (ExplicitKeyProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
            {
                return pi.ToList();
            }

            var explicitKeyProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is ExplicitKeyAttribute)).ToList();

            ExplicitKeyProperties[type.TypeHandle] = explicitKeyProperties;
            return explicitKeyProperties;
        }

        public static List<PropertyInfo> TypePropertiesCache(Type type)
        {
            if (TypeProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pis))
            {
                return pis.ToList();
            }

            var properties = type.GetProperties().Where(IsWriteable).ToArray();
            TypeProperties[type.TypeHandle] = properties;
            return properties.ToList();
        }

        private static bool IsWriteable(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false).AsList();
            if (attributes.Count != 1) return true;

            var writeAttribute = (WriteAttribute)attributes[0];
            return writeAttribute.Write;
        }

        public static DbColumnInfoAttribute GetDbColumnInfoAttribute(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(true).SingleOrDefault(w => w is DbColumnInfoAttribute) as DbColumnInfoAttribute;
        }

        public static bool IsAnonymousType(this Type type)
        {
            bool hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;
            return isAnonymousType;
        }

        public static string GetReferenceKey(object model)
        {
            List<PropertyInfo> keyList = ModelUtil.GetKeys(model);
            Dictionary<string, object> referenceKeyValues = keyList.ToDictionary(d => d.Name, d => d.GetValue(model, null));
            return referenceKeyValues.ToJsonString();
        }

        public static List<SqlSelectColumnInfo> GetAllColumnInfos<ModelType>(List<string> properties = null, bool isAppendTableNameToColumn = false, string tableName = null)
        {
            if (properties == null)
            {
                Type type = typeof(ModelType);
                properties = GetPropertiesNameOfType(type);
            }

            List<SqlSelectColumnInfo> sqlSelectColumns = properties.Select(s => new SqlSelectColumnInfo() { ColumnName = s, AliasName = s }).ToList();

            if (isAppendTableNameToColumn)
            {
                sqlSelectColumns.ForEach(f => f.TableName = tableName);
            }

            return sqlSelectColumns;
        }

        public static string GetColumnValueJsonSql<ModelType>()
        {
            Type type = typeof(ModelType);
            List<string> properties = GetPropertiesNameOfType(type);
            List<SqlSelectColumnInfo> sqlSelectColumns = properties.Select(s => new SqlSelectColumnInfo() { ColumnName = s, AliasName = s }).ToList();

            IEnumerable<string> jsonColumnSqls = sqlSelectColumns.
                Select(s => $"'\"{s.ColumnName}\" : ' + CASE WHEN {s.ColumnName} IS NULL THEN 'null' " +
                $"ELSE '\"' + STRING_ESCAPE(CONVERT(NVARCHAR(MAX), {s.ColumnName}),'JSON') + '\"' END ");

            string sql = "'{' + " + string.Join(" + ',' + ", jsonColumnSqls) + " + '}' ";

            return sql;
        }

        public static object GetModelValue(object model, string propertyName)
        {
            if (model == null)
            {
                return null;
            }

            PropertyInfo propertyInfo = model.GetType().GetProperty(propertyName);

            if (propertyInfo == null)
            {
                return null;
            }

            return propertyInfo.GetValue(model);
        }
    }
}