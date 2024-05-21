using JxBackendService.Model.ViewModel;

namespace JxBackendService.Interface.Service.GlobalSystem
{
    public interface ISendTelegramMessageService
    {
        void SendToCustomerService(BasicUserInfo user, string message);
    }
}