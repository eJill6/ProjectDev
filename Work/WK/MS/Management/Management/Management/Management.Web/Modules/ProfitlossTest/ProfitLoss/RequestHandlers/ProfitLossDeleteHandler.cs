using Serenity.Services;
using MyRequest = Serenity.Services.DeleteRequest;
using MyResponse = Serenity.Services.DeleteResponse;
using MyRow = Management.ProfitlossTest.ProfitLossRow;

namespace Management.ProfitlossTest
{
    public interface IProfitLossDeleteHandler : IDeleteHandler<MyRow, MyRequest, MyResponse> {}

    public class ProfitLossDeleteHandler : DeleteRequestHandler<MyRow, MyRequest, MyResponse>, IProfitLossDeleteHandler
    {
        public ProfitLossDeleteHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}