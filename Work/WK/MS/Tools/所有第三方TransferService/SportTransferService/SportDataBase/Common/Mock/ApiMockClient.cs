using SportDataBase.Model;

namespace SportDataBase.Common.Mock
{
    public class ApiMockClient : ApiClient
    {
        public override ApiResult<FundTransferResult> CheckFundTransfer(string vendor_trans_id)
        {
            return new ApiResult<FundTransferResult>()
            {
                error_code = 3,
                Data = new FundTransferResult()
                {
                    trans_id = 1724,
                    before_amount = 2000000.0000m,
                    after_amount = 0.0000m,
                    system_id = null,
                    status = 2,
                    transfer_date = "2022-04-19T06:03:15.05",
                    amount = 1000000.00m,
                    currency = 20
                },
                message = "Please wait 1 minutes, and then to check again.",
            };
        }
    }
}
