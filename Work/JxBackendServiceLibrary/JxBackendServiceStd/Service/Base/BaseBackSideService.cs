using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.OSS;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace JxBackendService.Service.Base
{
    public class BaseBackSideService : BaseService
    {
        private readonly IBWOperationLogService _bwOperationLogService;
        private readonly IObjectStorageService _ossService;

        protected IBWOperationLogService BWOperationLogService => _bwOperationLogService;
        protected IObjectStorageService OssService => _ossService;

        public BaseBackSideService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _bwOperationLogService = ResolveJxBackendService<IBWOperationLogService>();

            var ossSettingService = ResolveJxBackendService<IOSSSettingService>(DbConnectionTypes.Slave);
            _ossService = ResolveServiceForModel<IObjectStorageService>(ossSettingService.GetOSSClientSetting());
        }

        protected string GetOperationCompareContent(List<RecordCompareParam> compareParams, ActTypes actTypes)
        {
            switch (actTypes)
            {
                case ActTypes.Insert:
                case ActTypes.Delete:

                    return GetInsertOperationCompareContent(compareParams);

                case ActTypes.Update:

                    return GetUpdateOperationCompareContent(compareParams);
            }

            return string.Empty;
        }

        private string GetInsertOperationCompareContent(List<RecordCompareParam> compareParams)
            => GetOperationCompareContent(compareParams, (param) =>
            {
                if (!param.IsVisibleCompareValue)
                {
                    return null;
                }

                return $"{param.Title}: {param.NewValue}";
            });

        private string GetUpdateOperationCompareContent(List<RecordCompareParam> compareParams)
            => GetOperationCompareContent(compareParams, (param) =>
            {
                if (param.OriginValue == param.NewValue)
                {
                    return null;
                }

                if (!param.IsVisibleCompareValue)
                {
                    return param.Title;
                }

                return string.Format(BWOperationLogElement.CompareValueMessage,
                    param.Title, param.OriginValue, param.NewValue);
            });

        private string GetOperationCompareContent(List<RecordCompareParam> compareParams,
            Func<RecordCompareParam, string> compare)
        {
            var builders = new List<string>();

            foreach (RecordCompareParam param in compareParams)
            {
                string compareResult = compare(param);

                if (!compareResult.IsNullOrEmpty())
                {
                    builders.Add(compareResult);
                }
            }

            return string.Join(", ", builders);
        }

        protected string UploadToOSS(IFormFile file, string ossDirectoryPath)
        {
            return UploadToOSS(file.ToBytes(), file.FileName, ossDirectoryPath);
        }

        protected string UploadToOSS(byte[] bytes, string fileName, string ossDirectoryPath)
        {
            string fileExtension = Path.GetExtension(fileName).ToLower();
            string imageUrl = $"{ossDirectoryPath}/{Guid.NewGuid().ToString().Replace("-", "")}{fileExtension}";

            _ossService.UploadObject(bytes, imageUrl);

            return imageUrl;
        }
    }
}