namespace JxBackendService.Interface.Model.MiseLive.Request
{
    public interface IMiseLiveAppSettingService
    {
        string MSSealAddress { get; }

        string MSSealSalt { get; }
    }
}