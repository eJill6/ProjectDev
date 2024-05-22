using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPTDataBase.Model
{
    public class ResultModel
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 请求返回代码数值
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 请求返回代码描述
        /// </summary>
        public string Msg { get; set; }
    }
}
