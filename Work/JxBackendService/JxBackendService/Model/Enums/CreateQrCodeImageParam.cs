using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class CreateQrCodeImageParam : JxSystemEnvironment
    {
        public string AccountTitleNoSpaces { get; set; }
        public string AccountSecretKey { get; set; }
    }
}
