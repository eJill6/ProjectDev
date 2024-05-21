using JxBackendService.Common.Util;
using System;

namespace JxBackendService.Model.ViewModel.Authenticator
{
    public class QrCodeViewModel
    {
        public int UserID { get; set; }

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
}
