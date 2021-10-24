namespace JxBackendService.Interface.Service.Security
{
    public interface IBlockActionService
    {
        bool BlockWebActionClient(string ipAddress, int webActionTypeValue);
    }
}
