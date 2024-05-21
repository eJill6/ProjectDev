using JxBackendService.Model.Param.OSS;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.OSS
{
    public interface IProcessObjectStorageService
    {
        void DeleteOriginAndAESImage(IOSSSetting ossSetting, string imageUrl);

        void DeleteImageObject(IOSSSetting ossSetting, List<string> deleteImageUrls);

        void DeleteImageObject(IOSSSetting ossSetting, string deleteImageUrl);

        byte[] GetImageObject(IOSSSetting ossSetting, string fullFileName);

        void UploadOriginAndAESToImageOSS(IOSSSetting ossSetting, List<UpdateImageOSSParam> updateImageOSSParams, Action<int, string> uploadCallback,
            out List<string> deleteFullObjectNames);

        void UploadToOss(IOSSSetting ossSetting, List<UpdateImageOSSParam> updateImageOSSParams, Action<int, string> uploadCallback,
            out List<string> deleteFullObjectNames);
    }
}