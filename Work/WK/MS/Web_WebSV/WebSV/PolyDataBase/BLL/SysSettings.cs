using System;
using System.Data;
using System.Collections.Generic;
using SLPolyGame.Web.Model;
namespace SLPolyGame.Web.BLL
{
    /// <summary>
    /// SysSettings
    /// </summary>
    public class SysSettings
    {
        private readonly SLPolyGame.Web.DAL.SysSettings dal = new SLPolyGame.Web.DAL.SysSettings();
        public SysSettings()
        { }
        #region  Method

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
        public bool Exists(int SettingsID)
        {
            return dal.Exists(SettingsID);
        }

      

    

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int SettingsID)
        {

            return dal.Delete(SettingsID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string SettingsIDlist)
        {
            return dal.DeleteList(SettingsIDlist);
        }



  



        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  Method
        /// <summary>
        /// 获取系统配置信息
        /// </summary>
        public SLPolyGame.Web.Model.SysSettings GetSysSettings()
        {
            return dal.GetSysSettings();
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool SysUpdate(SLPolyGame.Web.Model.SysSettings model)
        {
            return dal.SysUpdate(model);
        }

        public string GetCustomerServiceUrl()
        {
            return dal.GetCustomerServiceUrl();
        }
	}
}

