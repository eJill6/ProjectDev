namespace JxBackendService.Model.Common.PM
{
    public class PMSLSharedAppSetting : BasePMAppSetting
    {
        private PMSLSharedAppSetting()
        {
            MerchantCodeConfigKey = "TPGame.PMSL.MerchantCode";
            ServiceUrlConfigKey = "TPGame.PMSL.ServiceBaseUrl";
            GrabServiceUrlConfigKey = "TPGame.PMSL.GrabServiceBaseUrl";
            SecretKeyConfigKey = "TPGame.PMSL.SecretKey";
            IVConfigKey = "TPGame.PMSL.IV";
        }

        public static PMSLSharedAppSetting Instance = new PMSLSharedAppSetting();

        public override int GameId => 66;

        public override string LaunchGameUrl => "launchGameById";
    }
}