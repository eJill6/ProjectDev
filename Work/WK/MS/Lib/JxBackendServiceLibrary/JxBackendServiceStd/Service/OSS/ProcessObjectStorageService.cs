using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.OSS;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.OSS;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace JxBackendService.Service.OSS
{
    public class ProcessObjectStorageService : BaseEnvLoginUserService, IProcessObjectStorageService
    {
        private readonly Lazy<IHttpWebRequestUtilService> _httpWebRequestUtilService;

        private readonly Lazy<ILogUtilService> _logUtilService;

        private readonly Lazy<IPlatformAESService> _platformAESService;

        public ProcessObjectStorageService(EnvironmentUser envLoginUser) : base(envLoginUser)
        {
            _httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
            _platformAESService = DependencyUtil.ResolveService<IPlatformAESService>();
        }

        public void UploadOriginAndAESToImageOSS(IOSSSetting ossSetting, List<UpdateImageOSSParam> updateImageOSSParams, Action<int, string> uploadCallback,
            out List<string> deleteFullObjectNames)
        {
            // 上傳原圖
            UploadToOss(ossSetting, updateImageOSSParams, uploadCallback, out deleteFullObjectNames);

            var updateAESImageOSSParams = new List<UpdateImageOSSParam>();

            foreach (UpdateImageOSSParam ossParam in updateImageOSSParams)
            {
                var fileInfo = new FileInfo(ossParam.ImageFileName);
                byte[] aesBytes = _platformAESService.Value.Encrypt(ossParam.ImageBytes);

                updateAESImageOSSParams.Add(new UpdateImageOSSParam
                {
                    KeyID = ossParam.KeyID,
                    Application = ossParam.Application,
                    ImageBytes = aesBytes,
                    ImageFileName = fileInfo.Name.Replace(fileInfo.Extension, ".aes"),
                    UploadOSSRemotePath = ossParam.UploadOSSRemotePath,
                });
            }

            // 上傳AES圖檔
            UploadToOss(
                ossSetting,
                updateAESImageOSSParams,
                uploadCallback: (index, imageUrl) => { }, //不存DB, 所以宣告一個不做事的action
                out deleteFullObjectNames);
        }

        public void UploadToOss(IOSSSetting ossSetting, List<UpdateImageOSSParam> updateImageOSSParams, Action<int, string> uploadCallback,
            out List<string> deleteFullObjectNames)
        {
            var objectStorageService = DependencyUtil.ResolveServiceForModel<IObjectStorageService>(ossSetting).Value;
            deleteFullObjectNames = new List<string>();

            for (int i = 0; i < updateImageOSSParams.Count; i++)
            {
                UpdateImageOSSParam updateImageOSSParam = updateImageOSSParams[i];

                if (!updateImageOSSParam.ImageFileName.IsNullOrEmpty() && updateImageOSSParam.ImageBytes.AnyAndNotNull())
                {
                    if (!updateImageOSSParam.ImageUrl.IsNullOrEmpty())
                    {
                        deleteFullObjectNames.Add(ConvertToFullObjectName(updateImageOSSParam.ImageUrl));
                    }

                    string webObjectName = CreateNewObjectName(updateImageOSSParam.KeyID, updateImageOSSParam.Application, updateImageOSSParam.ImageFileName);
                    string uploadOssFullFileName = updateImageOSSParam.UploadOSSRemotePath + webObjectName;

                    objectStorageService.UploadObject(updateImageOSSParam.ImageBytes, uploadOssFullFileName);
                    updateImageOSSParam.ImageUrl = ConvertToImageUrl(uploadOssFullFileName);

                    LoadByCDN(updateImageOSSParam.ImageUrl);
                    uploadCallback.Invoke(i, updateImageOSSParam.ImageUrl);
                }
            }
        }

        public void DeleteImageObject(IOSSSetting ossSetting, string deleteImageUrl)
        {
            DeleteImageObject(ossSetting, new List<string>() { deleteImageUrl });
        }

        public void DeleteImageObject(IOSSSetting ossSetting, List<string> deleteImageUrls)
        {
            var objectStorageService = DependencyUtil.ResolveServiceForModel<IObjectStorageService>(ossSetting).Value;

            foreach (string deleteImageUrl in deleteImageUrls)
            {
                if (!deleteImageUrl.IsNullOrEmpty())
                {
                    objectStorageService.DeleteObject(ConvertToFullObjectName(deleteImageUrl));
                }
            }
        }

        public void DeleteOriginAndAESImage(IOSSSetting ossSetting, string imageUrl)
        {
            string originImageUrl = imageUrl;
            string fullOriginImageUrl = _httpWebRequestUtilService.Value.CombineUrl(SharedAppSettings.BucketCdnDomain, imageUrl);

            var uriBuilder = new UriBuilder(fullOriginImageUrl);

            string path = uriBuilder.Uri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
            string aesImageUrl = Path.ChangeExtension(path, "aes");

            var deleteImageUrls = new List<string>
            {
                originImageUrl,
                aesImageUrl,
            };

            DeleteImageObject(ossSetting, deleteImageUrls);
        }

        public byte[] GetImageObject(IOSSSetting ossSetting, string fullFileName)
        {
            var objectStorageService = DependencyUtil.ResolveServiceForModel<IObjectStorageService>(ossSetting).Value;

            return objectStorageService.GetObject(fullFileName);
        }

        private string CreateNewObjectName(string keyId, JxApplication jxApplication, string fileName)
        {
            var fileInfo = new FileInfo(fileName);

            return $"{keyId}-{jxApplication.ShortValue}{fileInfo.Extension}";
        }

        private string ConvertToFullObjectName(string imageUrl) => imageUrl.TrimStart("/");

        private string ConvertToImageUrl(string fullObjectName) => $"/{fullObjectName}";

        private void LoadByCDN(string imageUrl)
        {
            bool isAESExtension = imageUrl.IsAESExtension();
            string cdnDomain = SharedAppSettings.BucketCdnDomain;

            if (isAESExtension)
            {
                cdnDomain = SharedAppSettings.BucketAESCdnDomain;
            }

            string fullImageUrl = _httpWebRequestUtilService.Value.CombineUrl(cdnDomain, imageUrl);

            JxTask.Run(EnvLoginUser, () =>
            {
                _httpWebRequestUtilService.Value.GetResponse(new WebRequestParam()
                {
                    Url = fullImageUrl,
                    Method = HttpMethod.Get
                }, out HttpStatusCode httpStatusCode);

                _logUtilService.Value.ForcedDebug($"{fullImageUrl} load完成");
            });
        }
    }
}