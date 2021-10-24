using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Exceptions
{
    public class OldPasswordNotValidException : FlowControlException
    {
        public OldPasswordNotValidException() : base(MessageElement.OldPasswordIsNotValid)
        {

        }
    }
}
