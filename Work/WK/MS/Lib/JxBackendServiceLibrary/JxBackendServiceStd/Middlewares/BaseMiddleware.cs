using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using System;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using Microsoft.AspNetCore.Http;

namespace JxBackendService.Middlewares
{
    public abstract class BaseMiddleware
    {
        private static readonly Lazy<IEnvironmentService> s_environmentService = DependencyUtil.ResolveService<IEnvironmentService>();

        protected EnvironmentUser EnvironmentUser;

        protected RequestDelegate Next { get; private set; }

        protected JxApplication Application => s_environmentService.Value.Application;

        public BaseMiddleware(RequestDelegate next)
        {
            Next = next;

            EnvironmentUser = new EnvironmentUser()
            {
                Application = Application,
                LoginUser = new BasicUserInfo()
                {
                    UserId = 0
                }
            };
        }
    }
}