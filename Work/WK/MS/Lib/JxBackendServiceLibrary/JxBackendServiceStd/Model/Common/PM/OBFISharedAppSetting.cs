namespace JxBackendService.Model.Common.PM
{
    public class OBFISharedAppSetting : BasePMAppSetting
    {
        public OBFISharedAppSetting()
        {
            MerchantCodeConfigKey = "TPGame.OBFI.MerchantCode";
            ServiceUrlConfigKey = "TPGame.OBFI.ServiceBaseUrl";
            GrabServiceUrlConfigKey = "TPGame.OBFI.GrabServiceBaseUrl";
            SecretKeyConfigKey = "TPGame.OBFI.SecretKey";
            IVConfigKey = "TPGame.OBFI.IV";
        }

        public static OBFISharedAppSetting Instance = new OBFISharedAppSetting();

        public override int GameId => 200;

        public override string LaunchGameUrl => "launchGameById";
    }
}