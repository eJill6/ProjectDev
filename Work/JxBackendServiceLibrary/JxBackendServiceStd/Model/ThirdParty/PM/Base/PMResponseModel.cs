using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.PM.Base
{
    public class PMLunchGameResponseModel : PMBaseResponseWtihDataModel<string>
    {
    }

    public class PMTransferModel
    {
        public int TransferIn { get; set; }

        public int TransferOut { get; set; }

        public int Balance { get; set; }
    }

    public class PMTransferResponseModel : PMBaseResponseWtihDataModel<PMTransferModel>
    {
    }

    public class PMQueryOrderModel
    {
        public int Status { get; set; }

        public int? Money { get; set; }

        public string OrderId { get; set; }

        public string Type { get; set; }
    }

    public class PMQueryOrderResponseModel : PMBaseResponseWtihDataModel<PMQueryOrderModel>
    {
    }

    public class PMBalanceModel
    {
        public decimal Balance { get; set; }
    }

    public class PMBalanceResponseModel : PMBaseResponseWtihDataModel<PMBalanceModel>
    { }

    public class PMBetLogModel
    {
        public List<PMBetLog> List { get; set; }
    }

    public class PMBetLogResponseModel : PMBaseResponseWtihDataModel<PMBetLogModel>
    {
        public int PageNum { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }
    }
}