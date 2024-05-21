namespace JxBackendService.Model.Common.IMOne
{
    public interface IIMAppSetting : IIMOneAppSetting
    {
        string IMLoginUrl { get; }
    }

    public class IMAppSetting : BaseIMOneAppSetting, IIMAppSetting
    {
        public IMAppSetting()
        { }

        public override string ProductWallet => "401";

        public override string GameCode => "ESPORTSBULL";

        public string IMLoginUrl => Get("IMLoginUrl");
    }
}