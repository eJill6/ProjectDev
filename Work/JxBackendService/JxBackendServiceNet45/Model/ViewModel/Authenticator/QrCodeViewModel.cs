using JxBackendService.Common.Util;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendServiceNet45.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendServiceNet45.Model.ViewModel.Authenticator
{
    public class QrCodeViewModel
    {
        public string DisplayManualEntryKey { get; set; }

        public string ImageUrl { get; set; }
        
        public DateTime UpdateDate { get; set; }
        
        public string UpdateDateText
        {
            get
            {
                return UpdateDate.ToFormatDateTimeString();
            }
            set { }
        }
    }

    public class CreateQrCodeViewModelParam : JxSystemEnvironment
    {
        public BaseBasicUserInfo SearchUser { get; set; }

        public AuthenticatorType CreateQrCodeWithType { get; set; }

        public bool IsForcedRefresh { get; set; }
    }
}
