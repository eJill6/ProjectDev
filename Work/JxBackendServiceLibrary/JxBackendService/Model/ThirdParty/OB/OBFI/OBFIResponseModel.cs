using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.OB.OBFI
{

    public class OBFILunchGameResponseModel : OBFIBaseResponseWtihDataModel<string>
    {
    }

    public class OBFITransferModel
    {
        public int transferIn { get; set; }
        public int transferOut { get; set; }
        public int balance { get; set; }
    }

    public class OBFITransferResponseModel : OBFIBaseResponseWtihDataModel<OBFITransferModel>
    {
    }

    public class OBFIQueryOrderModel
    {
        public int status { get; set; }
        public int? money { get; set; }
        public string orderId { get; set; }
        public string type { get; set; }
    }

    public class OBFIQueryOrderResponseModel : OBFIBaseResponseWtihDataModel<OBFIQueryOrderModel>
    {
    }

    public class OBFIBalanceModel
    {
        public decimal balance { get; set; }
    }

    public class OBFIBalanceResponseModel : OBFIBaseResponseWtihDataModel<OBFIBalanceModel> { }

    public class OBBetLogModel
    {
        public List<OBFIBetLog> list { get; set; }
    }

    public class OBFIBetLogResponseModel : OBFIBaseResponseWtihDataModel<OBBetLogModel>
    {
        public int pageNum { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }
    }

}
