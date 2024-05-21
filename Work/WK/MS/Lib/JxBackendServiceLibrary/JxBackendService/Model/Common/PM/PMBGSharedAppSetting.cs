namespace JxBackendService.Model.Common.PM
{
    public class PMBGSharedAppSetting : BasePMAppSetting
    {
        private PMBGSharedAppSetting()
        {
            MerchantCodeConfigKey = "TPGame.PMBG.MerchantCode";
            ServiceUrlConfigKey = "TPGame.PMBG.ServiceBaseUrl";
            GrabServiceUrlConfigKey = "TPGame.PMBG.GrabServiceBaseUrl";
            SecretKeyConfigKey = "TPGame.PMBG.SecretKey";
            IVConfigKey = "TPGame.PMBG.IV";
        }

        public static PMBGSharedAppSetting Instance = new PMBGSharedAppSetting();

        public override int GameId => 101;

        public override string LaunchGameUrl => "launchGame";
    }
}