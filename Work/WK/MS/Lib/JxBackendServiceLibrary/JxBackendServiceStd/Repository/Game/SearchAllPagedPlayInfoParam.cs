using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;

namespace JxBackendService.Repository.Game
{
    public class SearchAllPagedPlayInfoParam
    {
        public string GameCode { get; set; }

        public List<PlatformProduct> PlatformProducts { get; set; }

        //public int UserID { get; set; }

        public int PageSize { get; set; }

        public DateTime QueryStartDate { get; set; }

        public DateTime QueryEndDate { get; set; }

        public int PageNo { get; set; }
    }
}