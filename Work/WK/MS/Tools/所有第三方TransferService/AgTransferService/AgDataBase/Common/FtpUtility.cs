using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnterpriseDT.Net.Ftp;
using AgDataBase.Model;

namespace AgDataBase.Common
{
    public static class FtpUtility
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileFullName"></param>
        /// <param name="ftpAddress"></param>
        /// <param name="ftpPort"></param>
        /// <param name="ftpRemotePath">指定FTP服务的路径. 如:"ftpdemo\downloddemo\"</param>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPassword"></param>
        public static void UploadFile(string fileName, string fileFullName, string ftpAddress, int ftpPort, string ftpRemotePath, string ftpUser, string ftpPassword)
        {
            using (var conn = new FTPConnection())
            {
                conn.ServerAddress = ftpAddress;
                conn.ServerPort = ftpPort;
                conn.ServerDirectory = ftpRemotePath;
                conn.UserName = ftpUser;
                conn.Password = ftpPassword;
                conn.CommandEncoding = Encoding.GetEncoding("GBK");
                conn.TransferType = FTPTransferType.BINARY;
                conn.ConnectMode = FTPConnectMode.PASV;

                conn.Connect();
                conn.UploadFile(fileFullName, fileName);
            }
        }

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
            List<XMLFile> xmlFiles = new List<XMLFile>();

            using (var conn = new FTPConnection())
            {
                conn.ServerAddress = ftpAddress;
                conn.ServerPort = ftpPort;
                conn.ServerDirectory = ftpRemotePath;
                conn.UserName = ftpUser;
                conn.Password = ftpPassword;
                conn.CommandEncoding = Encoding.GetEncoding("GBK");
                conn.TransferType = FTPTransferType.BINARY;
                conn.ConnectMode = FTPConnectMode.PASV;

                conn.Connect();

                if (conn.DirectoryExists(ftpRemotePath))
                {
                    //获取指定目录的所有子目录和文件信息  
                    conn.ChangeWorkingDirectory(ftpRemotePath);
                    var files = conn.GetFileInfos();

                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            XMLFile xmlFile = new XMLFile();
                            xmlFile.Name = file.Name;
                            xmlFile.RemotePath = file.Path;
                            xmlFile.LastModified = file.LastModified;

                            xmlFiles.Add(xmlFile);
                        }
                    }
                }
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
            using (var conn = new FTPConnection())
            {
                conn.ServerAddress = ftpAddress;
                conn.ServerPort = ftpPort;
                conn.UserName = ftpUser;
                conn.Password = ftpPassword;
                conn.CommandEncoding = Encoding.GetEncoding("GBK");
                conn.ConnectMode = FTPConnectMode.PASV;
                conn.TransferType = FTPTransferType.BINARY;

                conn.Connect();
                conn.DownloadFile(localPath, remotePath);
            }
        }
    }
}
