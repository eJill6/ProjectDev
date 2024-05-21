using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.SpaPlayConfig
{
    /// <summary>
    /// 玩法大類
    /// </summary>
    public class PlayMode<T>
    {
        /// <summary>
        /// 玩法大類Id
        /// </summary>
        public int PlayModeId { get; set; }

        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string PlayModeName { get; set; }

        /// <summary>
        /// 所有的玩法
        /// </summary>
        public IList<T> PlayTypeInfos { get; set; }
    }
}