using JxBackendService.Model.ViewModel;

namespace JxBackendService.Interface.Service.Web
{
    public interface IHttpContextUserService
    {
        string GetUserKey();

        int GetUserId();

        BasicUserInfo GetBasicUserInfo();
    }
}