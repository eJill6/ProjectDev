using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLPolyGame.Web.Model;
using System.ServiceModel;
using SLPolyGame.Web.Common;
using Autofac.Integration.Wcf;
using Autofac;
using SLPolyGame.Web.Helpers;
using JxBackendService.DependencyInjection;

namespace PolyDataBase.Helpers
{
    public class MessageContextHelper
    {
        public static UserInfoToken GetUserInfoToken()
        {
            var messageHeadersElement = OperationContext.Current.IncomingMessageHeaders;
            var key = messageHeadersElement.GetHeader<string>("p2", string.Empty).Trim();
            var cacheUserInfo = DependencyUtil.ResolveService<ICacheBase>().GetCache(CacheKeyHelper.GetUserTokenKey(key));

            return JsonUtil.Deserialize<UserInfoToken>(cacheUserInfo);
        }
    }
}