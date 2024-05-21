using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class MyFavoritePostQueryParam: PageParam
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 帖子类型
        /// </summary>
        public int PostType { get; set; }
    }
}
