﻿using JxBackendService.Model.Attributes;

namespace JxBackendService.Model.ViewModel
{
    public class BaseBasicUserInfo
    {
        public int UserId { get; set; }
    }

    public class BasicUserInfo : BaseBasicUserInfo
    {
        [IgnoreRead]
        public string UserKey { get; set; }
    }

    public class BaseUserInfoToken : BaseBasicUserInfo
    {
        public string Key { get; set; }
    }
}