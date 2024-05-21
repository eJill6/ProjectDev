using Serenity.Services;
using MyRequest = Serenity.Services.ListRequest;
using MyResponse = Serenity.Services.ListResponse<Management.ProfitlossTest.ProfitLossRow>;
using MyRow = Management.ProfitlossTest.ProfitLossRow;

namespace Management.ProfitlossTest
{
    public interface IProfitLossListHandler : IListHandler<MyRow, MyRequest, MyResponse> {}

    public class ProfitLossListHandler : ListRequestHandler<MyRow, MyRequest, MyResponse>, IProfitLossListHandler
    {
        public ProfitLossListHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}