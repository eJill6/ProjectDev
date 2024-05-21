using MS.Core.MM.Attributes;
using System;
using System.ComponentModel;

namespace MS.Core.MMModel.Attributes
{
    public class ExpenseDescriptionAttribute : DescriptionAttributeBase
    {
        public ExpenseDescriptionAttribute(string description) : base(description)
        {
        }
    }
}
