using System;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.ViewModel
{
    public class SaveUserAuthInfo
    {
        public bool IsVerified { get; set; }

        [Required, StringLength(16)]
        public string MoneyPassword { get; set; }

        [Required, StringLength(16)]
        public string GooglePassword { get; set; }
    }
}
