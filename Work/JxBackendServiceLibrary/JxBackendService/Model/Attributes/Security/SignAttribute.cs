using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Attributes.Security
{
    public class SignAttribute : Attribute
    {
        public bool IsCamelCase { get; set; }

        public int SortNo { get; set; }

        public SignAttribute()
        {

        }

        public SignAttribute(bool isCamelCase, int sortNo)
        {
            IsCamelCase = isCamelCase;
            SortNo = sortNo;
        }
    }
}
