using BackSideWeb.Interface.Service;
using BackSideWeb.Model.Param.PublishRecord;
using JxBackendService.Model.Entity.PublishRecord;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace BackSideWeb.Service
{
    public class PublishRecordService : BaseService, IPublishRecordService
    {
        public PublishRecordService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public PagedResultModel<MMPostCommentViewModel> GetPagedPublishRecord(PublishRecordParam queryParam)
        {
            throw new NotImplementedException();
        }
    }
}
