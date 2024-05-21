using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Attributes.Validate
{
    public class RequiredAttribute : BaseAttribute
    {
        public override string ValidateError
        {
            get { 
                if(base.ValidateError!=null)
                {
                    return base.ValidateError;
                }
                return "该值不能为空";
            }
            set=>base.ValidateError = value;
        }
        public override bool Validate(object oValue)
        {

            return !(oValue == null);
        }
    }
}
