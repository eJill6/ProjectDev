using JxBackendService.Model.ViewModel;
using System.Collections.Generic;

namespace JxBackendService.Model.Param.User
{
    public class BackSideWebUser : BasicUserInfo
    {
        public string UserName { get; set; }

        public Dictionary<string, HashSet<int>> PermissionMap { get; set; } = new Dictionary<string, HashSet<int>>();
    }
}