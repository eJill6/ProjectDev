using Autofac;
using Autofac.Integration.Wcf;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ServiceModel;
using JxBackendServiceNF.Model.ServiceModel;
using Microsoft.Extensions.Logging;
using SLPolyGame.Web.Common;
using SLPolyGame.Web.Helpers;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SLPolyGame.Web
{
    public class DispatchMessageInspector : CommonDispatchMessageInspector, IDispatchMessageInspector
    {
        private static readonly JxApplication s_application = JxApplication.FrontSideWeb;

        public static string LastReceivedHeader;

        public DispatchMessageInspector(OperationDescription operation) : base(operation)
        {
        }

        protected override JxApplication Application => s_application;

        protected override Exception GetUnauthorizedException() => new Exception("登录已过期");

        protected override Exception CreateForbiddenException() => new Exception("登录已过期");

        public override object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            string action = request.Headers.Action.Substring(request.Headers.Action.LastIndexOf("/") + 1);

            if (!GlobalCache.WhiteFunctionList.Contains(action))
            {
                MessageHeaders messageHeadersElement = OperationContext.Current.IncomingMessageHeaders;

                int userId = 0;
                string key = string.Empty;

                try
                {
                    userId = System.Web.HttpUtility.UrlDecode(messageHeadersElement.GetHeader<String>("p1", "")).Trim().ToInt32();
                    key = System.Web.HttpUtility.UrlDecode(messageHeadersElement.GetHeader<String>("p2", "")).ToTrimString();
                }
                catch
                {
                }

                if (userId == 0 || string.IsNullOrEmpty(key))
                {
                    throw new Exception("登录已过期");
                }

                string cacheObj = DependencyUtil.ResolveService<ICacheBase>().GetCache(CacheKeyHelper.GetUserTokenKey(key));

                if (!string.IsNullOrWhiteSpace(cacheObj))
                {
                    Model.UserInfoToken userinfo = null;

                    try
                    {
                        userinfo = cacheObj.Deserialize<Model.UserInfoToken>();
                    }
                    catch (Exception ex)
                    {
                        DependencyUtil.ResolveService<ICacheBase>().DeleteCache(key);
                        AutofacHostFactory.Container.Resolve<ILogger<DispatchMessageInspector>>().LogError("反序列化UserInfoToken异常，cacheObj=" + cacheObj + "，action=" + action + "，详细信息：" + ex.Message + ",堆栈：" + ex.StackTrace);
                    }

                    if (userinfo != null && userinfo.UserId == userId)
                    {
                        return null;
                    }

                    throw new Exception("登录已过期");
                }
                else
                {
                    //string msg = "回传hash串“" + key + "”不正确，用户“" + name + "”被强制下线，本次登录IP：" + ip;
                    //LogsManager.Info(msg);
                    throw new Exception("登录已过期");
                }
            }

            return null;
        }
    }
}