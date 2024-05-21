using System;

namespace JxBackendService.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SingleInstanceAttribute : Attribute
    {
    }
}