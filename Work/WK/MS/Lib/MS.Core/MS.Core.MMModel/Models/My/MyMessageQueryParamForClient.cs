using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class MyMessageQueryParamForClient: PageParam
    {
        public MessageType MessageInfoType { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
