using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;

namespace JxBackendService.Service.Enums.MSL
{
    public class BasePlatformProductSettingMSLService : IPlatformProductSettingService
    {
        public virtual bool IsParseUserIdFromSuffix => true;

        public virtual OpenGameMode OpenMode => OpenGameMode.Redirect;

        public virtual LinkTarget MenuTarget => LinkTarget.Blank;
    }

    public class PlatformProductAGSettingMSLService : BasePlatformProductSettingMSLService, IPlatformProductAGSettingService
    {
        public bool IsDeleteRemoteFileAfterSave => true;

        public string GetRemotePath(AGGameType agGameType, string dateTimeRemotePath)
        {
            return $"{agGameType.Value}XMLFileDir/";
        }

        public string GetRemoteLostAndFoundPath(AGGameType agGameType, string dateTimeRemotePath)
        {
            return $"{agGameType.Value}LostAndFoundXMLFileDir/";
        }
    }

    public class PlatformProductJDBFISettingMSLService : BasePlatformProductSettingMSLService
    {
        public override bool IsParseUserIdFromSuffix => false;
    }
}