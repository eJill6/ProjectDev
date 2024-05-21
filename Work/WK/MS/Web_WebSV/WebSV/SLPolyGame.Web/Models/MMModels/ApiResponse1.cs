namespace SLPolyGame.Web.Models.MMModels
{
    public class ApiResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 錯誤碼
        /// </summary>
        public string Code { get; set; } = string.Empty;
    }
}