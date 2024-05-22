using JxBackendService.Model.ViewModel;
using SLPolyGame.Web;

namespace UnitTestProject.MockService
{
    public class ThirdPartyApiMockService : ThirdPartyApiService
    {
        protected override SLPolyGame.Web.Model.UserInfoToken GetUserInfoToken()
        {
            BasicUserInfo mockUser = MockServiceUtil.CreateUser().LoginUser;

            return new SLPolyGame.Web.Model.UserInfoToken()
            {
                Key = mockUser.UserKey,
                UserId = mockUser.UserId,
            };
        }
    }
}