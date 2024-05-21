using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.PM.PMBG
{
    public class PMBGLunchGameResponseModel : PMBGBaseResponseWtihDataModel<string>
    {
    }

    public class PMBGTransferModel
    {
        public int TransferIn { get; set; }

        public int TransferOut { get; set; }

        public int Balance { get; set; }
    }

    public class PMBGTransferResponseModel : PMBGBaseResponseWtihDataModel<PMBGTransferModel>
    {
    }

    public class PMBGQueryOrderModel
    {
        public int Status { get; set; }

        public int? Money { get; set; }

        public string OrderId { get; set; }

        public string Type { get; set; }
    }

    public class PMBGQueryOrderResponseModel : PMBGBaseResponseWtihDataModel<PMBGQueryOrderModel>
    {
    }

    public class PMBGBalanceModel
    {
        public decimal Balance { get; set; }
    }

    public class PMBGBalanceResponseModel : PMBGBaseResponseWtihDataModel<PMBGBalanceModel>
    { }

    public class PMBGBetLogModel
    {
        public List<PMBGBetLog> List { get; set; }
    }

    public class PMBGBetLogResponseModel : PMBGBaseResponseWtihDataModel<PMBGBetLogModel>
    {
        public int PageNum { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }
    }
}