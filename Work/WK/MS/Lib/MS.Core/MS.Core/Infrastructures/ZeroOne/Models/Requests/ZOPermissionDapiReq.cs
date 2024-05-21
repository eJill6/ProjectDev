namespace MS.Core.Infrastructures.ZeroOne.Models.Requests
{
    public class ZOPermissionDapiReq
    {
        public ZOPermissionDapiReq(VipPermission permission, int userId)
        {
            Permission = permission;
            UserId = userId;
        }

        public VipPermission Permission { get; set; }
        public int UserId { get; set; }
    }
}
