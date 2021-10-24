
namespace JxBackendService.Model.ViewModel.Merchant
{
    public class MerchantSettingModel
    {
        public bool IsRWD { get; set; }
    }

    public class RegisterSettingInfo
    {
        public bool IsUseRebate { get; set; }

        public decimal DefaultRebatePro { get; set; }

        public decimal MaxRebatePro { get; set; }

        public decimal? UpgradRebatePro { get; set; }

    }
}
