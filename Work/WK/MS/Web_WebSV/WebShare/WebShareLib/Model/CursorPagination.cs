using System.Collections.Generic;

namespace SLPolyGame.Web.Model
{
    public class CursorPagination<T>
    {
        public IEnumerable<T> Data { get; set; }

        public string NextCursor { get; set; }
    }
}
