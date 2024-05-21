using MS.Core.MMModel.Models.Post.Enums;

namespace MS.Core.MMModel.Models
{
    /// <summary>
    /// 上傳資料到雲倉儲的參數
    /// </summary>
    public class SaveMediaToOssParamForClient
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
        /// 媒體檔來源
        /// </summary>
        public SourceType SourceType { get; set; }

        /// <summary>
        /// 媒體檔類型
        /// </summary>
        public int MediaType { get; set; }
    }
}