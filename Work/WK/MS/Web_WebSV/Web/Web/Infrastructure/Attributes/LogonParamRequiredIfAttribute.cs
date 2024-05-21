using JxBackendService.Model.Attributes;
using JxBackendService.Model.Param.Client;
using JxBackendServiceNF.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Web.Models.Account;

namespace Web.Infrastructure.Attributes
{
    public class LogonParamRequiredIfAttribute : RequiredIfNFAttribute
    {
        public LogonParamRequiredIfAttribute(string[] clientWebPageNames)
        {
            OtherPropertyName = nameof(LogonParam.ClientWebPageValue);
            OtherPropertyValidValues = ConvertToValues(clientWebPageNames);
        }

        private object[] ConvertToValues(string[] clientWebPageNames)
        {
            var values = new List<object>();

            foreach (string clientWebPageName in clientWebPageNames)
            {
                FieldInfo field = typeof(ClientWebPage).GetField(clientWebPageName);

                if (field == null)
                {
                    continue;
                }

                values.Add((field.GetValue(field) as ClientWebPage).Value);
            }

            return values.ToArray();
        }
    }
}