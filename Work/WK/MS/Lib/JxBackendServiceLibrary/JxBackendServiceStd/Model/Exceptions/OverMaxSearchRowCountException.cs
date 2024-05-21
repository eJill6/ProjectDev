using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Exceptions
{
    public class OverMaxSearchRowCountException : FlowControlException
    {
        public OverMaxSearchRowCountException() : base(SelectItemElement.DownloadFileStatusOverMaxRowCount)
        {

        }
    }
}
