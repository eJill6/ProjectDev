using ProductTransferService.AgDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Ftp;

namespace ProductTransferService.AgDataBase.Common
{
    public static class FtpUtility
    {
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="ftpAddress"></param>
        /// <param name="ftpPort"></param>
        /// <param name="ftpRemotePath">指定FTP服务的路径. 如:"ftpdemo\downloddemo\"</param>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPassword"></param>
        /// <returns></returns>
        public static List<XMLFile> GetFiles(string ftpAddress, int ftpPort, string ftpRemotePath, string ftpUser, string ftpPassword)
        {
            var xmlFiles = new List<XMLFile>();

            List<FtpFileInfo> ftpFileInfos = FtpUtil.GetFileList(
                new FtpLoginParam()
                {
                    FtpAddress = ftpAddress,
                    FtpPort = ftpPort,
                    FtpUser = ftpUser,
                    FtpPassword = ftpPassword,
                },
                ftpRemotePath);

            foreach (FtpFileInfo file in ftpFileInfos)
            {
                var xmlFile = new XMLFile
                {
                    Name = file.FileName,
                    RemotePath = file.FullPath,
                    LastModified = file.ModifiedDate
                };

                xmlFiles.Add(xmlFile);
            }

            xmlFiles.Sort();

            return xmlFiles;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="ftpAddress"></param>
        /// <param name="ftpPort"></param>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="localPath">localPath要指定的详细的路径, 包括存放的文件名. 如:"D:\Program Files\demo.txt"</param>
        /// <param name="remotePath">remoteFile指定FTP服务的路径和文件名. 如:"ftpdemo\downloddemo\demo.txt"</param>
        public static void DownloadFile(string ftpAddress, int ftpPort, string ftpUser, string ftpPassword, string localPath, string remotePath)
        {
            FtpUtil.DownloadFile(
                new FtpLoginParam()
                {
                    FtpAddress = ftpAddress,
                    FtpPort = ftpPort,
                    FtpUser = ftpUser,
                    FtpPassword = ftpPassword,
                },
                localPath,
                remotePath);
        }
    }
}