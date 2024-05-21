using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Attributes.Validate
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =true)]
    public abstract class BaseAttribute:Attribute
    {
        public virtual string ValidateError { get; set; }

        public abstract bool Validate(object oValue);
    }
}
