using System.ComponentModel;

namespace SLPolyGame.Web.ErrorCodeMapping.User
{
    /// <summary>
    /// Define for internal use to increase the readability of error description and unify error codes.
    /// Todo : 1. Consider how to migrate it to web project.
    /// </summary>
    public enum UserProfileErrorCode
    {
        [Description("Data Table not found")]
        DtNotFound = -4,
        [Description("Data Column not found")]
        ColNotFound = -3,
        [Description("Member not found")]
        MemberNotFound = -2,
        [Description("Duplicate Nickname")]
        DuplicatNickname = -1,
        [Description("Success")]
        Success = 0,
        [Description("System Error")]
        SystemError = 1000
    }
}