using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Security
{
    public class SignInToken
    {
        public string UserKey { get; set; }

        public DateTime ExpiredDate { get; set; }
    }
}
