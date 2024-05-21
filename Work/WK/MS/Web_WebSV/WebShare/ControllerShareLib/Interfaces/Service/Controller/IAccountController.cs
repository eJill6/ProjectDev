using ControllerShareLib.Models.Account;

namespace ControllerShareLib.Interfaces.Service.Controller
{
    public interface IAccountControllerService
    {
        LogonResult LogOn(ValidateLogonParam logonParam);
    }
}