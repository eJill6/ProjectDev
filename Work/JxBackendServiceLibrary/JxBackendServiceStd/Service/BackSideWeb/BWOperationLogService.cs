using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
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

namespace JxBackendService.Service.BackSideWeb
{
    public class BWOperationLogService : BaseService, IBWOperationLogService, IBWOperationLogReadService
    {
        private readonly IBWOperationLogRep _bwOperationLogRep;

        public BWOperationLogService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _bwOperationLogRep = ResolveJxBackendService<IBWOperationLogRep>();
            
        }

        public BaseReturnModel CreateOperationLog(CreateBWOperationLogParam createParam)
        {
            IBackSideWebUserService _backSideWebUserService = DependencyUtil.ResolveService<IBackSideWebUserService>();
            BackSideWebUser backSideWebUser = _backSideWebUserService.GetUser();

            var operationLog = new BWOperationLog
            {
                PermissionKey = createParam.PermissionKey.Value,
                OperateUserName = backSideWebUser.UserName,
                UserID = createParam.UserID,
                ReferenceKey = createParam.ReferenceKey.ToTrimString(),
                Content = createParam.Content.ToTrimString(),
            };

            return _bwOperationLogRep.CreateByProcedure(operationLog).CastByJson<BaseReturnModel>();
        }

        public PagedResultModel<OperationLogViewModel> GetPagedBWOperationLogs(QueryBWOperationLogParam queryParam)
        {
            PagedResultModel<BWOperationLog> datas = _bwOperationLogRep.GetPagedOperationLog(queryParam);
            var models = datas.CastByJson<PagedResultModel<OperationLogViewModel>>();

            return models;
        }

        public OperationLogViewModel GetOperationLogById(int OperationID)
        {
            BWOperationLog datas = _bwOperationLogRep.GetOperationLogById(OperationID);
            var models = datas.CastByJson<OperationLogViewModel>();

            return models;
        }
    }
}