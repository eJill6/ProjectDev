using MS.Core.MMModel.Models.My.Enums;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.Core.MMModel.Models.My
{
    public class MyOfficialPostQueryParamForClient: PageParam
    {
        public MyBossPostStatus PostStatus { get; set; }
        public MyPostSortType SortType { get; set; }
    }
}
