namespace MS.Core.Infrastructure.OSS
{
    public interface IObjectStorageService
    {
        Task<bool> DeleteObject(string fullFileName);

        Task<bool> DeleteObjects(List<string> fullFileNames);

        Task<string[]> GetFullFileNames(string filter);

        Task<byte[]> GetObject(string fullFileName);

        Task<bool> UploadObject(byte[] fileByteData, string fullFileName);

        Task UploadObjectByFilePath(string fullFileName, string fileToUpload);

        Task<string> GetCdnPath(string path);

        /// <summary>
        /// 取出Backet所有的物件
        /// </summary>
        /// <param name="action">針對Key要處理的事情</param>
        /// <returns>非同步任務</returns>
        Task ExcutePaginators(Func<string, Task> action);

        /// <summary>
        /// 檔案是否存在
        /// </summary>
        /// <param name="fullFileName">檔案名稱</param>
        /// <returns>是否存在</returns>
        Task<bool> IsObjectExist(string fullFileName);
    }
}