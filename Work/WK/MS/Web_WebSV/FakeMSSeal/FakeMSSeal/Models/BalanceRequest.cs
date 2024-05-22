namespace FakeMSSeal.Models
{
    /// <summary>
    /// 餘額請求
    /// </summary>
    public class BalanceRequest : BaseRequest
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserId { get; set; }
    }
}
