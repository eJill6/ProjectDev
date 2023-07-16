using System;
using System.Collections.Generic;
using FluentFTP;

namespace JxBackendService.Model.Ftp
{
    public class FtpLoginParam
    {
        public string FtpAddress { get; set; }

        public int FtpPort { get; set; }

        public string FtpUser { get; set; }

        public string FtpPassword { get; set; }
    }

    public class FilePathInfo
    {
        public List<string> FileDirectories { get; set; }

        public string FileName { get; set; }
    }

    public class FtpFileInfo
    {
        /// <summary>
        /// 檔案/資料夾 名稱
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 無副檔名的檔案名稱
        /// </summary>
        public string FileNameWithoutExtension { get; set; }

        /// <summary>
        /// 檔案類型
        /// </summary>
        public FtpObjectType FileType { get; set; }

        /// <summary>
        /// 檔案/資料夾 路徑
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// 異動時間
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}