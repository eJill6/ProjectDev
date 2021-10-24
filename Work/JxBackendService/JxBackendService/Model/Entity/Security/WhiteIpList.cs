using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Entity.Security
{
    public class WhiteIpList
    {
        public string Ip { get; set; }

        public string Remark { get; set; }

        public bool IsWork { get; set; }

        [Key]
        public int ID { get; set; }

        public int IType { get; set; }
    }
}