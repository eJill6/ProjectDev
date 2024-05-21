using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.SpaPlayConfig
{
    /// <summary>
    /// 單排按鈕區塊
    /// </summary>
    public class Field
    {
        /// <summary>
        /// 標籤
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// 按鈕
        /// </summary>
        public IList<object> Numbers { get; set; } = new List<object>();
    }
}