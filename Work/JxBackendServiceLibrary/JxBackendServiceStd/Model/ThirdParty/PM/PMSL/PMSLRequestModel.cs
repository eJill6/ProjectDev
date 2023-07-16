namespace JxBackendService.Model.ThirdParty.PM.PMSL
{
    public class PMSLBaseUserInfoRequestModel
    {
        public string MemberId { get; set; }

        public string MemberPwd { get; set; }

        public string MemberIp { get; set; }
    }

    public class PMSLBaseUserInfoWithNameRequestModel : PMSLBaseUserInfoRequestModel
    {
        public string MemberName { get; set; }
    }

    public class PMSLLunchGameRequestModel : PMSLBaseUserInfoWithNameRequestModel
    {
        /// <summary>
        /// (0:web,1:h5,2:ios,3:android)
        /// </summary>
        public int DeviceType { get; set; }

        public int GameId { get; set; }
    }

    public class PMSLTransferRequestModel : PMSLBaseUserInfoWithNameRequestModel
    {
        public int Money { get; set; }

        public string OrderId { get; set; }
    }

    public class PMSLCheckTransferRequestModel
    {
        public string OrderId { get; set; }
    }

    public class PMSLBetLogRequestModel
    {
        public int BeginTime { get; set; }

        public int EndTime { get; set; }

        public int PageNum { get; set; }

        public int PageSize { get; set; }
    }
}