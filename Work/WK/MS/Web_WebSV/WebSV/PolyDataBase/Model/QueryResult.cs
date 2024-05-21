using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SLPolyGame.Web.Model
{
    [DataContract(Name = "QueryResult{0}")]
    public class QueryResult<T>
    {
        public QueryResult()
        {
            this.Results = new List<T>();
        }
        [DataMember]
        public List<T> Results { get; set; }

        [DataMember]
        public int TotalCount { get; set; }
    }
}
