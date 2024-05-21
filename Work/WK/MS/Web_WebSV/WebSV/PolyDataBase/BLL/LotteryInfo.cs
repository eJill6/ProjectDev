using System;
using System.Data;
using System.Collections.Generic;

using SLPolyGame.Web.Model;

namespace SLPolyGame.Web.BLL
{
    /// <summary>
    /// LotteryInfo
    /// </summary>
    public class LotteryInfo
    {
        private readonly SLPolyGame.Web.DAL.LotteryInfo dal = new SLPolyGame.Web.DAL.LotteryInfo();

        public LotteryInfo()
        { }

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
        public bool Exists(int LotteryID)
        {
            return dal.Exists(LotteryID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(SLPolyGame.Web.Model.LotteryInfo model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(SLPolyGame.Web.Model.LotteryInfo model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int LotteryID)
        {
            return dal.Delete(LotteryID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string LotteryIDlist)
        {
            return dal.DeleteList(LotteryIDlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SLPolyGame.Web.Model.LotteryInfo GetModel(int LotteryID)
        {
            return dal.GetModel(LotteryID);
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
        public List<SLPolyGame.Web.Model.LotteryInfo> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<SLPolyGame.Web.Model.LotteryInfo> DataTableToList(DataTable dt)
        {
            List<SLPolyGame.Web.Model.LotteryInfo> modelList = new List<SLPolyGame.Web.Model.LotteryInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                SLPolyGame.Web.Model.LotteryInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new SLPolyGame.Web.Model.LotteryInfo();
                    if (dt.Rows[n]["LotteryID"].ToString() != "")
                    {
                        model.LotteryID = int.Parse(dt.Rows[n]["LotteryID"].ToString());
                    }
                    model.LotteryType = dt.Rows[n]["LotteryType"].ToString();
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

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion Method

        #region 后台管理

        /// <summary>
        /// 获取彩种列表
        /// </summary>
        public List<SLPolyGame.Web.Model.LotteryInfo> GetLotteryType()
        {
            return dal.GetLotteryType();
        }

        /// <summary>
        /// 获取彩种列表
        /// </summary>
        public List<SLPolyGame.Web.Model.LotteryInfo> Get_WebLotteryType(int GameTypeID)
        {
            return dal.Get_WebLotteryType(GameTypeID);
        }

        /// <summary>
        /// 獲取玩法
        /// </summary>
        /// <param name="playModel">0. 所有玩法 1, 經典玩法, 2. 專家玩法</param>
        /// <returns></returns>
        public List<SLPolyGame.Web.Model.LotteryInfo> Get_AllWebLotteryType(int playModel = 0)
        {
            return dal.Pro_SearchLotteryInfo(playModel);
        }

        #endregion 后台管理
    }
}