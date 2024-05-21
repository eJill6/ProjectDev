namespace MS.Core.Infrastructures.ZeroOne.Models.Requests
{
    public class ZOUserInfoReq
    {
        public ZOUserInfoReq(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
