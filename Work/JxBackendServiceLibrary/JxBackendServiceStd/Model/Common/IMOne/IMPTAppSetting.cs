namespace JxBackendService.Model.Common.IMOne
{
    public class IMPTAppSetting : BaseIMOneAppSetting
    {
        public IMPTAppSetting() { }

        public override string ProductWallet => "102";

        public override string GameCode => null; //因為會有很多小遊戲,所以由外部指定
    }
}
