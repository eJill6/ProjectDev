using JxBackendService.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.Entity.BackSideUser
{
    public class BWUserAuthenticator
    {
        [ExplicitKey]
        public int UserID { get; set; }

        public string AuthenticatorType { get; set; }

        public string EncryptAccountSecretKey { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdateUser { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
