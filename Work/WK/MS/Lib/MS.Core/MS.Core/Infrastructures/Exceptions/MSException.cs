using MS.Core.Models;

namespace MS.Core.Infrastructures.Exceptions
{
    public class MSException : Exception
    {
        public string Code { get; set; }

        public new string? Message { get; set; }

        public ReturnCode ReturnCode => ReturnCode.GetDefault(Code);

        public MSException(ReturnCode returnCode)
        {
            Code = returnCode.Code;
        }

        public MSException(ReturnCode returnCode, string message)
        {
            Code = returnCode.Code;
            Message = message;
        }
    }
}
