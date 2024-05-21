using JxBackendService.Resource.Element;
using System;

namespace JxBackendService.Model.Exceptions
{
    public class JobSuspendException : Exception
    {
        public JobSuspendException(string invokeMethodName) : base(MessageElement.MethodName + invokeMethodName)
        {

        }
    }
}
