namespace MS.Core.MM.Models.Post.ServiceReq
{
    public class ReqOfficialPostData : OfficialPostData
    {
        public string PostId { get; set; } = string.Empty;

        public string? Nickname { get; set; }
    }
}