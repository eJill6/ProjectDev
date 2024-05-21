using JxBackendService.Interface.Model.User;

namespace SLPolyGame.Web.Model
{
    /// <summary>
    /// UserInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    //[Serializable]
    public class UserInfo
    {
        public UserInfo()
        {
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public decimal RebatePro { get; set; }

        public decimal AddedRebatePro { get; set; } = 0.0M;

        public decimal Available { get; set; }
    }

    public class UserAuthInformation : UserInfo, ITicketUserData, ILoginRequestParam
    {
        public string Key { get; set; }

        public string RoomNo { get; set; }

        public string GameID { get; set; }

        public string DepositUrl { get; set; }

        public int LogonMode { get; set; }

        public long ExpiredTimestamp { get; set; }
    }

    public class LoginRequestParam : ILoginRequestParam
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string RoomNo { get; set; }

        public string GameID { get; set; }

        public string DepositUrl { get; set; }

        public int LogonMode { get; set; }

        public int UserKeyExpiredMinutes { get; set; }

        public bool IsSlidingExpiration { get; set; }
    }
}