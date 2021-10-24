using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Security
{
    public class TicketUserData : BasicUserInfo
    {
        /// <summary>
        /// 是否使用無外框layout給webview串接
        /// </summary>
        public bool IsUsePartialView { get; set; }
    }
}
