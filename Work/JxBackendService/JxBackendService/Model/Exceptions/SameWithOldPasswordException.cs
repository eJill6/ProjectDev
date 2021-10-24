﻿using JxBackendService.Model.Attributes;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Exceptions
{    
    public class SameWithOldPasswordException : FlowControlException
    {
        public SameWithOldPasswordException() : base(MessageElement.OldPasswordSameAsNewPassword)
        {

        }
    }
}
