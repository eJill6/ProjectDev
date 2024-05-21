namespace MS.Core.Infrastructures.ZeroOne.Models.Responses
{
    public class ZOUserInfoRes
    {
        public int UserId { get; set; }
        public string NickName { get; set; }
        public decimal Point { get; set; }
        public decimal Amount { get; set; }
        public string Avatar { get; set; }
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否有手機認證過
        /// </summary>
        public bool HasPhone { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
    }
}