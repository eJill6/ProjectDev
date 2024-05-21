using Dapper;
using FreeRedis;
using MMService.DBTools;
using MS.Core.Models.Models;
using System.Collections;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace MS.Core.Infrastructures.DBTools
{
    public class SQLQueryUtil<T>
    {
        public static string TotalCount => "TotalCount";

        private static string GetTableName()
        {
            return typeof(T).Name.Replace("Model", "").Replace("Entity", "");
        }

        public static string BuildPageSql(DynamicParameters paras, ExpressionComponent<T> expressionComponent, PaginationModel pageData)
        {
            var tableName = GetTableName();

            string selectClause = GenerateSelectClause(expressionComponent);

            BuildWhereParasHelper parasHelper = BuildWhereParasHelper.Create(paras);

            var whereClause = GenerateWhereClause(parasHelper, expressionComponent);
            string orderByClause = GenerateOrderByClause(expressionComponent);

            int startRowNum = (pageData.PageNo <= 1) ? 1 : 1 + (pageData.PageNo - 1) * pageData.PageSize;
            int endRowNum = (startRowNum - 1) + pageData.PageSize;

            var startP = parasHelper.Add(startRowNum);
            var endP = parasHelper.Add(endRowNum);

            return @$"
SET @{TotalCount} = (SELECT COUNT(1) FROM {tableName} WITH(NOLOCK) {whereClause})
SELECT {selectClause}
FROM
(
    SELECT {selectClause}
        ,  ROW_NUMBER() OVER ({orderByClause}) AS RowNumber
    FROM {tableName} WITH(NOLOCK)
    {whereClause}
) AS T
WHERE T.RowNumber BETWEEN @{startP.Key} AND @{endP.Key};
";
        }

        public static string BuildCountSql(DynamicParameters paras, ExpressionComponent<T> expressionComponent)
        {
            var tableName = GetTableName();

            BuildWhereParasHelper parasHelper = BuildWhereParasHelper.Create(paras);

            var whereClause = GenerateWhereClause(parasHelper, expressionComponent);

            return @$"
Select Count(1)
FROM {tableName} WITH(NOLOCK)
{whereClause}";
        }

        public static string BuildSumSql(DynamicParameters paras, ExpressionComponent<T> expressionComponent)
        {
            var tableName = GetTableName();
            string sumClause = BuildSumClause(expressionComponent.GetPredicate().Body);

            BuildWhereParasHelper parasHelper = BuildWhereParasHelper.Create(paras);

            var whereClause = GenerateWhereClause(parasHelper, expressionComponent);

            return @$"
Select Sum({sumClause})
FROM {tableName} WITH(NOLOCK)
{whereClause}";
        }

        public static string BuildExistsSql(DynamicParameters paras, ExpressionComponent<T> expressionComponent)
        {
            var tableName = GetTableName();

            BuildWhereParasHelper parasHelper = BuildWhereParasHelper.Create(paras);

            var whereClause = GenerateWhereClause(parasHelper, expressionComponent);

            return @$"
                SELECT
	                CASE WHEN EXISTS
                    (
	                    SELECT TOP 1 1
                        FROM {tableName} WITH(NOLOCK)
                        {whereClause}
                    ) THEN 1 ELSE 0 END;";
        }

        public static string BuildSql(DynamicParameters paras, ExpressionComponent<T> expressionComponent)
        {
            var tableName = GetTableName();

            string selectClause = GenerateSelectClause(expressionComponent);

            BuildWhereParasHelper parasHelper = BuildWhereParasHelper.Create(paras);

            string whereClause = GenerateWhereClause(parasHelper, expressionComponent);

            string orderByClause = GenerateOrderByClause(expressionComponent);

            return @$"
SELECT {selectClause}
FROM {tableName} WITH(NOLOCK)
{whereClause}
{orderByClause}";
        }

        /// <summary>
        /// 生成 Select 語法
        /// </summary>
        /// <param name="expressionComponent"></param>
        /// <returns></returns>
        private static string GenerateSelectClause(ExpressionComponent<T> expressionComponent)
        {
            var selectClause = string.Empty;

            var selectPredicate = expressionComponent.GetPredicate();

            if (selectPredicate == null)
            {
                selectClause = string.Join(",", typeof(T).GetProperties().Select(e => e.Name));
            }
            else
            {
                selectClause = BuildSelectClause(selectPredicate.Body);
            }

            return selectClause;
        }

        /// <summary>
        /// 生成 OrderBy 語法
        /// </summary>
        /// <param name="expressionComponent"></param>
        /// <returns></returns>
        private static string GenerateOrderByClause(ExpressionComponent<T> expressionComponent)
        {
            var orderByClause = string.Empty;

            var orderByPredicate = expressionComponent.GetOrderByPredicate();

            if (orderByPredicate.Any())
            {
                orderByClause = $"ORDER BY {string.Join(@",", orderByPredicate.Select(p => BuildOrderByClause(p)))}";
            }
            return orderByClause;
        }

        /// <summary>
        /// 生成 Where 語法
        /// </summary>
        /// <param name="expressionComponent"></param>
        /// <returns></returns>
        private static string GenerateWhereClause(BuildWhereParasHelper parasHelper, ExpressionComponent<T> expressionComponent)
        {
            var wherePredicate = expressionComponent.GetWhereExpressions();

            var whereClause = string.Empty;

            if (wherePredicate.Any())
            {
                whereClause = $"WHERE {string.Join(@" AND ", wherePredicate.Select(p => BuildWhereClause(parasHelper, p.Body)))}";
            }
            return whereClause;
        }

        private static string BuildSelectClause(Expression expression)
        {
            switch (expression)
            {
                case NewExpression newExpression:
                    return string.Join(@",", newExpression.Arguments.Select(BuildSelectClause));

                case MemberExpression memberExpression:
                    return memberExpression.Member.Name;

                case MemberInitExpression memberInitExpression:
                    return string.Join(@",", memberInitExpression.Bindings.Select(e => e.Member.Name));

                default:
                    throw new NotSupportedException($"Expression type '{expression.GetType().Name}' not supported.");
            }
        }

        private static string BuildSumClause(Expression expression)
        {
            switch (expression)
            {
                case UnaryExpression:
                    var u = expression as UnaryExpression;
                    return BuildSumClause(u.Operand);

                case BinaryExpression:
                    var binary = expression as BinaryExpression;
                    var left = BuildSumClause(binary.Left);
                    var right = BuildSumClause(binary.Right);
                    var @operator = GetSqlOperator(binary.NodeType);
                    return $"({left}{@operator}{right})";

                case MemberExpression:
                    var member = expression as MemberExpression;
                    return member.Member.Name;

                default:
                    throw new NotSupportedException($"Expression type '{expression.GetType().Name}' not supported.");
            }
        }

        private static string BuildOrderByClause(OrderByExpressionComponent<T> orderByExpression)
        {
            string orderby = BuildOrderByClause(orderByExpression.OrderPredicates.Body);
            string sort = orderByExpression.IsDesc ? "DESC" : "ASC";
            return $"{orderby} {sort}";
        }

        private static string BuildOrderByClause(Expression expression)
        {
            switch (expression)
            {
                case UnaryExpression:
                    var u = expression as UnaryExpression;
                    return BuildSumClause(u.Operand);

                case BinaryExpression:
                    var binary = expression as BinaryExpression;
                    var left = BuildSumClause(binary.Left);
                    var right = BuildSumClause(binary.Right);
                    var @operator = GetSqlOperator(binary.NodeType);
                    return $"({left}{@operator}{right})";

                case MemberExpression:
                    var member = expression as MemberExpression;
                    return member.Member.Name;

                default:
                    throw new NotSupportedException($"Expression type '{expression.GetType().Name}' not supported.");
            }
        }

        private static string BuildWhereClause(BuildWhereParasHelper parasHelper, Expression expression, EntityTypeAttribute? entityTypeAttr = null)
        {
            switch (expression)
            {
                case BinaryExpression binary:
                    {
                        MemberExpression m = GetMemberExpression(binary);
                        ConstantExpression c = GetConstantExpression(binary);
                        //Nullable 特殊處理
                        if (m != null && c != null && IsNullableMemberExpression(m))
                        {
                            var nullableExpression = m.Expression as MemberExpression;
                            var column = nullableExpression.Member.Name;
                            if (c.Value.Equals(true))
                            {
                                return $"({column} IS NOT NULL)";
                            }
                            else if (c.Value.Equals(false))
                            {
                                return $"({column} IS NULL)";
                            }
                        }
                        else if(m != null && c != null && m.Type == typeof(string) && c.Value is null)
                        {
                            var column = m.Member.Name;
                            if(binary.NodeType == ExpressionType.NotEqual)
                            {
                                return $"({column} IS NOT NULL)";
                            }
                            return $"({column} IS NULL)";
                        }

                        if (m != null)
                        {
                            entityTypeAttr = GetEntityTypeAttr(m.Member.Name);
                        }

                        var left = BuildWhereClause(parasHelper, binary.Left, entityTypeAttr);
                        var right = BuildWhereClause(parasHelper, binary.Right, entityTypeAttr);
                        var @operator = GetSqlOperator(binary.NodeType);
                        return $"({left} {@operator} {right})";
                    }
                case ConstantExpression constant:
                    return FormatValue(parasHelper, constant.Value, entityTypeAttr);

                case ConditionalExpression conditional:
                    {
                        var condition = BuildWhereClause(parasHelper, conditional.Test);
                        var trueExpression = BuildWhereClause(parasHelper, conditional.IfTrue);
                        var falseExpression = BuildWhereClause(parasHelper, conditional.IfFalse);

                        return $"({condition} ? {trueExpression} : {falseExpression})";
                    }
                case MemberExpression member:
                    {
                        //Nullable 特殊處理
                        if (IsNullableMemberExpression(member))
                        {
                            var nullableExpression = member.Expression as MemberExpression;
                            var column = nullableExpression.Member.Name;

                            if (member.Member.Name == "Value")
                            {
                                return column;
                            }
                            else if (member.Member.Name == "HasValue")
                            {
                                return $"({column} IS NOT NULL)";
                            }
                        }

                        if (member.Member.MemberType != MemberTypes.Property)
                        {
                            var value = GetValue(member);
                            return FormatValue(parasHelper, value, entityTypeAttr);
                        }

                        if (member.Expression is MemberExpression innerMember || member.Expression == null)
                        {
                            var value = GetValue(member);
                            return FormatValue(parasHelper, value, entityTypeAttr);
                        }

                        return member.Member.Name;
                    }

                case UnaryExpression unary when unary.Type == typeof(DateTime):
                    {
                        DateTime dateTime =
                            Expression.Lambda<Func<DateTime>>(unary).Compile().Invoke();
                        return FormatValue(parasHelper, dateTime, entityTypeAttr);
                    }
                case UnaryExpression unary when unary.Operand is not null:
                    {
                        if (unary.NodeType == ExpressionType.Negate)
                        {
                            return $"-{BuildWhereClause(parasHelper, unary.Operand)}";
                        }
                        return BuildWhereClause(parasHelper, unary.Operand);
                    }
                case NewExpression exp when exp.Type == typeof(DateTime):
                    {
                        DateTime dateTime =
                            Expression.Lambda<Func<DateTime>>(exp).Compile().Invoke();
                        return FormatValue(parasHelper, dateTime, entityTypeAttr);
                    }
                case MethodCallExpression methodCall when methodCall.Method == typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }):
                    {
                        MemberExpression m = methodCall.Object as MemberExpression;

                        var column = m.Member.Name;

                        string value = string.Empty;

                        var v = GetValue(methodCall.Arguments[0]);
                        value = FormatValue(parasHelper, $"%{v}%", GetEntityTypeAttr(column));

                        return $"({column} LIKE {value})";
                    }
                case MethodCallExpression methodCall
                    when methodCall.Method.Name == nameof(Enumerable.Contains):
                    {
                        if (methodCall.Arguments.Count < 2 && methodCall.Object != null)
                        {
                            var name = BuildWhereClause(parasHelper, methodCall.Arguments[0]);
                            var value = BuildWhereClause(parasHelper, methodCall.Object, GetEntityTypeAttr(name));

                            return $"({name} IN {value})";
                        }
                        else if (methodCall.Arguments.Count == 2)
                        {
                            var column = BuildWhereClause(parasHelper, methodCall.Arguments[1]);
                            var value = BuildWhereClause(parasHelper, methodCall.Arguments[0], GetEntityTypeAttr(column));

                            return $"({column} IN {value})";
                        }
                        else
                        {
                            throw new NotSupportedException($"Expression type '{expression.GetType().Name}' not supported.");
                        }
                    }
                case MethodCallExpression methodCall
                    when methodCall.Method.Name == nameof(DateTime.AddMilliseconds)
                    || methodCall.Method.Name == nameof(DateTime.AddSeconds)
                    || methodCall.Method.Name == nameof(DateTime.AddMinutes)
                    || methodCall.Method.Name == nameof(DateTime.AddHours)
                    || methodCall.Method.Name == nameof(DateTime.AddDays)
                    || methodCall.Method.Name == nameof(DateTime.AddMonths)
                    || methodCall.Method.Name == nameof(DateTime.AddYears):
                    {
                        var name = BuildWhereClause(parasHelper, methodCall.Object);
                        var value = BuildWhereClause(parasHelper, methodCall.Arguments[0], GetEntityTypeAttr(name));
                        var sql = $"DATEADD({methodCall.Method.Name.Replace("Add", "").TrimEnd('s')}, {value}, {name})";
                        return sql;
                    }
                case ListInitExpression:
                case NewArrayExpression:
                    {
                        var result = Expression.Lambda(expression).Compile().DynamicInvoke() as IEnumerable;
                        IEnumerable<object> objectValue = result.Cast<object>();
                        return FormatValue(parasHelper, objectValue, entityTypeAttr);
                    }
                default:
                    throw new NotSupportedException($"Expression type '{expression.GetType().Name}' not supported.");
            }
        }

        private static EntityTypeAttribute? GetEntityTypeAttr(string column)
        {
            EntityTypeAttribute? entityTypeAttr = null;
            PropertyInfo? property = typeof(T).GetProperty(column);
            if (property != null)
            {
                entityTypeAttr = property.GetCustomAttribute<EntityTypeAttribute>();
            }

            return entityTypeAttr;
        }

        private static ConstantExpression GetConstantExpression(BinaryExpression binary)
        {
            if (binary.Right is ConstantExpression cRight)
            {
                return cRight;
            }
            else if (binary.Left is ConstantExpression cLeft)
            {
                return cLeft;
            }
            return null;
        }

        private static MemberExpression GetMemberExpression(BinaryExpression binary)
        {
            if (binary.Left is MemberExpression mLeft)
            {
                return mLeft;
            }
            else if (binary.Right is MemberExpression mRight)
            {
                return mRight;
            }
            return null;
        }

        private static bool IsNullableMemberExpression(MemberExpression member)
        {
            // 檢查是否為 Nullable 值的 HasValue 屬性
            if (member.Expression is MemberExpression nullableExpression)
            {
                var nullableType = nullableExpression.Type;
                return nullableType.IsGenericType && nullableType.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            return false;
        }

        private static object GetValue(Expression expression)
        {
            switch (expression)
            {
                case MemberExpression member:
                    return GetValue(member);

                case ConstantExpression constant:
                    return constant.Value?.ToString() ?? string.Empty;

                default:
                    throw new NotSupportedException($"Operator '{expression.GetType()}' not supported.");
            }
        }

        private static object GetValue(MemberExpression member)
        {
            object obj = null;
            if (member.Expression is MemberExpression innerMember)
            {
                obj = GetValue(innerMember);
            }
            else if (member.Expression is ConstantExpression constant)
            {
                obj = constant.Value;
            }
            else if (member.Expression is ParameterExpression)
            {
                return null;
            }
            var propertyInfo = member.Member as PropertyInfo;
            var fieldInfo = member.Member as FieldInfo;
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj);
            }
            else if (fieldInfo != null)
            {
                return fieldInfo.GetValue(obj);
            }
            return null;
        }

        private static string GetSqlOperator(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.Subtract:
                    return "-";

                case ExpressionType.Divide:
                    return "/";

                case ExpressionType.Multiply:
                    return "*";

                case ExpressionType.Add:
                    return "+";

                case ExpressionType.Equal:
                    return "=";

                case ExpressionType.NotEqual:
                    return "<>";

                case ExpressionType.GreaterThan:
                    return ">";

                case ExpressionType.GreaterThanOrEqual:
                    return ">=";

                case ExpressionType.LessThan:
                    return "<";

                case ExpressionType.LessThanOrEqual:
                    return "<=";

                case ExpressionType.AndAlso:
                    return "AND";

                case ExpressionType.OrElse:
                    return "OR";

                default:
                    throw new NotSupportedException($"Operator '{nodeType}' not supported.");
            }
        }

        private static string FormatValue(BuildWhereParasHelper parasHelper, object value, EntityTypeAttribute? entityTypeAttr)
        {
            if (entityTypeAttr != null)
            {
                if (value is not string && value is IEnumerable array)
                {
                    var vs = array.Cast<object>();
                    if (vs is IEnumerable<string> || vs is string[])
                    {
                        var pkey = parasHelper.Add(vs.Select(e => new DbString
                        {
                            Value = (string)e,
                            Length = entityTypeAttr.Length,
                            IsAnsi = entityTypeAttr.IsAnsi,
                            IsFixedLength = entityTypeAttr.IsFixedLength,
                        }));
                        return $"@{pkey.Key}";

                        //return $"({string.Join(",", vs.Select(e => FormatValue(parasHelper, e, entityTypeAttr)))})";
                    }

                    var r = parasHelper.Add(value);
                    return $"@{r.Key}";
                }

                string key = parasHelper.GetKey();
                parasHelper.AddDynamicParams(entityTypeAttr.GetParameters(key, value));
                return $"@{key}";
            }
            var par = parasHelper.Add(value);
            return $"@{par.Key}";
        }
    }

    public class BuildWhereParasHelper
    {
        protected int Index { get; set; }
        protected DynamicParameters Paras { get; set; }

        public BuildWhereParasHelper(DynamicParameters paras)
        {
            Index = 0;
            Paras = paras;
        }

        public static BuildWhereParasHelper Create(DynamicParameters paras)
        {
            return new BuildWhereParasHelper(paras);
        }

        public string GetKey()
        {
            return $"p{Index++}";
        }

        public (string Key, object Value) Add(object value)
        {
            string key = GetKey();
            Paras.Add(key, value);
            return (key, value);
        }

        public DynamicParameters AddDynamicParams(DynamicParameters parameters)
        {
            Paras.AddDynamicParams(parameters);
            return parameters;
        }

        public DynamicParameters GetParas()
        {
            return Paras;
        }
    }
}