namespace JxBackendService.Model.ThirdParty.IM
{
    public class IMBaseRequestModel
    {
        public string MerchantCode { get; set; }

        public string ProductWallet { get; set; }
    }

    public class IMBaseResponseModel
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }
}