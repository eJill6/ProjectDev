using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class Pagination<T> where T : class
    {
        public Pagination()
        {
            Data = new List<T>();
        }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public List<T> Data { get; set; }
    }
}