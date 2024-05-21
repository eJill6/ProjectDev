using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.AdminUserManager
{
    public class UserBossApplyInfoModel
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public int UserId { get; set; }

        public decimal EarnestMoney { get; set; }

        /// <summary>
        /// 增加发贴次数
        /// </summary>
        public int? ExtraPostCount { get; set; }
        /// <summary>
        /// 平台分成百分比
        /// </summary>
        public int? PlatformSharing { get; set; }

        /// <summary>
        /// 用户身份
        /// </summary>
        public int ApplyIdentity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 联系软件
        /// </summary>
        public string ContactApp { get; set; }

        /// <summary>
        /// 软件号码
        /// </summary>
        public string Contact { get; set; }
    }
}
