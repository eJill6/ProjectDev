using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using ProductTransferService.AgDataBase.DLL.FileService;
using ProductTransferService.AgDataBase.Model;
using System.Collections.Concurrent;

namespace AgDataBase.DLL.FileService
{
    public class AGRemoteOssXmlFileService : BaseAGRemoteXmlFileService, IAGRemoteXmlFileService
    {
        private static readonly ConcurrentQueue<XMLFile> s_xmlFiles = new ConcurrentQueue<XMLFile>();

        private static readonly ConcurrentDictionary<string, object> s_xmlFileNameMap = new ConcurrentDictionary<string, object>();

        public AGRemoteOssXmlFileService()
        {
        }

        public void DownloadAllRemoteNormalXmlFiles(AGGameType agGameType)
        {
            DownloadAllRemoteXmlFiles(agGameType);
        }

        public void DownloadAllRemoteLostAndFoundXmlFiles(AGGameType agGameType)
        {
            //do nothing;
        }

        public List<XMLFile> GetAllLocalXmlFiles(AGGameType agGameType)
        {
            var xmlFiles = new List<XMLFile>();

            while (s_xmlFiles.Any())
            {
                if (s_xmlFiles.TryDequeue(out XMLFile xmlFile))
                {
                    s_xmlFileNameMap.TryRemove(xmlFile.Name, out object value);
                    xmlFiles.Add(xmlFile);
                }
            }

            return xmlFiles;
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
                    LogUtilService.Error($"delete remotePath fail, path:{remotePath}, {ex.ToString()}");
                }
            }
        }

        private void DownloadAllRemoteXmlFiles(AGGameType agGameType)
        {
            //不同商戶在遠端的路徑不同,oss的部份整合後不再有LostAndFound
            string remotePath = PlatformProductAGSettingService.GetRemotePath(agGameType, dateTimeRemotePath: null);
            List<string> fullFileNames = BetLogFileService.GetFullFileNames(PlatformProduct.AG, remotePath);
            var downLoadXmlFiles = new List<XMLFile>();

            if (!fullFileNames.Any())
            {
                return;
            }

            foreach (string fullFileName in fullFileNames)
            {
                var xmlFile = new XMLFile()
                {
                    Name = fullFileName.Split('/').Last(),
                    RemotePath = fullFileName
                };

                downLoadXmlFiles.Add(xmlFile);

                if (!fullFileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    xmlFile.IsDeleteFromRemote = true;

                    continue;
                }
            }

            if (!downLoadXmlFiles.Any())
            {
                return;
            }

            LogUtilService.ForcedDebug($"TransferRemote {agGameType.Value} 準備下載檔案：{downLoadXmlFiles.Count}个文件");

            for (int i = 0; i < downLoadXmlFiles.Count; i++)
            {
                XMLFile xmlFile = downLoadXmlFiles[i];

                if (s_xmlFileNameMap.ContainsKey(xmlFile.Name))
                {
                    continue;
                }

                string processContent = $"{i + 1}/{downLoadXmlFiles.Count}";

                try
                {
                    xmlFile.FileContent = BetLogFileService.GetFileContent(xmlFile.RemotePath);

                    if (s_xmlFileNameMap.TryAdd(xmlFile.Name, value: null))
                    {
                        s_xmlFiles.Enqueue(xmlFile);
                    }

                    LogUtilService.ForcedDebug($"下载文件 {xmlFile.RemotePath} {processContent} 成功");
                }
                catch (Exception ex)
                {
                    LogUtilService.Error("下载文件 " + xmlFile.RemotePath + " 失败，详细信息：" + ex.Message);
                }
            }
        }
    }
}