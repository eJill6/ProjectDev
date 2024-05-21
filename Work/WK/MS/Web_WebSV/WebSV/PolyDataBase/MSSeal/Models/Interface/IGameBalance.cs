namespace SLPolyGame.Web.MSSeal.Models.Interface
{
    public interface IGameBalance : IGameIdInfo
    {
        decimal Balance { get; set; }

        decimal FreezeBalance { get; set; }
    }
}