namespace MS.Core.MM.Models.Auth.ServiceReq
{
    public class ReqBossIdentityApplyData : BossIdentityApplyData
    {
        /// <summary>
        /// 用戶id
        /// </summary>
        public int UserId { get; set; }

        public int ApplyIdentity { get; set; }
        /// <summary>
        /// 是否从后台事情boss
        /// </summary>
        public bool IsAdminApply { get; set; } = false;
        /// <summary>
        /// 申请ID
        /// </summary>
        public string ApplyId { get; set; }
    }
}