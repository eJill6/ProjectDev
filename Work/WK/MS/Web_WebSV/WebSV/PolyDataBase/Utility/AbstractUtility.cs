using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SLPolyGame.Web.Utility
{
    public abstract class AbstractUtility<T> 
        where T : new()
    {
        #region Property
        private static object objLock = new object();
        protected List<T> data = new List<T>();
        private DateTime NextLoadDataTime = DateTime.Now;
        private int TimeIntervalsSecond = 60;
        private bool NeedReload;

        protected const string SelectItemName = "请选择";
        protected const int SelectItemID = -1;

        public List<T> Data
        {
            get
            {
                if (NeedReload)
                {
                    if (DateTime.Now > NextLoadDataTime)
                    {
                        LoadData();
                    }
                }
                return data;
            }
        }
        #endregion

        #region Constructive
        public AbstractUtility(bool needreload = true)
        {
            NeedReload = needreload;
            LoadData();
        }
        #endregion

        #region Function
        /// <summary>
        /// 如果是會固定時間重取資料的Utility(NeedReload = true)
        /// 因為讀檔可能是非同步作業需要在完成LoadData時執行ResetTime()
        /// </summary>
        protected abstract void LoadData();
        public abstract string GetName(int id);
        public abstract int GetID(string name);


        protected void SetTimeIntervalsSecond(int second)
        {
            TimeIntervalsSecond = second;
        }

        protected void ResetTime()
        {
            NextLoadDataTime = DateTime.Now.AddSeconds(TimeIntervalsSecond);
        }
        #endregion

        


    }
}