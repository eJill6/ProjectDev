namespace JxBackendService.Interface.Service.Chat
{
    public interface IIdGeneratorService
    {
        long CreateId();

        int GetOrCreateGeneratorId(string machineName);

        bool TryCreateId(out long id);
    }
}