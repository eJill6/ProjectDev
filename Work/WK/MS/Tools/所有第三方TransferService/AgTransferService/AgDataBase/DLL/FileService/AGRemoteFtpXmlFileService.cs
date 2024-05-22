using AgDataBase.Common;
using AgDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendServiceNF.Common.Util;
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

        private readonly ITransferServiceAppSettingService _transferServiceAppSettingService;

        public AGRemoteFtpXmlFileService()
        {
            _transferServiceAppSettingService = DependencyUtil.ResolveService<ITransferServiceAppSettingService>();
        }

        public List<XMLFile> GetAllXmlFiles(AGGameType agGameType)
        {
            string localFolder = string.Empty;
            string partLocalFolder = string.Empty;
            var platformProductAGSettingService = DependencyUtil.ResolveKeyed<IPlatformProductAGSettingService>(SharedAppSettings.PlatformMerchant);
            IAgApi agApi = DependencyUtil.ResolveService<IAgApi>();
            string dateTimeRemotePath = agApi.GetDateTimeRemotePath();
            string remotePath = platformProductAGSettingService.GetRemotePath(agGameType, dateTimeRemotePath); //不同商戶在遠端的路徑不同

            if (agGameType == AGGameType.AGIN)
            {
                //ftpRemotePath = @"/AGIN/";
                localFolder = AGProfitLossInfo.AGINXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartAGINXMLFileDir;
            }
            else if (agGameType == AGGameType.XIN)
            {
                ///ftpRemotePath = @"/XIN/";
                localFolder = AGProfitLossInfo.XINXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartXINXMLFileDir;
            }
            else if (agGameType == AGGameType.HUNTER)
            {
                //ftpRemotePath = @"/HUNTER/";
                localFolder = AGProfitLossInfo.HUNTERXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartHUNTERXMLFileDir;
            }
            else if (agGameType == AGGameType.YOPLAY)
            {
                //ftpRemotePath = @"/YOPLAY/";
                localFolder = AGProfitLossInfo.YOPLAYXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartYOPLAYXMLFileDir;
            }

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
                    LogUtil.ForcedDebug("列取目录 " + remotePath + " 文件列表成功，共获取到 " + allXmlFiles.Count.ToString() + " 个文件");
                }
                else
                {
                    LogUtil.ForcedDebug("目录 " + remotePath + " 无文件，暂停解析");
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error("列取目录 " + remotePath + " 失败，详细信息：" + ex.Message);

                return new List<XMLFile>();
            }

            var downLoadXmlFiles = new List<XMLFile>();

            if (!allXmlFiles.Any())
            {
                return downLoadXmlFiles;
            }

            for (int i = allXmlFiles.Count - 1; i >= 0; i--)
            {
                XMLFile file = allXmlFiles[i];
                string fileName = file.Name;

                if (!fileName.ToLower().EndsWith(".xml"))
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
                return downLoadXmlFiles;
            }

            downLoadXmlFiles.Sort();
            LogUtil.ForcedDebug($"TransferRemote {agGameType.Value} 準備下載檔案：{downLoadXmlFiles.Count}个文件");

            for (int i = 0; i < downLoadXmlFiles.Count; i++)
            {
                string processContent = $"{i + 1}/{downLoadXmlFiles.Count}";
                XMLFile xmlFile = downLoadXmlFiles[i];
                string localPath = localFolder + xmlFile.Name;

                try
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

                    string remoteSubfolderFilePath = partLocalFolder.Replace("\\", "/") + xmlFile.Name;

                    //資料上傳oss複製給其他商戶
                    BetLogFileService.WriteRemoteContentToOtherMerchant(
                        PlatformProduct.AG,
                        remoteSubfolderFilePath,
                        localPath,
                        _transferServiceAppSettingService.CopyBetLogToMerchantCodes);

                    xmlFile.LocalPath = localPath;
                    string mapKey = GetXmlMapKey(agGameType, xmlFile.Name);
                    XmlContent xmlContent = null;

                    using (var streamReader = new StreamReader(xmlFile.LocalPath, Encoding.UTF8))
                    {
                        XmlFileMap.TryGetValue(mapKey, out xmlContent);
                        xmlFile.FileContent = streamReader.ReadToEnd();
                    }

                    LogUtil.ForcedDebug($"下载文件 {xmlFile.RemotePath} {processContent} 成功");
                    File.Delete(xmlFile.LocalPath);

                    if (xmlContent == null)
                    {
                        xmlContent = new XmlContent()
                        {
                            FileContent = xmlFile.FileContent,
                            CreateDate = DateTime.Now
                        };

                        XmlFileMap.TryAdd(mapKey, xmlContent);
                    }
                    else
                    {
                        //內容一樣不處理
                        if (xmlContent.FileContent == xmlFile.FileContent)
                        {
                            xmlFile.IsSkip = true;
                            LogUtil.ForcedDebug($"文件 {xmlFile.RemotePath} {processContent} Is Skip ");

                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error("下载文件 " + xmlFile.RemotePath + " 失败，详细信息：" + ex.Message);
                }
            }

            downLoadXmlFiles = downLoadXmlFiles.Where(p => !string.IsNullOrEmpty(p.LocalPath)).ToList();

            return downLoadXmlFiles;
        }

        public List<XMLFile> GetAllLostAndFoundXmlFiles(AGGameType agGameType)
        {
            var allXmlFiles = new List<XMLFile>();
            var downLoadXmlFiles = new List<XMLFile>();
            string localFolder = string.Empty;
            string partLocalFolder = string.Empty;

            if (agGameType == AGGameType.AGIN)
            {
                //remoteFolder = @"/AGIN/lostAndfound/";
                localFolder = AGProfitLossInfo.AGINLostAndFoundXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartAGINLostAndFoundXMLFileDir;
            }
            else if (agGameType == AGGameType.XIN)
            {
                //remoteFolder = @"/XIN/lostAndfound/";
                localFolder = AGProfitLossInfo.XINLostAndFoundXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartXINLostAndFoundXMLFileDir;
            }
            else if (agGameType == AGGameType.HUNTER)
            {
                //remoteFolder = @"/HUNTER/lostAndfound/";
                localFolder = AGProfitLossInfo.HUNTERLostAndFoundXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartHUNTERLostAndFoundXMLFileDir;
            }
            else if (agGameType == AGGameType.YOPLAY)
            {
                //remoteFolder = @"/YOPLAY/lostAndfound/";
                localFolder = AGProfitLossInfo.YOPLAYLostAndFoundXMLFileDir;
                partLocalFolder = AGProfitLossInfo.PartYOPLAYLostAndFoundXMLFileDir;
            }

            var platformProductAGSettingService = DependencyUtil.ResolveKeyed<IPlatformProductAGSettingService>(SharedAppSettings.PlatformMerchant);
            IAgApi agApi = DependencyUtil.ResolveService<IAgApi>();

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
                        LogsManager.Info("获取远程重试目录 " + remotePath + " 文件列表成功，共获取到 " + xmlFiles.Count.ToString() + " 个文件");
                        allXmlFiles.AddRange(xmlFiles);
                    }
                    else
                    {
                        LogsManager.Info("远程重试目录 " + remotePath + " 无文件，暂停解析");
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error("获取远程重试目录 " + remotePath + " 失败，详细信息：" + ex.Message);
                }
            }

            for (int i = allXmlFiles.Count - 1; i >= 0; i--)
            {
                if (allXmlFiles[i].Name.ToLower().EndsWith(".xml"))
                {
                    downLoadXmlFiles.Add(allXmlFiles[i]);
                }
            }

            if (!downLoadXmlFiles.Any())
            {
                return downLoadXmlFiles;
            }

            downLoadXmlFiles.Sort();
            LogUtil.ForcedDebug($"TransferRemote {agGameType.Value} 準備下載檔案：{downLoadXmlFiles.Count}个文件");

            foreach (XMLFile xmlFile in downLoadXmlFiles)
            {
                string localPath = localFolder + xmlFile.Name;

                try
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

                    string remoteSubfolderFilePath = partLocalFolder.Replace("\\", "/") + xmlFile.Name;

                    //資料上傳oss複製給其他商戶
                    BetLogFileService.WriteRemoteContentToOtherMerchant(
                        PlatformProduct.AG,
                        remoteSubfolderFilePath,
                        localPath,
                        _transferServiceAppSettingService.CopyBetLogToMerchantCodes);

                    xmlFile.LocalPath = localPath;

                    using (StreamReader file = new StreamReader(xmlFile.LocalPath, Encoding.UTF8))
                    {
                        xmlFile.FileContent = file.ReadToEnd();
                    }

                    File.Delete(xmlFile.LocalPath);
                }
                catch (Exception ex)
                {
                    LogUtil.Error("下载重试文件 " + xmlFile.RemotePath + " 失败，详细信息：" + ex.Message);
                }
            }

            downLoadXmlFiles = downLoadXmlFiles.Where(p => !string.IsNullOrEmpty(p.LocalPath)).ToList();

            return downLoadXmlFiles;
        }

        public void DeleteRemoteFile(XMLFile xmlFile)
        {
            //CTL其實不用砍, 外部會根據另一個服務設定,所以不會進來
            throw new NotSupportedException();
        }

        public void DeleteRemoteFiles(List<XMLFile> downLoadXmlFiles)
        {
            //CTL其實不用砍, 外部會根據另一個服務設定,所以不會進來
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
    }
}