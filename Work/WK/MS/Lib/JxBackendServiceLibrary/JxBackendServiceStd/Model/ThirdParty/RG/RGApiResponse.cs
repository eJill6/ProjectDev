namespace JxBackendService.Model.ThirdParty.RG
{
    public class BaseRGApiResponse
    {
        public string Code { get; set; }

        public string ErrorLog
        {
            get
            {
                if (IsSuccess)
                {
                    return null;
                }

                return $"Error Code = {Code}";
            }
        }

        public bool IsSuccess => Code == "ok";

        public bool IsNotFound => Code == "notfound";
    }

    public class RGTransferMoneyApiResult : BaseRGApiResponse
    {
        public string Trade_no { get; set; }

        public decimal Balance { get; set; }

        public decimal Before_balance { get; set; }
    }

    public class RGBalanceApiResult : BaseRGApiResponse
    {
        public decimal Balance { get; set; }
    }
}
