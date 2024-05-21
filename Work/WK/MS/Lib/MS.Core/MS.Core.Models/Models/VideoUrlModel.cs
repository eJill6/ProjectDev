namespace MS.Core.Models.Models
{
    /// <summary>
    /// 01影像Url的參數
    /// </summary>
    public class VideoUrlModel
    {
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// TimeSpan
        /// </summary>
        public long Ts { get; set; }

        /// <summary>
        /// Sign
        /// </summary>
        public string Sign { get; set; }
    }
}
