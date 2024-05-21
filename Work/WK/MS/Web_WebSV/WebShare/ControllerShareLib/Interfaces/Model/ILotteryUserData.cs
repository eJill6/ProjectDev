using JxBackendService.Interface.Model.User;

namespace ControllerShareLib.Interfaces.Model
{
    public interface ILotteryUserData : ITicketUserData
    {
        decimal RebatePro { get; set; }

        decimal AddedRebatePro { get; set; }
    }
}