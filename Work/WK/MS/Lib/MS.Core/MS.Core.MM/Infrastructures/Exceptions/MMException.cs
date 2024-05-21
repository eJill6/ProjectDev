using MS.Core.Infrastructures.Exceptions;
using MS.Core.Models;

namespace MS.Core.MM.Infrastructures.Exceptions
{
    public class MMException : MSException
    {
        public MMException(ReturnCode returnCode) : base(returnCode)
        {
        }

        public MMException(ReturnCode returnCode, string message) : base(returnCode, message)
        {
        }
    }
}
