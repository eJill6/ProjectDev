namespace SLPolyGame.Web.Models.MMModels
{
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