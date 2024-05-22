using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject.MockService
{
    public static class MockServiceUtil
    {
        public static EnvironmentUser CreateUser()
        {
            return new EnvironmentUser()
            {
                Application = JxApplication.FrontSideWeb,
                LoginUser = new BasicUserInfo()
                {
                    UserId = 6251
                }
            };
        }
    }
}