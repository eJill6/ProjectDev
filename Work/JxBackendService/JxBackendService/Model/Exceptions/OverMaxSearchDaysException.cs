using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Exceptions
{
    public class OverMaxSearchDaysException : FlowControlException
    {
        public OverMaxSearchDaysException(int maxSearchDays) : base(string.Format(MessageElement.MaxSearchDaysIsArg,maxSearchDays))
        {

        }
    }
}
