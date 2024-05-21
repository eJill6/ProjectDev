using BackSideWeb.Model.Param.PublishRecord;
using JxBackendService.Model.Entity.PublishRecord;
using JxBackendService.Model.Paging;

namespace BackSideWeb.Interface.Service
{
    public interface IPublishRecordService
    {
        PagedResultModel<MMPostCommentViewModel> GetPagedPublishRecord(PublishRecordParam queryParam);
    }
}
