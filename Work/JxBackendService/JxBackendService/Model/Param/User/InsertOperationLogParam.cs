using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.User
{
    public class BaseInsertOperationLogParam
    {
        public string Content { get; set; }
    }

    public class InsertFrontSideOperationLogParam : BaseInsertOperationLogParam
    {
        public int AffectedUserId { get; set; }

        public string AffectedUserName { get; set; }
    }

    public class InsertModifyMemberOperationLogParam : InsertFrontSideOperationLogParam
    {
        public JxOperationLogCategory Category { get; set; }
    }

    public class InsertModifySystemOperationLogParam : BaseInsertOperationLogParam
    {
        public JxOperationLogCategory Category { get; set; }
    }
}
