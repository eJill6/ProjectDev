namespace JxBackendService.Model.ThirdParty.OB.OBEB
{
    public class CreateUserModel
    {
        public string create { get; set; }
    }

    public class OBEBCreateUserResponseModel : OBEBBaseResponseWtihDataModel<CreateUserModel>
    {
    }

    public class ForwardGameModel
    {
        public string url { get; set; }
    }

    public class OBEBForwardGameResponseModel : OBEBBaseResponseWtihDataModel<ForwardGameModel>
    {
    }

    public class TansferModel
    {
        public string deposit { get; set; }

        public decimal balance { get; set; }
    }

    public class OBEBTransferResponseModel : OBEBBaseResponseWtihDataModel<TansferModel>
    {
    }

    public class CheckTransferModel
    {
        public string tradeNo { get; set; }

        public decimal amount { get; set; }

        public int transferStatus { get; set; }

        public string remark { get; set; }
    }

    public class OBEBCheckTransferResponseModel : OBEBBaseResponseWtihDataModel<CheckTransferModel>
    {
    }

    public class BalanceModel
    {
        public string balance { get; set; }
    }

    public class OBEBBalanceResponseModel : OBEBBaseResponseWtihDataModel<BalanceModel>
    {
    }

    public class OBEBBetLogResponseModel : OBEBBaseResponseWtihDataModel<OBEBBetLog>
    {
    }
}