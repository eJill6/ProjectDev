using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendServiceNet45.Model.Param
{
    public class SaveFindPasswordHashParam
    {
        public int PasswordType { get; set; }

        public string UserName { get; set; }

        public string NewPasswordHash { get; set; }

        public string VerifyCode { get; set; }
    }
}
