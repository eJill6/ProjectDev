﻿using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Models.Filters
{
    public class MyOrderPageBookingFilter:MyOrderBookingFilter
    {
        public PaginationModel Pagination { get; set; }
    }
}