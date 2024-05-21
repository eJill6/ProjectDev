using Serenity.Services;
using MyRequest = Serenity.Services.SaveRequest<Management.ProfitlossTest.ProfitLossRow>;
using MyResponse = Serenity.Services.SaveResponse;
using MyRow = Management.ProfitlossTest.ProfitLossRow;

namespace Management.ProfitlossTest
{
    public interface IProfitLossSaveHandler : ISaveHandler<MyRow, MyRequest, MyResponse> {}

    public class ProfitLossSaveHandler : SaveRequestHandler<MyRow, MyRequest, MyResponse>, IProfitLossSaveHandler
    {
        public ProfitLossSaveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}