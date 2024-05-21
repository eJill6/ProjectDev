using MS.Core.Models;

namespace MS.Core.Infrastructures.Exceptions
{
    public class ZeroOneException : MSException
    {
        public ZeroOneException(ReturnCode returnCode) : base(returnCode)
        {
        }

        public ZeroOneException(ReturnCode returnCode, string message) : base(returnCode, message)
        {
        }
    }
}
