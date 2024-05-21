using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Common;

namespace Web.Infrastructure
{
    public static class FilterContextHelper
    {
        public static BaseResponseData CreateUnauthorizedData()
        {
            var data = new BaseResponseData()
            {
                success = false,
                message = MessageElement.LoginIsExpired
            };

            return data;
        }

        public static BaseResponseData CreateTooFrequentlyData()
        {
            var data = new BaseResponseData()
            {
                success = false,
                message = MessageElement.TooFrequentlyToRest
            };

            return data;
        }
    }
}