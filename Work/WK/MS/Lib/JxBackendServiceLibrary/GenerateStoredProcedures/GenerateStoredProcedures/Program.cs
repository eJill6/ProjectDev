using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GenerateStoredProcedures
{
    public class ActType
    {
        public string Value { get; private set; }

        public string Name { get; private set; }

        private ActType()
        { }

        public static ActType Insert = new ActType() { Value = "Insert", Name = "新增" };

        public static ActType Update = new ActType() { Value = "Update", Name = "修改" };

        public static ActType Delete = new ActType() { Value = "Delete", Name = "刪除" };
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            string author = WindowsIdentity.GetCurrent().Name.Split('\\').Last();
            string tableNames = ConfigurationManager.AppSettings["TableNames"];
            int initStartSeq = Convert.ToInt32(ConfigurationManager.AppSettings["StartSeq"]);
            string actionTypeFilter = ConfigurationManager.AppSettings["ActionTypeFilter"];

            string[] tableNameArray = tableNames.Split(',');

            foreach (string tableName in tableNameArray)
            {
                HashSet<string> keys = GetPrimaryKeys(tableName);
                List<ColumnInfo> columnInfos = GetColumnInfos(tableName);
                SpContent spContent = null;
                int currentFileSeq;
                SpFileName spFileName;

                if (string.IsNullOrEmpty(actionTypeFilter) || ActType.Delete.Value.Equals(actionTypeFilter, StringComparison.OrdinalIgnoreCase))
                {
                    spContent = CreateProcedureSQL(ActType.Delete, tableName, keys, columnInfos, columnInfos, author);
                    currentFileSeq = GetMaxFileSeqFromDirectory(Environment.CurrentDirectory, initStartSeq) + 1;
                    spFileName = GetFormatFileName(currentFileSeq, "CREATE", GetProcedureName(ActType.Delete, tableName));
                    File.WriteAllText(spFileName.OutputFileName, spContent.OutputContent);
                    File.WriteAllText(spFileName.RollbackFileName, spContent.RollbackContent);
                }

                //如果自動增號欄位不是KEY,就排除
                string identityColumnName = GetIdentityColumnName(tableName);
                if (!string.IsNullOrEmpty(identityColumnName) && !keys.Contains(identityColumnName))
                {
                    columnInfos.RemoveAll(r => r.ColumnName.Equals(identityColumnName, StringComparison.CurrentCultureIgnoreCase));
                }

                if (string.IsNullOrEmpty(actionTypeFilter) || ActType.Update.Value.Equals(actionTypeFilter, StringComparison.OrdinalIgnoreCase))
                {
                    //update排除 CreateDate, CreateUser欄位
                    var updateColumnInfos = columnInfos.CloneByJson();
                    updateColumnInfos.RemoveAll(r => r.ColumnName.Equals("CreateDate", StringComparison.CurrentCultureIgnoreCase) ||
                    r.ColumnName.Equals("CreateUser", StringComparison.CurrentCultureIgnoreCase));

                    spContent = CreateProcedureSQL(ActType.Update, tableName, keys, updateColumnInfos, columnInfos, author);
                    currentFileSeq = GetMaxFileSeqFromDirectory(Environment.CurrentDirectory, initStartSeq) + 1;
                    spFileName = GetFormatFileName(currentFileSeq, "CREATE", GetProcedureName(ActType.Update, tableName));
                    File.WriteAllText(spFileName.OutputFileName, spContent.OutputContent);
                    File.WriteAllText(spFileName.RollbackFileName, spContent.RollbackContent);
                }

                if (string.IsNullOrEmpty(actionTypeFilter) || ActType.Insert.Value.Equals(actionTypeFilter, StringComparison.OrdinalIgnoreCase))
                {
                    //新增一律排除自動增號欄位
                    var insertColumnInfos = columnInfos.CloneByJson();
                    insertColumnInfos.RemoveAll(r => r.ColumnName.Equals("UpdateDate", StringComparison.CurrentCultureIgnoreCase) ||
                    r.ColumnName.Equals("UpdateUser", StringComparison.CurrentCultureIgnoreCase));
                    insertColumnInfos.RemoveAll(r => r.ColumnName.Equals(identityColumnName, StringComparison.CurrentCultureIgnoreCase));

                    spContent = CreateProcedureSQL(ActType.Insert, tableName, keys, insertColumnInfos, columnInfos, author);
                    currentFileSeq = GetMaxFileSeqFromDirectory(Environment.CurrentDirectory, initStartSeq) + 1;
                    spFileName = GetFormatFileName(currentFileSeq, "CREATE", GetProcedureName(ActType.Insert, tableName));
                    File.WriteAllText(spFileName.OutputFileName, spContent.OutputContent);
                    File.WriteAllText(spFileName.RollbackFileName, spContent.RollbackContent);
                }
            }
        }

        private static SpFileName GetFormatFileName(int seq, string action, string dbObjectName)
        {
            string template = $"{seq.ToString().PadLeft(3, '0')}-{action}-{GetDbName()}-{dbObjectName}" + "{0}" + ".sql";

            //01_Alter_MerchantAccount.sql; 01_Alter_MerchantAccount-rollback.sql
            SpFileName spFileName = new SpFileName()
            {
                OutputFileName = string.Format(template, string.Empty),
                RollbackFileName = string.Format(template, "-rollback"),
            };

            return spFileName;
        }

        private static int GetMaxFileSeqFromDirectory(string directoryPath, int initMaxSeq)
        {
            int maxSeq = initMaxSeq - 1;

            if (!Directory.Exists(directoryPath))
            {
                return maxSeq;
            }

            string[] filePaths = Directory.GetFiles(directoryPath);
            foreach (string filePath in filePaths)
            {
                FileInfo fileInfo = new FileInfo(filePath);

                if (".sql".Equals(fileInfo.Extension, StringComparison.CurrentCultureIgnoreCase))
                {
                    string seqString = fileInfo.Name.Split('-').FirstOrDefault();

                    if (int.TryParse(seqString, out int seq))
                    {
                        if (seq > maxSeq)
                        {
                            maxSeq = seq;
                        }
                    }
                }
            }

            return maxSeq;
        }

        private static string GetProcedureName(ActType actType, string tableName)
        {
            return $"Pro_{actType.Value}SingleRowTo{tableName}";
        }

        private static List<ColumnInfo> GetRequiredColumns(ActType actType, HashSet<string> keys, List<ColumnInfo> columninfos)
        {
            List<ColumnInfo> requiredColumns = null;

            if (actType.Value == ActType.Insert.Value || actType.Value == ActType.Update.Value)
            {
                requiredColumns = columninfos;
            }
            else
            {
                requiredColumns = columninfos.Where(w => keys.Contains(w.ColumnName)).ToList();
            }

            return requiredColumns;
        }

        private static string GetParametersDescription(ActType actType, HashSet<string> keys, List<ColumnInfo> columninfos)
        {
            List<ColumnInfo> requiredColumns = GetRequiredColumns(actType, keys, columninfos);
            return string.Join("\r\n", requiredColumns.Select(s => $"    @{s.ColumnName} {s.Description}"));
        }

        private static string GetDeclareParameters(ActType actType, HashSet<string> keys, List<ColumnInfo> columninfos)
        {
            List<ColumnInfo> requiredColumns = GetRequiredColumns(actType, keys, columninfos);
            StringBuilder sql = new StringBuilder();

            for (int i = 0; i < requiredColumns.Count; i++)
            {
                if (i > 0)
                {
                    sql.Append("\r\n");
                }

                ColumnInfo requiredColumn = requiredColumns[i];
                string result = $"    @{requiredColumn.ColumnName} {requiredColumn.DataType.ToUpper()}";

                if ((requiredColumn.DataType.Equals("decimal", StringComparison.CurrentCultureIgnoreCase) ||
                    requiredColumn.DataType.Equals("numeric", StringComparison.CurrentCultureIgnoreCase)))
                {
                    result += $"({requiredColumn.Precision}, {requiredColumn.Scale})";
                }
                else if (requiredColumn.MaxLength.HasValue)
                {
                    result += $"({requiredColumn.MaxLength})";
                }

                if (i != requiredColumns.Count - 1)
                {
                    result += ",";
                }

                result += $" --{requiredColumn.Description}";
                sql.Append(result);
            }

            return sql.ToString();
        }

        private static string GetExecuteSample(ActType actType, string tableName, HashSet<string> keys, List<ColumnInfo> columninfos)
        {
            List<ColumnInfo> requiredColumns = GetRequiredColumns(actType, keys, columninfos);

            string sql = $"EXECUTE dbo.{GetProcedureName(actType, tableName)}\r\n";
            sql += string.Join("\r\n", requiredColumns.Select(s =>
            {
                string result = $"    @{s.ColumnName} = ";

                if (s.Precision.HasValue && s.Scale.HasValue)
                {
                    result += "0";
                }
                else if (s.MaxLength.HasValue)
                {
                    result += "''";
                }
                else if (s.DataType.Equals("datetime", StringComparison.CurrentCultureIgnoreCase))
                {
                    result += $"'{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}'";
                }
                else
                {
                    result += "'0'";
                }

                result += ",";
                return result;
            }));

            return sql.Substring(0, sql.Length - 1);
        }

        private static SpContent CreateProcedureSQL(ActType actType, string tableName, HashSet<string> keys,
            List<ColumnInfo> filterColumnInfos, List<ColumnInfo> allColumnInfos, string author)
        {
            string procedureName = GetProcedureName(actType, tableName);
            string content = null;

            if (actType.Value == ActType.Insert.Value)
            {
                content = CreateInsertSQL(tableName, filterColumnInfos, allColumnInfos);
            }
            else if (actType.Value == ActType.Update.Value)
            {
                content = CreateUpdateSQL(tableName, keys, filterColumnInfos);
            }
            else if (actType.Value == ActType.Delete.Value)
            {
                content = CreateDeleteSQL(tableName, keys);
            }

            string sql = $@"USE {GetDbName()}
GO
/****************************************************************************************
'程式代號：{procedureName}
'程式名稱：{actType.Name}單筆{tableName}
'目　　的：
'參數說明：
(
{GetParametersDescription(actType, keys, filterColumnInfos)}
)
'依存　　：無
'傳回值　：無
'副作用　：
'備　註　：
'範　例　：
{GetExecuteSample(actType, tableName, keys, filterColumnInfos)}
'版本變更：
'　Ver.  YYYY/MM/DD    AUTHOR           COMMENTS
'  ====  ==========    ==========       ==========
'    1.  {DateTime.Today.ToString("yyyy/MM/dd")}    {author + new string(' ', 17 - author.Length) + "Create"}
****************************************************************************************/
CREATE PROCEDURE [dbo].[{procedureName}]
{GetDeclareParameters(actType, keys, filterColumnInfos)}
AS
BEGIN
{content}
END;
GO

GRANT EXECUTE ON [dbo].[{procedureName}] TO [LotteryUranus];
GO
";
            SpContent spContent = new SpContent()
            {
                OutputContent = sql,
                RollbackContent = CreateProcedureRollbackSQL(actType, tableName)
            };

            return spContent;
        }

        private static string CreateProcedureRollbackSQL(ActType actType, string tableName)
        {
            string procedureName = GetProcedureName(actType, tableName);
            return $@"USE {GetDbName()}
GO
DROP PROCEDURE {procedureName}
GO ";
        }

        private static string CreateInsertSQL(string tableName, List<ColumnInfo> filterColumnInfos, List<ColumnInfo> allColumnInfos)
        {
            List<string> columnNameList = filterColumnInfos
                .Select(s => s.ColumnName)
                .Where(w => !w.Equals(GetIdentityColumnName(tableName), StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            StringBuilder sql = new StringBuilder();
            string prefixEmptyLevel1 = new string(' ', 4);
            string prefixEmptyLevel2 = new string(' ', 8);
            sql.AppendLine($"{prefixEmptyLevel1}INSERT INTO [{tableName}]");

            for (int i = 0; i < columnNameList.Count; i++)
            {
                if (i == 0)
                {
                    sql.Append($"{prefixEmptyLevel2}( ");
                }
                else
                {
                    sql.Append($"{prefixEmptyLevel2}, ");
                }

                string suffix = string.Empty;
                if (i == columnNameList.Count - 1)
                {
                    suffix = ")";
                }

                sql.Append(columnNameList[i]);

                if (columnNameList[i].Equals("CreateDate", StringComparison.CurrentCultureIgnoreCase)
                    && allColumnInfos.Any(a => a.ColumnName.Equals("UpdateDate", StringComparison.CurrentCultureIgnoreCase)))
                {
                    sql.Append($"\r\n{prefixEmptyLevel2}, UpdateDate");
                }
                if (columnNameList[i].Equals("CreateUser", StringComparison.CurrentCultureIgnoreCase)
                    && allColumnInfos.Any(a => a.ColumnName.Equals("UpdateUser", StringComparison.CurrentCultureIgnoreCase)))
                {
                    sql.Append($"\r\n{prefixEmptyLevel2}, UpdateUser");
                }

                sql.AppendLine(suffix);
            }

            sql.AppendLine($"{prefixEmptyLevel1}VALUES");

            for (int i = 0; i < columnNameList.Count; i++)
            {
                if (i == 0)
                {
                    sql.Append($"{prefixEmptyLevel2}( ");
                }
                else
                {
                    sql.Append($"{prefixEmptyLevel2}, ");
                }

                string suffix = string.Empty;
                if (i == columnNameList.Count - 1)
                {
                    suffix = ");";
                }

                sql.Append($"@{columnNameList[i]}");

                if (columnNameList[i].Equals("CreateDate", StringComparison.CurrentCultureIgnoreCase)
                    && allColumnInfos.Any(a => a.ColumnName.Equals("UpdateDate", StringComparison.CurrentCultureIgnoreCase)))
                {
                    sql.Append($"\r\n{prefixEmptyLevel2}, @CreateDate");
                }
                if (columnNameList[i].Equals("CreateUser", StringComparison.CurrentCultureIgnoreCase)
                    && allColumnInfos.Any(a => a.ColumnName.Equals("UpdateUser", StringComparison.CurrentCultureIgnoreCase)))
                {
                    sql.Append($"\r\n{prefixEmptyLevel2}, @CreateUser");
                }

                sql.AppendLine(suffix);
            }

            return sql.ToString();
        }

        private static string CreateUpdateSQL(string tableName, HashSet<string> keys, List<ColumnInfo> columninfos)
        {
            List<string> updateColumnNameList = columninfos.Where(w => !keys.Contains(w.ColumnName)).Select(s => s.ColumnName).ToList();
            string prefixEmptyLevel1 = new string(' ', 4);
            string prefixEmptyLevel2 = new string(' ', 8);

            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"{prefixEmptyLevel1}UPDATE [{tableName}] \r\n{prefixEmptyLevel1}SET ");
            sql.AppendLine($"{prefixEmptyLevel2}{string.Join($"\r\n{prefixEmptyLevel2},", updateColumnNameList.Select(s => $"{s} = @{s}"))}");
            sql.AppendLine($"{prefixEmptyLevel1}WHERE");
            sql.AppendLine($"{prefixEmptyLevel2}{string.Join($"\r\n{prefixEmptyLevel2}AND ", keys.Select(s => $"{s} = @{s}"))}");
            return sql.ToString();
        }

        private static string CreateDeleteSQL(string tableName, HashSet<string> keys)
        {
            string prefixEmptyLevel1 = new string(' ', 4);
            string prefixEmptyLevel2 = new string(' ', 8);

            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"{prefixEmptyLevel1}DELETE FROM [{tableName}] ");
            sql.AppendLine($"{prefixEmptyLevel1}WHERE");
            sql.AppendLine($"{prefixEmptyLevel2}{string.Join($"\r\n{prefixEmptyLevel2}AND ", keys.Select(s => $"{s} = @{s}"))}");
            return sql.ToString();
        }

        private static string GetIdentityColumnName(string tableName)
        {
            using (SqlConnection sqlConnection = GetSqlConnection())
            {
                string sql = "SELECT [name] FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = @tableName ";

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                return sqlConnection.ExecuteScalar<string>(sql, new
                {
                    tableName = new DbString()
                    {
                        IsAnsi = true,
                        IsFixedLength = false,
                        Length = tableName.Length,
                        Value = tableName
                    }
                });
            }
        }

        private static bool HasIdentityColumn(string tableName)
        {
            return !string.IsNullOrEmpty(GetIdentityColumnName(tableName));
        }

        private static string GetDbName() => ConfigurationManager.AppSettings["DbName"];

        private static SqlConnection GetSqlConnection()
        {
            string dbName = GetDbName();
            string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString + $";Initial Catalog={dbName}";
            return new SqlConnection(connectionString);
        }

        private static HashSet<string> GetPrimaryKeys(string tableName)
        {
            using (SqlConnection sqlConnection = GetSqlConnection())
            {
                string sql = @"
SELECT column_name as keyColumn
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
    ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY'
    AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
    AND KU.table_name = @tableName
ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION; ";

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                return sqlConnection.Query<string>(sql, new
                {
                    tableName = new DbString()
                    {
                        IsAnsi = true,
                        IsFixedLength = false,
                        Length = tableName.Length,
                        Value = tableName
                    }
                }).ToHashSet();
            }
        }

        private static List<ColumnInfo> GetColumnInfos(string tableName)
        {
            using (SqlConnection sqlConnection = GetSqlConnection())
            {
                string sql = @"
SELECT * INTO #Columns
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = @tableName

SELECT
sc.name [ColumnName],
sep.value [Description],
CO.IS_NULLABLE AS IsNullable,
CO.DATA_TYPE AS DataType,
CO.CHARACTER_MAXIMUM_LENGTH AS MaxLength,
CO.NUMERIC_PRECISION AS Precision,
CO.NUMERIC_SCALE AS Scale
from sys.tables st
inner join sys.columns sc on st.object_id = sc.object_id
left join sys.extended_properties sep on st.object_id = sep.major_id
                                        and sc.column_id = sep.minor_id
                                        and sep.name = 'MS_Description'
INNER JOIN #Columns CO ON CO.TABLE_NAME = st.name AND CO.COLUMN_NAME = sc.name
where st.name = @tableName
DROP TABLE #Columns";

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                return sqlConnection.Query<ColumnInfo>(sql, new
                {
                    tableName = new DbString()
                    {
                        IsAnsi = true,
                        IsFixedLength = false,
                        Length = tableName.Length,
                        Value = tableName
                    }
                }).ToList();
            }
        }
    }
}