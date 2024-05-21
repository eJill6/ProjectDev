using MS.Core.MM.Models.Media;

namespace MS.Core.MM.Model.Media
{
    /// <summary>
    /// 上傳資料到雲倉儲的參數
    /// </summary>
    public class SaveMediaToOssParam : MediaInfo
    {
        /// <summary>
        /// 檔案內容
        /// </summary>
        public byte[] Bytes { get; set; } = new byte[0];

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 是否需要加密
        /// </summary>
        public bool IsNeedEncrypt { get; set; } = true;
    }
}