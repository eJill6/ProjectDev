using System.Collections.Generic;

namespace Web.Models.Base
{
    public class CursorPagination<T>
    {
        public IEnumerable<T> Data { get; set; }

        public string NextCursor { get; set; }
    }
}
