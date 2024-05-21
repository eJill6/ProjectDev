using System;
using System.Data;
using System.Collections.Generic;

using SLPolyGame.Web.Model;
namespace SLPolyGame.Web.BLL
{
	/// <summary>
	/// PlayTypeRadio
	/// </summary>
	public class PlayTypeRadio
	{
		private readonly SLPolyGame.Web.DAL.PlayTypeRadio dal=new SLPolyGame.Web.DAL.PlayTypeRadio();

        /// <summary>
        /// 获取各彩种购买单选方式
        /// </summary>
        /// <returns></returns>
        public List<SLPolyGame.Web.Model.PlayTypeRadio> GetPlayTypeRadio()
        {
            return dal.GetPlayTypeRadio();
        }
	}
}

