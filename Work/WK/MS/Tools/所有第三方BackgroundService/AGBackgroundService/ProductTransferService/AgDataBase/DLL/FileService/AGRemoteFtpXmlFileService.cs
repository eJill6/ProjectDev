using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using ProductTransferService.AgDataBase.Common;
using ProductTransferService.AgDataBase.DLL;
using ProductTransferService.AgDataBase.DLL.FileService;
using ProductTransferService.AgDataBase.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AgDataBase.DLL.FileService
{
    public class AGRemoteFtpXmlFileService : BaseAGRemoteXmlFileService, IAGRemoteXmlFileService
    {
        private static readonly int _maxTryCount = 5;

        private static readonly int _retryIntervalSeconds = 2;

        private readonly Lazy<ITransferServiceAppSettingService> _transferServiceAppSettingService;

        private static readonly object s_cacheWriteLocker = new object();

        private static readonly string s_lockerKeyFormat = "@#$AGDownloadFileLocker_{0}";

        public AGRemoteFtpXmlFileService()
        {
            _transferServiceAppSettingService = DependencyUtil.ResolveService<ITransferServiceAppSettingService>();
        }

        public void DownloadAllRemoteNormalXmlFiles(AGGameType agGameType)
        {
            var platformProductAGSettingService = DependencyUtil.ResolveKeyed<IPlatformProductAGSettingService>(SharedAppSettings.PlatformMerchant).Value;
            IAgApi agApi = DependencyUtil.ResolveService<IAgApi>().Value;

            string dateTimeRemotePath = agApi.GetDateTimeRemotePath();
            string remotePath = platformProductAGSettingService.GetRemotePath(agGameType, dateTimeRemotePath); //不同商戶在遠端的路徑不同

            LocalFolderInfo localFolderInfo = GetLocalFolderInfo(agGameType);

            if (CurrentCrawlDate != dateTimeRemotePath)
            {
                XmlFileMap.Clear();
                UpdateCurrentCrawlDate(dateTimeRemotePath);
            }

            List<XMLFile> allXmlFiles = null;

            try
            {
                allXmlFiles = RetryJobUtil.GetJobResultWithRetry(
                    () =>
                    {
                        return FtpUtility.GetFiles(
                            AGProfitLossInfo.FtpAddress,
                            AGProfitLossInfo.FtpPort,
                            remotePath,
                            AGProfitLossInfo.FtpUser,
                            AGProfitLossInfo.FtpPassword);
                    },
                    _maxTryCount,
                    _retryIntervalSeconds);

                if (allXmlFiles.Count > 0)
                {
                    LogUtilService.ForcedDebug("列取目录 " + remotePath + " 文件列表成功，共获取到 " + allXmlFiles.Count.ToString() + " 个文件");
                }
                else
                {
                    LogUtilService.ForcedDebug("目录 " + remotePath + " 无文件，暂停解析");
                }
            }
            catch (Exception ex)
            {
                LogUtilService.Error("列取目录 " + remotePath + " 失败，详细信息：" + ex.Message);

                return;
            }

            var downLoadXmlFiles = new List<XMLFile>();

            if (!allXmlFiles.Any())
            {
                return;
            }

            for (int i = allXmlFiles.Count - 1; i >= 0; i--)
            {
                XMLFile file = allXmlFiles[i];
                string fileName = file.Name;

                if (!fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string mapKey = GetXmlMapKey(agGameType, fileName);

                bool isAddToDownloadList = false;

                //沒處理過
                if (!XmlFileMap.ContainsKey(mapKey))
                {
                    isAddToDownloadList = true;
                }
                else
                {
                    XmlContent xmlContent = XmlFileMap[mapKey];

                    //重複處理一小時內的資料
                    if (DateTime.Now.Subtract(xmlContent.CreateDate).TotalHours <= 1d)
                    {
                        isAddToDownloadList = true;
                    }
                }

                if (isAddToDownloadList)
                {
                    downLoadXmlFiles.Add(file);
                }
            }

            if (!downLoadXmlFiles.Any())
            {
                return;
            }

            downLoadXmlFiles = downLoadXmlFiles.OrderBy(o => o.Name).ToList();
            LogUtilService.ForcedDebug($"TransferRemote {agGameType.Value} 準備下載檔案：{downLoadXmlFiles.Count}个文件");

            for (int i = 0; i < downLoadXmlFiles.Count; i++)
            {
                string processContent = $"{i + 1}/{downLoadXmlFiles.Count}";
                XMLFile xmlFile = downLoadXmlFiles[i];
                string localPath = Path.Combine(localFolderInfo.LocalFolder, xmlFile.Name);

                try
                {
                    object locker = GetLocker(xmlFile);

                    lock (locker)
                    {
                        if (File.Exists(localPath))
                        {
                            File.Delete(localPath);
                        }

                        RetryJobUtil.DoJobWithRetry(
                            () =>
                            {
                                FtpUtility.DownloadFile(
                                    AGProfitLossInfo.FtpAddress,
                                    AGProfitLossInfo.FtpPort,
                                    AGProfitLossInfo.FtpUser,
                                    AGProfitLossInfo.FtpPassword,
                                    localPath,
                                    xmlFile.RemotePath);
                            },
                            _maxTryCount,
                            _retryIntervalSeconds);
                    }
                }
                catch (Exception ex)
                {
                    LogUtilService.Error("下载文件 " + xmlFile.RemotePath + " 失败，详细信息：" + ex.Message);
                }
            }
        }

        public void DownloadAllRemoteLostAndFoundXmlFiles(AGGameType agGameType)
        {
            var allXmlFiles = new List<XMLFile>();
            var downLoadXmlFiles = new List<XMLFile>();

            LocalFolderInfo localFolderInfo = GetLocalFolderInfo(agGameType);
            var platformProductAGSettingService = DependencyUtil.ResolveKeyed<IPlatformProductAGSettingService>(SharedAppSettings.PlatformMerchant).Value;
            IAgApi agApi = DependencyUtil.ResolveService<IAgApi>().Value;

            for (var i = 0; i < 5; i++)
            {
                string dateTimeRemotePath = agApi.GetDateTimeRemotePath(-i);
                string remotePath = platformProductAGSettingService.GetRemoteLostAndFoundPath(agGameType, dateTimeRemotePath);
                //string ftpRemotePath = @"/AGIN/lostAndfound/20160204";

                try
                {
                    List<XMLFile> xmlFiles = RetryJobUtil.GetJobResultWithRetry(
                        () =>
                        {
                            return FtpUtility.GetFiles(
                                AGProfitLossInfo.FtpAddress,
                                AGProfitLossInfo.FtpPort,
                                remotePath,
                                AGProfitLossInfo.FtpUser,
                                AGProfitLossInfo.FtpPassword);
                        },
                        _maxTryCount,
                        _retryIntervalSeconds);

                    if (xmlFiles.Count > 0)
                    {
                        LogUtilService.Info("获取远程重试目录 " + remotePath + " 文件列表成功，共获取到 " + xmlFiles.Count.ToString() + " 个文件");
                        allXmlFiles.AddRange(xmlFiles);
                    }
                    else
                    {
                        LogUtilService.Info("远程重试目录 " + remotePath + " 无文件，暂停解析");
                    }
                }
                catch (Exception ex)
                {
                    LogUtilService.Error("获取远程重试目录 " + remotePath + " 失败，详细信息：" + ex.Message);
                }
            }

            for (int i = allXmlFiles.Count - 1; i >= 0; i--)
            {
                if (allXmlFiles[i].Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    downLoadXmlFiles.Add(allXmlFiles[i]);
                }
            }

            if (!downLoadXmlFiles.Any())
            {
                return;
            }

            downLoadXmlFiles = downLoadXmlFiles.OrderBy(o => o.Name).ToList();
            LogUtilService.ForcedDebug($"TransferRemote {agGameType.Value} 準備下載檔案：{downLoadXmlFiles.Count}个文件");

            foreach (XMLFile xmlFile in downLoadXmlFiles)
            {
                string localPath = Path.Combine(localFolderInfo.LocalFolder, xmlFile.Name);

                try
                {
                    object locker = GetLocker(xmlFile);

                    lock (locker)
                    {
                        if (File.Exists(localPath))
                        {
                            File.Delete(localPath);
                        }

                        RetryJobUtil.DoJobWithRetry(
                            () =>
                            {
                                FtpUtility.DownloadFile(
                                    AGProfitLossInfo.FtpAddress,
                                    AGProfitLossInfo.FtpPort,
                                    AGProfitLossInfo.FtpUser,
                                    AGProfitLossInfo.FtpPassword,
                                    localPath,
                                    xmlFile.RemotePath);
                            },
                            _maxTryCount,
                            _retryIntervalSeconds);
                    }
                }
                catch (Exception ex)
                {
                    LogUtilService.Error("下载重试文件 " + xmlFile.RemotePath + " 失败，详细信息：" + ex.Message);
                }
            }
        }

        public List<XMLFile> GetAllLocalXmlFiles(AGGameType agGameType)
        {
            LocalFolderInfo localFolderInfo = GetLocalFolderInfo(agGameType);
            List<FileInfo> fileInfos = Directory.GetFiles(localFolderInfo.LocalFolder).Select(s => new FileInfo(s)).ToList();
            var xmlFiles = new List<XMLFile>();

            if (!fileInfos.Any())
            {
                return xmlFiles;
            }

            foreach (FileInfo fileInfo in fileInfos)
            {
                if (fileInfo.Extension.ToLower() != ".xml")
                {
                    continue;
                }

                var xmlFile = new XMLFile()
                {
                    LocalPath = Path.Combine(localFolderInfo.LocalFolder, fileInfo.Name),
                    CreateDate = fileInfo.CreationTime,
                    LastModified = fileInfo.LastWriteTime,
                    Name = fileInfo.Name,
                };

                object locker = GetLocker(xmlFile);

                try
                {
                    lock (locker)
                    {
                        //資料上傳oss複製給其他商戶
                        string remoteSubfolderFilePath = localFolderInfo.PartLocalFolder.Replace("\\", "/") + fileInfo.Name;

                        BetLogFileService.WriteRemoteContentToOtherMerchant(
                            PlatformProduct.AG,
                            remoteSubfolderFilePath,
                            xmlFile.LocalPath,
                            _transferServiceAppSettingService.Value.CopyBetLogToMerchantCodes);

                        using (StreamReader streamReader = new StreamReader(xmlFile.LocalPath, Encoding.UTF8))
                        {
                            xmlFile.FileContent = streamReader.ReadToEnd();
                        }

                        xmlFiles.Add(xmlFile);
                        File.Delete(xmlFile.LocalPath);

                        string mapKey = GetXmlMapKey(agGameType, xmlFile.Name);

                        if (XmlFileMap.TryGetValue(mapKey, out XmlContent xmlContent))
                        {
                            //內容一樣不處理
                            if (xmlContent.FileContent == xmlFile.FileContent)
                            {
                                xmlFile.IsSkip = true;
                                LogUtilService.ForcedDebug($"文件 {xmlFile.RemotePath} Is Skip ");

                                continue;
                            }
                        }
                        else
                        {
                            xmlContent = new XmlContent()
                            {
                                FileContent = xmlFile.FileContent,
                                CreateDate = DateTime.Now
                            };

                            XmlFileMap.TryAdd(mapKey, xmlContent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtilService.Error("讀取重试文件 " + xmlFile.RemotePath + " 失败，详细信息：" + ex.Message);
                }
            }

            return xmlFiles;
        }

        public void DeleteRemoteFile(XMLFile xmlFile)
        {
            //抓取原始FTP不用砍, 外部會根據另一個服務設定,所以不會進來
            throw new NotSupportedException();
        }

        public void DeleteRemoteFiles(List<XMLFile> downLoadXmlFiles)
        {
            //抓取原始FTP不用砍, 外部會根據另一個服務設定,所以不會進來
            throw new NotSupportedException();
            //var ftpLoginParam = new FtpLoginParam()
            //{
            //    FtpAddress = AGProfitLossInfo.FtpAddress,
            //    FtpPort = AGProfitLossInfo.FtpPort,
            //    FtpUser = AGProfitLossInfo.FtpUser,
            //    FtpPassword = AGProfitLossInfo.FtpPassword
            //};

            //foreach (XMLFile xmlFile in downLoadXmlFiles)
            //{
            //    try
            //    {
            //        if (FtpUtil.DeleteFtpFile(ftpLoginParam, xmlFile.RemotePath))
            //        {
            //            string mapKey = GetXmlMapKey(agGameType, xmlFile.Name);
            //            _xmlFileMap.TryRemove(mapKey, out XmlContent value);
            //        }

            //        Task.Delay(300).Wait();
            //    }
            //    catch (Exception ex)
            //    {
            //        LogUtil.Error(ex);
            //    }
            //}
        }

        private LocalFolderInfo GetLocalFolderInfo(AGGameType agGameType)
        {
            string localFolder = string.Empty;
            string partLocalFolder = string.Empty;

            if (agGameType == AGGameType.AGIN)
            {
                localFolder = AGProfitLossInfo.AGINXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartAGINXMLFileDir;
            }
            else if (agGameType == AGGameType.XIN)
            {
                localFolder = AGProfitLossInfo.XINXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartXINXMLFileDir;
            }
            else if (agGameType == AGGameType.HUNTER)
            {
                localFolder = AGProfitLossInfo.HUNTERXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartHUNTERXMLFileDir;
            }
            else if (agGameType == AGGameType.YOPLAY)
            {
                localFolder = AGProfitLossInfo.YOPLAYXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartYOPLAYXMLFileDir;
            }

            return new LocalFolderInfo
            {
                LocalFolder = localFolder,
                PartLocalFolder = partLocalFolder,
            };
        }

        private static object GetLocker(XMLFile xmlFile)
        {
            object locker = null;

            lock (s_cacheWriteLocker)
            {
                string lockerCacheKey = string.Format(s_lockerKeyFormat, xmlFile.Name);

                locker = MemoryCacheUtil.GetCache(lockerCacheKey, false, false, 300, true, () =>
                {
                    return new object();
                });
            }

            return locker;
        }
    }
}