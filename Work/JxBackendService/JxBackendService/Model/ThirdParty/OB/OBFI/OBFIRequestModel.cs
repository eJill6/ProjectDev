namespace JxBackendService.Model.ThirdParty.OB.OBFI
{
    public class OBFIBaseUserInfoRequestModel
    {
        public string memberId { get; set; }
        public string memberPwd { get; set; }
        public string memberIp { get; set; }
    }

    public class OBFIBaseUserInfoWithNameRequestModel : OBFIBaseUserInfoRequestModel
    {
        public string memberName { get; set; }
    }

    public class OBFILunchGameRequestModel : OBFIBaseUserInfoWithNameRequestModel
    {
        /// <summary>
        /// (0:web,1:h5,2:ios,3:android)
        /// </summary>
        public int deviceType { get; set; }
        public int gameId { get; set; }
    }

    public class OBFITransferRequestModel : OBFIBaseUserInfoWithNameRequestModel
    {
        public int money { get; set; }
        public string orderId { get; set; }
    }

    public class OBFICheckTransferRequestModel
    {
        public string orderId { get; set; }
    }

    public class OBFIBetLogRequestModel
    {
        public int beginTime { get; set; }
        public int endTime { get; set; }
        public int pageNum { get; set; }
        public int pageSize { get; set; }
    }

}
