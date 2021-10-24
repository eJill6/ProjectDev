using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Exceptions
{
    public class OverMaxSearchRowCountException : FlowControlException
    {
        public OverMaxSearchRowCountException() : base(SelectItemElement.DownloadFileStatusOverMaxRowCount)
        {

        }
    }
}
