using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ViewModel
{
    public class ExceptionInfoViewModel
    {
        public int code { get; set; }

        public string message { get; set; }

        public string details { get; set; }

        public bool IsLoginExpired { get; set; }
    }
}