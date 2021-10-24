using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Common.Util
{
    public static class AssignValueOnceUtil
    {
        public static T GetAssignValueOnce<T>(this T propertyValue, Func<T> getPropertyValueJob)
        {
            if(propertyValue == null)
            {
                return getPropertyValueJob.Invoke();
            }

            return propertyValue;
        }
    }
}
