using System.Collections.Generic;

namespace JxBackendService.Interface.Service.OSS
{
    public interface IObjectStorageService
    {
        //void CopyObject(string sourceFullFileName, string targetFullFileName);

        void DeleteObject(string fullFileName);

        void DeleteObjects(List<string> fullFileNames);

        List<string> GetFullFileNames(string filter);

        byte[] GetObject(string fullFileName);

        void UploadObject(byte[] fileByteData, string fullFileName);

        void UploadObjectByFilePath(string fullFileName, string fileToUpload);
    }
}