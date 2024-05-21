namespace MS.Core.MMModel.Models
{
    public class NotifyVideoProcess
    {
        /// <summary>
        /// 原始檔案路徑
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 會回通知是哪一筆資料在處理
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 1:横屏、2:竖屏
        /// </summary>
        public int Orientation { get; set; } = 2;
    }
}
