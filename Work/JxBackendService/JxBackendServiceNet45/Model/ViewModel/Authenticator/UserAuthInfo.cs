using System;
using JxBackendService.Model.ViewModel;

namespace JxBackendServiceNet45.Model.ViewModel.Authenticator
{
    public class UserAuthInfo : BaseBasicUserInfo
    {
        public bool IsVerified { get; set; }

        public QrCodeViewModel QrCodeViewModel { get; set; }
    }
}
