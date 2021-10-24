using JxBackendService.Model.Entity.User.Authenticator;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Permission
{
    public class UserAuthenticatorInfo
    {
        public UserAuthenticator UserAuthenticator { get; set; }
        public UserAuthenticatorStatuses UserAuthenticatorStatus { get; set; }
    }
}
