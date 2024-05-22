namespace FakeMSSeal.Models.ZeroOne
{
    public class ZOVipPermissionReq
    {
        public VipPermission Permission { get; set; }
        public int UserId { get; set; }
        public long Ts { get; set; } = 0;
    }
}
