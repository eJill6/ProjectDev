using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Post.ServiceReq
{
    public class ReqMyOfficialStorePost : PageParam
    {
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
