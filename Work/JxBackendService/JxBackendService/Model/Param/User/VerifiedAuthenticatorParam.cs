using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.User
{
    public class VerifiedAuthenticatorParam
    {
        public BaseBasicUserInfo User { get; set; }
        
        [Required]
        public string MoneyPasswordHash { get; set; }
        
        [Required]
        [StringLength(6)]
        public string Pin { get; set; }

        public bool IsVerified { get; set; }
    }
}