using ProductTransferService.AgDataBase.Model;
using JxBackendService.Model.Enums.ThirdParty;

namespace ProductTransferService.AgDataBase.DLL.FileService
{
    public interface IAGRemoteXmlFileService
    {
        void DeleteRemoteFile(XMLFile xmlFile);

        void DeleteRemoteFiles(List<XMLFile> downLoadXmlFiles);

        void DownloadAllRemoteNormalXmlFiles(AGGameType agGameType);

        void DownloadAllRemoteLostAndFoundXmlFiles(AGGameType agGameType);

        List<XMLFile> GetAllLocalXmlFiles(AGGameType agGameType);
    }
}