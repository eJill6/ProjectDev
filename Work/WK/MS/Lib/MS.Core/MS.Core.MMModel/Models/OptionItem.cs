using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models
{
    public class OptionItem
    {        /// <summary>
             /// 項目編號
             /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// 項目值
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}