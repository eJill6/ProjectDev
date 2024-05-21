namespace JxBackendService.Interface.Model.User
{
    public interface IMiseLogonUserParam
    {
        string RoomNo { get; set; }

        string GameID { get; set; }

        string DepositUrl { get; set; }

        int LogonMode { get; set; }
    }

    public interface ITicketUserData : IMiseLogonUserParam
    {
        int UserId { get; set; }

        string Key { get; set; }
    }

    public interface ILotteryUserData : ITicketUserData
    {
        decimal RebatePro { get; set; }

        decimal AddedRebatePro { get; set; }
    }
}