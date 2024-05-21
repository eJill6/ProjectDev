using Serenity.Services;
using MyRequest = Serenity.Services.RetrieveRequest;
using MyResponse = Serenity.Services.RetrieveResponse<Management.BetHistory.PalyInfoRow>;
using MyRow = Management.BetHistory.PalyInfoRow;

namespace Management.BetHistory
{
    public interface IPalyInfoRetrieveHandler : IRetrieveHandler<MyRow, MyRequest, MyResponse> {}

    public class PalyInfoRetrieveHandler : RetrieveRequestHandler<MyRow, MyRequest, MyResponse>, IPalyInfoRetrieveHandler
    {
        public PalyInfoRetrieveHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}