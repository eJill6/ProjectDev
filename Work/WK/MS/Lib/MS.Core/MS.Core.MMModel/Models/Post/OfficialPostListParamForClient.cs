using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.Post
{
    public class OfficialPostListParamForClient : PageParam
    {
        /// <summary>
        /// 店铺id
        /// </summary>
        public string ApplyId { get; set; } = string.Empty;

        /// <summary>
        /// 区域代码
        /// </summary>
        public string[] AreaCode { get; set; }

        /// <summary>
        /// 發贴人 Id
        /// </summary>
        public int? UserId { get; set; }
    }
}
