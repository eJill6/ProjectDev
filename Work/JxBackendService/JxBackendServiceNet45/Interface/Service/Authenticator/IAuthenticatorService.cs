using Google.Authenticator;
using JxBackendService.Model.Enums;
using JxBackendServiceNet45.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendServiceNet45.Interface.Service.Authenticator
{
    public interface IAuthenticatorService
    {
        AuthenticatorType AuthenticatorType { get; }

        SetupCode GetSetupCode(CreateQrCodeImageParam createQrCodeImageParam);
        
        bool IsPinValid(string accountSecretKey, string clientPin, bool isCompareExactly);
    }
}
