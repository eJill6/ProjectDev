using Serenity.Services;
using MyRequest = Serenity.Services.ListRequest;
using MyResponse = Serenity.Services.ListResponse<Management.ProductManagement.FrontsideMenuRow>;
using MyRow = Management.ProductManagement.FrontsideMenuRow;

namespace Management.ProductManagement
{
    public interface IFrontsideMenuListHandler : IListHandler<MyRow, MyRequest, MyResponse> {}

    public class FrontsideMenuListHandler : ListRequestHandler<MyRow, MyRequest, MyResponse>, IFrontsideMenuListHandler
    {
        public FrontsideMenuListHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}