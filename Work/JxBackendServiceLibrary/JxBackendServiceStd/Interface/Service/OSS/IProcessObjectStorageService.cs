using System;
using System.Collections.Generic;
using JxBackendService.Model.Param.OSS;

namespace JxBackendService.Interface.Service.OSS
{
    public interface IProcessObjectStorageService
    {
        void DeleteImageObject(IOSSSetting ossSetting, List<string> deleteImageUrls);

        void DeleteImageObject(IOSSSetting ossSetting, string deleteImageUrl);

        void UploadToOss(IOSSSetting ossSetting, List<UpdateImageOSSParam> updateImageOSSParams, Action<int, string> uploadCallback,
            out List<string> deleteFullObjectNames);
    }
}