using JxBackendService.Model.ViewModel;

namespace ControllerShareLib.Interfaces.Service.Security
{
    public interface IHeaderInspectorService
    {
        Dictionary<string, string> CreateRequestHeader(BasicUserInfo basicUserInfo);
    }
}