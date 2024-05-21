using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.BackSideUser;
using JxBackendService.Interface.Service.BackSideUser;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.Entity.BackSideUser;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.SystemSetting;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service.BackSideWeb
{
    public class BWOperationLogService : BaseService, IBWOperationLogService, IBWOperationLogReadService
    {
        private readonly Lazy<IBWOperationLogRep> _bwOperationLogRep;

        public BWOperationLogService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _bwOperationLogRep = ResolveJxBackendService<IBWOperationLogRep>();
        }

        public BaseReturnModel CreateOperationLog(CreateBWOperationLogParam createParam)
        {
            if (createParam.PermissionKey.OperationType == null)
            {
                throw new Exception($"寫入操作日誌失敗。此PermissionKey: {createParam.PermissionKey} 沒有關聯到OperationType");
            }

            CreateBWOperationLogByTypeParam byTypeParam = createParam.CastByJson<CreateBWOperationLogByTypeParam>();
            byTypeParam.OperationType = createParam.PermissionKey.OperationType;

            return CreateOperationLog(byTypeParam);
        }

        public BaseReturnModel CreateOperationLog(CreateBWOperationLogByTypeParam createParam)
        {
            IBackSideWebUserService backSideWebUserService = DependencyUtil.ResolveService<IBackSideWebUserService>().Value;
            BackSideWebUser backSideWebUser = backSideWebUserService.GetUser();

            var operationLog = new BWOperationLog
            {
                PermissionKey = string.Empty,
                OperationType = createParam.OperationType.Value,
                OperateUserName = backSideWebUser.UserName,
                UserID = createParam.UserID,
                ReferenceKey = createParam.ReferenceKey.ToTrimString(),
                Content = createParam.Content.ToTrimString(),
            };

            return _bwOperationLogRep.Value.CreateByProcedure(operationLog).CastByJson<BaseReturnModel>();
        }

        public PagedResultModel<OperationLogViewModel> GetPagedBWOperationLogs(QueryBWOperationLogParam queryParam)
        {
            PagedResultModel<BWOperationLog> datas = _bwOperationLogRep.Value.GetPagedOperationLog(queryParam);
            var models = datas.CastByJson<PagedResultModel<OperationLogViewModel>>();
            models.ResultList.ForEach(data => ProcessContentViewModel(data));

            return models;
        }

        public OperationLogViewModel GetOperationLogById(int operationID)
        {
            BWOperationLog data = _bwOperationLogRep.Value.GetSingleByKey(InlodbType.Inlodb, new BWOperationLog { OperationID = operationID });
            var model = data.CastByJson<OperationLogViewModel>();
            ProcessContentViewModel(model);

            return model;
        }

        private void ProcessContentViewModel(OperationLogViewModel model)
        {
            model.ContentModel = model.Content;

            if (model.Content.IsValidJson()
                && model.OperationTypeSetting != null
                && model.OperationTypeSetting.OperationContentModelType != null
                && model.OperationTypeSetting.OperationContentModelType.IsClass)
            {
                model.ContentModel = model.Content.Deserialize(model.OperationTypeSetting.OperationContentModelType);
            }
        }
    }
}