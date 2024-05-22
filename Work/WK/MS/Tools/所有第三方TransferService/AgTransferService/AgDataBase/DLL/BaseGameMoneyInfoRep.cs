using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Maticsoft.DBUtility;
using System.Reflection;
using AgDataBase.Model;

namespace AgDataBase.DLL
{
    public class BaseGameMoneyInfoRep
    {
        private static readonly int _minRecheckOrderMinutes = -3;

        protected string SearchTPGameProcessingMoneyInfo(string tableKey, string tableName)
        {
            string sql = $@"
-- 撈取設定分鐘前為處理中的訂單
DECLARE @ProcessingDate DATETIME = DATEADD(MINUTE, {_minRecheckOrderMinutes}, GETDATE())
DECLARE	@StartDate DATETIME = @ProcessingDate - 3,
		@EndDate DATETIME =  @ProcessingDate

SELECT  {tableKey}, Amount, OrderID, OrderTime, Handle, 
        HandTime, UserID, UserName, [Status], Memo
FROM    Inlodb.dbo.{tableName} WITH(NOLOCK)
WHERE   ([Status] = @ProcessingStatus AND OrderTime >= @StartDate AND OrderTime <= @EndDate) OR 
         [Status] = @ManualStatus ";
            return sql;
        }
    }
}

