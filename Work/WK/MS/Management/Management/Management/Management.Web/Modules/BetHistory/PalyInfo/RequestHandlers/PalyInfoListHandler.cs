using Serenity.Services;
using MyRequest = Serenity.Services.ListRequest;
using MyResponse = Serenity.Services.ListResponse<Management.BetHistory.PalyInfoRow>;
using MyRow = Management.BetHistory.PalyInfoRow;

namespace Management.BetHistory
{
    public interface IPalyInfoListHandler : IListHandler<MyRow, MyRequest, MyResponse> {}

    public class PalyInfoListHandler : ListRequestHandler<MyRow, MyRequest, MyResponse>, IPalyInfoListHandler
    {
        public PalyInfoListHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}