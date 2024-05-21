using Serenity.Services;
using MyRequest = Serenity.Services.RetrieveRequest;
using MyResponse = Serenity.Services.RetrieveResponse<Management.ProductManagement.FrontsideMenuRow>;
using MyRow = Management.ProductManagement.FrontsideMenuRow;

namespace Management.ProductManagement
{
    public interface IFrontsideMenuRetrieveHandler : IRetrieveHandler<MyRow, MyRequest, MyResponse> {}

    public class FrontsideMenuRetrieveHandler : RetrieveRequestHandler<MyRow, MyRequest, MyResponse>, IFrontsideMenuRetrieveHandler
    {
        public FrontsideMenuRetrieveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}