using JxBackendService.Model.ViewModel;

namespace JxBackendService.Model.Param.Authenticator
{
    public class CreateQrCodeImageParam : JxSystemEnvironment
    {
        public string AccountTitleNoSpaces { get; set; }

        public string AccountSecretKey { get; set; }
    }
}
