using Serenity.Services;
using MyRequest = Serenity.Services.DeleteRequest;
using MyResponse = Serenity.Services.DeleteResponse;
using MyRow = Management.BetHistory.PalyInfoRow;

namespace Management.BetHistory
{
    public interface IPalyInfoDeleteHandler : IDeleteHandler<MyRow, MyRequest, MyResponse> {}

    public class PalyInfoDeleteHandler : DeleteRequestHandler<MyRow, MyRequest, MyResponse>, IPalyInfoDeleteHandler
    {
        public PalyInfoDeleteHandler(IRequestContext context)
             : base(context)
        {
        }
    }
}