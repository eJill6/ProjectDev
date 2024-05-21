namespace JxBackendService.Model.Param.Authenticator
{
    public class ValidVerifyCodeParam
    {
        public int UserId { get; set; }

        public string VerifyCode { get; set; }

        public bool IsCompareExactly { get; set; } = true;
    }
}
