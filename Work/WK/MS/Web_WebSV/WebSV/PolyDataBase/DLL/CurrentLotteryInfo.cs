using Microsoft.Data.SqlClient;
using PolyDataBase.Extensions;
using SLPolyGame.Web.Common;
using SLPolyGame.Web.DBUtility;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SLPolyGame.Web.DAL
{
    /// <summary>
    /// 数据访问类:CurrentLotteryInfo
    /// </summary>
    public class CurrentLotteryInfo
    {
        public CurrentLotteryInfo()
        { }

        #region Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("CurrentLotteryID", "CurrentLotteryInfo");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CurrentLotteryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from CurrentLotteryInfo");
            strSql.Append(" where CurrentLotteryID=@CurrentLotteryID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CurrentLotteryID", SqlDbType.Int,4)};
            parameters[0].Value = CurrentLotteryID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.CurrentLotteryInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CurrentLotteryInfo(");
            strSql.Append("CurrentLotteryTime,LotteryType,CurrentLotteryNum,LotteryID,IssueNo,AddTime,UpdateTime,IsLottery)");
            strSql.Append(" values (");
            strSql.Append("@CurrentLotteryTime,@LotteryType,@CurrentLotteryNum,@LotteryID,@IssueNo,@AddTime,@UpdateTime,@IsLottery)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@CurrentLotteryTime", SqlDbType.DateTime),
                    new SqlParameter("@LotteryType", SqlDbType.NVarChar,50),
                    new SqlParameter("@CurrentLotteryNum", SqlDbType.NVarChar,50),
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                    new SqlParameter("@IssueNo", SqlDbType.NVarChar,50),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@IsLottery", SqlDbType.Bit,1)};
            parameters[0].Value = model.CurrentLotteryTime;
            parameters[1].Value = model.LotteryType;
            parameters[2].Value = model.CurrentLotteryNum;
            parameters[3].Value = model.LotteryID;
            parameters[4].Value = model.IssueNo;
            parameters[5].Value = model.AddTime;
            parameters[6].Value = model.UpdateTime;
            parameters[7].Value = model.IsLottery;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.CurrentLotteryInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CurrentLotteryInfo set ");
            strSql.Append("CurrentLotteryTime=@CurrentLotteryTime,");
            strSql.Append("LotteryType=@LotteryType,");
            strSql.Append("CurrentLotteryNum=@CurrentLotteryNum,");
            strSql.Append("LotteryID=@LotteryID,");
            strSql.Append("IssueNo=@IssueNo,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("IsLottery=@IsLottery");
            strSql.Append(" where CurrentLotteryID=@CurrentLotteryID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CurrentLotteryID", SqlDbType.Int,4),
                    new SqlParameter("@CurrentLotteryTime", SqlDbType.DateTime),
                    new SqlParameter("@LotteryType", SqlDbType.NVarChar,50),
                    new SqlParameter("@CurrentLotteryNum", SqlDbType.NVarChar,50),
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                    new SqlParameter("@IssueNo", SqlDbType.NVarChar,50),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@IsLottery", SqlDbType.Bit,1)};
            parameters[0].Value = model.CurrentLotteryID;
            parameters[1].Value = model.CurrentLotteryTime;
            parameters[2].Value = model.LotteryType;
            parameters[3].Value = model.CurrentLotteryNum;
            parameters[4].Value = model.LotteryID;
            parameters[5].Value = model.IssueNo;
            parameters[6].Value = model.AddTime;
            parameters[7].Value = model.UpdateTime;
            parameters[8].Value = model.IsLottery;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int CurrentLotteryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from CurrentLotteryInfo ");
            strSql.Append(" where CurrentLotteryID=@CurrentLotteryID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CurrentLotteryID", SqlDbType.Int,4)
};
            parameters[0].Value = CurrentLotteryID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string CurrentLotteryIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from CurrentLotteryInfo ");
            strSql.Append(" where CurrentLotteryID in (" + CurrentLotteryIDlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.CurrentLotteryInfo GetModel(int CurrentLotteryID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CurrentLotteryID,CurrentLotteryTime,LotteryType,CurrentLotteryNum,LotteryID,IssueNo,AddTime,UpdateTime,IsLottery from CurrentLotteryInfo ");
            strSql.Append(" where CurrentLotteryID=@CurrentLotteryID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CurrentLotteryID", SqlDbType.Int,4)
};
            parameters[0].Value = CurrentLotteryID;

            Model.CurrentLotteryInfo model = new Model.CurrentLotteryInfo();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CurrentLotteryID"].ToString() != "")
                {
                    model.CurrentLotteryID = int.Parse(ds.Tables[0].Rows[0]["CurrentLotteryID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString() != "")
                {
                    model.CurrentLotteryTime = DateTime.Parse(ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString());
                }
                model.LotteryType = ds.Tables[0].Rows[0]["LotteryType"].ToString();
                model.CurrentLotteryNum = ds.Tables[0].Rows[0]["CurrentLotteryNum"].ToString();
                if (ds.Tables[0].Rows[0]["LotteryID"].ToString() != "")
                {
                    model.LotteryID = int.Parse(ds.Tables[0].Rows[0]["LotteryID"].ToString());
                }
                model.IssueNo = ds.Tables[0].Rows[0]["IssueNo"].ToString();
                if (ds.Tables[0].Rows[0]["AddTime"].ToString() != "")
                {
                    model.AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(ds.Tables[0].Rows[0]["UpdateTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsLottery"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsLottery"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsLottery"].ToString().ToLower() == "true"))
                    {
                        model.IsLottery = true;
                    }
                    else
                    {
                        model.IsLottery = false;
                    }
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CurrentLotteryID,CurrentLotteryTime,LotteryType,CurrentLotteryNum,LotteryID,IssueNo,AddTime,UpdateTime,IsLottery ");
            strSql.Append(" FROM CurrentLotteryInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" CurrentLotteryID,CurrentLotteryTime,LotteryType,CurrentLotteryNum,LotteryID,IssueNo,AddTime,UpdateTime,IsLottery ");
            strSql.Append(" FROM CurrentLotteryInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "CurrentLotteryInfo";
			parameters[1].Value = "";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion Method

        #region 前

        /// <summary>
        /// 获取彩票信息
        /// </summary>
        /// <param name="lotteryid"></param>
        /// <returns></returns>
        public Model.CurrentLotteryInfo GetLotteryInfos(int lotteryid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"DECLARE @LotteryNo NVARCHAR(50)
                            --当前期号
                            DECLARE @EndTime DATETIME
                            --当前期封单时间
                            DECLARE @CurrentTime DATETIME
                            --当前时间
                            DECLARE @RemainTime FLOAT
                            --剩余时间
                            DECLARE @lottery_result NVARCHAR(50)
                            --上期开奖结果
                            DECLARE @PreLotteryNo NVARCHAR(50)
                            --上期期号
                            DECLARE @LotteryType NVARCHAR(50)
                            --彩种名称

                            SELECT TOP 1
                                    @LotteryType = LotteryType
                            FROM    dbo.LotteryInfo WITH ( NOLOCK )
                            WHERE   LotteryID = @lotteryid
                                    AND [Status] = 1

                            IF ( @LotteryType IS NOT NULL )
                                BEGIN

                                    SELECT TOP 1
                                            @LotteryNo = IssueNo ,
                                            @EndTime = CurrentLotteryTime
                                    FROM    dbo.currentlotteryinfo WITH ( NOLOCK )
                                    WHERE   lotteryid = @lotteryid
                                            AND IsLottery = 0
                                            AND CurrentLotteryTime > GETDATE()
                                    ORDER BY CurrentLotteryTime ASC

                                    IF ( @endTime IS NOT NULL )
                                        SET @RemainTime = ( DATEDIFF(ms, GETDATE(), @endTime) )

                                    SELECT TOP 1
                                            @PreLotteryNo = IssueNo ,
                                            @lottery_result = CurrentLotteryNum
                                    FROM    dbo.currentlotteryinfo WITH ( NOLOCK )
                                    WHERE   lotteryid = @lotteryid
                                            AND CurrentLotteryTime < GETDATE()
                                    ORDER BY CurrentLotteryTime DESC

                                END
                            ELSE
                                BEGIN
                                    SELECT TOP 1
                                            @PreLotteryNo = IssueNo ,
                                            @lottery_result = CurrentLotteryNum
                                    FROM    dbo.currentlotteryinfo WITH ( NOLOCK )
                                    WHERE   lotteryid = @lotteryid
                                            AND IsLottery = 1
                                            AND CurrentLotteryTime < GETDATE()
                                    ORDER BY CurrentLotteryTime DESC
                                END

                            IF ( @PreLotteryNo = @LotteryNo )
                                SET @PreLotteryNo = ''

                            SELECT  @LotteryType AS LotteryType ,
                                    @LotteryNo AS LotteryNo ,
                                    @EndTime AS EndTime ,
                                    @RemainTime AS RemainTime ,
                                    @lottery_result AS Lottery_result ,
                                    @PreLotteryNo AS PreLotteryNo ,
                                    ( GETDATE() ) AS CurrentTime
                            ");
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4)
                                        };
            parameters[0].Value = lotteryid;

            DataSet ds = DbHelperSQL.Main.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ConvertToNextIssueNo(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        private static Model.CurrentLotteryInfo ConvertToNextIssueNo(DataRow row)
        {
            var model = new Model.CurrentLotteryInfo();
            
            if (row.Table.Columns.Contains("LotteryID") && row["LotteryID"].ToString() != "")
            {
                model.LotteryID = int.Parse(row["LotteryID"].ToString());
            }
            if (row["LotteryType"].ToString() != "")
            {
                model.LotteryType = row["LotteryType"].ToString();
            }
            if (row["LotteryNo"].ToString() != "")
            {
                model.LotteryNo = row["LotteryNo"].ToString();
            }
            if (row["EndTime"].ToString() != "")
            {
                model.EndTime = DateTime.Parse(row["EndTime"].ToString());
            }
            if (row["CurrentTime"].ToString() != "")
            {
                model.CurrentTime = DateTime.Parse(row["CurrentTime"].ToString());
            }
            if (row["Lottery_result"].ToString() != "")
            {
                model.Lottery_result = row["Lottery_result"].ToString();
            }
            if (row["PreLotteryNo"].ToString() != "")
            {
                model.PreLotteryNo = row["PreLotteryNo"].ToString();
            }
            if (row["RemainTime"].ToString() != "")
            {
                model.RemainTime = float.Parse(row["RemainTime"].ToString());
            }
            return model;
        }

        public IEnumerable<Model.CurrentLotteryInfo> GetNextIssueNos(string lotteryIds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"DROP TABLE IF EXISTS #TotalResult

                            CREATE TABLE #TotalResult(
                                LotteryID INT,
                                LotteryType NVARCHAR(50), --彩种名称
                                LotteryNo NVARCHAR(50), --当前期号
                                EndTime DATETIME, --当前期封单时间
                                RemainTime FLOAT, --剩余时间
                                Lottery_result NVARCHAR(50), --上期开奖结果
                                PreLotteryNo NVARCHAR(50) --上期期号    
                            )

                            INSERT INTO #TotalResult(
                                LotteryID
                            )
                            SELECT [value]
                            FROM OPENJSON(@LotteryIdsJson)


                            UPDATE TR
                            SET
                                LotteryType = LI.LotteryType
                            FROM #TotalResult TR
                            INNER JOIN dbo.LotteryInfo LI WITH(NOLOCK) ON 
                                TR.LotteryID = LI.LotteryID AND 
                                LI.[Status] = 1

                            DROP TABLE IF EXISTS #CurrentLotteryInfoAfterNow

                            SELECT
                                TR.LotteryID,
                                (SELECT TOP 1 
                                    CL.CurrentLotteryID
                                FROM dbo.CurrentLotteryInfo CL WITH(NOLOCK)
                                WHERE 
                                    CL.LotteryID = TR.LotteryID AND 
                                    CL.IsLottery = 0 AND 
                                    CL.CurrentLotteryTime > GETDATE()
                                ORDER BY CL.CurrentLotteryTime ASC) AS CurrentLotteryID
                            INTO #CurrentLotteryInfoAfterNow
                            FROM #TotalResult TR
                            WHERE TR.LotteryType IS NOT NULL

                            DECLARE @Now DATETIME = GETDATE()

                            UPDATE TR
                            SET
                                LotteryNo = CL.IssueNo,
                                EndTime = CL.CurrentLotteryTime,
                                RemainTime = CASE WHEN CL.CurrentLotteryTime IS NOT NULL THEN DATEDIFF(MS, @Now, CL.CurrentLotteryTime) END
                            FROM #TotalResult TR
                            INNER JOIN #CurrentLotteryInfoAfterNow AN ON TR.LotteryID = AN.LotteryID
                            INNER JOIN CurrentLotteryInfo CL ON CL.CurrentLotteryID = AN.CurrentLotteryID

                            DROP TABLE IF EXISTS #CurrentLotteryInfoBeforeNow

                            SELECT
                                TR.LotteryID,
                                (SELECT TOP 1 CurrentLotteryID
                                FROM dbo.CurrentLotteryInfo CL WITH(NOLOCK)
                                WHERE 
                                    CL.LotteryID = TR.LotteryID AND 
                                    CurrentLotteryTime < GETDATE() AND
                                    (TR.LotteryType IS NOT NULL OR (TR.LotteryType IS NULL AND IsLottery = 1))
                                ORDER BY CurrentLotteryTime DESC) AS CurrentLotteryID
                            INTO #CurrentLotteryInfoBeforeNow
                            FROM #TotalResult TR
                            WHERE TR.LotteryType IS NOT NULL

                            UPDATE TR
                            SET
                                PreLotteryNo = CASE WHEN TR.LotteryNo = CL.IssueNo THEN '' ELSE CL.IssueNo END,
                                Lottery_result = CL.CurrentLotteryNum	
                            FROM #TotalResult TR
                            INNER JOIN #CurrentLotteryInfoBeforeNow BN ON TR.LotteryID = BN.LotteryID
                            INNER JOIN CurrentLotteryInfo CL ON CL.CurrentLotteryID = BN.CurrentLotteryID

                            SELECT
                                LotteryID,
                                LotteryType,
                                LotteryNo,
                                EndTime,
                                RemainTime,
                                Lottery_result,
                                PreLotteryNo,
                                GETDATE() AS CurrentTime
                            FROM #TotalResult");
            
            SqlParameter[] parameters = {
                new SqlParameter("@LotteryIdsJson", SqlDbType.VarChar,1000)
            };
            
            parameters[0].Value = lotteryIds;

            DataSet ds = DbHelperSQL.Main.Query(strSql.ToString(), parameters);
            if (ds?.Tables != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        yield return ConvertToNextIssueNo(row);
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取即将开奖的一期的信息
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public Model.CurrentLotteryInfo GetCurrenIssuNo(int lotteryId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from dbo.CurrentLotteryInfo with(nolock) where CurrentLotteryNum='' and LotteryID=@LotteryID and currentlotterytime>=getdate()");
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4)
                                        };
            parameters[0].Value = lotteryId;

            Model.CurrentLotteryInfo model = new Model.CurrentLotteryInfo();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CurrentLotteryID"].ToString() != "")
                {
                    model.CurrentLotteryID = int.Parse(ds.Tables[0].Rows[0]["CurrentLotteryID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString() != "")
                {
                    model.CurrentLotteryTime = DateTime.Parse(ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString());
                }
                model.LotteryType = ds.Tables[0].Rows[0]["LotteryType"].ToString();
                model.CurrentLotteryNum = ds.Tables[0].Rows[0]["CurrentLotteryNum"].ToString();
                if (ds.Tables[0].Rows[0]["LotteryID"].ToString() != "")
                {
                    model.LotteryID = int.Parse(ds.Tables[0].Rows[0]["LotteryID"].ToString());
                }
                model.IssueNo = ds.Tables[0].Rows[0]["IssueNo"].ToString();
                if (ds.Tables[0].Rows[0]["AddTime"].ToString() != "")
                {
                    model.AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(ds.Tables[0].Rows[0]["UpdateTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsLottery"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsLottery"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsLottery"].ToString().ToLower() == "true"))
                    {
                        model.IsLottery = true;
                    }
                    else
                    {
                        model.IsLottery = false;
                    }
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 倒数第二期开奖信息
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public Model.CurrentLotteryInfo GetLastLotteryNum(int lotteryId, string cunum)
        {
            if (!SafeSql.CheckParams(cunum))
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from dbo.CurrentLotteryInfo with(nolock) where IssueNo<>@cunum and LotteryID=@LotteryID order by CurrentLotteryTime desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                    new SqlParameter("@cunum", SqlDbType.NVarChar,50)
};
            parameters[0].Value = lotteryId;
            parameters[1].Value = cunum;

            Model.CurrentLotteryInfo model = new Model.CurrentLotteryInfo();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CurrentLotteryID"].ToString() != "")
                {
                    model.CurrentLotteryID = int.Parse(ds.Tables[0].Rows[0]["CurrentLotteryID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString() != "")
                {
                    model.CurrentLotteryTime = DateTime.Parse(ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString());
                }
                model.LotteryType = ds.Tables[0].Rows[0]["LotteryType"].ToString();
                model.CurrentLotteryNum = ds.Tables[0].Rows[0]["CurrentLotteryNum"].ToString();
                if (ds.Tables[0].Rows[0]["LotteryID"].ToString() != "")
                {
                    model.LotteryID = int.Parse(ds.Tables[0].Rows[0]["LotteryID"].ToString());
                }
                model.IssueNo = ds.Tables[0].Rows[0]["IssueNo"].ToString();
                if (ds.Tables[0].Rows[0]["AddTime"].ToString() != "")
                {
                    model.AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(ds.Tables[0].Rows[0]["UpdateTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsLottery"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsLottery"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsLottery"].ToString().ToLower() == "true"))
                    {
                        model.IsLottery = true;
                    }
                    else
                    {
                        model.IsLottery = false;
                    }
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取前4期开奖情况 old
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public List<Model.CurrentLotteryInfo> GetCurrentLotteryInfo(int lotteryId)
        {
            string strSql = "select top 4  CurrentLotteryTime,CurrentLotteryNum,IssueNo,LotteryID,UpdateTime from dbo.CurrentLotteryInfo with(nolock) where  LotteryID=@LotteryID and CurrentLotteryNum<>'' order by CurrentLotteryTime desc ";
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4)};
            parameters[0].Value = lotteryId;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        public List<Model.CurrentLotteryInfo> GetCurrentLotteryInfo(int lotteryId, int count)
        {
            string strSql = "select top (@Count) CurrentLotteryTime,CurrentLotteryNum,IssueNo,LotteryID,UpdateTime from dbo.CurrentLotteryInfo with(nolock) where  LotteryID=@LotteryID and CurrentLotteryNum<>'' order by CurrentLotteryTime desc ";
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                    new SqlParameter("@Count", SqlDbType.Int,4)
                    };
            parameters[0].Value = lotteryId;
            parameters[1].Value = count;
            //parameters[2].Value = DateTime.Now.Date;
            //parameters[3].Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        public List<Model.CurrentLotteryInfo> GetCurrentLotteryInfo1(int lotteryId, int count)
        {
            string strSql = "select top (@Count) CurrentLotteryTime,CurrentLotteryNum,IssueNo,LotteryID,UpdateTime from dbo.CurrentLotteryInfo with(nolock) where  LotteryID=@LotteryID and CurrentLotteryNum<>'' order by CurrentLotteryTime desc ";
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                    new SqlParameter("@Count", SqlDbType.Int,4)
                    };
            parameters[0].Value = lotteryId;
            parameters[1].Value = count;
            //parameters[2].Value = DateTime.Now.Date;
            //parameters[3].Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        public List<Model.CurrentLotteryInfo> GetCurrentLotteryInfo_Sec(int lotteryId, int count, int UserID)
        {
            string strSql = @"select top (@Count) AddTime AS CurrentLotteryTime,CurrentLotteryNum,IssueNo,LotteryID,UpdateTime
            from dbo.SecHistoryLotteryInfo with(nolock) where  LotteryID=@LotteryID AND UserID=@UserID AND CurrentLotteryNum<>'' order by CurrentLotteryID desc ";
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                    new SqlParameter("@Count", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)
                    };
            parameters[0].Value = lotteryId;
            parameters[1].Value = count;
            parameters[2].Value = UserID;
            //parameters[2].Value = DateTime.Now.Date;
            //parameters[3].Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        public List<Model.CurrentLotteryInfo> GetCurrentLotteryInfo_Sec1(int lotteryId, int count, int UserID)
        {
            string strSql = @"select top (@Count) AddTime AS CurrentLotteryTime,CurrentLotteryNum,IssueNo,LotteryID,UpdateTime
            from dbo.SecHistoryLotteryInfo with(nolock) where  LotteryID=@LotteryID AND UserID=@UserID AND CurrentLotteryNum<>'' order by CurrentLotteryID desc ";
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                    new SqlParameter("@Count", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)
                    };
            parameters[0].Value = lotteryId;
            parameters[1].Value = count;
            parameters[2].Value = UserID;
            //parameters[2].Value = DateTime.Now.Date;
            //parameters[3].Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        public List<Model.CurrentLotteryInfo> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query)
        {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "CurrentLotteryTime desc";
            }

            if (!SafeSql.CheckParams(query.SortBy))
            {
                return null;
            }

            StringBuilder where = new StringBuilder();
            string strSql = "select top (@Count) CurrentLotteryTime,CurrentLotteryNum,IssueNo,LotteryID,UpdateTime from dbo.CurrentLotteryInfo with(nolock) where LotteryID=@LotteryID and CurrentLotteryNum<>'' #where# order by #sort# ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Count", SqlDbType.Int, 4));
            parameters.Add(new SqlParameter("@LotteryID", SqlDbType.Int, 4));
            parameters.Add(new SqlParameter("@SortBy", SqlDbType.VarChar));
            parameters[0].Value = query.Count;
            parameters[1].Value = query.LotteryId;
            parameters[2].Value = query.SortBy;

            // 若為騰訊系列無法使用日期區間篩選，會漏篩首期，需要改用IssueNo比對
            IEnumerable<Constant.LotteryType> qqLotteryTypes = new Constant.LotteryType[] {
                Constant.LotteryType.QQSSC,
                Constant.LotteryType.QQ5,
                Constant.LotteryType.QQRCPK10,
                Constant.LotteryType.QQRC5PK10,
                Constant.LotteryType.MQQ,
                Constant.LotteryType.MQQ5
            };

            bool isQQLottery = qqLotteryTypes.Any(lt => (int)lt == query.LotteryId);

            if (isQQLottery)
            {
                string issueNoDateFormat = "yyMMdd";

                if (query.Start.HasValue)
                {
                    where.Append(" AND IssueNo >= @StartIssueNo");
                    var startIssueNoParameter = new SqlParameter("@StartIssueNo", SqlDbType.VarChar);
                    startIssueNoParameter.Value = $"{query.Start.Value.ToString(issueNoDateFormat)}0000";
                    parameters.Add(startIssueNoParameter);
                }

                if (query.End.HasValue)
                {
                    where.Append(" AND IssueNo <= @EndIssueNo");
                    var endIssueNoParameter = new SqlParameter("@EndIssueNo", SqlDbType.VarChar);
                    endIssueNoParameter.Value = $"{query.End.Value.ToString(issueNoDateFormat)}2359";
                    parameters.Add(endIssueNoParameter);
                }
            }
            else
            {
                if (query.Start != null)
                {
                    where.Append(" AND CurrentLotteryTime >=@StartTime");
                    var startTime = new SqlParameter("@StartTime", SqlDbType.DateTime);
                    startTime.Value = query.Start.Value.Date;
                    parameters.Add(startTime);
                }
                if (query.End != null)
                {
                    where.Append(" AND CurrentLotteryTime <=@EndTime");
                    var endTime = new SqlParameter("@EndTime", SqlDbType.DateTime);
                    endTime.Value = query.End.Value.Date.AddDays(1).AddSeconds(-1);
                    parameters.Add(endTime);
                }
                if (!string.IsNullOrEmpty(query.StartIssueNo))
                {
                    where.Append(" AND IssueNo >=@StartIssueNo");
                    var startIssueNo = new SqlParameter("@StartIssueNo", SqlDbType.VarChar);
                    startIssueNo.Value = query.StartIssueNo;
                    parameters.Add(startIssueNo);
                }
                if (!string.IsNullOrEmpty(query.EndIssueNo))
                {
                    where.Append(" AND IssueNo <=@EndIssueNo");
                    var endIssueNo = new SqlParameter("@EndIssueNo", SqlDbType.VarChar);
                    endIssueNo.Value = query.EndIssueNo;
                    parameters.Add(endIssueNo);
                }
            }

            strSql = strSql.Replace("#where#", where.ToString());
            strSql = strSql.Replace("#sort#", query.SortBy);

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters.ToArray());
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        public List<Model.CurrentLotteryInfo> QuerySecCurrentLotteryInfo(CurrentLotteryQueryInfo query)
        {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "CurrentLotteryID desc";
            }

            if (!SafeSql.CheckParams(query.SortBy))
            {
                return null;
            }

            StringBuilder where = new StringBuilder();
            string strSql = "select top (@Count) AddTime AS CurrentLotteryTime,CurrentLotteryNum,IssueNo,LotteryID,UpdateTime from dbo.SecHistoryLotteryInfo with(nolock) where LotteryID=@LotteryID and UserID =@UserID and CurrentLotteryNum<>'' #where# order by #sort# ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Count", SqlDbType.Int, 4));
            parameters.Add(new SqlParameter("@LotteryID", SqlDbType.Int, 4));
            parameters.Add(new SqlParameter("@SortBy", SqlDbType.VarChar));
            parameters.Add(new SqlParameter("@UserID", SqlDbType.Int, 4));
            parameters[0].Value = query.Count;
            parameters[1].Value = query.LotteryId;
            parameters[2].Value = query.SortBy;
            parameters[3].Value = query.UserID;
            if (query.Start != null)
            {
                where.Append(" AND UpdateTime >=@StartTime");
                var startTime = new SqlParameter("@StartTime", SqlDbType.DateTime);
                startTime.Value = query.Start.Value.Date;
                parameters.Add(startTime);
            }
            if (query.Start != null)
            {
                where.Append(" AND UpdateTime <=@EndTime");
                var endTime = new SqlParameter("@EndTime", SqlDbType.DateTime);
                endTime.Value = query.End.Value.Date.AddDays(1).AddSeconds(-1);
                parameters.Add(endTime);
            }
            if (!string.IsNullOrEmpty(query.StartIssueNo))
            {
                where.Append(" AND IssueNo >=@StartIssueNo");
                var startIssueNo = new SqlParameter("@StartIssueNo", SqlDbType.VarChar);
                startIssueNo.Value = query.StartIssueNo;
                parameters.Add(startIssueNo);
            }
            if (!string.IsNullOrEmpty(query.EndIssueNo))
            {
                where.Append(" AND IssueNo >=@EndIssueNo");
                var endIssueNo = new SqlParameter("@EndIssueNo", SqlDbType.VarChar);
                endIssueNo.Value = query.StartIssueNo;
                parameters.Add(endIssueNo);
            }

            strSql = strSql.Replace("#where#", where.ToString());
            strSql = strSql.Replace("#sort#", query.SortBy);

            DataSet ds = DbHelperSQL_Bak.Query(strSql.ToString(), parameters.ToArray());
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据期号查询
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public Model.CurrentLotteryInfo GetIssueNoLotteryNum(int lotteryId, string IssueNo)
        {
            if (!SafeSql.CheckParams(IssueNo))
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select top 1 * from CurrentLotteryInfo with(nolock) where  LotteryID=@LotteryID and IssueNo=@IssueNo");
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                    new SqlParameter("@IssueNo", SqlDbType.NVarChar,50)
                                        };
            parameters[0].Value = lotteryId;
            parameters[1].Value = IssueNo;
            Model.CurrentLotteryInfo model = new Model.CurrentLotteryInfo();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CurrentLotteryID"].ToString() != "")
                {
                    model.CurrentLotteryID = int.Parse(ds.Tables[0].Rows[0]["CurrentLotteryID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString() != "")
                {
                    model.CurrentLotteryTime = DateTime.Parse(ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString());
                }
                model.LotteryType = ds.Tables[0].Rows[0]["LotteryType"].ToString();
                model.CurrentLotteryNum = ds.Tables[0].Rows[0]["CurrentLotteryNum"].ToString();
                if (ds.Tables[0].Rows[0]["LotteryID"].ToString() != "")
                {
                    model.LotteryID = int.Parse(ds.Tables[0].Rows[0]["LotteryID"].ToString());
                }
                model.IssueNo = ds.Tables[0].Rows[0]["IssueNo"].ToString();
                if (ds.Tables[0].Rows[0]["AddTime"].ToString() != "")
                {
                    model.AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(ds.Tables[0].Rows[0]["UpdateTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsLottery"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsLottery"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsLottery"].ToString().ToLower() == "true"))
                    {
                        model.IsLottery = true;
                    }
                    else
                    {
                        model.IsLottery = false;
                    }
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 最后一条 new
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public Model.CurrentLotteryInfo GetEndLotteryNum(int lotteryId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select top 1 * from  dbo.CurrentLotteryInfo with(nolock) where LotteryID=@LotteryID
order by CurrentLotteryTime desc ");
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4)
                                        };
            parameters[0].Value = lotteryId;

            Model.CurrentLotteryInfo model = new Model.CurrentLotteryInfo();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["CurrentLotteryID"].ToString() != "")
                {
                    model.CurrentLotteryID = int.Parse(ds.Tables[0].Rows[0]["CurrentLotteryID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString() != "")
                {
                    model.CurrentLotteryTime = DateTime.Parse(ds.Tables[0].Rows[0]["CurrentLotteryTime"].ToString());
                }
                model.LotteryType = ds.Tables[0].Rows[0]["LotteryType"].ToString();
                model.CurrentLotteryNum = ds.Tables[0].Rows[0]["CurrentLotteryNum"].ToString();
                if (ds.Tables[0].Rows[0]["LotteryID"].ToString() != "")
                {
                    model.LotteryID = int.Parse(ds.Tables[0].Rows[0]["LotteryID"].ToString());
                }
                model.IssueNo = ds.Tables[0].Rows[0]["IssueNo"].ToString();
                if (ds.Tables[0].Rows[0]["AddTime"].ToString() != "")
                {
                    model.AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(ds.Tables[0].Rows[0]["UpdateTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsLottery"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsLottery"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsLottery"].ToString().ToLower() == "true"))
                    {
                        model.IsLottery = true;
                    }
                    else
                    {
                        model.IsLottery = false;
                    }
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        #endregion 前

        /// <summary>
        ///  DataTable转List<model>
        /// </summary>
        public static List<Model.CurrentLotteryInfo> ToList(DataTable dt)
        {
            // 定义集合
            List<Model.CurrentLotteryInfo> ts = new List<Model.CurrentLotteryInfo>();

            // 获得此模型的类型
            Type type = typeof(Model.CurrentLotteryInfo);

            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                Model.CurrentLotteryInfo t = new Model.CurrentLotteryInfo();

                // 获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;

                    // 检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            if (
                                (value.GetType() == typeof(Int64) && pi.PropertyType == typeof(string))
                                || (value.GetType() == typeof(Int32) && pi.PropertyType == typeof(string))
                                )
                                pi.SetValue(t, value.ToString(), null);
                            else
                                pi.SetValue(t, value, null);
                        }
                    }
                }

                ts.Add(t);
            }

            return ts;
        }

        public QueryResult<Model.CurrentLotteryInfo> QueryLotteryHistoryResult(int? lotteryId, string issueNo, DateTime? startDate, DateTime? endDate, int pageindex, int pagesize, string sort)
        {
            sort = "UpdateTime DESC";
            QueryResult<Model.CurrentLotteryInfo> result = new QueryResult<Model.CurrentLotteryInfo>();
            string sql = @"
SELECT @TotalCount = COUNT(1)
FROM dbo.CurrentLotteryInfo WITH(NOLOCK)
#Where#

SELECT TOP (@PageSize)
        CurrentLotteryID,
        CurrentLotteryTime ,
        LotteryType ,
        CurrentLotteryNum ,
        LotteryID ,
        IssueNo ,
        AddTime ,
        UpdateTime ,
        IsLottery
FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY #SortField# ) AS RowNumber ,
                    CurrentLotteryID,
                    CurrentLotteryTime ,
                    LotteryType ,
                    CurrentLotteryNum ,
                    LotteryID ,
                    IssueNo ,
                    AddTime ,
                    UpdateTime ,
                    IsLottery
          FROM      dbo.CurrentLotteryInfo WITH ( NOLOCK )
          #Where#
        ) A
WHERE   RowNumber > @PageIndex * @PageSize ORDER BY #SortField#";

            StringBuilder filter = new StringBuilder();
            filter.Append("WHERE 1=1");
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@PageIndex", pageindex));
            paramList.Add(new SqlParameter("@PageSize", pagesize));
            paramList.Add(new SqlParameter("@Sort", sort));
            paramList.Add(new SqlParameter { ParameterName = "@TotalCount", Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int });
            if (lotteryId != null)
            {
                filter.Append(" AND LotteryId = @LotteryId");
                paramList.Add(new SqlParameter("@LotteryId", lotteryId));
            }
            if (startDate != null)
            {
                filter.Append(" AND UpdateTime >= @StartDate");
                paramList.Add(new SqlParameter("@StartDate", startDate));
            }
            if (endDate != null)
            {
                filter.Append(" AND UpdateTime <= @EndDate");
                paramList.Add(new SqlParameter("@EndDate", endDate.Value.AddDays(1).AddSeconds(-1)));
            }
            if (!string.IsNullOrEmpty(issueNo))
            {
                filter.Append(" AND IssueNo = @IssueNo");
                paramList.Add(new SqlParameter("@IssueNo", issueNo));
            }
            var dateBtween = DateTime.Now.AddDays(-2);
            filter.Append(" AND UpdateTime >= @BDate");
            paramList.Add(new SqlParameter("@BDate", dateBtween.Date));

            sql = sql.Replace("#Where#", filter.ToString());
            sql = sql.Replace("#SortField#", sort);
            DataSet ds = DbHelperSQL.Query(sql.ToString(), paramList.ToArray());
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        result.Results = ToList(ds.Tables[0]);
                        result.TotalCount = Convert.ToInt32(paramList.First(p => p.ParameterName == "@TotalCount").Value);
                    }
                }
            }

            return result;
        }

        public CursorPagination<Model.CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor)
        {
            var emptyResult = new CursorPagination<Model.CurrentLotteryInfo>
            {
                Data = Array.Empty<Model.CurrentLotteryInfo>(),
                NextCursor = string.Empty
            };

            if ((start > end) || (count <= 0))
            {
                return emptyResult;
            }

            if (!cursor.IsNullOrEmpty())
            {
                try
                {
                    cursor = cursor.FromBase64String();
                }
                catch (Exception ex)
                {
                    return emptyResult;
                }
            }

            string sql = @"SELECT TOP (@Count) [CurrentLotteryID],[CurrentLotteryTime]
                          ,[LotteryType],[CurrentLotteryNum],[LotteryID],[IssueNo]
                          ,[AddTime],[UpdateTime],[IsLottery],[Msg]
                          FROM [dbo].[CurrentLotteryInfo]
                          WITH(NOLOCK)
                          WHERE [LotteryID] = @LotteryID
                          AND [CurrentLotteryNum] <> ''
                          #Cursor#
                          AND [CurrentLotteryTime] BETWEEN @Start AND @End
                          ORDER BY [CurrentLotteryID] DESC";

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Count", count));
            parameters.Add(new SqlParameter("@LotteryID", lotteryId));

            if (!cursor.IsNullOrEmpty())
            {
                if (int.TryParse(cursor, out int cursorInt))
                {
                    sql = sql.Replace("#Cursor#", "AND [CurrentLotteryID] < @Cursor");
                    parameters.Add(new SqlParameter("@Cursor", cursorInt));
                }
                else
                {
                    return emptyResult;
                }
            }
            else
            {
                sql = sql.Replace("#Cursor#", string.Empty);
            }

            parameters.Add(new SqlParameter("@Start", start));
            parameters.Add(new SqlParameter("@End", end));

            DataSet ds = DbHelperSQL_Bak.Query(sql, parameters.ToArray());

            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        string nextCursor = string.Empty;
                        var data = ToList(ds.Tables[0]);

                        if (data.Any())
                        {
                            nextCursor = data.Last().CurrentLotteryID.ToString().ToBase64String();
                        }

                        return new CursorPagination<Model.CurrentLotteryInfo>()
                        {
                            Data = data,
                            NextCursor = nextCursor
                        };
                    }
                }
            }

            return emptyResult;
        }

        #region 后台

        /// <summary>
        /// 获取彩票开奖历史
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public List<Model.CurrentLotteryInfo> GetLoHistory(int lotteryId, string IssueNo, DateTime start, DateTime end)
        {
            if (!SafeSql.CheckParams(IssueNo))
            {
                return null;
            }
            string strSql = "select CurrentLotteryID,CurrentLotteryTime,CurrentLotteryNum,IssueNo,LotteryType,LotteryID,IsLottery from CurrentLotteryInfo with(nolock) where  1=1 ";
            string filtrate = "";
            SqlParameter[] parameters = {
                    new SqlParameter("@LotteryID", SqlDbType.Int,4),
                                        new SqlParameter("@IssueNo", SqlDbType.NVarChar,50),
                                        new SqlParameter("@starttime", SqlDbType.DateTime),
                                        new SqlParameter("@endtime", SqlDbType.DateTime),
                                        };
            if (lotteryId > 0)
            {
                filtrate += " and  LotteryID=@LotteryID ";
                parameters[0].Value = lotteryId;
            }
            if (IssueNo != "")
            {
                filtrate += " and  IssueNo=@IssueNo ";
                parameters[1].Value = IssueNo;
            }

            if (start != null)
            {
                filtrate += " and  CurrentLotteryTime>=@starttime ";
                parameters[2].Value = start;
            }
            if (end != null)
            {
                filtrate += " and  CurrentLotteryTime<=@endtime ";
                parameters[3].Value = end;
            }
            filtrate += " order by CurrentLotteryTime desc";
            strSql += filtrate;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        return ToList(ds.Tables[0]);
                    }
                }
            }
            return null;
        }

        #endregion 后台
    }
}