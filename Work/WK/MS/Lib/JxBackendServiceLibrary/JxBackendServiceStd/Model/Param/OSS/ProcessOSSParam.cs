using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Param.OSS
{
    public class UpdateImageOSSParam
    {
        public string KeyID { get; set; }

        public string ImageFileName { get; set; }

        public string ImageUrl { get; set; }

        public byte[] ImageBytes { get; set; }

        public JxApplication Application { get; set; }

        /// <summary> 不同功能放置不同OSS遠端資料夾路徑 </summary>
        public string UploadOSSRemotePath { get; set; }
    }
}