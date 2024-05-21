using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class EditDoBusinessTimeParamter
    {
        /// <summary>
        /// BossId
        /// </summary>
        public string BossId { get; set; }
        /// <summary>
        /// 修改的内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 修改类型
        /// </summary>
        public int EditType { get; set; }
    }
}
