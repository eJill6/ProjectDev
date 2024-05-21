using JxBackendService.Interface.Model.User;

namespace JxBackendService.Model.ViewModel.Web
{
    public class TicketUserData : ITicketUserData
    {
        public int UserId { get; set; }

        public string Key { get; set; }

        public string RoomNo { get; set; }

        public string GameID { get; set; }

        public string DepositUrl { get; set; }

        public int LogonMode { get; set; }
    }

    public class CachedUserInfoToken : TicketUserData, IUserIdAndName
    {
        public string UserName { get; set; }
    }
}