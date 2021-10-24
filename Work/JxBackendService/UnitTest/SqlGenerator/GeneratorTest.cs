using JxBackendService.Common.Util;
using JxBackendService.Model;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UnitTest.Base;

namespace UnitTest.SqlGenerator
{
    [TestClass]
    public class GeneratorTest : BaseTest
    {
        private readonly string _outputBaseDirectory = "SqlGenerator\\Template\\output\\";
        [TestMethod]
        public void GenerateAll()
        {
            string[] filePaths = Directory.GetFiles("SqlGenerator\\Template");
            int startSeq = 130;

            if (!Directory.Exists(_outputBaseDirectory))
            {
                Directory.CreateDirectory(_outputBaseDirectory);
            }

            foreach (string filePath in Directory.GetFiles(_outputBaseDirectory))
            {
                File.Delete(filePath);
            }

            int currrentSeq = startSeq;

            foreach (string filePath in filePaths)
            {
                if (filePath.Contains("-rollback"))
                {
                    continue;
                }

                string rollbackFilePath = filePath.Replace(".txt", "-rollback.txt");

                string content = File.ReadAllText(filePath);
                string rollbackContent = File.ReadAllText(rollbackFilePath);

                foreach (PlatformProduct product in PlatformProduct.GetAll())
                {
                    string outputContent = content.Replace("(ProductCode)", product.Value);
                    string outputFilePath = new FileInfo(filePath).Name.Replace("(ProductCode)", product.Value).Replace(".txt", ".sql");
                    File.WriteAllText(_outputBaseDirectory + $"{currrentSeq}-{outputFilePath}", outputContent);

                    //rollback
                    string rollbackOutputContent = rollbackContent.Replace("(ProductCode)", product.Value);
                    string rollbackOutputFilePath = new FileInfo(rollbackFilePath).Name.Replace("(ProductCode)", product.Value).Replace(".txt", ".sql");
                    File.WriteAllText(_outputBaseDirectory + $"{currrentSeq}-{rollbackOutputFilePath}", rollbackOutputContent);

                    currrentSeq++;
                }
            }

            
        }

        [TestMethod]
        public void CreateAlterProGetTableSequenceSQL()
        {
            var sql = new StringBuilder();

            foreach (PlatformProduct product in PlatformProduct.GetAll())
            {
                sql.Append(CreateAlterProGetTableSequenceSQL(product));
            }

            File.WriteAllText(_outputBaseDirectory + "AlterProGetTableSequenceSQL.sql", sql.ToString());
        }

        [TestMethod]
        public void CreateTransferDataSqlFile()
        {
            var sql = new StringBuilder();

            foreach (PlatformProduct product in PlatformProduct.GetAll())
            {
                sql.Append(CreateTransferDataSQL(product));
            }

            File.WriteAllText(_outputBaseDirectory + "InsertTransferDataSQL.sql", sql.ToString());
        }

        [TestMethod]
        public void CreateClearDataSqlFile()
        {
            var sql = new StringBuilder();

            foreach (PlatformProduct product in PlatformProduct.GetAll())
            {
                sql.Append(CreateClearDataSQL(product));
            }

            File.WriteAllText(_outputBaseDirectory + "InsertClearDataSQL.sql", sql.ToString());
        }

        [TestMethod]
        public void GenerateCUD()
        {
            string configFilePath = "..\\..\\..\\GenerateStoredProcedures\\GenerateStoredProcedures\\bin\\Debug\\GenerateStoredProcedures.exe.config";
            string configContnet = File.ReadAllText(configFilePath);
            var tableNames = new List<string>();

            foreach (PlatformProduct product in PlatformProduct.GetAll())
            {
                tableNames.Add($"VIPFlowProduct{product.Value}Log");
                tableNames.Add($"VIPPointsProduct{product.Value}Log");
            }

            configContnet = configContnet.Replace("{TableName}", string.Join(",", tableNames));
            File.WriteAllText(configFilePath, configContnet);
            System.Diagnostics.Process.Start("..\\..\\..\\GenerateStoredProcedures\\GenerateStoredProcedures\\bin\\Debug\\GenerateStoredProcedures.exe");            
        }

        private string CreateAlterProGetTableSequenceSQL(PlatformProduct product)
        {
            string appendSQL = $@"
    ELSE IF @TableName = 'VIPPointsProduct{product.Value}Log'
    BEGIN
        SET @SequenceName = 'SEQ_VIPPointsProduct{product.Value}Log_SEQID' 
    END;
    ELSE IF @TableName = 'VIPFlowProduct{product.Value}Log'
    BEGIN
        SET @SequenceName = 'SEQ_VIPFlowProduct{product.Value}Log_SEQID' 
    END;";
            return appendSQL;
        }

        private string CreateTransferDataSQL(PlatformProduct product)
        {
            string sql = $@"
    INSERT INTO VIPPointsProduct{product.Value}Log(
		SEQID,
		UserID,		
		CreateUser,
		CreateDate,
		UpdateUser,
		UpdateDate,
		ChangeType,
		OldAccumulatePoints,
		NewAccumulatePoints,
		ChangePoints,
		MemoJson,
		BetTime,
		ProfitLossTime,
		BetMoney,
		AllBetMoney,
		PalyID)
	SELECT  
		SEQID,
		UserID,
		CreateUser,
		CreateDate,
		UpdateUser,
		UpdateDate,
		ChangeType,
		OldAccumulatePoints,
		NewAccumulatePoints,
		ChangePoints,
		MemoJson,
		BetTime,
		ProfitLossTime,
		BetMoney,
		AllBetMoney,
		PalyID
	FROM Inlodb.dbo.VIPPointsProduct{product.Value}Log WITH(NOLOCK)  
	WHERE CreateDate > @StartTime AND CreateDate <= @EndTime
  

	INSERT INTO VIPFlowProduct{product.Value}Log(
		SEQID,
		UserID,
		CreateUser,
		CreateDate,
		UpdateUser,
		UpdateDate,
		ChangeType,
		ChangeFlowAmount,
		Multiple,
		OldFlowAccountAmount,
		NewFlowAccountAmount,
		RefID,
		MemoJson)
	SELECT
		SEQID,
		UserID,
		CreateUser,
		CreateDate,
		UpdateUser,
		UpdateDate,
		ChangeType,
		ChangeFlowAmount,
		Multiple,
		OldFlowAccountAmount,
		NewFlowAccountAmount,
		RefID,
		MemoJson
	FROM Inlodb.dbo.VIPFlowProduct{product.Value}Log WITH(NOLOCK)  
	WHERE CreateDate > @StartTime AND CreateDate <= @EndTime
";

            return sql;
        }

		private string CreateClearDataSQL(PlatformProduct product)
		{
			string sql = $@"
    INSERT INTO #SEQIDS (SEQID)
    SELECT TOP (@RowCount) SEQID	
	FROM VIPPointsProduct{product.Value}Log WITH(NOLOCK)
    WHERE CreateDate < @MaxLogTime

    DELETE VIPPointsProduct{product.Value}Log
    WHERE SEQID IN (SELECT SEQID FROM #SEQIDS )

	TRUNCATE TABLE #SEQIDS

	INSERT INTO #SEQIDS (SEQID)
    SELECT TOP (@RowCount) SEQID	
	FROM VIPFlowProduct{product.Value}Log WITH(NOLOCK)
    WHERE CreateDate < @MaxLogTime

    DELETE VIPFlowProduct{product.Value}Log
    WHERE SEQID IN (SELECT SEQID FROM #SEQIDS )

	TRUNCATE TABLE #SEQIDS	
";

			return sql;
		}		
	}
}