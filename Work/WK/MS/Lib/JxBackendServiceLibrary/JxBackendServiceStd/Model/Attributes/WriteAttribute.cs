using System;

namespace JxBackendService.Model.Attributes
{
    public class WriteAttribute : Attribute
    {
        public WriteAttribute(bool write)
        {
            Write = write;
        }

        public bool Write { get; }
    }
}
