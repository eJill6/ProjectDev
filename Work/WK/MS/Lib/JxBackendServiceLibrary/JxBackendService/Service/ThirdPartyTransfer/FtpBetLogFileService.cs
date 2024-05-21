using FluentFTP;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Ftp;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class FtpBetLogFileService : IBetLogFileService
    {
        private static readonly string _settingLocationWriteTo = "settingLocationWriteTo";

        private static readonly bool _isOverWrite = false;

        private static readonly int _maxTryCount = 5;

        private static readonly int _retryIntervalSeconds = 2;

        public FtpBetLogFileService()
        {
        }

        public RequestAndResponse GetBetLogContent(PlatformProduct product, long lastFileSeq)
        {
            return RetryJobUtil.GetJobResultWithRetry(() =>
            {
                var requestAndResponse = new RequestAndResponse();

                string remotePath = $"{FtpSharedSettings.FtpRemoteFilePath}/{product.Value}/";
                List<FtpFileInfo> ftpFileInfos = FtpUtil.GetFileList(FtpSharedSettings.FtpLoginParam, remotePath)
                    .Where(w => w.FileType == FtpObjectType.File)
                    .OrderBy(o => long.Parse(o.FileNameWithoutExtension))
                    .ToList();

                string downloadFileName = string.Empty;

                // 取未處理最舊一筆檔案回來
                foreach (FtpFileInfo ftpFileInfo in ftpFileInfos)
                {
                    long downloadFileSeq = long.Parse(ftpFileInfo.FileNameWithoutExtension);

                    if (downloadFileSeq > lastFileSeq)
                    {
                        requestAndResponse.RequestBody = downloadFileSeq.ToString();
                        downloadFileName = ftpFileInfo.FileNameWithoutExtension;
                        break;
                    }
                    else
                    {
                        //不可一下載就刪除,若沒有存進sqlite,下次遠端資料就沒有了,所以改為遇到處理做過的檔案就刪除
                        LogUtil.ForcedDebug($"DeleteFtpFile: {ftpFileInfo.FullPath}");
                        FtpUtil.DeleteFtpFile(FtpSharedSettings.FtpLoginParam, ftpFileInfo.FullPath);
                    }
                }

                if (downloadFileName.IsNullOrEmpty())
                {
                    return requestAndResponse;
                }

                string remoteFilePath = $"{remotePath}/{downloadFileName}.json";
                string saveLocalFilePath = $"{FtpSharedSettings.SaveLocalFilePath}/{product.Value}/{downloadFileName}.json";

                LogUtil.ForcedDebug($"DownloadFile: {remoteFilePath}");

                bool isDownloadSuccess = FtpUtil.DownloadFile(
                    FtpSharedSettings.FtpLoginParam,
                    saveLocalFilePath,
                    remoteFilePath);

                if (!isDownloadSuccess)
                {
                    LogUtil.Error(new { isDownloadSuccess, saveLocalFilePath, remoteFilePath }.ToJsonString());
                    requestAndResponse.RequestBody = null;

                    return null;
                }

                // 讀檔回來處理
                requestAndResponse.ResponseContent = File.ReadAllText(saveLocalFilePath, Encoding.UTF8);
                // 做完將資料進行刪除
                File.Delete(saveLocalFilePath);

                return requestAndResponse;
            },
            _maxTryCount,
            _retryIntervalSeconds);
        }

        /// <summary>
        /// 寫檔
        /// </summary>
        public void WriteRemoteContentToOtherPlace(PlatformProduct product, long fileSeq, string content)
        {
            var directoryInfos = GetLocationWriteTo(product);

            if (!directoryInfos.Any())
            {
                LogUtil.Error("沒有可寫入的目錄路徑");

                return;
            }

            foreach (DirectoryInfo directoryInfo in directoryInfos)
            {
                try
                {
                    string destinitionFilePath = null;

                    for (int i = 1; i <= 10; i++)
                    {
                        bool isAppendSuffix = (i > 1);
                        string newFileName = GetFormatFileName(fileSeq.ToString(), isAppendSuffix);
                        destinitionFilePath = Path.Combine(directoryInfo.FullName, newFileName);

                        if (!File.Exists(destinitionFilePath))
                        {
                            break;
                        }
                    }

                    if (destinitionFilePath.IsNullOrEmpty())
                    {
                        LogUtil.Error("Create New File Name Fail");

                        continue;
                    }

                    LogUtil.Info($"{nameof(WriteRemoteContentToOtherPlace)} = {new { destinitionFilePath, _isOverWrite }.ToJsonString()}");

                    using (StreamWriter sw = new StreamWriter(destinitionFilePath, _isOverWrite, Encoding.UTF8))
                    {
                        sw.Write(content);
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"{nameof(WriteRemoteContentToOtherPlace)} Fail; Exception:{ex}");
                }
            }
        }

        private List<DirectoryInfo> GetLocationWriteTo(PlatformProduct product)
        {
            var directoryInfos = new List<DirectoryInfo>();

            string locationWriteTo = ConfigurationManager.AppSettings[_settingLocationWriteTo];

            if (locationWriteTo.IsNullOrEmpty())
            {
                return directoryInfos;
            }

            List<string> paths = locationWriteTo.Split(';').Where(w => !w.IsNullOrEmpty()).ToList();

            if (!paths.Any())
            {
                return directoryInfos;
            }

            foreach (string path in paths)
            {
                string formatPath = Regex.Replace(path, "{ProductCode}", product.Value, RegexOptions.IgnoreCase);

                try
                {
                    if (!Directory.Exists(formatPath))
                    {
                        Directory.CreateDirectory(formatPath);
                    }

                    DirectoryInfo directoryInfo = new DirectoryInfo(formatPath);
                    directoryInfos.Add(directoryInfo);
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"轉換{nameof(_settingLocationWriteTo)}設定 {formatPath}，轉換為DirectoryInfo Error :{ex}");
                }
            }

            return directoryInfos;
        }

        private string GetFormatFileName(string fileName, bool isAppendSuffix)
        {
            string newFileName = $"{fileName}";

            if (isAppendSuffix)
            {
                newFileName += "-" + Guid.NewGuid().ToString();
            }

            newFileName += ".json"; //容舊線上的附檔名名稱

            return newFileName;
        }
    }
}