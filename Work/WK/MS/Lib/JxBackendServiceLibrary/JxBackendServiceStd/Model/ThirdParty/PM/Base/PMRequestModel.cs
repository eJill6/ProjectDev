namespace JxBackendService.Model.ThirdParty.PM.Base
{
    public class PMBaseUserInfoRequestModel
    {
        public string MemberId { get; set; }

        public string MemberPwd { get; set; }

        public string MemberIp { get; set; }
    }

    public class PMBaseUserInfoWithNameRequestModel : PMBaseUserInfoRequestModel
    {
        public string MemberName { get; set; }
    }

    public class PMLunchGameRequestModel : PMBaseUserInfoWithNameRequestModel
    {
        /// <summary>
        /// (0:web,1:h5,2:ios,3:android)
        /// </summary>
        public int DeviceType { get; set; }

        public int GameId { get; set; }
    }

    public class PMTransferRequestModel : PMBaseUserInfoWithNameRequestModel
    {
        public int Money { get; set; }

        public string OrderId { get; set; }
    }

    public class PMCheckTransferRequestModel
    {
        public string OrderId { get; set; }
    }

    public class PMBetLogRequestModel
    {
        public int BeginTime { get; set; }

        public int EndTime { get; set; }

        public int PageNum { get; set; }

        public int PageSize { get; set; }
    }
}