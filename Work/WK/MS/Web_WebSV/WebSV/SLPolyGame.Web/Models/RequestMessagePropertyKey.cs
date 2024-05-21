using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLPolyGame.Web.Models
{
    public class RequestMessagePropertyKey : BaseStringValueModel<RequestMessagePropertyKey>
    {
        private RequestMessagePropertyKey()
        { }

        public static RequestMessagePropertyKey EnvironmentUser = new RequestMessagePropertyKey() { Value = "EnvironmentUser" };
    }
}