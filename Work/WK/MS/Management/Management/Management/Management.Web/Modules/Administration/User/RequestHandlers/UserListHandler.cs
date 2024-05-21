using Serenity.Services;
using MyRequest = Management.Administration.UserListRequest;
using MyResponse = Serenity.Services.ListResponse<Management.Administration.UserRow>;
using MyRow = Management.Administration.UserRow;

namespace Management.Administration
{
    public interface IUserListHandler : IListHandler<MyRow, MyRequest, MyResponse> { }

    public class UserListHandler : ListRequestHandler<MyRow, MyRequest, MyResponse>, IUserListHandler
    {
        public UserListHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}