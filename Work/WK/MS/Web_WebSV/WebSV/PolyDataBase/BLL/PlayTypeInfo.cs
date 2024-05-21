using System;
using System.Data;
using System.Collections.Generic;

using SLPolyGame.Web.Model;
namespace SLPolyGame.Web.BLL
{
	/// <summary>
	/// PlayTypeInfo
	/// </summary>
	public class PlayTypeInfo
	{
		private readonly SLPolyGame.Web.DAL.PlayTypeInfo dal=new SLPolyGame.Web.DAL.PlayTypeInfo();

        /// <summary>
        /// 获取各彩种选号方式列表
        /// </summary>
        /// <returns></returns>
        public List<SLPolyGame.Web.Model.PlayTypeInfo> GetPlayTypeInfo()
        {
            return dal.GetPlayTypeInfo();
        }
        /// <summary>
        /// 根据彩种ID获取各彩种选号方式列表
        /// </summary>
        /// <returns></returns>
        public List<SLPolyGame.Web.Model.PlayTypeInfo> GetPlayType(int LotteryId)
        {
            return dal.GetPlayType(LotteryId);
        }
	}
}

