using AgDataBase.Model;
using JxBackendService.Model.Enums.ThirdParty;
using System.Collections.Generic;

namespace AgDataBase.DLL.FileService
{
    public interface IAGRemoteXmlFileService
    {
        void DeleteRemoteFile(XMLFile xmlFile);

        void DeleteRemoteFiles(List<XMLFile> downLoadXmlFiles);

        List<XMLFile> GetAllLostAndFoundXmlFiles(AGGameType agGameType);

        List<XMLFile> GetAllXmlFiles(AGGameType agGameType);
    }
}