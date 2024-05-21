using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class MyOfficialPostQueryParam: MyOfficialPostQueryParamForClient
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }
    }
}
