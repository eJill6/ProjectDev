namespace MS.Core.MMModel.Models
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

    /// <inheritdoc/>
    /// <typeparam name="TData">資料</typeparam>
    public class ApiResponse<TData> : ApiResponse
    {
        /// <summary>
        /// 資料
        /// </summary>
        public TData Data { get; set; }
    }
}
