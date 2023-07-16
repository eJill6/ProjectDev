using Dapper;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Base;
using JxBackendService.Model.Enums;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Routing;

namespace JxBackendService.Repository.Extensions
{
    public static partial class DapperSqlMapperExtensions
    {
        private static readonly int s_stringDefaultSize = 10;

        private static readonly ConcurrentDictionary<InlodbType, ConcurrentDictionary<RuntimeTypeHandle, string>> s_dbQueriesMap =
            new ConcurrentDictionary<InlodbType, ConcurrentDictionary<RuntimeTypeHandle, string>>();

        public static List<KeyValuePair<string, object>> GetPrimaryKeyValues<T>(this T model)
        {
            return ModelUtil.GetKeys<T>().Select(s => new KeyValuePair<string, object>(s.Name, s.GetValue(model))).ToList();
        }

        public static T GetByKey<T>(this IDbConnection connection, T param, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            return connection.GetByKey(null, param, transaction, commandTimeout);
        }

        /// <summary>
        /// Returns a single entity by a single id from table "Ts".
        /// Id must be marked with [Key] attribute.
        /// Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
        /// for optimal performance.
        /// </summary>
        public static T GetByKey<T>(this IDbConnection connection, InlodbType inlodbType, T param, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            Type type = typeof(T);
            string sql = GetByKeySQL(inlodbType, type);
            object validParam = param.ConvertValidDynamicParameters();
            T obj = connection.Query<T>(sql, validParam, transaction, commandTimeout: commandTimeout).SingleOrDefault();

            return obj;
        }

        public static string GetByKeySQL(InlodbType inlodbType, Type type)
        {
            if (!s_dbQueriesMap.TryGetValue(inlodbType, out ConcurrentDictionary<RuntimeTypeHandle, string> getQueries))
            {
                getQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();
                s_dbQueriesMap.TryAdd(inlodbType, getQueries);
            }

            if (!getQueries.TryGetValue(type.TypeHandle, out string sql))
            {
                List<PropertyInfo> keyList = ModelUtil.GetKeys(type);

                string tableName = ModelUtil.GetTableName(type);
                List<string> columnList = ModelUtil.GetPropertiesNameOfType(type);

                string fullTableName = tableName;

                if (inlodbType != null)
                {
                    fullTableName = $"{inlodbType.Value}.dbo.{tableName}";
                }

                sql = $"SELECT {string.Join(", ", columnList)} FROM {fullTableName} WITH(NOLOCK) "
                    + $"WHERE {string.Join(" AND ", keyList.Select(s => s.Name + " = @" + s.Name)) }";

                getQueries[type.TypeHandle] = sql;
            }

            return sql;
        }

        public static object ConvertValidDynamicParameters(this object param)
        {
            object result = param.ConvertValidSqlParameters();

            if (result is List<CustomizedDynamicParameter>)
            {
                return (result as List<CustomizedDynamicParameter>).ToDynamicParameters();
            }

            return result;
        }

        public static object ConvertValidSqlParameters(this object param)
        {
            if (param == null)
            {
                return null;
            }

            if (param is DynamicParameters || param is List<CustomizedDynamicParameter>)
            {
                if (param is DynamicParameters)
                {
                    var dynamicParams = param as DynamicParameters;
                    ResizeDynamicParametersString(ref dynamicParams);
                    return dynamicParams;
                }

                return param;
            }

            List<CustomizedDynamicParameter> customizedDynamicParameters = null;
            bool isAnonymousType = param.GetType().IsAnonymousType();
            customizedDynamicParameters = new List<CustomizedDynamicParameter>();

            foreach (PropertyInfo propertyInfo in ModelUtil.TypePropertiesCache(param.GetType()))
            {
                if (propertyInfo.GetCustomAttributes(true).Any(a => a is DapperIgnoreAttribute))
                {
                    continue;
                }

                if (propertyInfo.PropertyType == typeof(string) ||
                    ((propertyInfo.PropertyType.IsGenericType ||
                    propertyInfo.PropertyType.IsArray) && propertyInfo.PropertyType.ToString().Contains("System.String")))
                {
                    #region string類都轉為dbstring

                    DbColumnInfoAttribute dbColumnInfoAttribute = null;

                    //匿名型別沒辦法掛attribute, 只能判斷是否用純string, 應該要轉為dbstring
                    if (!isAnonymousType)
                    {
                        dbColumnInfoAttribute = propertyInfo.GetDbColumnInfoAttribute();
                    }

                    SqlDbType sqlDbType = SqlDbType.NVarChar;
                    int? size = null;

                    if (dbColumnInfoAttribute != null)
                    {
                        sqlDbType = dbColumnInfoAttribute.SqlDbType;
                        size = dbColumnInfoAttribute.Size;
                    }

                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        object paramValue = propertyInfo.GetValue(param);
                        string value = null;

                        if (paramValue == null)
                        {
                            value = paramValue.ToTrimString();
                        }
                        else
                        {
                            value = paramValue.ToString();
                        }

                        if (!size.HasValue)
                        {
                            if (value.Length > 0)
                            {
                                size = value.Length;
                            }
                            else
                            {
                                size = s_stringDefaultSize;
                            }
                        }

                        customizedDynamicParameters.Add(new CustomizedDynamicParameter()
                        {
                            Name = propertyInfo.Name,
                            Value = value.ToDbString(sqlDbType, size.Value)
                        });
                    }
                    else
                    {
                        List<string> stringParams = propertyInfo.GetValue(param)?.CastByJson<List<string>>().Where(w => w != null).ToList();

                        if (!size.HasValue)
                        {
                            int maxSize = 0;

                            if (stringParams.AnyAndNotNull())
                            {
                                maxSize = stringParams.Max(m => m.ToTrimString().Length);
                            }

                            if (maxSize > 0)
                            {
                                size = maxSize;
                            }
                            else
                            {
                                size = s_stringDefaultSize;
                            }
                        }

                        customizedDynamicParameters.Add(new CustomizedDynamicParameter()
                        {
                            Name = propertyInfo.Name,
                            Value = stringParams,
                            DbColumnInfoAttribute = new DbColumnInfoAttribute(sqlDbType, size.Value)
                        });
                    }

                    #endregion string類都轉為dbstring
                }
                else
                {
                    //非string就看有無掛attribute
                    object value = propertyInfo.GetValue(param);

                    if (isAnonymousType)
                    {
                        customizedDynamicParameters.Add(new CustomizedDynamicParameter() { Name = propertyInfo.Name, Value = value });
                    }
                    else
                    {
                        if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            if (Convert.ToDateTime(value) == DateTime.MinValue)
                            {
                                continue;
                            }
                        }

                        var customizedDynamicParameter = new CustomizedDynamicParameter()
                        {
                            Name = propertyInfo.Name,
                            Value = propertyInfo.GetValue(param)
                        };

                        DbType? dbType = propertyInfo.PropertyType.ToDbType();

                        if (dbType.HasValue)
                        {
                            customizedDynamicParameter.DbColumnInfoAttribute = new DbColumnInfoAttribute(dbType.Value);
                            customizedDynamicParameters.Add(customizedDynamicParameter);
                        }
                        else if (propertyInfo.PropertyType.IsGenericType || propertyInfo.PropertyType.IsArray)
                        {
                            customizedDynamicParameters.Add(customizedDynamicParameter);
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                }
            }

            return customizedDynamicParameters;
        }

        public static long SaveByProcedure<T>(this IDbConnection connection, ActTypes actType, T model) where T : class
        {
            return connection.SaveByProcedure(actType, null, model);
        }

        public static long SaveByProcedure<T>(this IDbConnection connection, ActTypes actType, string tableName, T model) where T : class
        {
            if (!(model.ConvertValidSqlParameters() is List<CustomizedDynamicParameter> customizedDynamicParameters))
            {
                throw new InvalidCastException();
            }

            Type type = typeof(T);
            List<PropertyInfo> typeProperties = ModelUtil.TypePropertiesCache(type);
            List<PropertyInfo> keyProperties = ModelUtil.KeyPropertiesCache(type);
            List<PropertyInfo> explicitKeyProperties = ModelUtil.ExplicitKeyPropertiesCache(type);

            if (tableName.IsNullOrEmpty())
            {
                tableName = ModelUtil.GetTableName(typeof(T));
            }

            StringBuilder sql = new StringBuilder();
            IEnumerable<string> query = typeProperties.Select(s => s.Name);
            string procedureName = GetProcedureName(actType, tableName);
            List<string> columnNameList = null;

            if (actType == ActTypes.Insert)
            {
                columnNameList = query.ToList();
                columnNameList.RemoveAll(r => r.Equals(nameof(BaseEntityModel.UpdateDate), StringComparison.CurrentCultureIgnoreCase) ||
                r.Equals(nameof(BaseEntityModel.UpdateUser), StringComparison.CurrentCultureIgnoreCase));

                if (keyProperties.AnyAndNotNull())
                {
                    //排除自動增號欄位
                    columnNameList = columnNameList.Where(w => !keyProperties.Select(s => s.Name).Contains(w)).ToList();
                }
            }
            else if (actType == ActTypes.Update)
            {
                columnNameList = query.ToList();
                columnNameList.RemoveAll(r =>
                    r.Equals(nameof(BaseEntityModel.CreateDate), StringComparison.CurrentCultureIgnoreCase) ||
                    r.Equals(nameof(BaseEntityModel.CreateUser), StringComparison.CurrentCultureIgnoreCase));
            }
            else if (actType == ActTypes.Delete)
            {
                columnNameList = keyProperties.Concat(explicitKeyProperties).Select(s => s.Name).ToList();
            }

            sql.AppendLine($"EXEC [{procedureName}] {string.Join(",", columnNameList.Select(s => $"@{s} = @{s}"))};");

            if (keyProperties.AnyAndNotNull() && actType == ActTypes.Insert)
            {
                sql.Append("SELECT @@IDENTITY; ");
                return connection.ExecuteScalar<long>(sql.ToString(), customizedDynamicParameters.ToDynamicParameters());
            }
            else
            {
                return connection.Execute(sql.ToString(), customizedDynamicParameters.ToDynamicParameters());
            }
        }

        //public static long OwmsInsert<T>(this IDbConnection connection, T model) where T : class
        //{
        //    string tableName = ModelUtil.GetTableName(typeof(T));
        //    return OwmsInsert(connection, tableName, model);
        //}

        //public static long OwmsInsert<T>(this IDbConnection connection, string tableName, T model) where T : class
        //{
        //    if (!(model.ConvertValidSqlParameters() is List<CustomizedDynamicParameter> owmsDynamicParameters))
        //    {
        //        throw new InvalidCastException();
        //    }

        //    Type type = typeof(T);
        //    List<PropertyInfo> typeProperties = ModelUtil.TypePropertiesCache(type);
        //    List<PropertyInfo> keyProperties = ModelUtil.KeyPropertiesCache(type);

        //    List<PropertyInfo> explicitKeyProperties = ModelUtil.ExplicitKeyPropertiesCache(type);

        //    if (tableName.IsNullOrEmpty())
        //    {
        //        tableName = ModelUtil.GetTableName(typeof(T));
        //    }

        //    //擴充套件無法使用DynamicParameters, 若直接轉DynamicParameter,又會出現無法取得key,value的情況,只好先轉成自訂model再轉回來
        //    StringBuilder sql = new StringBuilder();
        //    IEnumerable<string> query = typeProperties.Select(s => s.Name);

        //    if (keyProperties.AnyAndNotNull())
        //    {
        //        query = query.Where(w => !keyProperties.Select(s => s.Name).Contains(w));
        //    }

        //    List<string> columnNameList = query.ToList();

        //    sql.Append($"INSERT INTO [{tableName}] ({string.Join(",", columnNameList)}) " +
        //        $"VALUES({string.Join(",", columnNameList.Select(s => "@" + s))});");

        //    if (keyProperties.AnyAndNotNull())
        //    {
        //        sql.Append("SELECT @@IDENTITY; ");
        //        return connection.ExecuteScalar<long>(sql.ToString(), owmsDynamicParameters.ToDynamicParameters());
        //    }
        //    else
        //    {
        //        return connection.Execute(sql.ToString(), owmsDynamicParameters.ToDynamicParameters());
        //    }
        //}

        //public static bool OwmsUpdate<T>(this IDbConnection connection, T model, int? commandTimeout = null) where T : class
        //{
        //    if (!(model.ConvertValidSqlParameters() is List<CustomizedDynamicParameter> owmsDynamicParameters))
        //    {
        //        throw new InvalidCastException();
        //    }

        //    Type type = typeof(T);
        //    List<PropertyInfo> typeProperties = ModelUtil.TypePropertiesCache(type);
        //    List<PropertyInfo> keyProperties = ModelUtil.KeyPropertiesCache(type);
        //    List<PropertyInfo> explicitKeyProperties = ModelUtil.ExplicitKeyPropertiesCache(type);
        //    string tableName = ModelUtil.GetTableName(type);

        //    StringBuilder sql = new StringBuilder();
        //    IEnumerable<string> query = typeProperties.Select(s => s.Name);
        //    IEnumerable<string> keyNames = keyProperties.Select(s => s.Name);
        //    IEnumerable<string> explicitKeyNames = explicitKeyProperties.Select(s => s.Name);

        //    if (keyProperties.AnyAndNotNull())
        //    {
        //        query = query.Where(w => !keyProperties.Select(s => s.Name).Contains(w));
        //    }

        //    if (explicitKeyProperties.AnyAndNotNull())
        //    {
        //        query = query.Where(w => !explicitKeyProperties.Select(s => s.Name).Contains(w));
        //    }

        //    List<string> columnNameList = query.ToList();

        //    sql.Append($"UPDATE [{tableName}] SET {string.Join(",", columnNameList.Select(s => $"{s} = @{s}"))} " +
        //        $"WHERE {string.Join(" AND ", keyNames.Union(explicitKeyNames).Select(s => $"{s} = @{s}"))};");

        //    sql.Append("SELECT @@ROWCOUNT; ");

        //    return connection.ExecuteScalar<int>(sql.ToString(),
        //        owmsDynamicParameters.ToDynamicParameters(),
        //        commandTimeout: commandTimeout) > 0;
        //}

        //public static bool OwmsDelete<T>(this IDbConnection connection, T model, int? commandTimeout = null) where T : class
        //{
        //    if (!(model.ConvertValidSqlParameters() is List<CustomizedDynamicParameter> owmsDynamicParameters))
        //    {
        //        throw new InvalidCastException();
        //    }

        //    Type type = typeof(T);
        //    List<PropertyInfo> keyProperties = ModelUtil.KeyPropertiesCache(type);
        //    List<PropertyInfo> explicitKeyProperties = ModelUtil.ExplicitKeyPropertiesCache(type);
        //    string tableName = ModelUtil.GetTableName(type);

        //    StringBuilder sql = new StringBuilder();
        //    IEnumerable<string> keyNames = keyProperties.Select(s => s.Name);
        //    IEnumerable<string> explicitKeyNames = explicitKeyProperties.Select(s => s.Name);

        //    sql.Append($"DELETE FROM [{tableName}] WHERE {string.Join(" AND ", keyNames.Union(explicitKeyNames).Select(s => $"{s} = @{s}"))};");
        //    sql.Append("SELECT @@ROWCOUNT; ");

        //    return connection.ExecuteScalar<int>(sql.ToString(),
        //        owmsDynamicParameters.ToDynamicParameters(),
        //        commandTimeout: commandTimeout) > 0;
        //}

        private static string GetProcedureName(ActTypes actType, string tableName)
        {
            string actName;

            switch (actType)
            {
                case ActTypes.Insert:
                    actName = "Insert";
                    break;

                case ActTypes.Update:
                    actName = "Update";
                    break;

                case ActTypes.Delete:
                    actName = "Delete";
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return $"Pro_{actName}SingleRowTo{tableName}";
        }

        private static ConcurrentDictionary<Type, DbType> _typeMap = null;

        public static DbType? ToDbType(this Type type)
        {
            if (_typeMap == null)
            {
                var temp = new ConcurrentDictionary<Type, DbType>();
                temp.TryAdd(typeof(byte), DbType.Byte);
                temp.TryAdd(typeof(sbyte), DbType.SByte);
                temp.TryAdd(typeof(short), DbType.Int16);
                temp.TryAdd(typeof(ushort), DbType.UInt16);
                temp.TryAdd(typeof(int), DbType.Int32);
                temp.TryAdd(typeof(uint), DbType.UInt32);
                temp.TryAdd(typeof(long), DbType.Int64);
                temp.TryAdd(typeof(ulong), DbType.UInt64);
                temp.TryAdd(typeof(float), DbType.Single);
                temp.TryAdd(typeof(double), DbType.Double);
                temp.TryAdd(typeof(decimal), DbType.Decimal);
                temp.TryAdd(typeof(bool), DbType.Boolean);
                temp.TryAdd(typeof(string), DbType.String);
                temp.TryAdd(typeof(char), DbType.StringFixedLength);
                temp.TryAdd(typeof(Guid), DbType.Guid);
                temp.TryAdd(typeof(DateTime), DbType.DateTime);
                temp.TryAdd(typeof(DateTimeOffset), DbType.DateTimeOffset);
                temp.TryAdd(typeof(byte[]), DbType.Binary);
                temp.TryAdd(typeof(byte?), DbType.Byte);
                temp.TryAdd(typeof(sbyte?), DbType.SByte);
                temp.TryAdd(typeof(short?), DbType.Int16);
                temp.TryAdd(typeof(ushort?), DbType.UInt16);
                temp.TryAdd(typeof(int?), DbType.Int32);
                temp.TryAdd(typeof(uint?), DbType.UInt32);
                temp.TryAdd(typeof(long?), DbType.Int64);
                temp.TryAdd(typeof(ulong?), DbType.UInt64);
                temp.TryAdd(typeof(float?), DbType.Single);
                temp.TryAdd(typeof(double?), DbType.Double);
                temp.TryAdd(typeof(decimal?), DbType.Decimal);
                temp.TryAdd(typeof(bool?), DbType.Boolean);
                temp.TryAdd(typeof(char?), DbType.StringFixedLength);
                temp.TryAdd(typeof(Guid?), DbType.Guid);
                temp.TryAdd(typeof(DateTime?), DbType.DateTime);
                temp.TryAdd(typeof(DateTimeOffset?), DbType.DateTimeOffset);
                _typeMap = temp;
            }

            if (_typeMap.TryGetValue(type, out DbType dbType))
            {
                return dbType;
            }
            else
            {
                return null;
            }
        }

        public static DynamicParameters ToDynamicParameters(this List<CustomizedDynamicParameter> list)
        {
            var dynamicParameters = new DynamicParameters();

            foreach (CustomizedDynamicParameter customizedDynamicParameter in list)
            {
                DbColumnInfoAttribute dbColumnInfo = customizedDynamicParameter.DbColumnInfoAttribute;

                //沒型別資料由DAPPER自己決定
                if (dbColumnInfo == null ||
                    (dbColumnInfo != null && dbColumnInfo.DbType == DbType.Binary))
                {
                    dynamicParameters.Add(customizedDynamicParameter.Name, customizedDynamicParameter.Value);

                    //if (owmsDynamicParameter.Value != null)
                    //{
                    //    Type valueType = owmsDynamicParameter.Value.GetType();
                    //    if (valueType.IsGenericType || valueType.IsArray)
                    //    {
                    //        var enumerable = owmsDynamicParameter.Value as IEnumerable;
                    //        IList<object> filteredEnumerable = new List<object>();
                    //        foreach (var en in enumerable)
                    //        {
                    //            if (en != null)
                    //            {
                    //                filteredEnumerable.Add(en);
                    //            }
                    //        }

                    //        dynamicParameters.Add(owmsDynamicParameter.Name, filteredEnumerable);
                    //    }
                    //    else
                    //    {
                    //        dynamicParameters.Add(owmsDynamicParameter.Name, owmsDynamicParameter.Value);
                    //    }
                    //}
                    //else
                    //{
                    //    dynamicParameters.Add(owmsDynamicParameter.Name, owmsDynamicParameter.Value);
                    //}
                }
                else
                {
                    List<string> values = null;

                    if (customizedDynamicParameter.Value != null)
                    {
                        Type paramValueType = customizedDynamicParameter.Value.GetType();
                        if (paramValueType.IsGenericType || paramValueType.IsArray)
                        {
                            values = customizedDynamicParameter.Value.CastByJson<List<string>>();
                        }
                    }

                    //如果是集合類型就分別轉為不同的dbstring
                    if (values != null)
                    {
                        List<DbString> dbStrings = values.Where(w => w != null)
                            .Select(s => s.ToDbString(dbColumnInfo.SqlDbType, dbColumnInfo.Size))
                            .ToList();
                        dynamicParameters.Add(customizedDynamicParameter.Name, dbStrings);
                    }
                    else
                    {
                        //單一value類型
                        int? size = null;
                        byte? precision = null;
                        byte? scale = null;

                        if (dbColumnInfo.Size > 0)
                        {
                            size = dbColumnInfo.Size;
                        }

                        if (dbColumnInfo.Precision > 0)
                        {
                            precision = dbColumnInfo.Precision;
                        }

                        if (dbColumnInfo.Scale > 0)
                        {
                            scale = dbColumnInfo.Scale;
                        }

                        dynamicParameters.Add(customizedDynamicParameter.Name, customizedDynamicParameter.Value,
                            dbType: dbColumnInfo.DbType,
                            size: size,
                            precision: precision,
                            scale: scale);
                    }
                }
            }

            return dynamicParameters;
        }

        public static void ResizeDynamicParametersString(ref DynamicParameters dynamicParameters)
        {
            var routeValueDictionary = new RouteValueDictionary(dynamicParameters.GetType()
                .GetField("parameters", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(dynamicParameters));

            bool isReplaceOld = false;
            DynamicParameters newDynamicParameters = new DynamicParameters();

            foreach (var routeValue in (routeValueDictionary["Values"] as IEnumerable))
            {
                string paramName = routeValue.GetType().GetProperty("Name").GetValue(routeValue) as string;
                DbType? dbType = routeValue.GetType().GetProperty("DbType").GetValue(routeValue) as DbType?;
                object paramValue = routeValue.GetType().GetProperty("Value").GetValue(routeValue);

                if ((dbType.HasValue &&
                    (dbType == DbType.AnsiString ||
                    dbType == DbType.AnsiStringFixedLength ||
                    dbType == DbType.String ||
                    dbType == DbType.StringFixedLength)) ||
                    paramValue is string)
                {
                    int? size = routeValue.GetType().GetProperty("Size").GetValue(routeValue) as int?;

                    if (!size.HasValue)
                    {
                        string stringValue = string.Empty;
                        if (paramValue != null)
                        {
                            stringValue = paramValue.ToString();
                        }

                        isReplaceOld = true;
                        newDynamicParameters.Add(paramName, stringValue.ToNVarchar(stringValue.Length));
                    }
                }
                else
                {
                    newDynamicParameters.Add(paramName, paramValue);
                }
            }

            if (isReplaceOld)
            {
                dynamicParameters = newDynamicParameters;
            }
        }
    }

    public class CustomizedDynamicParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public DbColumnInfoAttribute DbColumnInfoAttribute { get; set; }
    }
}