using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Interface.Service.BackSideUser
{
    public interface IBWUserInfoService
    {
        BaseReturnDataModel<int> Create(CreateBWUserInfoParam createParam);

        BaseReturnModel Delete(int userId);

        EditUserInfo GetEditUserInfo(int userId);

        PagedResultModel<UserManagementViewModel> GetPagedBWUserInfos(QueryBWUserInfoParam queryParam);

        BaseReturnModel Update(UpdateBWUserInfoParam updateParam);
    }
}