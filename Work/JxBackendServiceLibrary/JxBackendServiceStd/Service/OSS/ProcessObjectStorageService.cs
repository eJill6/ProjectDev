using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.OSS;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.OSS;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.OSS
{
    public class ProcessObjectStorageService : BaseService, IProcessObjectStorageService
    {
        private readonly IHttpWebRequestUtilService _httpWebRequestUtilService;

        private readonly ILogUtilService _logUtilService;

        public ProcessObjectStorageService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _httpWebRequestUtilService = DependencyUtil.ResolveService<IHttpWebRequestUtilService>();
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        public void UploadToOss(IOSSSetting ossSetting, List<UpdateImageOSSParam> updateImageOSSParams, Action<int, string> uploadCallback,
            out List<string> deleteFullObjectNames)
        {
            var objectStorageService = ResolveServiceForModel<IObjectStorageService>(ossSetting);
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

                    string webObjectName = CreateNewObjectName(updateImageOSSParam.SEQID, updateImageOSSParam.Application, updateImageOSSParam.ImageFileName);
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
            var objectStorageService = ResolveServiceForModel<IObjectStorageService>(ossSetting);

            foreach (string deleteImageUrl in deleteImageUrls)
            {
                if (!deleteImageUrl.IsNullOrEmpty())
                {
                    objectStorageService.DeleteObject(ConvertToFullObjectName(deleteImageUrl));
                }
            }
        }

        private string CreateNewObjectName(string seqId, JxApplication jxApplication, string fileName)
        {
            var fileInfo = new FileInfo(fileName);

            return $"{seqId}-{jxApplication.ShortValue}-{DateTime.Now.ToUnixOfTime()}{fileInfo.Extension}";
        }

        private string ConvertToFullObjectName(string imageUrl) => imageUrl.TrimStart("/");

        private string ConvertToImageUrl(string fullObjectName) => $"/{fullObjectName}";

        private void LoadByCDN(string imageUrl)
        {
            string fullImageUrl = _httpWebRequestUtilService.CombineUrl(SharedAppSettings.BucketCdnDomain, imageUrl);

            JxTask.Run(EnvLoginUser, () =>
            {
                _httpWebRequestUtilService.GetResponse(new WebRequestParam()
                {
                    Url = fullImageUrl,
                    Method = HttpMethod.Get
                }, out HttpStatusCode httpStatusCode);

                _logUtilService.ForcedDebug($"{fullImageUrl} load完成");
            });
        }
    }
}