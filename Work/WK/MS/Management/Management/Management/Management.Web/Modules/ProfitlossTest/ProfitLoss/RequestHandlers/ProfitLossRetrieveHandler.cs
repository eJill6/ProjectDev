using Serenity.Services;
using MyRequest = Serenity.Services.RetrieveRequest;
using MyResponse = Serenity.Services.RetrieveResponse<Management.ProfitlossTest.ProfitLossRow>;
using MyRow = Management.ProfitlossTest.ProfitLossRow;

namespace Management.ProfitlossTest
{
    public interface IProfitLossRetrieveHandler : IRetrieveHandler<MyRow, MyRequest, MyResponse> {}

    public class ProfitLossRetrieveHandler : RetrieveRequestHandler<MyRow, MyRequest, MyResponse>, IProfitLossRetrieveHandler
    {
        public ProfitLossRetrieveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}