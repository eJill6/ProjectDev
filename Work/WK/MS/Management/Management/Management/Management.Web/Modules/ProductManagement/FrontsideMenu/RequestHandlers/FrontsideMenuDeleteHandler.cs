using Serenity.Services;
using MyRequest = Serenity.Services.DeleteRequest;
using MyResponse = Serenity.Services.DeleteResponse;
using MyRow = Management.ProductManagement.FrontsideMenuRow;

namespace Management.ProductManagement
{
    public interface IFrontsideMenuDeleteHandler : IDeleteHandler<MyRow, MyRequest, MyResponse> {}

    public class FrontsideMenuDeleteHandler : DeleteRequestHandler<MyRow, MyRequest, MyResponse>, IFrontsideMenuDeleteHandler
    {
        public FrontsideMenuDeleteHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}