using System;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Exceptions
{
    public class JobSuspendException : Exception
    {
        public JobSuspendException(string invokeMethodName) : base(MessageElement.MethodName + invokeMethodName)
        {

        }
    }
}
