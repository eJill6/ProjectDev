using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.Base
{
    public class BaseBackSideService : BaseService
    {
        private readonly Lazy<IBWOperationLogService> _bwOperationLogService;

        protected IBWOperationLogService BWOperationLogService => _bwOperationLogService.Value;

        public BaseBackSideService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _bwOperationLogService = ResolveJxBackendService<IBWOperationLogService>();
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
    }
}