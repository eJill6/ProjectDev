using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Interface.Model.Attribute
{
    public interface IRequiredIfAttribute
    {
        string OtherPropertyName { get; set; }

        object[] OtherPropertyValidValues { get; set; }
    }
}