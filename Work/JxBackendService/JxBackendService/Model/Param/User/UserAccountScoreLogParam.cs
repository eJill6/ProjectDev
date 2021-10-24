using System;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Model.Param.User
{
    public class BackendUserScoreSearchParam: BaseUserScoreSearchParam
    {
        public string QueryScoreKey { get; set; }
    }

    public class UserScoreSearchParam : BaseUserScoreSearchParam
    {
        public string GroupName { get; set; }
        public int ItemKey { get; set; }
    }

    public class BaseScoreSearchParam
    {
        public int PageSize { get; set; }
        public int PageNum { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BaseUserScoreSearchParam : BaseScoreSearchParam
    {
        public int UserID { get; set; }
    }
}
