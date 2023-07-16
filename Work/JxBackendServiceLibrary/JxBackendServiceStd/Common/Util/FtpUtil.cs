using FluentFTP;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Ftp;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JxBackendService.Common.Util
{
    public static class FtpUtil
    {
        public static bool UploadFile(FtpLoginParam ftpLoginParam, string localFilePath, string remoteFilePath)
        {
            string targetFilePath = GetRemoteFilePath(remoteFilePath);
            var ftpStatus = FtpStatus.Failed;

            using (FtpClient ftpClient = GetFtpClient(ftpLoginParam))
            {
                ftpClient.Connect();
                // 切換檔案目錄
                FilePathInfo filePathInfo = GetFilePathInfo(remoteFilePath);

                if (filePathInfo == null)
                {
                    return false;
                }

                SetUploadFileDirectory(ftpClient, filePathInfo.FileDirectories);
                ftpStatus = ftpClient.UploadFile(localFilePath, filePathInfo.FileName);
                ftpClient.Disconnect();
            }

            return ftpStatus == FtpStatus.Success;
        }

        public static bool DownloadFile(FtpLoginParam ftpLoginParam, string localFilePath, string remoteFilePath)
        {
            var ftpStatus = FtpStatus.Failed;

            using (FtpClient ftpClient = GetFtpClient(ftpLoginParam))
            {
                ftpClient.Connect();
                ftpStatus = ftpClient.DownloadFile(localFilePath, remoteFilePath);
                ftpClient.Disconnect();
            }

            return ftpStatus == FtpStatus.Success;
        }

        public static List<FtpFileInfo> GetFileList(FtpLoginParam ftpLoginParam, string remoteFilePath)
        {
            string targetFilePath = GetRemoteFilePath(remoteFilePath);
            FtpListItem[] ftpListItems = null;

            using (FtpClient ftpClient = GetFtpClient(ftpLoginParam))
            {
                ftpClient.Connect();
                ftpListItems = ftpClient.GetListing(targetFilePath);
                ftpClient.Disconnect();
            }

            List<FtpFileInfo> ftpFileInfos = ftpListItems.Select(s => new FtpFileInfo
            {
                FileName = s.Name,
                FileNameWithoutExtension = TrimFileExtension(s.Name),
                FileType = s.Type,
                FullPath = s.FullName,
                ModifiedDate = s.Modified,
                CreateDate = s.Created,
            }).ToList();

            return ftpFileInfos;
        }

        public static bool DeleteFtpFile(FtpLoginParam ftpLoginParam, string remoteFilePath)
        {
            string targetFilePath = GetRemoteFilePath(remoteFilePath);

            using (var ftpClient = GetFtpClient(ftpLoginParam))
            {
                bool isExist = ftpClient.FileExists(targetFilePath);

                if (isExist == false)
                {
                    var logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
                    logUtilService.Warn("FtpUtil DeleteFtpFile Fail. File Not Exist.");

                    return false;
                }

                ftpClient.DeleteFile(targetFilePath);
                return true;
            }
        }

        /// <summary>
        /// 移除副檔名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string TrimFileExtension(string fileName)
        {
            int index = fileName.IndexOf(".");

            if (index < 0)
            {
                return fileName;
            }

            return fileName.Substring(0, index);
        }

        private static string GetRemoteFilePath(string remoteFilePath)
        {
            return remoteFilePath.ToTrimString().TrimStart("/").TrimEnd("/");
        }

        /// <summary>
        /// 切換/建立檔案目錄
        /// </summary>
        /// <param name="ftpClient"></param>
        /// <param name="fileDirectories"></param>
        private static void SetUploadFileDirectory(FtpClient ftpClient, List<string> fileDirectories)
        {
            foreach (string fileDir in fileDirectories)
            {
                ftpClient.CreateDirectory(fileDir);
                ftpClient.SetWorkingDirectory(fileDir);
            }
        }

        /// <summary>
        /// 取得檔案路徑相關目錄檔名資訊
        /// </summary>
        /// <param name="remoteFilePath"></param>
        /// <returns></returns>
        private static FilePathInfo GetFilePathInfo(string remoteFilePath)
        {
            if (remoteFilePath.IsNullOrEmpty())
            {
                return null;
            }

            string[] splitContents = remoteFilePath.Split('/').Where(w => !w.IsNullOrEmpty()).ToArray();
            int totalCount = splitContents.Length;

            var filePathInfo = new FilePathInfo
            {
                FileDirectories = splitContents.Take(totalCount - 1).ToList(),
                FileName = splitContents.Last()
            };

            return filePathInfo;
        }

        /// <summary>
        /// 設定登入FTP相關資訊
        /// </summary>
        /// <param name="ftpLoginParam"></param>
        /// <returns></returns>
        private static FtpClient GetFtpClient(FtpLoginParam ftpLoginParam)
        {
            FtpClient ftpClient = new FtpClient
            {
                Host = ftpLoginParam.FtpAddress,
                Port = ftpLoginParam.FtpPort,
            };

            ftpClient.Config.ConnectTimeout = 30000;
            ftpClient.Config.ReadTimeout = 45000;
            ftpClient.Config.DataConnectionType = FtpDataConnectionType.PASV;
            ftpClient.Config.ListingDataType = FtpDataType.Binary;
            ftpClient.Config.DownloadDataType = FtpDataType.Binary;
            ftpClient.Config.UploadDataType = FtpDataType.Binary;

            if (!ftpLoginParam.FtpUser.IsNullOrEmpty() &&
                !ftpLoginParam.FtpPassword.IsNullOrEmpty())
            {
                ftpClient.Credentials = new NetworkCredential(ftpLoginParam.FtpUser, ftpLoginParam.FtpPassword);
            }

            return ftpClient;
        }
    }
}