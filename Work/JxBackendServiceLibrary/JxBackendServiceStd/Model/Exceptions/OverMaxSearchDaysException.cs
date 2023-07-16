using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Exceptions
{
    public class OverMaxSearchDaysException : FlowControlException
    {
        public OverMaxSearchDaysException(int maxSearchDays) : base(string.Format(MessageElement.MaxSearchDaysIsArg, maxSearchDays))
        {

        }

        public OverMaxSearchDaysException(string message) : base(message)
        {

        }
    }
}
