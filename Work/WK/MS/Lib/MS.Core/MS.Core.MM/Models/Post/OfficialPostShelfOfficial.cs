using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Post
{
    public class OfficialPostShelfOfficial
    {
        public string[] PostIds { get; set; } = new string[0];
        public int IsDelete { get; set; }

        /// <summary>
        /// 会员ID(Admin删除帖子需要用到)
        /// </summary>
        public int UserId { get; set; }
    }
}
