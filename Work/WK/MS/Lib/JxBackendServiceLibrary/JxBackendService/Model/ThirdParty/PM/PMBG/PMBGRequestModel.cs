namespace JxBackendService.Model.ThirdParty.PM.PMBG
{
    public class PMBGBaseUserInfoRequestModel
    {
        public string MemberId { get; set; }

        public string MemberPwd { get; set; }

        public string MemberIp { get; set; }
    }

    public class PMBGBaseUserInfoWithNameRequestModel : PMBGBaseUserInfoRequestModel
    {
        public string MemberName { get; set; }
    }

    public class PMBGLunchGameRequestModel : PMBGBaseUserInfoWithNameRequestModel
    {
        /// <summary>
        /// (0:web,1:h5,2:ios,3:android)
        /// </summary>
        public int DeviceType { get; set; }

        public int GameId { get; set; }
    }

    public class PMBGTransferRequestModel : PMBGBaseUserInfoWithNameRequestModel
    {
        public int Money { get; set; }

        public string OrderId { get; set; }
    }

    public class PMBGCheckTransferRequestModel
    {
        public string OrderId { get; set; }
    }

    public class PMBGBetLogRequestModel
    {
        public int BeginTime { get; set; }

        public int EndTime { get; set; }

        public int PageNum { get; set; }

        public int PageSize { get; set; }
    }
}