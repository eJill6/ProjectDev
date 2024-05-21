using ControllerShareLib.Interfaces.Model;
using JxBackendService.Interface.Model.User;

namespace ControllerShareLib.Models.Base
{
    public class TicketUserData : ITicketUserData
    {
        public string Key { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        public string RoomNo { get; set; }

        public string GameID { get; set; }

        public string DepositUrl { get; set; }

        public int LogonMode { get; set; }
    }

    public class LotteryUserData : TicketUserData, ILotteryUserData
    {
        public decimal RebatePro { get; set; }

        public decimal AddedRebatePro { get; set; }
    }
}