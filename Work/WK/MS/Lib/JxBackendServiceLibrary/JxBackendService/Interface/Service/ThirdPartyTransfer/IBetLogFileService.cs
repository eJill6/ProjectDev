using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.ThirdPartyTransfer
{
    public interface IBetLogFileService
    {
        void DeleteFile(string fullFileName);

        void DeleteFiles(List<string> fullFileNames);

        RequestAndResponse GetBetLogContent(PlatformProduct product, long lastFileSeq);

        string GetFileContent(string fullFileName);

        List<string> GetFullFileNames(PlatformProduct product, string remoteSubfolderPath);

        void WriteRemoteContentToOtherMerchant(PlatformProduct product, long fileSeq, string content, List<string> copyToMerchantCodes);

        void WriteRemoteContentToOtherMerchant(PlatformProduct product, string remoteSubfolderFilePath, string fileToUpload, List<string> copyToMerchantCodes);
    }
}