using Serenity.Services;
using MyRequest = Serenity.Services.SaveRequest<Management.ProductManagement.FrontsideMenuRow>;
using MyResponse = Serenity.Services.SaveResponse;
using MyRow = Management.ProductManagement.FrontsideMenuRow;

namespace Management.ProductManagement
{
    public interface IFrontsideMenuSaveHandler : ISaveHandler<MyRow, MyRequest, MyResponse> {}

    public class FrontsideMenuSaveHandler : SaveRequestHandler<MyRow, MyRequest, MyResponse>, IFrontsideMenuSaveHandler
    {
        public FrontsideMenuSaveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}