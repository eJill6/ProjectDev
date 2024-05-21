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

    public interface ILoginRequestParam : IUserIdAndName, IMiseLogonUserParam
    {        
    }

    public interface IUserIdAndName
    {
        int UserId { get; set; }

        string UserName { get; set; }
    }
}