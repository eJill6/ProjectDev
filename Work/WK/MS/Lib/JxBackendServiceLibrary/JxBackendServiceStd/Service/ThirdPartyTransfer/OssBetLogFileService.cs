using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.OSS;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class OssBetLogFileService : BaseService, IBetLogFileService
    {
        private static readonly int _maxTryCount = 5;

        private static readonly int _retryIntervalSeconds = 2;

        private static readonly string s_remoteMerchantPathTemplate = "UploadBetLogService/Upload/{0}/"; // 商戶代碼

        private static readonly string s_remoteProductFolderTemplate = s_remoteMerchantPathTemplate + "{1}/"; // 產品代碼

        private static readonly string s_remoteProductFileTemplate = s_remoteProductFolderTemplate + "{2}.json"; // 檔名

        private readonly Lazy<IObjectStorageService> _objectStorageService;

        private readonly Lazy<ILogUtilService> _logUtilService;

        public OssBetLogFileService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            var coreOssSettingService = ResolveJxBackendService<IOSSSettingService>().Value;
            _objectStorageService = DependencyUtil.ResolveServiceForModel<IObjectStorageService>(coreOssSettingService.GetCoreOSSClientSetting());
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public RequestAndResponse GetBetLogContent(PlatformProduct product, long lastFileSeq)
        {
            return RetryJobUtil.GetJobResultWithRetry(() =>
            {
                var requestAndResponse = new RequestAndResponse();
                string merchantCode = SharedAppSettings.PlatformMerchant.Value;

                List<string> fullFileNames = _objectStorageService.Value.GetFullFileNames(string.Format(s_remoteProductFolderTemplate, merchantCode, product));
                fullFileNames.Sort();
                string downloadFileName = string.Empty;

                // 取未處理最舊一筆檔案回來
                foreach (string fullFileName in fullFileNames)
                {
                    string fileNameWithoutExtension = fullFileName.Split('/').Last().TrimEnd(".json");
                    long downloadFileSeq = long.Parse(fileNameWithoutExtension);

                    if (downloadFileSeq > lastFileSeq)
                    {
                        requestAndResponse.RequestBody = downloadFileSeq.ToString();
                        downloadFileName = fullFileName;
                        break;
                    }
                    else
                    {
                        //不可一下載就刪除,若沒有存進sqlite,下次遠端資料就沒有了,所以改為遇到處理做過的檔案就刪除
                        _logUtilService.Value.ForcedDebug($"DeleteOssFile: {fullFileName}");
                        DeleteFile(fullFileName);
                    }
                }

                if (downloadFileName.IsNullOrEmpty())
                {
                    return requestAndResponse;
                }

                // 讀檔回來處理
                byte[] bytes = _objectStorageService.Value.GetObject(downloadFileName);
                string fileContent = Encoding.UTF8.GetString(bytes);
                requestAndResponse.ResponseContent = fileContent;

                return requestAndResponse;
            },
            _maxTryCount,
            _retryIntervalSeconds);
        }

        /// <summary>
        /// 寫檔
        /// </summary>
        public void WriteRemoteContentToOtherMerchant(PlatformProduct product, long fileSeq, string content, List<string> copyToMerchantCodes)
        {
            //上傳到各自商戶的資料夾
            foreach (string merchantCode in copyToMerchantCodes)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                string uploadFullFileName = string.Format(s_remoteProductFileTemplate, merchantCode, product, fileSeq);
                _objectStorageService.Value.UploadObject(bytes, uploadFullFileName);
                _logUtilService.Value.ForcedDebug($"UploadObject:{uploadFullFileName}");
            }
        }

        /// <summary>
        /// 自定義子目錄路徑寫檔
        /// </summary>
        public void WriteRemoteContentToOtherMerchant(PlatformProduct product, string remoteSubfolderFilePath, string fileToUpload, List<string> copyToMerchantCodes)
        {
            //上傳到各自商戶的資料夾
            foreach (string merchantCode in copyToMerchantCodes)
            {
                string uploadFullFileName = string.Format(s_remoteProductFolderTemplate, merchantCode, product) + remoteSubfolderFilePath;
                _objectStorageService.Value.UploadObjectByFilePath(uploadFullFileName, fileToUpload);
                _logUtilService.Value.ForcedDebug($"UploadObjectByFilePath:{uploadFullFileName}");
            }
        }

        public List<string> GetFullFileNames(PlatformProduct product, string remoteSubfolderPath)
        {
            string merchantCode = SharedAppSettings.PlatformMerchant.Value;
            string filter = string.Format(s_remoteProductFolderTemplate, merchantCode, product) + remoteSubfolderPath;
            List<string> fullFileNames = _objectStorageService.Value.GetFullFileNames(filter);

            return fullFileNames;
        }

        public string GetFileContent(string fullFileName)
        {
            byte[] bytes = _objectStorageService.Value.GetObject(fullFileName);
            string fileContent = Encoding.UTF8.GetString(bytes);

            return fileContent;
        }

        public void DeleteFile(string fullFileName)
        {
            _objectStorageService.Value.DeleteObject(fullFileName);
        }

        public void DeleteFiles(List<string> fullFileNames)
        {
            _objectStorageService.Value.DeleteObjects(fullFileNames);
        }
    }
}