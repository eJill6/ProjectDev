using MS.Core.Models;

namespace MS.Core.Infrastructures.Exceptions
{
    public class MSSqlException : MSException
    {
        public MSSqlException(ReturnCode returnCode) : base(returnCode)
        {
        }
        public MSSqlException(ReturnCode returnCode, string message) : base(returnCode, message)
        {
        }
    }
}
