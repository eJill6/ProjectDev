using JxBackendService.Model.Enums.ThirdParty;

namespace JxBackendService.Interface.Service.Enums.Product
{
    public interface IPlatformProductSettingService
    {
        bool IsParseUserIdFromSuffix { get; }

        OpenGameMode OpenMode { get; }

        LinkTarget MenuTarget { get; }
    }

    public interface IPlatformProductAGSettingService : IPlatformProductSettingService
    {
        string GetRemotePath(AGGameType agGameType, string dateTimeRemotePath);

        string GetRemoteLostAndFoundPath(AGGameType agGameType, string dateTimeRemotePath);

        bool IsDeleteRemoteFileAfterSave { get; }
    }
}