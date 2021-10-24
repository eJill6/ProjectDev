namespace JxBackendService.Model.Common
{
    public abstract class IMLotterySharedAppSettings : SharedAppSettings
    {
        /* API List */
        public readonly string CreateAccountUrl = "Player/Register";
        public readonly string CheckAccountExistUrl = "Player/CheckExists";
        public readonly string TransferUrl = "Transaction/PerformTransfer";
        public readonly string CheckTransferUrl = "Transaction/CheckTransferStatus";
        public readonly string GetBalanceUrl = "Player/GetBalance";
        public readonly string GetBetLogUrl = "Report/GetBetLog";
        public readonly string LaunchGameUrl = "Game/NewLaunchGame";
        public readonly string LaunchMobileGameUrl = "Game/NewLaunchMobileGame";

        public abstract string Enabled { get; }
        public abstract string MerchantCode { get; }
        public abstract string ServiceUrl { get; }
        public abstract string Language { get; }
        public abstract string Currency { get; }
        public abstract string ProductWallet { get; }
        public abstract string GameCode { get; }

    }

    public class IMVRAppSettings : IMLotterySharedAppSettings
    {
        private IMVRAppSettings() { }

        public static IMVRAppSettings Instance = new IMVRAppSettings();

        public override string Enabled => Get("TPGame.IMRG.Enabled");
        public override string MerchantCode => Get("TPGame.IMVR.MerchantCode");
        public override string ServiceUrl => Get("TPGame.IMVR.ServiceBaseUrl");
        public override string Language => Get("TPGame.IMVR.Language");
        public override string Currency => Get("TPGame.IMVR.Currency");
        public override string ProductWallet => "503";
        public override string GameCode => "imlotto20001";
    }

    public class IMSGAppSettings : IMLotterySharedAppSettings
    {
        private IMSGAppSettings() { }

        public static IMSGAppSettings Instance = new IMSGAppSettings();

        public override string Enabled => Get("TPGame.IMSG.Enabled");
        public override string MerchantCode => Get("TPGame.IMSG.MerchantCode");
        public override string ServiceUrl => Get("TPGame.IMSG.ServiceBaseUrl");
        public override string Language => Get("TPGame.IMSG.Language");
        public override string Currency => Get("TPGame.IMSG.Currency");
        public override string ProductWallet => "504";
        public override string GameCode => "imlotto30001";
    }
}
