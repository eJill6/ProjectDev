using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Param.Authenticator
{
    public class CreateQrCodeImageParam : JxSystemEnvironment
    {
        public string AccountTitleNoSpaces { get; set; }

        public string AccountSecretKey { get; set; }
    }
}
