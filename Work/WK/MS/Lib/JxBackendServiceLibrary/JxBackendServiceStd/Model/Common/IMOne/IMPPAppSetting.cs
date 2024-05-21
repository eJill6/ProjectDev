namespace JxBackendService.Model.Common.IMOne
{
    public class IMPPAppSetting : BaseIMOneAppSetting
    {
        public IMPPAppSetting() { }

        public override string ProductWallet => "101";

        public override string GameCode => null; //因為會有很多小遊戲,所以由外部指定
    }
}
