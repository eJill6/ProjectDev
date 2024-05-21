using System;
using System.Collections.Generic;
using System.Data;
using SLPolyGame.Web.Model;

namespace SLPolyGame.Web.BLL
{
    /// <summary>
    /// CurrentLotteryInfo
    /// </summary>
    public class CurrentLotteryInfo
    {
        private readonly DAL.CurrentLotteryInfo dal = new DAL.CurrentLotteryInfo();

        #region Method

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CurrentLotteryID)
        {
            return dal.Exists(CurrentLotteryID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.CurrentLotteryInfo model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.CurrentLotteryInfo model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int CurrentLotteryID)
        {
            return dal.Delete(CurrentLotteryID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string CurrentLotteryIDlist)
        {
            return dal.DeleteList(CurrentLotteryIDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.CurrentLotteryInfo GetModel(int CurrentLotteryID)
        {
            return dal.GetModel(CurrentLotteryID);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.CurrentLotteryInfo> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.CurrentLotteryInfo> DataTableToList(DataTable dt)
        {
            List<Model.CurrentLotteryInfo> modelList = new List<Model.CurrentLotteryInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Model.CurrentLotteryInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Model.CurrentLotteryInfo();
                    if (dt.Rows[n]["CurrentLotteryID"].ToString() != "")
                    {
                        model.CurrentLotteryID = int.Parse(dt.Rows[n]["CurrentLotteryID"].ToString());
                    }
                    if (dt.Rows[n]["CurrentLotteryTime"].ToString() != "")
                    {
                        model.CurrentLotteryTime = DateTime.Parse(dt.Rows[n]["CurrentLotteryTime"].ToString());
                    }
                    model.LotteryType = dt.Rows[n]["LotteryType"].ToString();
                    model.CurrentLotteryNum = dt.Rows[n]["CurrentLotteryNum"].ToString();
                    if (dt.Rows[n]["LotteryID"].ToString() != "")
                    {
                        model.LotteryID = int.Parse(dt.Rows[n]["LotteryID"].ToString());
                    }
                    model.IssueNo = dt.Rows[n]["IssueNo"].ToString();
                    if (dt.Rows[n]["AddTime"].ToString() != "")
                    {
                        model.AddTime = DateTime.Parse(dt.Rows[n]["AddTime"].ToString());
                    }
                    if (dt.Rows[n]["UpdateTime"].ToString() != "")
                    {
                        model.UpdateTime = DateTime.Parse(dt.Rows[n]["UpdateTime"].ToString());
                    }
                    if (dt.Rows[n]["IsLottery"].ToString() != "")
                    {
                        if ((dt.Rows[n]["IsLottery"].ToString() == "1") || (dt.Rows[n]["IsLottery"].ToString().ToLower() == "true"))
                        {
                            model.IsLottery = true;
                        }
                        else
                        {
                            model.IsLottery = false;
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        #endregion Method

        /// <summary>
        /// 获取彩票信息
        /// </summary>
        /// <param name="lotteryID"></param>
        /// <returns></returns>
        public Model.CurrentLotteryInfo GetLotteryInfos(int lotteryID)
        {
            var result = dal.GetLotteryInfos(lotteryID);
            return result;
        }
        
        /// <summary>
        /// 查詢多筆彩種上期、本期開獎資訊
        /// </summary>
        /// <param name="lotteryIds"></param>
        /// <returns></returns>
        public IEnumerable<Model.CurrentLotteryInfo> GetNextIssueNos(string lotteryIds)
        {
            return dal.GetNextIssueNos(lotteryIds);
        }

        /// <summary>
        /// 获取即将开奖的一期的信息
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public Model.CurrentLotteryInfo GetCurrenIssuNo(int lotteryId)
        {
            return dal.GetCurrenIssuNo(lotteryId);
        }

        /// <summary>
        /// 倒数第二期开奖信息
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public Model.CurrentLotteryInfo GetLastLotteryNum(int lotteryId, string cunum)
        {
            return dal.GetLastLotteryNum(lotteryId, cunum);
        }

        /// <summary>
        /// 获取前3期开奖情况
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public List<Model.CurrentLotteryInfo> GetCurrentLotteryInfo(int lotteryId)
        {
            return dal.GetCurrentLotteryInfo(lotteryId);
        }

        public List<Model.CurrentLotteryInfo> GetCurrentLotteryInfo(int lotteryID, int count)
        {
            var result = dal.GetCurrentLotteryInfo(lotteryID, count);
            return result;
        }

        public List<Model.CurrentLotteryInfo> GetCurrentLotteryInfo1(int lotteryID, int count)
        {
            var result = dal.GetCurrentLotteryInfo1(lotteryID, count);
            return result;
        }

        public List<Model.CurrentLotteryInfo> GetCurrentLotteryInfo_Sec(int lotteryID, int count, int UserID)
        {
            var result = dal.GetCurrentLotteryInfo_Sec(lotteryID, count, UserID);
            return result;
        }

        public List<Model.CurrentLotteryInfo> GetCurrentLotteryInfo_Sec1(int lotteryID, int count, int UserID)
        {
            var result = dal.GetCurrentLotteryInfo_Sec1(lotteryID, count, UserID);
            return result;
        }

        public List<Model.CurrentLotteryInfo> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query)
        {
            var result = dal.QueryCurrentLotteryInfo(query);
            return result;
        }

        public List<Model.CurrentLotteryInfo> QuerySecCurrentLotteryInfo(CurrentLotteryQueryInfo query)
        {
            var result = dal.QuerySecCurrentLotteryInfo(query);
            return result;
        }

        /// <summary>
        /// 最后一条
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public Model.CurrentLotteryInfo GetEndLotteryNum(int lotteryId)
        {
            return dal.GetEndLotteryNum(lotteryId);
        }

        /// <summary>
        /// 根据期号查询
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public Model.CurrentLotteryInfo GetIssueNoLotteryNum(int lotteryId, string IssueNo)
        {
            return dal.GetIssueNoLotteryNum(lotteryId, IssueNo);
        }

        /// <summary>
        /// 获取彩票开奖历史
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <returns></returns>
        public List<Model.CurrentLotteryInfo> GetLoHistory(int lotteryId, string IssueNo, DateTime start, DateTime end)
        {
            return dal.GetLoHistory(lotteryId, IssueNo, start, end);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lotteryId"></param>
        /// <param name="issueNo"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public QueryResult<Model.CurrentLotteryInfo> QueryLotteryHistoryResult(int? lotteryId, string issueNo, DateTime? startDate, DateTime? endDate, int pageindex, int pagesize, string sort)
        {
            return dal.QueryLotteryHistoryResult(lotteryId, issueNo, startDate, endDate, pageindex, pagesize, sort);
        }

        public CursorPagination<Model.CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor)
        {
            return dal.GetCursorPaginationDrawResult(lotteryId, start, end, count, cursor);
        }
    }
}