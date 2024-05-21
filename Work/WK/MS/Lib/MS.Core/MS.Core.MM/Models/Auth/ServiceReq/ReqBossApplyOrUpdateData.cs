using MS.Core.MMModel.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Auth.ServiceReq
{
    public class ReqBossApplyOrUpdateData: OfficialShopDetailForclient
    {
        /// <summary>
        /// 用戶id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 是否从后台事情boss
        /// </summary>
        public bool IsAdminApply { get; set; } = false;
        /// <summary>
        /// 店铺ID
        /// </summary>
        public string ApplyId { get; set; }
    }
}
