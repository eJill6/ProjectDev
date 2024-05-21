using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.PM.PMSL
{
    public class PMSLLunchGameResponseModel : PMSLBaseResponseWtihDataModel<string>
    {
    }

    public class PMSLTransferModel
    {
        public int TransferIn { get; set; }
        public int TransferOut { get; set; }
        public int Balance { get; set; }
    }

    public class PMSLTransferResponseModel : PMSLBaseResponseWtihDataModel<PMSLTransferModel>
    {
    }

    public class PMSLQueryOrderModel
    {
        public int Status { get; set; }
        public int? Money { get; set; }
        public string OrderId { get; set; }
        public string Type { get; set; }
    }

    public class PMSLQueryOrderResponseModel : PMSLBaseResponseWtihDataModel<PMSLQueryOrderModel>
    {
    }

    public class PMSLBalanceModel
    {
        public decimal Balance { get; set; }
    }

    public class PMSLBalanceResponseModel : PMSLBaseResponseWtihDataModel<PMSLBalanceModel>
    { }

    public class PMSLBetLogModel
    {
        public List<PMSLBetLog> List { get; set; }
    }

    public class PMSLBetLogResponseModel : PMSLBaseResponseWtihDataModel<PMSLBetLogModel>
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
}