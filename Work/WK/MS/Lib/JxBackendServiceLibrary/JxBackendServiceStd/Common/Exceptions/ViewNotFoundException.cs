using System;

namespace JxBackendService.Common.Exceptions
{
    public class ViewNotFoundException : Exception
    {
        public ViewNotFoundException(string view) : base($"{view} not found.")
        {
        }
    }
}
