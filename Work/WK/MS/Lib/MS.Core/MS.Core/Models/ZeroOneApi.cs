namespace MS.Core.Models
{
    public class ZeroOneApi
    {
        private ZeroOneApi(string apiUrl)
        {
            ApiUrl = apiUrl;
        }

        private static ZeroOneApi Factory(string apiUrl)
        {
            return new ZeroOneApi(apiUrl);
        }

        private string ApiUrl { get; set; }

        public string GetApiUrl()
        {
            return ApiUrl;
        }
        /// <summary>
        /// 權限
        /// </summary>
        public static ZeroOneApi Permission => Factory("/dapi/permission");
        /// <summary>
        /// 會員資訊
        /// </summary>
        public static ZeroOneApi UserInfo => Factory("/dapi/user");
        /// <summary>
        /// 鑽石
        /// </summary>
        public static ZeroOneApi Point => Factory("/dapi/point");
        /// <summary>
        /// 餘額
        /// </summary>
        public static ZeroOneApi Cash => Factory("/dapi/amount");

        /// <summary>
        /// 影片上傳
        /// </summary>
        public static ZeroOneApi MediaUpload => Factory("/dapi/media/upload?path=video");

        /// <summary>
        /// 影片合併
        /// </summary>
        public static ZeroOneApi MediaMerge => Factory("/dapi/media/file/merge");
    }
}
