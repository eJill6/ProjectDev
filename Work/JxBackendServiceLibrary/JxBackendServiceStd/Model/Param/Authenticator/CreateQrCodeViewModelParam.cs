namespace JxBackendService.Model.Param.Authenticator
{
    public class CreateQrCodeViewModelParam
    {
        public int UserId{ get; set; }

        public bool IsForcedRefresh { get; set; }
    }
}
