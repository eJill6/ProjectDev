namespace MS.Core.MM.Models.Auth.ServiceReq
{
    public class ReqUserId
    {
        /// <summary>
        /// 用戶id
        /// </summary>
        public int UserId { get; set; }

        public string ContactApp { get; set; }
        public string ContactInfo { get; set; }
    }
}