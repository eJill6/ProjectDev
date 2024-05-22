using AgDataBase.Common;
using AgDataBase.Model;
using JxBackendService.Common.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendServiceNF.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgDataBase.DLL.FileService
{
    public class AGRemoteOssXmlFileService : BaseAGRemoteXmlFileService, IAGRemoteXmlFileService
    {
        public AGRemoteOssXmlFileService()
        {
        }

        public List<XMLFile> GetAllXmlFiles(AGGameType agGameType)
        {
            string dateTimeRemotePath = AGApi.GetDateTimeRemotePath();
            string remotePath = PlatformProductAGSettingService.GetRemotePath(agGameType, dateTimeRemotePath); //不同商戶在遠端的路徑不同
            List<string> fullFileNames = BetLogFileService.GetFullFileNames(PlatformProduct.AG, remotePath);
            var downLoadXmlFiles = new List<XMLFile>();

            if (!fullFileNames.Any())
            {
                return downLoadXmlFiles;
            }

            foreach (string fullFileName in fullFileNames)
            {
                var xmlFile = new XMLFile()
                {
                    Name = fullFileName.Split('/').Last(),
                    RemotePath = fullFileName
                };

                downLoadXmlFiles.Add(xmlFile);

                if (!fullFileName.ToLower().EndsWith(".xml"))
                {
                    xmlFile.IsDeleteFromRemote = true;

                    continue;
                }

                string mapKey = GetXmlMapKey(agGameType, fullFileName);

                bool isAddToDownloadList = false;

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
                    xmlFile.IsDeleteFromRemote = true;
                }
            }

            if (!downLoadXmlFiles.Any())
            {
                return downLoadXmlFiles;
            }

            LogUtil.ForcedDebug($"TransferRemote {agGameType.Value} 準備下載檔案：{downLoadXmlFiles.Count}个文件");
            downLoadXmlFiles.Sort();

            for (int i = 0; i < downLoadXmlFiles.Count; i++)
            {
                string processContent = $"{i + 1}/{downLoadXmlFiles.Count}";
                XMLFile xmlFile = downLoadXmlFiles[i];

                try
                {
                    xmlFile.FileContent = BetLogFileService.GetFileContent(xmlFile.RemotePath);
                    string mapKey = GetXmlMapKey(agGameType, xmlFile.Name);

                    XmlFileMap.TryGetValue(mapKey, out XmlContent xmlContent);

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
                            LogsManager.Info($"文件 {xmlFile.RemotePath} {processContent} Is Skip ");

                            continue;
                        }
                    }

                    LogUtil.ForcedDebug($"下载文件 {xmlFile.RemotePath} {processContent} 成功");
                }
                catch (Exception ex)
                {
                    LogUtil.Error("下载文件 " + xmlFile.RemotePath + " 失败，详细信息：" + ex.Message);
                }
            }

            return downLoadXmlFiles;
        }

        public List<XMLFile> GetAllLostAndFoundXmlFiles(AGGameType agGameType)
        {
            string dateTimeRemotePath = AGApi.GetDateTimeRemotePath();
            string remotePath = PlatformProductAGSettingService.GetRemoteLostAndFoundPath(agGameType, dateTimeRemotePath); //不同商戶在遠端的路徑不同
            List<string> fullFileNames = BetLogFileService.GetFullFileNames(PlatformProduct.AG, remotePath);
            var downLoadXmlFiles = new List<XMLFile>();

            if (!fullFileNames.Any())
            {
                return downLoadXmlFiles;
            }

            foreach (string fullFileName in fullFileNames)
            {
                var xmlFile = new XMLFile()
                {
                    Name = fullFileName.Split('/').Last(),
                    RemotePath = fullFileName
                };

                downLoadXmlFiles.Add(xmlFile);

                if (!fullFileName.ToLower().EndsWith(".xml"))
                {
                    xmlFile.IsDeleteFromRemote = true;

                    continue;
                }
            }

            if (!downLoadXmlFiles.Any())
            {
                return downLoadXmlFiles;
            }

            LogUtil.ForcedDebug($"TransferRemoteLostAndFound {agGameType.Value} 準備下載檔案：{downLoadXmlFiles.Count}个文件");
            downLoadXmlFiles.Sort();

            for (int i = 0; i < downLoadXmlFiles.Count; i++)
            {
                string processContent = $"{i + 1}/{downLoadXmlFiles.Count}";
                XMLFile xmlFile = downLoadXmlFiles[i];

                try
                {
                    xmlFile.FileContent = BetLogFileService.GetFileContent(xmlFile.RemotePath);
                    LogUtil.ForcedDebug($"下载文件 {xmlFile.RemotePath} {processContent} 成功");
                }
                catch (Exception ex)
                {
                    LogUtil.Error("下载文件 " + xmlFile.RemotePath + " 失败，详细信息：" + ex.Message);
                }
            }

            return downLoadXmlFiles;
        }

        public void DeleteRemoteFile(XMLFile xmlFile)
        {
            DeleteRemoteFiles(new List<XMLFile>() { xmlFile });
        }

        public void DeleteRemoteFiles(List<XMLFile> xmlFiles)
        {
            foreach (XMLFile xmlFile in xmlFiles)
            {
                string remotePath = xmlFile.RemotePath;

                try
                {
                    BetLogFileService.DeleteFile(remotePath);
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"delete remotePath fail, path:{remotePath}, {ex.ToString()}");
                }
            }
        }
    }
}