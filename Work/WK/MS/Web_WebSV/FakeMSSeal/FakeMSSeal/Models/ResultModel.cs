namespace FakeMSSeal.Models
{
    public class ResultModel<T> where T : class
    {
        /// <summary>
        /// 成功與否
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 資料內容
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        [Obsolete("改用Error")]
        public string Msg { get; set; }


        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Error { get; set; }
    }
}