namespace FakeMSSeal.Models
{
    public class BalanceDetail
    {
        /// <summary>
        /// 餘額資訊
        /// </summary>
        public string Balance { get; set; }
    }

    public class BalanceResult : ResultModel<BalanceDetail>
    {
    }
}
